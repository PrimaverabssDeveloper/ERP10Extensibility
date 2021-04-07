using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Platform.Services;
using StdPlatBE100;
using StdPlatBS100;

namespace SUGIMPL_OME.ERP_Plataforma
{
    public class UIPlataforma : Plataforma
    {
        const string cBARRA_GERAL = "barraGER";
        const string cXMERIBBON_GROUP = "XMEGroup";
        const string cXMERIBBON_BTNIMPORT = "XMEBTN_ImportDocuments";


        /// <summary>
        /// Processes before the company opening.
        /// </summary>
        /// <param name="Cancel"></param>
        /// <param name="e"></param>
        public override void AntesDeAbrirEmpresa(ref bool Cancel, ExtensibilityEventArgs e)
        {
            base.AntesDeAbrirEmpresa(ref Cancel, e);

            CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);
            mngr.BeforeOpenCompany(ref Cancel);
        }


        /// <summary>
        /// Processes after the company opening.
        /// </summary>
        /// <param name="e"></param>
        public override void DepoisDeAbrirEmpresa(ExtensibilityEventArgs e)
        {
            base.DepoisDeAbrirEmpresa(e);

            // Check pending documents to import
            CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);
            mngr.CheckPendingDocuments();

            // Add the application menus to the Ribbon (Tab GERAL)
            StdBSPRibbon RibbonEvents = PSO.Ribbon;
            RibbonEvents.Executa += RibbonEvents_Executa;
        }


        /// <summary>
        /// Create/update the extension menu structure
        /// </summary>
        /// <param name="e"></param>
        public override void DepoisDeCriarMenus(ExtensibilityEventArgs e)
        {
            base.DepoisDeCriarMenus(e);

            // Add the GROUP to the general tab
            PSO.Ribbon.CriaRibbonGroup(cBARRA_GERAL, "MULTI EMPRESA", cXMERIBBON_GROUP);
            // Create the Button.
            PSO.Ribbon.CriaRibbonButton(cBARRA_GERAL, cXMERIBBON_GROUP, cXMERIBBON_BTNIMPORT, "Importar Documentos", true, Properties.Resources.IMG_ImportDocuments);
        }


        /// <summary>
        /// Run the ribbon commands
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Comando"></param>
        private void RibbonEvents_Executa(string Id, string Comando)
        {
            try
            {
                switch (Id)
                {
                    case cXMERIBBON_BTNIMPORT:
                        CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);
                        mngr.ImportDocuments();
                        break;
                }
            }
            catch (System.Exception ex)
            {
                PSO.Dialogos.MostraAviso("Fail to execute the command.", StdBSTipos.IconId.PRI_Informativo, ex.Message);
            }
        }
    }
}
