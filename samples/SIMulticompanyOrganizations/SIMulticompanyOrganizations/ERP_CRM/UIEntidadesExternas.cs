using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.ContactsOpportunities.Editors;
using StdPlatBS100;

namespace SUGIMPL_OME.ERP_CRM
{
    public class UIEntidadesExternas : FichaEntidadesExternas
    {
        public override void DepoisDeGravar(string strEntidade, ExtensibilityEventArgs e)
        {
            base.DepoisDeGravar(strEntidade, e);

            if (Convert.ToBoolean(BSO.CRM.EntidadesExternas.DaValorAtributo(strEntidade, "CDU_EntidadeGrupo")))
            {
                CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);

                StdBSDialogoEspera oDialog = PSO.Dialogos.MostraDialogoEspera(
                    "A atualizar a entidade nas outras empresas do grupo.",
                    0,
                    StdBSTipos.IconId.PRI_Informativo,
                    StdBSTipos.AnimId.PRI_AviCalculos,
                    StdBSTipos.FormPos.PRI_Centrado);

                List<String> updatedCompanies = mngr.UpdateEntity("E", strEntidade);

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

        public override void DepoisDeAnular(string strEntidade, ExtensibilityEventArgs e)
        {
            base.DepoisDeAnular(strEntidade, e);

            if (Convert.ToBoolean(BSO.CRM.EntidadesExternas.DaValorAtributo(strEntidade, "CDU_EntidadeGrupo")))
            {
                CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);

                StdBSDialogoEspera oDialog = PSO.Dialogos.MostraDialogoEspera(
                    "A remover a entidade das outras empresas do grupo.",
                    0,
                    StdBSTipos.IconId.PRI_Informativo,
                    StdBSTipos.AnimId.PRI_AviCalculos,
                    StdBSTipos.FormPos.PRI_Centrado);

                List<String> updatedCompanies = mngr.RemoveEntity(strEntidade, "E");

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
