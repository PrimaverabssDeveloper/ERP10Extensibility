using BasBE100;
using ErpBS100;
using StdPlatBS100;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CustomSmartAction
{
    public class CriaArtigos
    {
        private readonly ErpBS _primavera;
        private readonly StdBSInterfPub _plataforma;

        string _conteudoCSV;
        public CriaArtigos(ErpBS erpBS, StdBSInterfPub platBS, string conteudoCSV)
        {
            _primavera = erpBS;
            _plataforma = platBS;
            _conteudoCSV = conteudoCSV;
        }

        public void Executar(System.Windows.Forms.TextBox textBoxResultado)
        {
            try
            {
                // Contexto enviado como txtContext: descreve ao LLM o que é o CSV
                string txtContext =
                    "O conteúdo em contextData é uma tabela de preços de artigos em formato CSV " +
                    "com separador ';' e as seguintes colunas: " +
                    "Referência, Descrição, Preço Unitário (€), Taxa IVA (%). ";

                // Prompt enviado como txtPrompt: instrui o LLM sobre o JSON a produzir
                string txtPrompt =
                    "Converte cada linha do CSV (excluindo o cabeçalho) para um objecto JSON com a seguinte estrutura:\n" +
                    "{\n" +
                    "  \"Artigo\": {\n" +
                    "    \"Artigo\": \"<Referência>\",\n" +
                    "    \"Descricao\": \"<Descrição>\",\n" +
                    "    \"IVA\": <Taxa IVA como número inteiro, ex: 23>\n" +
                    "  },\n" +
                    "  \"ArtigoMoeda\": {\n" +
                    "    \"Artigo\": \"<Referência>\",\n" +
                    "    \"PVP1\": <Preço Unitário como número decimal, ex: 8.50>,\n" +
                    "    \"Moeda\": \"EUR\",\n" +
                    "    \"Unidade\": \"UN\"\n" +
                    "  }\n" +
                    "}\n" +
                    "Devolve apenas um array JSON com todos os objectos, sem texto adicional, " +
                    "sem markdown, sem comentários. O separador decimal do PVP1 deve ser ponto (.).";

                string contextData = _conteudoCSV;

                string result = _plataforma.Pulse.UserPromptFunction(txtContext, contextData, txtPrompt);

                textBoxResultado.Text = result;

                var summary = Import(result);

                textBoxResultado.Text = $"  Importação concluída: {summary.Success}/{summary.Total} artigos criados" +
                  (summary.Skipped > 0 ? $", {summary.Skipped} ignorados." : ".");

                if (summary.Errors.Count > 0)
                {
                    textBoxResultado.Text += "\n\nErros:\n" + string.Join("\n", summary.Errors);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class ImportSummary
        {
            public int Total { get; set; }
            public int Success { get; set; }
            public int Skipped { get; set; }
            public List<string> Errors { get; set; } = new List<string>();
            public List<string> Imported { get; set; } = new List<string>();
        }

        public ImportSummary Import(string json)
        {
            var summary = new ImportSummary();

            JArray items;
            try
            {
                items = JArray.Parse(json);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    $"O JSON devolvido pelo LLM não é válido: {ex.Message}\n\nConteúdo recebido:\n{json}");
            }

            summary.Total = items.Count;

            foreach (JObject item in items.OfType<JObject>())
            {
                string referencia = "(desconhecida)";
                try
                {
                    JObject artObj = (JObject)item["Artigo"];
                    JObject moedaObj = (JObject)item["ArtigoMoeda"];

                    referencia = artObj["Artigo"]?.ToString() ?? string.Empty;
                    string descricao = artObj["Descricao"]?.ToString() ?? string.Empty;
                    string iva = artObj["IVA"]?.ToString() ?? "23";
                    double pvp1 = moedaObj["PVP1"]?.ToObject<double>() ?? 0.0;

                    if (string.IsNullOrEmpty(referencia))
                    {
                        summary.Skipped++;
                        summary.Errors.Add("Linha ignorada: campo Artigo vazio.");
                        continue;
                    }

                    // ── Artigo ───────────────────────────────────────────────
                    BasBEArtigo basBEArtigo = new BasBEArtigo();
                    basBEArtigo.Artigo = referencia;
                    basBEArtigo.Descricao = descricao;
                    basBEArtigo.IVA = iva;
                    basBEArtigo.MovStock = "S";

                    _primavera.Base.Artigos.PreencheDadosRelacionados(basBEArtigo);
                    _primavera.Base.Artigos.Actualiza(basBEArtigo);

                    // ── Preço ────────────────────────────────────────────────
                    BasBEArtigoMoeda basBEArtigoMoeda = new BasBEArtigoMoeda();
                    basBEArtigoMoeda.Artigo = referencia;
                    basBEArtigoMoeda.PVP1 = pvp1;
                    basBEArtigoMoeda.Moeda = "EUR";
                    basBEArtigoMoeda.Unidade = "UN";

                    _primavera.Base.ArtigosPrecos.Actualiza(basBEArtigoMoeda);

                    summary.Success++;
                    summary.Imported.Add($"    ✓ {referencia} — {descricao}");
                }
                catch (Exception ex)
                {
                    summary.Skipped++;
                    summary.Errors.Add($"{referencia}: {ex.Message}");
                }
            }

            return summary;
        }
    }
}
