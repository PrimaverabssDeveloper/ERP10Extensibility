using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Platform.Services;
using StdPlatBS100;
using System;
using System.Diagnostics;
using System.Drawing;

namespace Primavera.CustomRibbon
{
    public class PrimaveraRibbon : Plataforma
    {
        #region Private Variables

        private StdBSPRibbon RibbonEvents;

        #endregion

        #region Override

        public override void DepoisDeCriarMenus(ExtensibilityEventArgs e)
        {
            base.DepoisDeCriarMenus(e);

            RegisterAddin();
        }

        #endregion

        #region  Private Events

        private void RibbonEvents_Executa(string Id, string Comando)
        {
            try
            {
                // Trace.
                this.PSO.Diagnosticos.Trace("The user has clicked the extensibility button.");

                // Trace to file.
                this.PSO.Diagnosticos.TraceFicheiro(@"C:\erp.log", "The user has clicked the extensibility button.");

                //You must change the application path.
                Process.Start(@"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\devenv.exe");
            }
            catch (System.Exception ex)
            {
               PSO.Dialogos.MostraAviso("The file don't exist.",StdBSTipos.IconId.PRI_Informativo,ex.Message);
            }
        }

        #endregion

        #region Register the new tab inside PRIMAVERA Ribbon.

        public void RegisterAddin()
        {
            // Register the Ribbon button.
            RibbonEvents = this.PSO.Ribbon;
            RibbonEvents.Executa += RibbonEvents_Executa;

            // Register the add-in.
            CriateTab();
            CreateGroup();
            CreateGroupButton32(RibbonConstants.CIDBOTAO1, "Visual Studio", Resource.VS2017_256x256);
        }

        #endregion

        #region Private methods

        private void CriateTab()
        {
           this.PSO.Ribbon.CriaRibbonTab("PRIMAVERA", RibbonConstants.cIDTAB, 10);
        }

        private void CreateGroup()
        {
            this.PSO.Ribbon.CriaRibbonGroup(RibbonConstants.cIDTAB, "Extensibility", RibbonConstants.CIDGROUP);
        }

        private void CreateGroupButton32(string buttonId, string buttonDescription, Image buttonImage )
        {

            this.PSO.Ribbon.CriaRibbonButton(RibbonConstants.cIDTAB, RibbonConstants.CIDGROUP, buttonId, buttonDescription,true, buttonImage);
        }

        #endregion
    }
}