using Newtonsoft.Json.Linq;
using Primavera.Extensibility.BusinessEntities.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using VndBE100;

namespace CustomSmartAction
{
    public class SmartAction : Primavera.Extensibility.CustomSmartAction.CustomSmartAction
    {
        // Mantém referências entre BeforeExecution e AfterExecution
        private formProcessando _formProcessando;
        private Dictionary<string, byte[]> _uploadedFiles;

        public override CustomSmartActionRequest BeforeExecution<T>(string smartActionName, T businessEntity)
        {
            if (smartActionName.Equals("FUNC001", StringComparison.InvariantCultureIgnoreCase) && businessEntity is RhpBE100.RhpBEFuncionario funcionario)
            {
                // ── 1. Selecção de múltiplos ficheiros ────────────────────────
                List<string> selectedPaths;
                using (var formSelecao = new formSelecionarFicheiros())
                {
                    if (formSelecao.ShowDialog() != System.Windows.Forms.DialogResult.OK || formSelecao.SelectedFiles.Count == 0)
                    {
                        return new CustomSmartActionRequest();
                    }

                    selectedPaths = new List<string>(formSelecao.SelectedFiles);
                }

                // ── 2. Lê os bytes de cada ficheiro ───────────────────────────
                var files = new Dictionary<string, byte[]>();
                foreach (var path in selectedPaths)
                {
                    files[Path.GetFileName(path)] = File.ReadAllBytes(path);
                }
                _uploadedFiles = files; // guardado para extracção de foto em AfterExecution

                // ── 3. Mostra o loader numa thread STA própria ───────────────
                // O framework bloqueia a UI thread durante o processamento do AI,
                // por isso o form precisa da sua própria message loop para ficar
                // responsivo (animar, reagir ao botão Cancelar, disparar o timeout).
                var formReady = new ManualResetEventSlim(false);
                var thread = new Thread(() =>
                {
                    _formProcessando = new formProcessando();
                    // Sinaliza assim que o Handle estiver criado — seguro invocar a partir daí
                    _formProcessando.HandleCreated += (s, e) => formReady.Set();
                    Application.Run(_formProcessando);
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = true; // não impede o processo de terminar
                thread.Start();
                formReady.Wait(5000); // aguarda no máximo 5 s até o form estar pronto

                return new CustomSmartActionRequest()
                {
                    Prompt = @"És um assistente de extração de dados de RH.
Extraia a informação sobre novos colaboradores do(s) documento(s) fornecido(s) e 
devolva-a como um único objeto JSON estritamente válido, correspondente ao esquema abaixo. 
Não inclua qualquer texto, explicação ou formatação markdown fora do objeto JSON.

REGRAS:
1. Extraia apenas informação explicitamente presente no documento. Não infira, adivinhe nem invente valores.
2. Se um campo não for encontrado, defina o seu valor como null (não omita a chave).
3. Normalize as datas para o formato ISO 8601 (AAAA-MM-DD).
4. Normalize os números de telefone para o formato E.164 sempre que possível (+[código do país][número]).
5. Preserve os nomes legais exatamente como estão escritos, incluindo diacríticos.
6. Não extraia números de documentos de identificação governamentais que não constem no esquema abaixo

ESQUEMA:
{
  ""title"": ""Funcionários"",
  ""type"": [ ""object"", ""null"" ],
  ""properties"": {

    ""Nome"":            { ""type"": [""string"", ""null""], ""maxLength"": 80 },
    ""PrimeiroNome"":    { ""type"": [""string"", ""null""], ""maxLength"": 80 },
    ""PrimeiroApelido"": { ""type"": [""string"", ""null""], ""maxLength"": 60 },
    ""SegundoApelido"":  { ""type"": [""string"", ""null""], ""maxLength"": 60 },
    ""NomeAbreviado"":   { ""type"": [""string"", ""null""], ""maxLength"": 25, ""description"": ""Exactly one word from PrimeiroNome (the very first word only, even if PrimeiroNome has multiple words) + Exactly one word from SegundoApelido (the very last word only, even if it has multiple words, use PrimeiroApelido if SegundoApelido is empty). Example: PrimeiroNome='Maria Fernanda', SegundoApelido='Ferreira Oliveira' -> 'Maria Oliveira'. Max 25 chars."" },
    ""DataNascimento"":  { ""type"": [""string"", ""null""], ""format"": ""date-time"" },
    ""Sexo"":            { ""type"": [""string"", ""null""], ""maxLength"": 1,  ""description"": ""0-Masculino; 1-Feminino"" },
    ""Naturalidade"":    { ""type"": [""string"", ""null""], ""maxLength"": 35 },
    ""Nacionalidade"":   { ""type"": [""string"", ""null""], ""maxLength"": 3  },
    ""EstadoCivil"":     { ""type"": [""string"", ""null""], ""maxLength"": 3,  ""description"": ""001-Casado; 002-Solteiro; 003-Viuvo; 004-Divorciado; 005-Separado jud.; 006-Uniao de facto; 007-Outros"" },
    ""GrupoSanguineo"":  { ""type"": [""string"", ""null""], ""maxLength"": 10 },

    ""Morada"":    { ""type"": [""string"", ""null""], ""maxLength"": 80  },
    ""Localidade"": { ""type"": [""string"", ""null""], ""maxLength"": 80  },
    ""CodPostal1"": { ""type"": [""string"", ""null""], ""maxLength"": 15  },
    ""CodPostal2"": { ""type"": [""string"", ""null""], ""maxLength"": 35  },
    ""Distrito"":  { ""type"": [""string"", ""null""], ""maxLength"": 2   },
    ""Concelho"": { ""type"": [ ""string"", ""null"" ], ""maxLength"": 4 },
    ""NomeConcelho"": { ""type"": [ ""string"", ""null"" ], ""maxLength"": 35 },
    ""NomeDistrito"": { ""type"": [ ""string"", ""null"" ], ""maxLength"": 35 },
    ""Freguesia"": { ""type"": [ ""string"", ""null"" ], ""maxLength"": 2 },
    ""NomeFreguesia"": { ""type"": [ ""string"", ""null"" ], ""maxLength"": 50 },
    ""Pais"":      { ""type"": [""string"", ""null""], ""maxLength"": 2   },
    ""Telefone"":  { ""type"": [""string"", ""null""], ""maxLength"": 15  },
    ""Telemovel"": { ""type"": [""string"", ""null""], ""maxLength"": 15  },
    ""Email"":     { ""type"": [""string"", ""null""], ""maxLength"": 512 },

    ""NumeroBI"":               { ""type"": [""string"", ""null""], ""maxLength"": 20 },
    ""LocalEmissaoBI"":         { ""type"": [""string"", ""null""], ""maxLength"": 20 },
    ""DataEmissaoBI"":          { ""type"": [""string"", ""null""], ""format"": ""date-time"" },
    ""DataValidadeBI"":         { ""type"": [""string"", ""null""], ""format"": ""date-time"" },
    ""NumContribuinte"":        { ""type"": [""string"", ""null""], ""maxLength"": 20 },
    ""NumBeneficiario"":        { ""type"": [""string"", ""null""], ""maxLength"": 15 },
    ""NumeroPassaporte"":       { ""type"": [""string"", ""null""], ""maxLength"": 20 },
    ""LocalEmissaoPassaporte"": { ""type"": [""string"", ""null""], ""maxLength"": 20 },
    ""DataEmissaoPassaporte"":  { ""type"": [""string"", ""null""], ""format"": ""date-time"" },
    ""DataValidadePassaporte"": { ""type"": [""string"", ""null""], ""format"": ""date-time"" },
    ""NumeroIE"":               { ""type"": [""string"", ""null""], ""maxLength"": 20 },
    ""LocalEmissaoIE"":         { ""type"": [""string"", ""null""], ""maxLength"": 20 },
    ""DataEmissaoIE"":          { ""type"": [""string"", ""null""], ""format"": ""date-time"" },
    ""DataValidadeIE"":         { ""type"": [""string"", ""null""], ""format"": ""date-time"" },
    ""CartaoResidente"":        { ""type"": [""string"", ""null""], ""maxLength"": 50 },
    ""LocalEmCR"":              { ""type"": [""string"", ""null""], ""maxLength"": 20 },
    ""DataEmissaoCR"":          { ""type"": [""string"", ""null""], ""format"": ""date-time"" },
    ""DataValidadeCR"":         { ""type"": [""string"", ""null""], ""format"": ""date-time"" },

    ""Profissao"":          { ""type"": [""string"", ""null""], ""maxLength"": 12 },
    ""Habilitacao"":        { ""type"": [""string"", ""null""], ""maxLength"": 3, ""description"": ""101-Ensino basico 1c; 102-2c; 103-3c; 200-Secundario; 300-Bacharelato; 400-Licenciatura; 500-Mestrado; 600-Doutoramento"" },
    ""DataHabilitacao"":    { ""type"": [""string"", ""null""], ""format"": ""date-time"" },
    ""Qualificacao"":       { ""type"": [""string"", ""null""], ""maxLength"": 3, ""description"": ""001-Dirigente; 002-Quadro superior; 003-Quadro medio; 005-Alt. qualificado; 006-Semi-qualificado; 007-Nao qualificado; 008-Praticante"" },
    ""DataIniProfissao"":   { ""type"": [""string"", ""null""], ""format"": ""date-time"" },
    ""CartaConducao"":      { ""type"": [""string"", ""null""], ""maxLength"": 15 },
    ""CategoriaCartaCond"": { ""type"": [""string"", ""null""], ""maxLength"": 40 },
    ""DataValidadeCarta"":  { ""type"": [""string"", ""null""], ""format"": ""date-time"" },
    ""NotasBiograficas"":   { ""type"": [""string"", ""null""] },

    ""DataAdmissao"":    { ""type"": [""string"",  ""null""], ""format"": ""date-time"" },
    ""DataFimContrato"": { ""type"": [""string"",  ""null""], ""format"": ""date-time"" },
    ""TipoContrato"":   { ""type"": [""integer"", ""null""], ""description"": ""1-Permanente; 2-Termo certo; 3-Termo incerto; 7-Outros"" },
    ""Vencimento"":     { ""type"": [""number"",  ""null""] },
    ""VencimentoMensal"": { ""type"": [""number"", ""null""] },
    ""Moeda"":          { ""type"": [""string"",  ""null""], ""maxLength"": 3  },
    ""HorasSemanais"":  { ""type"": [""number"",  ""null""] },
    ""Departamento"":   { ""type"": [""string"",  ""null""], ""maxLength"": 10 },
    ""Categoria"":      { ""type"": [""string"",  ""null""], ""maxLength"": 10 },
    ""CargoPrincipal"": { ""type"": [""string"",  ""null""], ""maxLength"": 5  },

    ""IRS"":               { ""type"": [""string"",  ""null""], ""maxLength"": 2 },
    ""Dependentes"":       { ""type"": [""integer"", ""null""] },
    ""Deficientes"":       { ""type"": [""integer"", ""null""] },
    ""NumTitulares"":      { ""type"": [""integer"", ""null""], ""description"": ""0-Unico Titular; 1-Dois Titulares"" },
    ""DomicilioFiscal"":   { ""type"": [""integer"", ""null""], ""description"": ""0-Continente; 1-Acores; 2-Madeira; 3-Estrangeiro"" },
    ""ConjugueDeficiente"": { ""type"": [""boolean"", ""null""] },
  }
}

DOCUMENTO(S) A EXTRAIR:
- ficheiros anexados: pode ser mais do que um.",
                    Files = files
                };
            }
            else if (smartActionName.Equals("FUNC002", StringComparison.InvariantCultureIgnoreCase) && businessEntity is RhpBE100.RhpBEFuncionario funcionario2)
            {
                return new CustomSmartActionRequest()
                {
                    Contexts = new List<CustomSmartActionContext>()
                    {
                        new CustomSmartActionContext()
                        {
                            Background = "És um supervisor de equipa que dá feedback construtivo.",
                        }
                    },
                    Prompt = "Escreve uma mensagem de elogio a um funcionário por ter completado um projeto antes do prazo, incentivando-o a continuar o bom trabalho."
                };
            }
            else if (smartActionName.Equals("Artigo", StringComparison.InvariantCultureIgnoreCase) && businessEntity is BasBE100.BasBEArtigo artigo)
            {
                var data = new DataTable();
                data.Columns.AddRange(new[] { new DataColumn("Item"), new DataColumn("Description"), new DataColumn("Price") });
                data.Rows.Add("A0001", "Item 1", "40.60");
                data.Rows.Add("A0002", "Item 2", "20.33");
                data.Rows.Add("A0003", "Item 3", "55.50");
                data.Rows.Add("A0004", "Item 4", "35.20");

                return new CustomSmartActionRequest()
                {
                    Contexts = new List<CustomSmartActionContext>()
                    {
                        new CustomSmartActionContext()
                        {
                            Background = "Lista de Artigos com preços em euros.",
                            Data = data
                        }
                    },
                    Prompt = "Qual o artigo mais caro e o respetivo preço?"
                };
            }
            else if (smartActionName.Equals("Cliente", StringComparison.InvariantCultureIgnoreCase) && businessEntity is BasBE100.BasBECliente cliente)
            {
                string sql = $"SELECT Cliente, Nome, LimiteCred FROM Clientes WHERE Cliente = '{cliente.Cliente}'";
                var data = this.BSO.ConsultaDataTable(sql);

                return new CustomSmartActionRequest()
                {
                    Contexts = new List<CustomSmartActionContext>()
                    {
                        new CustomSmartActionContext()
                        {
                            Background = "Informação do Cliente com o respetivo limite de crédito.",
                            Data = data
                        }
                    },
                    Prompt = "Qual é o limite de crédito deste cliente?"
                };
            }
            else if (smartActionName.Equals("VND.002", StringComparison.InvariantCultureIgnoreCase) && businessEntity is VndBE100.VndBEDocumentoVenda documentoVenda)
            {
                return new CustomSmartActionRequest()
                {
                    Contexts = new List<CustomSmartActionContext>()
                    {
                        new CustomSmartActionContext()
                        {
                            Background = "Documento de vendas com detalhes do cliente, datas, totais e lista de artigos vendidos.",
                            Data = BuildSalesDocumentTable(businessEntity as VndBEDocumentoVenda)
                        }
                    },
                    Prompt = "Com base neste documento de vendas, qual é a análise sobre o cliente, o volume de vendas, os descontos aplicados e o valor final (TotalMerc+TotalOutros+TotalIVA-TotalDesc)?"
                };
            }

            return new CustomSmartActionRequest();
        }

        private string GetCode(string table, string searchColumn, string returnColumn, string filterValue)
        {
            string code = null;
            string strSql = $"SELECT TOP 1 {returnColumn} FROM {table} WHERE {searchColumn} COLLATE Latin1_General_CI_AI = @filterValue";

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@filterValue", filterValue)
            };

            DataTable dt = this.PSO.Registos.ConsultaDataTable(strSql, sqlParameters);

            foreach (DataRow row in dt.Rows)
            {
                code = row[returnColumn].ToString();
            }

            return code;
        }

        // ── JSON → RhpBEFuncionario ────────────────────────────────────────────

        /// <summary>
        /// Mapeia o JSON devolvido pelo AI para as propriedades escalares do RhpBEFuncionario.
        /// A chave JSON corresponde ao nome exato da propriedade; a conversão de tipo é feita
        /// via reflexão. Colecções e campos de sistema são ignorados.
        /// </summary>
        private void MapFuncionarioFromJson(RhpBE100.RhpBEFuncionario func, string json)
        {
            // Sanitiza caso o AI devolva o JSON dentro de markdown (```json ... ```)
            string sanitized = json.Trim();
            if (sanitized.StartsWith("```"))
            {
                int lineEnd = sanitized.IndexOf('\n');
                int fenceEnd = sanitized.LastIndexOf("```");
                if (lineEnd >= 0 && fenceEnd > lineEnd)
                    sanitized = sanitized.Substring(lineEnd + 1, fenceEnd - lineEnd - 1).Trim();
            }

            var j = JObject.Parse(sanitized);

            // Campos que NÃO devem ser sobrescritos a partir do documento
            var skip = new HashSet<string>(StringComparer.Ordinal)
            {
                // chave primária e campos de sistema
                "Funcionario", "EstadoBE", "EstadoBECadastro",
                "ChaveLog", "CodigoTabLog", "AbvtApl",
                "NumPeriodoProcessado", "UltimoAnoProcessado", "DataUltimoProc",
                "ServiceType", "IsDisposed", "EmModoEdicao", "PropExcluirLog",
                // colecções complexas (tratadas pelo ERP, não pelo AI)
                "FormasPagamento", "PlanoRenovFunc", "FuncAgregados",
                "FuncRendimentos", "Descontos", "Remuneracoes",
                "CentrosCusto", "PlanoFuncional",
                "AnaliticasDebito", "AnaliticasCredito", "CamposUtil",
            };

            var type = typeof(RhpBE100.RhpBEFuncionario);

            foreach (JProperty token in j.Properties())
            {
                if (skip.Contains(token.Name))           continue;
                if (token.Value.Type == JTokenType.Null) continue;
                if (token.Value.Type == JTokenType.Array
                 || token.Value.Type == JTokenType.Object) continue;

                // String vazia ou só espaços == "não encontrado" — não apaga valor existente
                if (token.Value.Type == JTokenType.String
                    && string.IsNullOrWhiteSpace(token.Value.Value<string>()))
                    continue;

                PropertyInfo pi = type.GetProperty(token.Name,
                    BindingFlags.Public | BindingFlags.Instance);

                if (pi == null || !pi.CanWrite) continue;

                try { ApplyValue(func, pi, token.Value); }
                catch { /* ignora conversões individuais que falhem */ }
            }
        }

        /// <summary>Converte o JToken para o tipo da propriedade e aplica-o.</summary>
        private void ApplyValue(object obj, PropertyInfo pi, JToken value)
        {
            Type t = pi.PropertyType;

            if (t == typeof(string))
            {
                pi.SetValue(obj, value.Value<string>());
            }
            else if (t == typeof(DateTime))
            {
                if (DateTime.TryParse(value.Value<string>(), CultureInfo.InvariantCulture,
                                      DateTimeStyles.RoundtripKind, out DateTime dt))
                    pi.SetValue(obj, dt);
            }
            else if (t == typeof(double))   { pi.SetValue(obj, value.Value<double>()); }
            else if (t == typeof(float))    { pi.SetValue(obj, value.Value<float>()); }
            else if (t == typeof(int))      { pi.SetValue(obj, value.Value<int>()); }
            else if (t == typeof(short))    { pi.SetValue(obj, value.Value<short>()); }
            else if (t == typeof(byte))     { pi.SetValue(obj, value.Value<byte>()); }
            else if (t == typeof(bool))     { pi.SetValue(obj, value.Value<bool>()); }
            else if (t.IsEnum)
            {
                // O schema devolve enums como inteiros
                pi.SetValue(obj, Enum.ToObject(t, value.Value<int>()));
            }
        }

        // ── Extracção de foto do PDF ──────────────────────────────────────────────

        /// <summary>
        /// Percorre todos os ficheiros enviados e devolve o JPEG com perfil mais próximo
        /// de uma fotografia de retrato (aspect ratio ~3:4). Os LLMs não conseguem produzir
        /// dados binários de imagem, por isso a extracção é feita directamente nos bytes do PDF.
        /// </summary>
        private byte[] ExtractPortraitFromFiles(Dictionary<string, byte[]> files)
        {
            if (files == null) return null;

            byte[] best = null;
            double bestScore = 0;

            foreach (var kvp in files)
            {
                foreach (byte[] candidate in ExtractJpegsFromBytes(kvp.Value))
                {
                    try
                    {
                        using (var ms = new MemoryStream(candidate))
                        using (var img = System.Drawing.Image.FromStream(ms, false, false))
                        {
                            int w = img.Width, h = img.Height;
                            if (w < 60 || h < 60) continue;       // demasiado pequeno

                            double ratio = (double)h / w;
                            if (ratio < 0.8 || ratio > 3.0) continue; // não é retrato

                            // Score: aspect ratio próximo de 3:4 (0.75 height/width → ratio ~1.33)
                            // e tamanho razoável para uma foto de documento
                            double ratioScore  = 1.0 - Math.Abs(ratio - 1.33) / 1.33;
                            double sizeScore   = Math.Min(1.0, (w * h) / (double)(300 * 400));
                            double score       = ratioScore * 0.65 + sizeScore * 0.35;

                            if (score > bestScore)
                            {
                                bestScore = score;
                                best = candidate;
                            }
                        }
                    }
                    catch { }
                }
            }

            return best;
        }

        /// <summary>
        /// Extrai todos os blocos JPEG encontrados nos bytes de um ficheiro (PDF ou outro)
        /// procurando os marcadores SOI (FF D8 FF) e EOI (FF D9).
        /// Funciona para PDFs com imagens JPEG embebidas sem compressão adicional.
        /// </summary>
        private List<byte[]> ExtractJpegsFromBytes(byte[] data, int minSize = 4096)
        {
            var result = new List<byte[]>();
            int pos = 0;

            while (pos < data.Length - 3)
            {
                // Procura marcador SOI: FF D8 FF
                if (data[pos] != 0xFF || data[pos + 1] != 0xD8 || data[pos + 2] != 0xFF)
                {
                    pos++;
                    continue;
                }

                int start = pos;

                // Procura marcador EOI: FF D9 — começa depois do SOI
                int end = -1;
                for (int i = start + 3; i < data.Length - 1; i++)
                {
                    if (data[i] == 0xFF && data[i + 1] == 0xD9)
                    {
                        end = i + 2;
                        break;
                    }
                }

                if (end < 0) break;

                int len = end - start;
                if (len >= minSize)
                {
                    var jpeg = new byte[len];
                    Array.Copy(data, start, jpeg, 0, len);
                    result.Add(jpeg);
                }

                pos = end;
            }

            return result;
        }

        public override void AfterExecution(string smartActionName, string result)
        {
            try
            {
                if (smartActionName.Equals("FUNC001", StringComparison.InvariantCultureIgnoreCase))
                {
                    // Fecha o loader — o AI terminou
                    _formProcessando?.CloseForm();
                    _formProcessando = null;

                    var func = base.BusinessEntity as RhpBE100.RhpBEFuncionario;

                    if (func != null)
                    {
                        MapFuncionarioFromJson(func, result);

                        string strSql = @"SELECT TOP 1 Freguesia, Concelho, Distrito 
FROM Freguesias 
WHERE Descricao COLLATE Latin1_General_CI_AI = @nomeFreguesia OR Descricao COLLATE Latin1_General_CI_AI = @localidade";

                        List<SqlParameter> sqlParameters = new List<SqlParameter>()
                        {
                            new SqlParameter("@nomeFreguesia", func.NomeFreguesia),
                            new SqlParameter("@localidade", func.Localidade)
                        };

                        DataTable dt = this.PSO.Registos.ConsultaDataTable(strSql, sqlParameters);

                        foreach (DataRow row in dt.Rows)
                        {
                            func.Freguesia = row["Freguesia"].ToString();
                            func.Concelho = row["Concelho"].ToString();
                            func.Distrito = row["Distrito"].ToString();
                        }

                        func.Nacionalidade = GetCode("Nacionalidades", "Descricao", "Nacionalidade", func.Nacionalidade);
                        func.Profissao = GetCode("Profissoes", "Descricao", "Profissao", func.Profissao);
                        func.Habilitacao = GetCode("Habilitacoes", "Descricao", "Habilitacao", func.Habilitacao);

                        // Extrai foto directamente dos bytes do PDF — os LLMs não conseguem
                        // produzir dados binários de imagem como output, por isso esta extracção
                        // é feita em C# pesquisando os marcadores JPEG embebidos no PDF.
                        try
                        {
                            byte[] fotoBytes = ExtractPortraitFromFiles(_uploadedFiles);
                            if (fotoBytes != null && func != null)
                            {
                                string fileName = "F" + Guid.NewGuid().ToString("N") + ".jpg";
                                File.WriteAllBytes(
                                    Path.Combine(this.BSO.RecursosHumanos.Params.PathFoto, fileName),
                                    fotoBytes);
                                func.Foto = fileName;
                            }
                        }
                        catch { }
                    }
                }
                else if (smartActionName.Equals("CLIENTES001", StringComparison.InvariantCultureIgnoreCase))
                {
                    var cli = base.BusinessEntity as BasBE100.BasBECliente;

                    if (cli == null)
                    {
                        cli = new BasBE100.BasBECliente();
                    }

                    cli.Nome = "Cliente atualizado pelo AI";
                }
                else if (smartActionName.Equals("VND.003", StringComparison.InvariantCultureIgnoreCase))
                {
                    string pastaDownloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    string caminhoPdf = HtmlPdfPrinter.ImprimirHtmlParaPdf(result, pastaDownloads, @"C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe");
                }
                else
                {
                    this.PSO.MensagensDialogos.MostraAviso("Resultado da execução da Smart Action.", StdBE100.StdBETipos.IconId.PRI_Informativo, result);
                }
            }
            catch (Exception ex)
            {
                this.PSO.MensagensDialogos.MostraErro("Ocorreu um erro:", StdBE100.StdBETipos.IconId.PRI_Critico, ex.Message);
            }
            finally
            {
                // Garante que o loader fecha e a memória é libertada mesmo em caso de erro
                _formProcessando?.CloseForm();
                _formProcessando = null;
                _uploadedFiles = null;
            }
        }

        private DataTable BuildSalesDocumentTable(VndBE100.VndBEDocumentoVenda documentoVenda)
        {
            var dt = new DataTable();
            dt.Columns.AddRange(new[]
            {
                new DataColumn("TipoDoc"),    new DataColumn("Serie"),     new DataColumn("NumDoc"),
                new DataColumn("Entidade"),   new DataColumn("DataDoc"),   new DataColumn("TotalMerc"),
                new DataColumn("TotalDesc"),  new DataColumn("TotalIva"),  new DataColumn("TotalOutros"),
                new DataColumn("TotalDocumento"), new DataColumn("NumLinha"), new DataColumn("Artigo"),
                new DataColumn("Descricao"),  new DataColumn("Quantidade"), new DataColumn("PrecUnit"),
                new DataColumn("Desconto1"),  new DataColumn("Desconto2"), new DataColumn("Desconto3")
            });

            int numLinha = 0;
            foreach (VndBE100.VndBELinhaDocumentoVenda linha in documentoVenda.Linhas)
            {
                numLinha++;
                dt.Rows.Add(
                    documentoVenda.Tipodoc,
                    documentoVenda.Serie,
                    documentoVenda.NumDoc.ToString(CultureInfo.InvariantCulture),
                    documentoVenda.Entidade,
                    documentoVenda.DataDoc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    documentoVenda.TotalMerc.ToString("0.00", CultureInfo.InvariantCulture),
                    documentoVenda.TotalDesc.ToString("0.00", CultureInfo.InvariantCulture),
                    documentoVenda.TotalIva.ToString("0.00", CultureInfo.InvariantCulture),
                    documentoVenda.TotalOutros.ToString("0.00", CultureInfo.InvariantCulture),
                    documentoVenda.TotalDocumento.ToString("0.00", CultureInfo.InvariantCulture),
                    numLinha.ToString(CultureInfo.InvariantCulture),
                    linha.Artigo,
                    linha.Descricao,
                    linha.Quantidade.ToString("0.00", CultureInfo.InvariantCulture),
                    linha.PrecUnit.ToString("0.00", CultureInfo.InvariantCulture),
                    linha.Desconto1.ToString("0.00", CultureInfo.InvariantCulture),
                    linha.Desconto2.ToString("0.00", CultureInfo.InvariantCulture),
                    linha.Desconto3.ToString("0.00", CultureInfo.InvariantCulture)
                );
            }

            return dt;
        }
    }
}
