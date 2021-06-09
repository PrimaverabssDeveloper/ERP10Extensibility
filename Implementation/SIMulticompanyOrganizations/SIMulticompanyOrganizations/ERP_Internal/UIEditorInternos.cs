using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Internal.Editors;

namespace SUGIMPL_OME.ERP_Internal
{
    public class UIEditorInternos : EditorInternos
    {
        public override void TeclaPressionada(int KeyCode, int Shift, ExtensibilityEventArgs e)
        {
            base.TeclaPressionada(KeyCode, Shift, e);

            if (KeyCode == 80 && Shift == 3)
            {
                CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);

                if (DocumentoInterno.TipoEntidade == "F")
                {
                    mngr.PR_GlobalPosition("F", DocumentoInterno.Entidade);
                }
                else if (DocumentoInterno.TipoEntidade == "C")
                {
                    mngr.PR_GlobalPosition("C", DocumentoInterno.Entidade);
                }
                KeyCode = 0;
            }

        }
    }
}
