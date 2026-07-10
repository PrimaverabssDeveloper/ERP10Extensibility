using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OfficeOpenXml;

namespace CustomSmartAction
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Model
    // ─────────────────────────────────────────────────────────────────────────

    public class PriceRecord
    {
        public string Reference { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VatRate { get; set; }  // e.g. 0.23 for 23%
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  File-type detection
    // ─────────────────────────────────────────────────────────────────────────

    public enum SourceFileType { Pdf, Excel, Unknown }

    public static class FileTypeDetector
    {
        private static readonly byte[] PdfMagic = { 0x25, 0x50, 0x44, 0x46 }; // %PDF
        private static readonly byte[] XlsxMagic = { 0x50, 0x4B, 0x03, 0x04 }; // PK (ZIP)

        public static SourceFileType Detect(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Ficheiro não encontrado.", filePath);

            byte[] header = new byte[4];
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                fs.Read(header, 0, 4);

            if (header.SequenceEqual(PdfMagic)) return SourceFileType.Pdf;
            if (header.SequenceEqual(XlsxMagic)) return SourceFileType.Excel;

            // Fallback: check extension
            string ext = System.IO.Path.GetExtension(filePath).ToLowerInvariant();
            if (ext == ".pdf") return SourceFileType.Pdf;
            if (ext == ".xlsx" || ext == ".xls") return SourceFileType.Excel;

            return SourceFileType.Unknown;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Excel reader  (EPPlus 4.x — no licence required for non-commercial)
    // ─────────────────────────────────────────────────────────────────────────

    public static class ExcelPriceReader
    {
        // Recognised header keywords (lowercase, trimmed)
        private static readonly string[] RefKeywords = { "referência", "referencia", "ref", "código", "codigo", "sku" };
        private static readonly string[] DescKeywords = { "descrição", "descricao", "descr", "artigo", "produto", "description" };
        private static readonly string[] PriceKeywords = { "preço", "preco", "price", "unit", "valor" };
        private static readonly string[] VatKeywords = { "iva", "vat", "tax", "imposto" };

        public static List<PriceRecord> Read(string filePath)
        {
            var records = new List<PriceRecord>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Iterate every worksheet
                foreach (ExcelWorksheet ws in package.Workbook.Worksheets)
                {
                    int rows = ws.Dimension?.Rows ?? 0;
                    int cols = ws.Dimension?.Columns ?? 0;
                    if (rows < 2 || cols < 2) continue;

                    // Find header row (first row whose cells match enough keywords)
                    int headerRow = FindHeaderRow(ws, rows, cols);
                    if (headerRow < 0) continue;

                    // Map column indices
                    int colRef = -1, colDesc = -1, colPrice = -1, colVat = -1;
                    for (int c = 1; c <= cols; c++)
                    {
                        string h = CellText(ws, headerRow, c).ToLowerInvariant().Trim();
                        if (colRef < 0 && RefKeywords.Any(k => h.Contains(k))) colRef = c;
                        if (colDesc < 0 && DescKeywords.Any(k => h.Contains(k))) colDesc = c;
                        if (colPrice < 0 && PriceKeywords.Any(k => h.Contains(k))) colPrice = c;
                        if (colVat < 0 && VatKeywords.Any(k => h.Contains(k))) colVat = c;
                    }

                    if (colRef < 0 || colDesc < 0 || colPrice < 0) continue;

                    // Read data rows
                    for (int r = headerRow + 1; r <= rows; r++)
                    {
                        string refVal = CellText(ws, r, colRef).Trim();
                        string descVal = CellText(ws, r, colDesc).Trim();
                        string priceVal = CellText(ws, r, colPrice).Trim();

                        // Skip empty or summary rows
                        if (string.IsNullOrEmpty(refVal) || string.IsNullOrEmpty(descVal)) continue;
                        if (refVal.ToUpperInvariant() == "TOTAL") continue;

                        decimal price = ParseDecimal(priceVal);
                        decimal vatRate = 0m;

                        if (colVat > 0)
                        {
                            string vatVal = CellText(ws, r, colVat).Trim();
                            vatRate = ParseVatRate(vatVal);
                        }
                        else
                        {
                            // Default VAT rate when no column is present
                            vatRate = 0.23m;
                        }

                        records.Add(new PriceRecord
                        {
                            Reference = refVal,
                            Description = descVal,
                            UnitPrice = price,
                            VatRate = vatRate
                        });
                    }
                }
            }

            return records;
        }

        private static int FindHeaderRow(ExcelWorksheet ws, int rows, int cols)
        {
            for (int r = 1; r <= Math.Min(rows, 15); r++)
            {
                int matches = 0;
                for (int c = 1; c <= cols; c++)
                {
                    string h = CellText(ws, r, c).ToLowerInvariant();
                    if (RefKeywords.Any(k => h.Contains(k)) ||
                        DescKeywords.Any(k => h.Contains(k)) ||
                        PriceKeywords.Any(k => h.Contains(k)) ||
                        VatKeywords.Any(k => h.Contains(k)))
                        matches++;
                }
                if (matches >= 2) return r;
            }
            return -1;
        }

        private static string CellText(ExcelWorksheet ws, int row, int col)
        {
            var cell = ws.Cells[row, col];
            if (cell?.Value == null) return string.Empty;
            return cell.Value.ToString();
        }

        private static decimal ParseDecimal(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return 0m;
            // Remove currency symbols and whitespace, normalise decimal separator
            string cleaned = Regex.Replace(raw, @"[€$£\s]", "").Replace(",", ".");
            // Handle cases like "1.234.56" → keep last dot as decimal
            int lastDot = cleaned.LastIndexOf('.');
            if (lastDot > 0 && lastDot == cleaned.Length - 3)
                cleaned = cleaned.Replace(".", "").Insert(cleaned.Length - 3, ".");
            decimal result;
            decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            return result;
        }

        // Parses a VAT rate from multiple possible formats:
        //   "23%"  → 0.23
        //   "0.23" → 0.23   (already a fraction)
        //   "23"   → 0.23   (bare integer, assumed percentage)
        private static decimal ParseVatRate(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return 0m;

            bool hasPercent = raw.Contains('%');
            string cleaned = Regex.Replace(raw, @"[%\s]", "").Replace(",", ".");

            decimal value;
            if (!decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return 0m;

            // If the source cell stored 0.23 (Excel percentage format) EPPlus returns 0.23 directly
            // If it came in as "23" or "23%" we convert to fraction
            if (hasPercent || value >= 1m)
                return value / 100m;

            return value;  // already a fraction (e.g. 0.23)
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  PDF reader
    //
    //  Uses pdftotext -layout (poppler-utils) which preserves the spatial layout
    //  of the page by padding columns with spaces proportional to their X position.
    //  This means columns are reliably separated by 2+ spaces, so \s{2,} works.
    //
    //  Requirement: pdftotext must be installed on the machine.
    //    Windows : https://github.com/oschwartz10612/poppler-windows/releases
    //              Add the bin\ folder to PATH.
    //    Linux   : sudo apt install poppler-utils
    //    macOS   : brew install poppler
    // ─────────────────────────────────────────────────────────────────────────

    public static class PdfPriceReader
    {
        private static readonly Regex VatPercentPattern =
            new Regex(@"^(\d{1,3})%$", RegexOptions.Compiled);

        // ── Public entry point ───────────────────────────────────────────────

        public static List<PriceRecord> Read(string filePath)
        {
            string layoutText = RunPdftotext(filePath);
            var lines = layoutText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // Find header line
            int headerIdx = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                string low = lines[i].ToLowerInvariant();
                if ((low.Contains("refer") || low.Contains("ref")) &&
                    (low.Contains("descri") || low.Contains("artigo")) &&
                    (low.Contains("pre") || low.Contains("price")))
                {
                    headerIdx = i;
                    break;
                }
            }

            // Determine column order from header tokens (defaults: ref=0, desc=1, price=2, vat=3)
            int colRef = 0, colDesc = 1, colPrice = 2, colVat = 3;
            if (headerIdx >= 0)
            {
                var hdrTokens = SplitColumns(lines[headerIdx]);
                for (int c = 0; c < hdrTokens.Count; c++)
                {
                    string h = hdrTokens[c].ToLowerInvariant();
                    if (h.Contains("ref") || h.Contains("código") || h.Contains("codigo")) colRef = c;
                    else if (h.Contains("descri") || h.Contains("artigo")) colDesc = c;
                    else if (h.Contains("pre") || h.Contains("price")) colPrice = c;
                    else if (h.Contains("iva") || h.Contains("vat") || h.Contains("taxa")) colVat = c;
                }
            }

            var records = new List<PriceRecord>();
            int startIdx = headerIdx >= 0 ? headerIdx + 1 : 0;

            for (int i = startIdx; i < lines.Length; i++)
            {
                var cols = SplitColumns(lines[i]);
                if (cols.Count < 2) continue;

                string refVal = Get(cols, colRef);
                string descVal = Get(cols, colDesc);

                if (string.IsNullOrEmpty(refVal) && string.IsNullOrEmpty(descVal)) continue;
                if (refVal.ToUpperInvariant() == "TOTAL") continue;
                if (descVal.ToUpperInvariant().Contains("DESCRI")) continue;

                decimal price = ParsePrice(Get(cols, colPrice));
                decimal vatRate = ParseVatRate(cols, colVat);

                if (price == 0m) continue;

                records.Add(new PriceRecord
                {
                    Reference = refVal,
                    Description = descVal,
                    UnitPrice = price,
                    VatRate = vatRate
                });
            }

            return records;
        }

        // ── pdftotext discovery & invocation ────────────────────────────────

        // Candidate locations where poppler is commonly installed on Windows.
        private static readonly string[] WindowsFallbackDirs = new[]
        {
            @"C:\Program Files\poppler\Library\bin",
            @"C:\Program Files\poppler\bin",
            @"C:\poppler\bin",
            @"C:\poppler\Library\bin",
            @"C:\tools\poppler\bin",
        };

        // Resolves the full path to pdftotext, trying PATH first then common
        // Windows install directories. Throws with install instructions if not found.
        private static string FindPdftotext()
        {
            // Try PATH (works on Linux/macOS/Windows if added to PATH)
            string fromPath = FindInPath("pdftotext.exe") ?? FindInPath("pdftotext");
            if (fromPath != null) return fromPath;

            // Try known Windows installation directories
            foreach (string dir in WindowsFallbackDirs)
            {
                string candidate = Path.Combine(dir, "pdftotext.exe");
                if (File.Exists(candidate)) return candidate;
            }

            throw new InvalidOperationException(
                "Nao foi possivel encontrar o pdftotext (poppler-utils).\n\n" +
                "Para instalar no Windows:\n" +
                "  1. Descarregue de: https://github.com/oschwartz10612/poppler-windows/releases\n" +
                "  2. Extraia para uma pasta, por exemplo: C:\\poppler\n" +
                "  3. Adicione C:\\poppler\\Library\\bin ao PATH do sistema, OU\n" +
                "     coloque o executavel em uma destas pastas:\n" +
                string.Join("\n", WindowsFallbackDirs.Select(d => "       " + d)) + "\n\n" +
                "Linux  : sudo apt install poppler-utils\n" +
                "macOS  : brew install poppler");
        }

        private static string FindInPath(string exeName)
        {
            string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            foreach (string dir in pathEnv.Split(Path.PathSeparator))
            {
                try
                {
                    string full = Path.Combine(dir.Trim(), exeName);
                    if (File.Exists(full)) return full;
                }
                catch { /* ignore malformed PATH entries */ }
            }
            return null;
        }

        private static string RunPdftotext(string pdfPath)
        {
            string exe = FindPdftotext();

            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = exe,
                Arguments = $"-layout -enc UTF-8 \"{pdfPath}\" -",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                StandardOutputEncoding = Encoding.UTF8
            };

            using (var proc = System.Diagnostics.Process.Start(psi))
            {
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                {
                    string err = proc.StandardError.ReadToEnd();
                    throw new InvalidOperationException(
                        $"pdftotext falhou (exit {proc.ExitCode}): {err}");
                }

                return output;
            }
        }

        // ── Column splitting ─────────────────────────────────────────────────

        // pdftotext -layout separates columns with 2+ spaces — split on that
        private static List<string> SplitColumns(string line)
        {
            return Regex.Split(line.Trim(), @"\s{2,}")
                        .Select(p => p.Trim())
                        .Where(p => p.Length > 0)
                        .ToList();
        }

        private static string Get(List<string> cols, int idx)
        {
            return idx >= 0 && idx < cols.Count ? cols[idx] : string.Empty;
        }

        // ── Value parsers ────────────────────────────────────────────────────

        private static decimal ParsePrice(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return 0m;
            string cleaned = Regex.Replace(raw, @"[€$£\s]", "");
            // European decimal: "8,50" → "8.50"
            if (Regex.IsMatch(cleaned, @"^\d{1,3},\d{2}$"))
                cleaned = cleaned.Replace(",", ".");
            else
                cleaned = cleaned.Replace(",", "");
            decimal result;
            decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            return result;
        }

        private static decimal ParseVatRate(List<string> cols, int colVat)
        {
            string raw = Get(cols, colVat).Trim();
            if (string.IsNullOrEmpty(raw)) return 0.23m;

            // "23%" → 0.23
            var m = VatPercentPattern.Match(raw);
            if (m.Success)
            {
                decimal v;
                if (decimal.TryParse(m.Groups[1].Value, out v)) return v / 100m;
            }

            // "0.23" or "0,23" → 0.23
            string normalised = raw.Replace(",", ".");
            if (Regex.IsMatch(normalised, @"^0\.\d{2}$"))
            {
                decimal v;
                if (decimal.TryParse(normalised, NumberStyles.Any, CultureInfo.InvariantCulture, out v))
                    return v;
            }

            return 0.23m; // default
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  CSV writer
    // ─────────────────────────────────────────────────────────────────────────

    public static class CsvWriter
    {
        private const string Separator = ";";

        public static string Build(List<PriceRecord> records)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Join(Separator, "Referência", "Descrição", "Preço Unitário (€)", "Taxa IVA (%)"));

            foreach (var r in records)
            {
                // Output VAT rate as a percentage value, e.g. 0.23 → "23"
                string vatDisplay = (r.VatRate * 100m).ToString("F0", CultureInfo.InvariantCulture);

                sb.AppendLine(string.Join(Separator,
                    EscapeCsv(r.Reference),
                    EscapeCsv(r.Description),
                    r.UnitPrice.ToString("F2", new CultureInfo("pt-PT")),
                    vatDisplay
                ));
            }

            return sb.ToString();
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (value.Contains(Separator) || value.Contains('"') || value.Contains('\n'))
                return '"' + value.Replace("\"", "\"\"") + '"';
            return value;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Converter  (orchestrator)
    // ─────────────────────────────────────────────────────────────────────────

    public static class PriceTableConverter
    {
        public static string Convert(string inputPath)
        {
            SourceFileType type = FileTypeDetector.Detect(inputPath);

            Console.WriteLine($"  Tipo detectado: {type}");

            List<PriceRecord> records;
            switch (type)
            {
                case SourceFileType.Excel:
                    records = ExcelPriceReader.Read(inputPath);
                    break;
                case SourceFileType.Pdf:
                    records = PdfPriceReader.Read(inputPath);
                    break;
                default:
                    throw new NotSupportedException(
                        "Formato de ficheiro não suportado. Use .pdf ou .xlsx.");
            }

            if (records.Count == 0)
                throw new InvalidOperationException(
                    "Nenhum registo encontrado no ficheiro. Verifique a estrutura do documento.");

            return CsvWriter.Build(records);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Entry point
    // ─────────────────────────────────────────────────────────────────────────

    public static class FileReaderToCsvConverter
    {
        public static string FilePdfExcelReader()
        {
            string filePath = PromptFilePath();
            if (filePath == null)
            {
                return string.Empty;
            }

            try
            {
                string csvContent = PriceTableConverter.Convert(filePath);

                // Preview first few lines of the CSV string
                return csvContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

        }


        // ── UI helpers ───────────────────────────────────────────────────────

        private static string PromptFilePath()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Seleccione a tabela de preços";
                dlg.Filter = "Tabelas de preços (*.pdf;*.xlsx)|*.pdf;*.xlsx" +
                             "|Ficheiros PDF (*.pdf)|*.pdf" +
                             "|Ficheiros Excel (*.xlsx)|*.xlsx";
                dlg.FilterIndex = 1;      // start with combined filter
                dlg.RestoreDirectory = true;
                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                    return dlg.FileName;
            }

            return null;  // user cancelled
        }


    }
}