using Primavera.Extensibility.BusinessEntities.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using VndBE100;

namespace CustomSmartAction
{
    public class SmartAction : Primavera.Extensibility.CustomSmartAction.CustomSmartAction
    {
        public override CustomSmartActionRequest BeforeExecution<T>(string smartActionName, T businessEntity)
        {
            if (smartActionName.Equals("Artigo", StringComparison.InvariantCultureIgnoreCase) && businessEntity is BasBE100.BasBEArtigo artigo)
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

        public override void AfterExecution(string smartActionName, string result)
        {
            try
            {
                if (smartActionName.Equals("VND.003", StringComparison.InvariantCultureIgnoreCase))
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
