using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.Base.Editors;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Threading;

namespace SUGIMPL_OME.Base
{
    public class UiFichaArtigos : FichaArtigos
    {

        public override void AntesDeGravar(ref bool Cancel, ExtensibilityEventArgs e)
        {
            base.AntesDeGravar(ref Cancel, e);

        }

        public override void DepoisDeGravar(string Artigo, ExtensibilityEventArgs e)
        {
            base.DepoisDeGravar(Artigo, e);

            if (Convert.ToBoolean(BSO.Base.Artigos.DaValorAtributo(Artigo, "CDU_ArtigoGrupo")))
            {

                CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);

                StdPlatBS100.StdBSDialogoEspera dlgUPD = PSO.Dialogos.MostraDialogoEspera(
                    "A actualizar o artigo nas outras empresas do grupo.", 
                    0, 
                    StdPlatBS100.StdBSTipos.IconId.PRI_Informativo, 
                    StdPlatBS100.StdBSTipos.AnimId.PRI_AviCalculos, 
                    StdPlatBS100.StdBSTipos.FormPos.PRI_Centrado);

                List<String> updatedCompanies = mngr.UpdateItem_GroupCompanies(Artigo);
                dlgUPD.Termina();

                if (updatedCompanies.Count > 0)
                {
                    PSO.Dialogos.MostraMensagem(
                        eTipo: StdPlatBS100.StdBSTipos.TipoMsg.PRI_Detalhe,
                        sMensagem: "Foram actualizados artigos nas empresas do grupo.",
                        eIcon: StdPlatBS100.StdBSTipos.IconId.PRI_Informativo,
                        sDetalhe: string.Join(",", updatedCompanies),
                        bActivaDetalhe: true);
                }
            }
        }
    }
}
