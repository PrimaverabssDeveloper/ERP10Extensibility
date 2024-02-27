
using Primavera.TemplateProcurement.Extensibility.Ext.UI;
using Primavera.TemplateProcurement.Extensibility.Resource;

namespace Primavera.TemplateProcurement.Extensibility
{
    public class CustomCode : Primavera.Extensibility.CustomCode.CustomCode
    {
        public void frmParametrosOMNIA() 
        {
            var frm = new frmParametrosOMNIA(this.BSO, this.PSO); 

            frm.ShowDialog();
        }

    }
}
