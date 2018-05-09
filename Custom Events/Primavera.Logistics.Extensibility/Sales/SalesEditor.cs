using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Sales.Editors;
using Primavera.Logistics.Extensibility.Static;

namespace Primavera.Logistics.Extensibility.Sales
{
    public class SalesEditor : EditorVendas
    {
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