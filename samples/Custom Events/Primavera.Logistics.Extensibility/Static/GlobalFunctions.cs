using System;
using System.Text;
using System.Text.RegularExpressions;
using ErpBS100;
using StdPlatBS100;

namespace Primavera.Logistics.Extensibility.Static
{
    internal static class GlobalFunctions
    {
        /// <summary>
        ///     Print to PDF the invoice.
        /// </summary>
        /// <param name="pso">pso</param>
        /// <param name="bso">bso</param>
        /// <param name="reportPath">Where the will be printed.</param>
        /// <param name="DocType">The document type.</param>
        /// <param name="DocSeries">The document series.</param>
        /// <param name="DocNumber">The document number.</param>
        public static void PrintInvoice(StdBSInterfPub pso,
            ErpBS bso,
            string reportPath,
            string DocType,
            string DocSeries,
            int DocNumber)
        {
            var reportTemplate = "GCPVLS01";

            try
            {
                var strSelFormula = $"{{CabecDoc.TipoDoc}}=\'{DocType}\' and {{CabecDoc.Serie}} = \'{DocSeries}\' AND {{CabecDoc.NumDoc}}={Convert.ToString(DocNumber)}";

                pso.Mapas.Inicializar("VND");

                var strFormula = new StringBuilder();
                strFormula.Append($"StringVar Nome:='{bso.Contexto.IDNome}';");
                strFormula.Append($"StringVar Morada:='{bso.Contexto.IDMorada}';");
                strFormula.Append($"StringVar Localidade:='{bso.Contexto.IDLocalidade}';");
                strFormula.Append($"StringVar CodPostal:='{bso.Contexto.IDCodPostal} {bso.Contexto.IDCodPostalLocal}';");
                strFormula.Append($"StringVar Telefone:='{bso.Contexto.IDTelefone}';");
                strFormula.Append($"StringVar Fax:='{bso.Contexto.IDFax}';");
                strFormula.Append($"StringVar Contribuinte:='{bso.Contexto.IFNIF}';");
                strFormula.Append($"StringVar CapitalSocial:='{bso.Contexto.ICCapitalSocial}';");
                strFormula.Append($"StringVar Conservatoria:='{bso.Contexto.ICConservatoria}';");
                strFormula.Append($"StringVar Matricula:='{bso.Contexto.ICMatricula}';");
                strFormula.Append($"StringVar MoedaCapitalSocial:='{bso.Contexto.ICMoedaCapSocial}';");

                pso.Mapas.SetFormula("DadosEmpresa", strFormula.ToString());

                var strParametros = new StringBuilder();
                strParametros.Append("NumberVar TipoDesc;");
                strParametros.Append("NumberVar DecQde;");
                strParametros.Append("NumberVar DecPrecUnit;");
                strParametros.Append("StringVar MotivoIsencao;");
                strParametros.Append("BooleanVar UltimaPag;");
                strParametros.Append("StringVar PRI_TextoCertificacao;");
                strParametros.Append("TipoDesc:= 0;");
                strParametros.Append("DecQde:=3;");
                strParametros.Append($"DecPrecUnit:={pso.FuncoesGlobais.DaCasasDecimais("Moedas", "DecArredonda")};");
                strParametros.Append("UltimaPag:=False;");
                strParametros.Append($"PRI_TextoCertificacao:='{bso.Vendas.Documentos.DevolveTextoAssinaturaDoc(DocType, DocSeries, DocNumber, "000")}';");

                pso.Mapas.SetFormula("InicializaParametros", strParametros.ToString());
                pso.Mapas.Destino = 0;
                pso.Mapas.SetFileProp(StdBSTipos.CRPEExportFormat.efPdf, reportPath);

                pso.Mapas.MostraErros = true;
                pso.Mapas.ImprimeListagem(reportTemplate, "Invoice", "P", 1, "N", strSelFormula, 0, false, true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while printing the document. \n" + ex.Message);
            }
        }

        /// <summary>
        ///     Check the zip code.
        /// </summary>
        public static bool ValidateZipCode(string zipCode)
        {
            var re = new Regex(@"^\d{4}-\d{3}?$", RegexOptions.Multiline);
            var theMatches = re.Matches(zipCode);

            return theMatches.Count > 0;
        }
    }
}