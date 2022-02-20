using BasBE100;
using Primavera.Extensibility.Attributes;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Sales.Editors;
using Primavera.Logistics.Extensibility.Static;

namespace Primavera.Logistics.Extensibility.Sales
{

    public class SalesEditor : EditorVendas
    {
        /// <summary>
        /// This academic sample provides a manual mechanism to do the interface refresh.
        /// In cases where the interface only need to be updated in certain circustances,
        /// than we can control manually the refresh using this attribute and then Refresh and Commit to screen.
        /// Improve the performance of the editor.
        /// </summary>
        [ContextSyncManagement(ContextSyncManagement.Manual)]
        public override void ArtigoIdentificado(string Artigo, int NumLinha, ref bool Cancel, ExtensibilityEventArgs e)
        {
            base.ArtigoIdentificado(Artigo, NumLinha, ref Cancel, e);

            if (Artigo == "A0001")
            {
                //PreencheBEO
                this.RefreshContext();

                this.DocumentoVenda.Linhas.GetEdita(NumLinha).Descricao = "Linha Alterada";

                //MostraBEO
                this.CommitContext();
            }
        }

        public override void AntesDeGravar(ref bool Cancel, ExtensibilityEventArgs e)
        {
            base.AntesDeGravar(ref Cancel, e);

            if (!GlobalFunctions.ValidateZipCode(DocumentoVenda.CodigoPostal))
            {
                PSO.Dialogos.MostraAviso("The zip code is invalid. A valid format must be like this '9999-999'.");
                Cancel = true;
            }
        }
    }
}