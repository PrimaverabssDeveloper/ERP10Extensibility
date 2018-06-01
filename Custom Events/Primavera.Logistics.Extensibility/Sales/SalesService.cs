using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Sales.Services;
using Primavera.Logistics.Extensibility.Static;
using System;
using System.Text;
using System.Threading.Tasks;
using VndBE100;

namespace Primavera.Logistics.Extensibility.Sales
{
    public class SalesService : VndBSVendas
    {
        public override void DepoisDeGravar(VndBEDocumentoVenda clsDocumentoVenda,
            ref string strAvisos,
            ref string IdDocLiqRet,
            ref string IdDocLiqRetGar,
            ExtensibilityEventArgs e)
        {
            try
            {
                const string outputPdf = @"C:\temp\invoice.pdf";
                const string emailAdress = @" -- INSERT A VALID EMAIL WHERE -- ";

                PrintPdfAndEmailDocument(clsDocumentoVenda, outputPdf, emailAdress);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot send the email to the customer. \n" + ex.Message);
            }
            finally
            {
                base.DepoisDeGravar(clsDocumentoVenda, ref strAvisos, ref IdDocLiqRet, ref IdDocLiqRetGar, e);
            }
        }

        /// <summary>
        ///     This is an auxiliary method because it's invoker "VndBSVendas.DepoisDeGravar" has parameters by reference
        ///     and async methods doesn't support parameters by reference.
        /// </summary>
        private async void PrintPdfAndEmailDocument(VndBEDocumentoVenda clsDocumentoVenda,
            string outputPdf,
            string emailAdress)
        {
            //  Print asyncronously in a new thread, because Crystal Reports uses UI and since
            //  we're in the context of the ERP Engine we can't handle Crystal Reports UI.
            await Task.Run(() => GlobalFunctions.PrintInvoice(PSO, BSO, outputPdf, clsDocumentoVenda.Tipodoc, clsDocumentoVenda.Serie, clsDocumentoVenda.NumDoc))
                .ContinueWith(taskResult =>
                {
                    var strAssunto = new StringBuilder();

                    strAssunto.Append("A new invoice was created.");
                    strAssunto.Append(clsDocumentoVenda.Tipodoc);
                    strAssunto.Append(clsDocumentoVenda.Serie);
                    strAssunto.Append(clsDocumentoVenda.NumDoc);

                    PSO.Mail.Inicializa();
                    PSO.Mail.EnviaMailEx(emailAdress, null, null, strAssunto.ToString(), null, outputPdf, false);
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}