using ErpBS100;
using StdPlatBS100;
using System;
using System.Text;
using System.Text.RegularExpressions;
using static StdPlatBS100.StdBSTipos;

namespace Primavera.Logistics.Extensibility
{
    internal static class GlobalFunctions
    {
        /// <summary>
        /// Check the zip code.
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public static bool ValidateZipCode(string zipCode)
        {
            Regex re = new Regex(@"^\d{4}-\d{3}?$", RegexOptions.Multiline);

            MatchCollection theMatches = re.Matches(zipCode);

            return theMatches.Count > 0 ? true : false;
        }

        /// <summary>
        /// Print to PDF the invoice.
        /// </summary>
        /// <param name="pso">pso</param>
        /// <param name="bso">bso</param>
        /// <param name="reportPath">Where the will be printed.</param>
        /// <param name="DocType">The document type.</param>
        /// <param name="DocSeries">The document series.</param>
        /// <param name="DocNumber">The document number.</param>
        public static void PrintInvoice(StdBSInterfPub pso, ErpBS bso, string reportPath, string DocType, string DocSeries, int DocNumber)
        {
            StringBuilder strFormula_ = new StringBuilder();
            StringBuilder strParametros_ = new StringBuilder();
            StringBuilder strSelFormula_ = new StringBuilder();

            string reportTemplate = "GCPVLS01";

            try
            {
                strSelFormula_ = new StringBuilder("{CabecDoc.TipoDoc}='" + DocType + "' and {CabecDoc.Serie} = '" + DocSeries + "' AND {CabecDoc.NumDoc}=" + Convert.ToString(DocNumber));

                pso.Mapas.Inicializar("VND");
                strFormula_.Append("StringVar Nome:='" + bso.Contexto.IDNome + "';");
                strFormula_.Append("StringVar Morada:='" + bso.Contexto.IDMorada + "';");
                strFormula_.Append("StringVar Localidade:='" + bso.Contexto.IDLocalidade + "';");
                strFormula_.Append("StringVar CodPostal:='" + bso.Contexto.IDCodPostal + " " + bso.Contexto.IDCodPostalLocal + "';");
                strFormula_.Append("StringVar Telefone:='" + bso.Contexto.IDTelefone + "';");
                strFormula_.Append("StringVar Fax:='" + bso.Contexto.IDFax + "';");
                strFormula_.Append("StringVar Contribuinte:='" + bso.Contexto.IFNIF + "';");
                strFormula_.Append("StringVar CapitalSocial:='" + bso.Contexto.ICCapitalSocial + "';");
                strFormula_.Append("StringVar Conservatoria:='" + bso.Contexto.ICConservatoria + "';");
                strFormula_.Append("StringVar Matricula:='" + bso.Contexto.ICMatricula + "';");
                strFormula_.Append("StringVar MoedaCapitalSocial:='" + bso.Contexto.ICMoedaCapSocial + "';");

                pso.Mapas.SetFormula("DadosEmpresa", strFormula_.ToString());

                strParametros_.Append("NumberVar TipoDesc;");
                strParametros_.Append("NumberVar DecQde;");
                strParametros_.Append("NumberVar DecPrecUnit;");
                strParametros_.Append("StringVar MotivoIsencao;");
                strParametros_.Append("BooleanVar UltimaPag;");
                strParametros_.Append("StringVar PRI_TextoCertificacao;");
                strParametros_.Append("TipoDesc:= 0;");
                strParametros_.Append("DecQde:=3;");
                strParametros_.Append("DecPrecUnit:=" + pso.FuncoesGlobais.DaCasasDecimais("Moedas", "DecArredonda") + ";");
                strParametros_.Append("UltimaPag:=False;");
                strParametros_.Append("PRI_TextoCertificacao:='" + bso.Vendas.Documentos.DevolveTextoAssinaturaDoc(DocType, DocSeries, DocNumber, "000") + "';");

                pso.Mapas.SetFormula("InicializaParametros", strParametros_.ToString());
                pso.Mapas.Destino = 0;
                pso.Mapas.SetFileProp(CRPEExportFormat.efPdf, reportPath);

                pso.Mapas.ImprimeListagem(reportTemplate, "Invoice", "P", 1, "N", strSelFormula_.ToString(), 0, false, true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while printing the document. \n" + ex.Message.ToString());
            }
        }
    }
}