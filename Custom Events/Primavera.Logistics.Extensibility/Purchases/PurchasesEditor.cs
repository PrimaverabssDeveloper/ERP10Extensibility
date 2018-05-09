using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Purchases.Editors;
using Primavera.Logistics.Extensibility.Static;


namespace Primavera.Logistics.Extensibility.Purchases
{
    public class PurchasesEditor : EditorCompras
    {
        public override void AntesDeGravar(ref bool Cancel, ExtensibilityEventArgs e)
        {
            base.AntesDeGravar(ref Cancel, e);

            if (!GlobalFunctions.ValidateZipCode(DocumentoCompra.CodigoPostal))
            {
                PSO.Dialogos.MostraAviso("The zip code is invalid. A valid format must be like this '9999-999'.");
                Cancel = true;
            }
        }
    }
}