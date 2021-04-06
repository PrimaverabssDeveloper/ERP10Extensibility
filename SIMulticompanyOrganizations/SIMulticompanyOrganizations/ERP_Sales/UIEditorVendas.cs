using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Sales.Editors;

namespace SUGIMPL_OME.ERP_Sales
{
    public class UIEditorVendas : EditorVendas
    {

        public override void TeclaPressionada(int KeyCode, int Shift, ExtensibilityEventArgs e)
        {
            base.TeclaPressionada(KeyCode, Shift, e);

            if (KeyCode == 80 && Shift == 3)
            {
                CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);
                mngr.PR_GlobalPosition("C", DocumentoVenda.Entidade);
                KeyCode = 0;
            }
        }


        public override void ClienteIdentificado(string Cliente, ref bool Cancel, ExtensibilityEventArgs e)
        {
            base.ClienteIdentificado(Cliente, ref Cancel, e);

            CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);
            List<String> companiesCreditExceeded = mngr.PR_CreditLimitExceeded(Cliente);

            if (companiesCreditExceeded.Count > 0)
            {
                if (BSO.Contexto.ObjUtilizador.Administrador)
                {
                    PSO.Dialogos.MostraMensagem(
                        StdPlatBS100.StdBSTipos.TipoMsg.PRI_Detalhe,
                        "O cliente tem o crédito bloqueado ou excedido no grupo.",
                        StdPlatBS100.StdBSTipos.IconId.PRI_Informativo,
                        String.Format("Empresas com situação bloqueada: {0}. O utilizador {1} pode prosseguir por ter perfil de administrador.",
                            string.Join(",", companiesCreditExceeded),
                            BSO.Contexto.ObjUtilizador.Nome));
                }
                else
                {
                    Cancel = true;
                    if (PSO.Dialogos.MostraMensagem(
                        StdPlatBS100.StdBSTipos.TipoMsg.PRI_SimNao,
                        String.Format("O cliente tem o crédito bloqueado/excedido nas seguintes empresas: {0}. Activar PIN de desbloqueio?", string.Join(",", companiesCreditExceeded)),
                        StdPlatBS100.StdBSTipos.IconId.PRI_Questiona) == StdPlatBS100.StdBSTipos.ResultMsg.PRI_Sim)
                    {
                        Cancel = false;
                        if (!mngr.PLT_CheckPassword(Properties.Settings.Default.UnlockPIN))
                        {
                            Cancel = true;
                            PSO.Dialogos.MostraMensagem(StdPlatBS100.StdBSTipos.TipoMsg.PRI_SimplesOk, "PIN inválido!", StdPlatBS100.StdBSTipos.IconId.PRI_Critico);
                        }
                    }
                }
            }
        }
    }
}
