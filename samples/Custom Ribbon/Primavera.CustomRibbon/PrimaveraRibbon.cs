using System.Diagnostics;
using System.Drawing;
using Primavera.CustomRibbon.Properties;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Platform.Services;
using StdPlatBS100;

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

                switch (Id)
                {
                    case RibbonConstants.cTAB1_GROUP1_IDBUTTON1:
                        //You must change the application path.
                        Process.Start(@"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\devenv.exe");
                        break;
                }
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
            CreateGroupButton32(RibbonConstants.cTAB1_GROUP1_IDBUTTON1, "Visual Studio 2017", Resources.VS2017_256x256);
            CreateGroupButton32(RibbonConstants.cTAB2_GROUP1_IDBUTTON1, "Visual Studio 2019", Resources.VS2017_256x256);
        }

        #endregion

        #region Private methods

        private void CriateTab()
        {
           this.PSO.Ribbon.CriaRibbonTab("TAB 1", RibbonConstants.cIDTAB1, 10);
           this.PSO.Ribbon.CriaRibbonTab("TAB 2", RibbonConstants.cIDTAB2, 10);
        }

        private void CreateGroup()
        {
            this.PSO.Ribbon.CriaRibbonGroup(RibbonConstants.cIDTAB1, "Extensibility 1", RibbonConstants.cIDTAB1_GROUP1);
            this.PSO.Ribbon.CriaRibbonGroup(RibbonConstants.cIDTAB2, "Extensibility 2", RibbonConstants.cIDTAB2_GROUP1);
        }

        private void CreateGroupButton32(string buttonId, string buttonDescription, Image buttonImage )
        {
            this.PSO.Ribbon.CriaRibbonButton(RibbonConstants.cIDTAB1, RibbonConstants.cIDTAB1_GROUP1, buttonId, buttonDescription, true, buttonImage);
            this.PSO.Ribbon.CriaRibbonButton(RibbonConstants.cIDTAB2, RibbonConstants.cIDTAB2_GROUP1, buttonId, buttonDescription, true, buttonImage);
        }

        #endregion
    }
}