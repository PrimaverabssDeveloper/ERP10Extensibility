using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.Base.Editors;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using StdPlatBS100;

namespace SUGIMPL_OME.ERP_Base
{
    public class UIFichaFornecedores : FichaFornecedores
    {
        public override void TeclaPressionada(int KeyCode, int Shift, ExtensibilityEventArgs e)
        {
            base.TeclaPressionada(KeyCode, Shift, e);

            if (KeyCode == 80 && Shift == 3)
            {
                CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);
                mngr.PR_GlobalPosition("F", Fornecedor.Fornecedor);
                KeyCode = 0;
            }
        }

        public override void DepoisDeGravar(string Fornecedor, ExtensibilityEventArgs e)
        {
            base.DepoisDeGravar(Fornecedor, e);

            if (Convert.ToBoolean(BSO.Base.Fornecedores.DaValorAtributo(Fornecedor, "CDU_EntidadeGrupo")))
            {
                CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);

                StdBSDialogoEspera oDialog = PSO.Dialogos.MostraDialogoEspera(
                    "A atualizar o fornecedor nas outras empresas do grupo.",
                    0,
                    StdBSTipos.IconId.PRI_Informativo,
                    StdBSTipos.AnimId.PRI_AviCalculos,
                    StdBSTipos.FormPos.PRI_Centrado);

                List<String> updatedCompanies = mngr.UpdateEntity("F",Fornecedor);

                oDialog.Termina();

                if (updatedCompanies.Count > 0)
                {
                    PSO.Dialogos.MostraMensagem(
                        StdBSTipos.TipoMsg.PRI_Detalhe,
                        "Foram atualizadas entidades em outras empresas do grupo.",
                        StdBSTipos.IconId.PRI_Informativo,
                        String.Format("Empresas afetadas: {0}", String.Join(",", updatedCompanies)),
                        bActivaDetalhe: true);
                }
            }
        }

        public override void DepoisDeAnular(string Fornecedor, ExtensibilityEventArgs e)
        {
            base.DepoisDeAnular(Fornecedor, e);

            if (Convert.ToBoolean(BSO.Base.Fornecedores.DaValorAtributo(Fornecedor, "CDU_EntidadeGrupo")))
            {
                CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);

                StdBSDialogoEspera oDialog = PSO.Dialogos.MostraDialogoEspera(
                    "A remover o fornecedor das outras empresas do grupo.",
                    0,
                    StdBSTipos.IconId.PRI_Informativo,
                    StdBSTipos.AnimId.PRI_AviCalculos,
                    StdBSTipos.FormPos.PRI_Centrado);

                List<String> updatedCompanies = mngr.RemoveEntity(Fornecedor, "F");

                oDialog.Termina();

                if (updatedCompanies.Count > 0)
                {
                    PSO.Dialogos.MostraMensagem(
                        StdBSTipos.TipoMsg.PRI_Detalhe,
                        "Foram removidas entidades em outras empresas do grupo.",
                        StdBSTipos.IconId.PRI_Informativo,
                        String.Format("Empresas afetadas: {0}", String.Join(",", updatedCompanies)),
                        bActivaDetalhe: true);
                }
            }
        }
    }
}
