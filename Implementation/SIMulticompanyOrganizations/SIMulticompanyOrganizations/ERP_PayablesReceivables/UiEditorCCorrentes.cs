using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasBE100;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.PayablesReceivables.Editors;


namespace SUGIMPL_OME.CCorrentes
{
    public class UiEditorCCorrentes : EditorCCorrentes
    {
        public override void TeclaPressionada(int KeyCode, int Shift, BasBETiposGcp.TE_DocCCorrentes TDocumento, ExtensibilityEventArgs e)
        {
            base.TeclaPressionada(KeyCode, Shift, TDocumento, e);
            
            if (KeyCode == 80 && Shift == 3)
            {
                CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);
                if (this.DocumentoLiquidacao.TipoEntidade == "F")
                    mngr.PR_GlobalPosition("F", DocumentoLiquidacao.Entidade);
                else
                    mngr.PR_GlobalPosition("C", DocumentoLiquidacao.Entidade);
                KeyCode = 0;
            }
        }
    }
}
