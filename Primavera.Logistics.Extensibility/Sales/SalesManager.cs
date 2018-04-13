using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Sales.Services;
using System;
using System.Text;
using VndBE100;

namespace Primavera.Logistics.Extensibility.Sales
{
    public class SalesManager : VndBSVendas
    {
        public override void DepoisDeGravar(VndBEDocumentoVenda clsDocumentoVenda, ref string strAvisos, ref string IdDocLiqRet, ref string IdDocLiqRetGar, ExtensibilityEventArgs e)
        {
            string outputPDF = @"C:\temp\invoice.pdf";
            string emailAdress = @"sergio.sereno@primaverabss.com";

            try
            {
                GlobalFunctions.PrintInvoice(PSO, BSO, outputPDF , clsDocumentoVenda.Tipodoc, clsDocumentoVenda.Serie, clsDocumentoVenda.NumDoc);
                
                StringBuilder strAssunto = new StringBuilder();

                strAssunto.Append("A new invoice was created.");

                strAssunto.Append(clsDocumentoVenda.Tipodoc);
                strAssunto.Append(clsDocumentoVenda.Serie);
                strAssunto.Append(clsDocumentoVenda.NumDoc);

                PSO.Mail.Inicializa();

                PSO.Mail.EnviaMailEx(emailAdress, null, null, strAssunto.ToString(), null, outputPDF, false);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot send the email to the customer. \n" + ex.Message.ToString());
            }
            finally
            {
                base.DepoisDeGravar(clsDocumentoVenda, ref strAvisos, ref IdDocLiqRet, ref IdDocLiqRetGar, e);
            }
        }

    }
}