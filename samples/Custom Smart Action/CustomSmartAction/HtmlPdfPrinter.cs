using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CustomSmartAction
{
    public static class HtmlPdfPrinter
    {
        public static string ImprimirHtmlParaPdf(
            string htmlCompleto,
            string pastaDestino,
            string caminhoWkhtmltopdf)
        {
            if (string.IsNullOrWhiteSpace(htmlCompleto))
                throw new ArgumentException("O HTML não pode estar vazio.", nameof(htmlCompleto));

            htmlCompleto = htmlCompleto.Replace("```html", "").Replace("```", "");

            if (string.IsNullOrWhiteSpace(pastaDestino))
                throw new ArgumentException("A pasta de destino é obrigatória.", nameof(pastaDestino));

            if (!Directory.Exists(pastaDestino))
                Directory.CreateDirectory(pastaDestino);

            if (!File.Exists(caminhoWkhtmltopdf))
                throw new FileNotFoundException(
                    $"Nao foi possivel encontrar '{caminhoWkhtmltopdf}'. Verifique o caminho do executável ou instale o wkhtmltopdf.\n\n" +
                    "Para instalar no Windows:\n" +
                    "  1. Descarregue de: https://wkhtmltopdf.org/\n" +
                    "  2. Instale e verifique o caminho do executável. Caso seja instalado numa pasta diferente, ajuste o caminho conforme necessário.");

            string nomePdf = "documento_" + Guid.NewGuid().ToString("N").Substring(0, 8) + ".pdf";
            string caminhoPdfDestino = Path.Combine(pastaDestino, nomePdf);

            string tempHtml = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".html");

            try
            {
                File.WriteAllText(tempHtml, htmlCompleto, Encoding.UTF8);

                var argumentos =
                    "--encoding utf-8 " +
                    "--enable-local-file-access " +
                    "\"" + tempHtml + "\" " +
                    "\"" + caminhoPdfDestino + "\"";

                var processo = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = caminhoWkhtmltopdf,
                        Arguments = argumentos,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true
                    }
                };

                processo.Start();

                string erro = processo.StandardError.ReadToEnd();
                processo.WaitForExit();

                if (processo.ExitCode != 0)
                    throw new Exception("Erro ao gerar PDF: " + erro);

                AbrirPdf(caminhoPdfDestino);

                return caminhoPdfDestino;
            }
            finally
            {
                if (File.Exists(tempHtml))
                    File.Delete(tempHtml);
            }
        }

        private static void AbrirPdf(string caminhoPdf)
        {
            if (!File.Exists(caminhoPdf))
                throw new FileNotFoundException("PDF não foi encontrado para abertura.", caminhoPdf);

            Process.Start(new ProcessStartInfo
            {
                FileName = caminhoPdf,
                UseShellExecute = true
            });
        }
    }
}