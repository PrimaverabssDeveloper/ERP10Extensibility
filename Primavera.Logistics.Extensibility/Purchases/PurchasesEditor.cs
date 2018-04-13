using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Purchases.Editors;

namespace Primavera.Logistics.Extensibility.Purchases
{
    public class PurchasesEditor : EditorCompras
    {
        public override void AntesDeGravar(ref bool Cancel, ExtensibilityEventArgs e)
        {
            if (!GlobalFunctions.ValidateZipCode(this.DocumentoCompra.CodigoPostal))
            {
                PSO.Dialogos.MostraAviso("The zip code is invalid. A valid format must be like this '9999-999'.");
                Cancel = true;
            }
            base.AntesDeGravar(ref Cancel, e);
        }
    }
}