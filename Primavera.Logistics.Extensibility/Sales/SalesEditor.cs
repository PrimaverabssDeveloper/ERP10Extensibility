using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Sales.Editors;

namespace Primavera.Logistics.Extensibility.Purchases
{
    public class SalesEditor: EditorVendas
    {
        public override void AntesDeGravar(ref bool Cancel, ExtensibilityEventArgs e)
        {
            if (!GlobalFunctions.ValidateZipCode(this.DocumentoVenda.CodigoPostal))
            {
                PSO.Dialogos.MostraAviso("The zip code is invalid. A valid format must be like this '9999-999'.");
                Cancel = true;
            }
            base.AntesDeGravar(ref Cancel, e);
        }
    }
}