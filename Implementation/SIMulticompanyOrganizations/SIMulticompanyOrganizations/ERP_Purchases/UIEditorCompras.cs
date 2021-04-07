using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Purchases.Editors;

namespace SUGIMPL_OME.ERP_Purchases
{
    public class UIEditorCompras : EditorCompras
    {
        public override void TeclaPressionada(int KeyCode, int Shift, ExtensibilityEventArgs e)
        {
            base.TeclaPressionada(KeyCode, Shift, e);


            if (KeyCode == 80 && Shift == 3)
            {
                CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);
                mngr.PR_GlobalPosition("F", DocumentoCompra.Entidade);
                KeyCode = 0;
            }
        }
    }
}
