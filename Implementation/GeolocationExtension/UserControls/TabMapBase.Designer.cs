using Primavera.Extensibility.Integration.Context;
using System;

namespace Primavera.Platform.Geolocation
{
    public partial class TabMapBase
    {
        //// Used to "trick" the designer to load the correct resource files
        //internal class CustomComponentResourceManager : System.ComponentModel.ComponentResourceManager
        //{
        //    public CustomComponentResourceManager(Type type, string resourceName)
        //       : base(type)
        //    {
        //        this.BaseNameField = resourceName;
        //    }
        //}

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        ///// <summary>
        ///// Clean up any resources being used.
        ///// </summary>
        ///// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabMapBase));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.hideContainerRight = new DevExpress.XtraBars.Docking.AutoHideContainer();
            this.markersPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.btnDeleteUserMarkerr = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddMapMarker = new DevExpress.XtraEditors.SimpleButton();
            this.btnRemoveMapMarker = new DevExpress.XtraEditors.SimpleButton();
            this.listRouteMarkers = new DevExpress.XtraEditors.ListBoxControl();
            this.listUserMarkers = new DevExpress.XtraEditors.ListBoxControl();
            this.directionsPanel = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer2 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barSaveRoute = new DevExpress.XtraBars.BarButtonItem();
            this.barDrawRoute = new DevExpress.XtraBars.BarButtonItem();
            this.barClearRoute = new DevExpress.XtraBars.BarButtonItem();
            this.barRouteLabel = new DevExpress.XtraBars.BarStaticItem();
            this.barChooseRoute = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.barDeleteRoute = new DevExpress.XtraBars.BarButtonItem();
            this.barRouteOptions = new DevExpress.XtraBars.BarSubItem();
            this.barAvoidHighways = new DevExpress.XtraBars.BarCheckItem();
            this.barWalkingMode = new DevExpress.XtraBars.BarCheckItem();
            this.barTools = new DevExpress.XtraBars.Bar();
            this.barMapLabel = new DevExpress.XtraBars.BarStaticItem();
            this.barMapServer = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.barSearchLabel = new DevExpress.XtraBars.BarStaticItem();
            this.barSearch = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemSearchControl1 = new DevExpress.XtraEditors.Repository.RepositoryItemSearchControl();
            this.barSaveMarker = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.clientName = new DevExpress.XtraBars.BarStaticItem();
            this.btnPopupRemoveMarker = new DevExpress.XtraBars.BarButtonItem();
            this.btnPopupSaveMarker = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem4 = new DevExpress.XtraBars.BarStaticItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.controlContainer1 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.mapControl = new GMap.NET.WindowsForms.GMapControl();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.hideContainerRight.SuspendLayout();
            this.markersPanel.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listRouteMarkers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listUserMarkers)).BeginInit();
            this.directionsPanel.SuspendLayout();
            this.controlContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockManager1
            // 
            this.dockManager1.AutoHideContainers.AddRange(new DevExpress.XtraBars.Docking.AutoHideContainer[] {
            this.hideContainerRight});
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // hideContainerRight
            // 
            this.hideContainerRight.BackColor = System.Drawing.SystemColors.Control;
            this.hideContainerRight.Controls.Add(this.markersPanel);
            this.hideContainerRight.Controls.Add(this.directionsPanel);
            this.hideContainerRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.hideContainerRight.Location = new System.Drawing.Point(688, 49);
            this.hideContainerRight.Name = "hideContainerRight";
            this.hideContainerRight.Size = new System.Drawing.Size(21, 406);
            // 
            // markersPanel
            // 
            this.markersPanel.AutoScroll = true;
            this.markersPanel.Controls.Add(this.dockPanel1_Container);
            this.markersPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.markersPanel.ID = new System.Guid("a4feee07-c376-4339-9b10-59aabd64afbd");
            this.markersPanel.Location = new System.Drawing.Point(0, 0);
            this.markersPanel.Name = "markersPanel";
            this.markersPanel.Options.ShowCloseButton = false;
            this.markersPanel.OriginalSize = new System.Drawing.Size(268, 200);
            this.markersPanel.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.markersPanel.SavedIndex = 1;
            this.markersPanel.Size = new System.Drawing.Size(268, 406);
            this.markersPanel.Text = "Marcadores";
            this.markersPanel.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.btnDeleteUserMarkerr);
            this.dockPanel1_Container.Controls.Add(this.btnAddMapMarker);
            this.dockPanel1_Container.Controls.Add(this.btnRemoveMapMarker);
            this.dockPanel1_Container.Controls.Add(this.listRouteMarkers);
            this.dockPanel1_Container.Controls.Add(this.listUserMarkers);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 26);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(261, 377);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // btnDeleteUserMarkerr
            // 
            this.btnDeleteUserMarkerr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteUserMarkerr.Location = new System.Drawing.Point(159, 350);
            this.btnDeleteUserMarkerr.Name = "btnDeleteUserMarkerr";
            this.btnDeleteUserMarkerr.Size = new System.Drawing.Size(99, 23);
            this.btnDeleteUserMarkerr.TabIndex = 24;
            this.btnDeleteUserMarkerr.Text = "Apagar Marcador";
            this.btnDeleteUserMarkerr.Click += new System.EventHandler(this.BtnDeleteUserMarker_Click);
            // 
            // btnAddMapMarker
            // 
            this.btnAddMapMarker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddMapMarker.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddMapMarker.ImageOptions.Image")));
            this.btnAddMapMarker.Location = new System.Drawing.Point(208, 89);
            this.btnAddMapMarker.Name = "btnAddMapMarker";
            this.btnAddMapMarker.Size = new System.Drawing.Size(22, 22);
            this.btnAddMapMarker.TabIndex = 23;
            this.btnAddMapMarker.Click += new System.EventHandler(this.BtnAddMapMarker_Click);
            // 
            // btnRemoveMapMarker
            // 
            this.btnRemoveMapMarker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveMapMarker.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveMapMarker.ImageOptions.Image")));
            this.btnRemoveMapMarker.Location = new System.Drawing.Point(236, 89);
            this.btnRemoveMapMarker.Name = "btnRemoveMapMarker";
            this.btnRemoveMapMarker.Size = new System.Drawing.Size(22, 22);
            this.btnRemoveMapMarker.TabIndex = 22;
            this.btnRemoveMapMarker.Click += new System.EventHandler(this.BtnRemoveMapMarker_Click);
            // 
            // listRouteMarkers
            // 
            this.listRouteMarkers.AllowDrop = true;
            this.listRouteMarkers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listRouteMarkers.Location = new System.Drawing.Point(3, 3);
            this.listRouteMarkers.Name = "listRouteMarkers";
            this.listRouteMarkers.Size = new System.Drawing.Size(255, 80);
            this.listRouteMarkers.TabIndex = 21;
            this.listRouteMarkers.ToolTip = "Lista de Marcadores da Rota";
            this.listRouteMarkers.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListRouteMarkers_DragDrop);
            this.listRouteMarkers.DragOver += new System.Windows.Forms.DragEventHandler(this.ListRouteMarkers_DragOver);
            this.listRouteMarkers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListRouteMarkers_MouseDown);
            // 
            // listUserMarkers
            // 
            this.listUserMarkers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listUserMarkers.Location = new System.Drawing.Point(3, 117);
            this.listUserMarkers.Name = "listUserMarkers";
            this.listUserMarkers.Size = new System.Drawing.Size(255, 227);
            this.listUserMarkers.TabIndex = 16;
            this.listUserMarkers.ToolTip = "Lista de Marcadores do Utilizador";
            this.listUserMarkers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListUserMarkers_MouseDoubleClick);
            // 
            // directionsPanel
            // 
            this.directionsPanel.Controls.Add(this.controlContainer2);
            this.directionsPanel.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.directionsPanel.ID = new System.Guid("a44c74af-0311-47d3-8cf6-581eb4b42235");
            this.directionsPanel.Location = new System.Drawing.Point(0, 0);
            this.directionsPanel.Name = "directionsPanel";
            this.directionsPanel.Options.ShowCloseButton = false;
            this.directionsPanel.OriginalSize = new System.Drawing.Size(316, 200);
            this.directionsPanel.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.directionsPanel.SavedIndex = 0;
            this.directionsPanel.Size = new System.Drawing.Size(316, 403);
            this.directionsPanel.Text = "Direções";
            this.directionsPanel.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // controlContainer2
            // 
            this.controlContainer2.Controls.Add(this.webBrowser1);
            this.controlContainer2.Location = new System.Drawing.Point(4, 26);
            this.controlContainer2.Name = "controlContainer2";
            this.controlContainer2.Size = new System.Drawing.Size(309, 374);
            this.controlContainer2.TabIndex = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(309, 374);
            this.webBrowser1.TabIndex = 18;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.barTools});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.clientName,
            this.btnPopupRemoveMarker,
            this.btnPopupSaveMarker,
            this.barSearch,
            this.barDrawRoute,
            this.barSaveRoute,
            this.barRouteOptions,
            this.barStaticItem4,
            this.barRouteLabel,
            this.barSaveMarker,
            this.barClearRoute,
            this.barAvoidHighways,
            this.barWalkingMode,
            this.barMapLabel,
            this.barSearchLabel,
            this.barMapServer,
            this.barChooseRoute,
            this.barDeleteRoute});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 41;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSearchControl1,
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2,
            this.repositoryItemLookUpEdit1,
            this.repositoryItemLookUpEdit2});
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSaveRoute, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barDrawRoute, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barClearRoute, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barRouteLabel),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Width, this.barChooseRoute, "", false, true, true, 190),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barDeleteRoute, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barRouteOptions, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar2.OptionsBar.DisableClose = true;
            this.bar2.OptionsBar.DrawBorder = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barSaveRoute
            // 
            this.barSaveRoute.Caption = "Gravar";
            this.barSaveRoute.Hint = "Gravar a rota atual";
            this.barSaveRoute.Id = 13;
            this.barSaveRoute.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barSaveRoute.ImageOptions.Image")));
            this.barSaveRoute.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G));
            this.barSaveRoute.Name = "barSaveRoute";
            this.barSaveRoute.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarSaveRoute_ItemClick);
            // 
            // barDrawRoute
            // 
            this.barDrawRoute.Caption = "Traçar";
            this.barDrawRoute.Hint = "Desenha a rota";
            this.barDrawRoute.Id = 12;
            this.barDrawRoute.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barDrawRoute.ImageOptions.Image")));
            this.barDrawRoute.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T));
            this.barDrawRoute.Name = "barDrawRoute";
            this.barDrawRoute.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarDrawRoute_ItemClick);
            // 
            // barClearRoute
            // 
            this.barClearRoute.Caption = "Limpar";
            this.barClearRoute.Hint = "Limpar mapa";
            this.barClearRoute.Id = 26;
            this.barClearRoute.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barClearRoute.ImageOptions.Image")));
            this.barClearRoute.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L));
            this.barClearRoute.Name = "barClearRoute";
            this.barClearRoute.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarClearRoute_ItemClick);
            // 
            // barRouteLabel
            // 
            this.barRouteLabel.Caption = "Rota:";
            this.barRouteLabel.Id = 23;
            this.barRouteLabel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barRouteLabel.ImageOptions.Image")));
            this.barRouteLabel.Name = "barRouteLabel";
            this.barRouteLabel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // barChooseRoute
            // 
            this.barChooseRoute.Edit = this.repositoryItemLookUpEdit2;
            this.barChooseRoute.Id = 35;
            this.barChooseRoute.Name = "barChooseRoute";
            this.barChooseRoute.EditValueChanged += new System.EventHandler(this.BarChooseRoute_EditValueChanged);
            // 
            // repositoryItemLookUpEdit2
            // 
            this.repositoryItemLookUpEdit2.AutoHeight = false;
            this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            this.repositoryItemLookUpEdit2.NullText = "Escolha a Rota";
            this.repositoryItemLookUpEdit2.ShowFooter = false;
            this.repositoryItemLookUpEdit2.ShowHeader = false;
            this.repositoryItemLookUpEdit2.UseAdvancedMode = DevExpress.Utils.DefaultBoolean.False;
            // 
            // barDeleteRoute
            // 
            this.barDeleteRoute.Caption = "Apagar";
            this.barDeleteRoute.Hint = "Apagar a rota atual";
            this.barDeleteRoute.Id = 36;
            this.barDeleteRoute.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barDeleteRoute.ImageOptions.Image")));
            this.barDeleteRoute.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A));
            this.barDeleteRoute.Name = "barDeleteRoute";
            this.barDeleteRoute.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarDeleteRoute_ItemClick);
            // 
            // barRouteOptions
            // 
            this.barRouteOptions.AllowDrawArrow = DevExpress.Utils.DefaultBoolean.True;
            this.barRouteOptions.Caption = "Opções";
            this.barRouteOptions.Hint = "Definir opções da rota";
            this.barRouteOptions.Id = 17;
            this.barRouteOptions.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barRouteOptions.ImageOptions.Image")));
            this.barRouteOptions.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barAvoidHighways),
            new DevExpress.XtraBars.LinkPersistInfo(this.barWalkingMode)});
            this.barRouteOptions.Name = "barRouteOptions";
            this.barRouteOptions.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barRouteOptions.ShowNavigationHeader = DevExpress.Utils.DefaultBoolean.False;
            // 
            // barAvoidHighways
            // 
            this.barAvoidHighways.Caption = "Evitar AutoEstrada";
            this.barAvoidHighways.CloseSubMenuOnClickMode = DevExpress.Utils.DefaultBoolean.False;
            this.barAvoidHighways.Id = 27;
            this.barAvoidHighways.Name = "barAvoidHighways";
            // 
            // barWalkingMode
            // 
            this.barWalkingMode.Caption = "Percurso Pedestre";
            this.barWalkingMode.CloseSubMenuOnClickMode = DevExpress.Utils.DefaultBoolean.False;
            this.barWalkingMode.Id = 28;
            this.barWalkingMode.Name = "barWalkingMode";
            // 
            // barTools
            // 
            this.barTools.BarName = "Tools";
            this.barTools.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.barTools.DockCol = 0;
            this.barTools.DockRow = 1;
            this.barTools.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barTools.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barMapLabel),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Width, this.barMapServer, "", false, true, true, 160),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSearchLabel),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Width, this.barSearch, "", false, true, true, 259),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSaveMarker, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.barTools.OptionsBar.DisableClose = true;
            this.barTools.OptionsBar.DrawBorder = false;
            this.barTools.OptionsBar.DrawDragBorder = false;
            this.barTools.Text = "Tools";
            // 
            // barMapLabel
            // 
            this.barMapLabel.Caption = "Mapa:";
            this.barMapLabel.Id = 32;
            this.barMapLabel.Name = "barMapLabel";
            this.barMapLabel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // barMapServer
            // 
            this.barMapServer.Edit = this.repositoryItemLookUpEdit1;
            this.barMapServer.Id = 34;
            this.barMapServer.Name = "barMapServer";
            this.barMapServer.EditValueChanged += new System.EventHandler(this.BarMapServer_EditValueChanged);
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.DropDownRows = 3;
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.NullText = "Escolha o mapa";
            this.repositoryItemLookUpEdit1.ShowFooter = false;
            this.repositoryItemLookUpEdit1.ShowHeader = false;
            this.repositoryItemLookUpEdit1.UseAdvancedMode = DevExpress.Utils.DefaultBoolean.False;
            // 
            // barSearchLabel
            // 
            this.barSearchLabel.Caption = "Pesquisa:";
            this.barSearchLabel.Id = 33;
            this.barSearchLabel.Name = "barSearchLabel";
            // 
            // barSearch
            // 
            this.barSearch.Edit = this.repositoryItemSearchControl1;
            this.barSearch.Id = 7;
            this.barSearch.Name = "barSearch";
            toolTipItem1.Text = "Possibilidades de pesquisa: Nomes, Morada ou Coordenadas ([-]000,000000; [-]000,0" +
    "00000)";
            superToolTip1.Items.Add(toolTipItem1);
            this.barSearch.SuperTip = superToolTip1;
            this.barSearch.ShownEditor += new DevExpress.XtraBars.ItemClickEventHandler(this.BarSearch_ShownEditor);
            // 
            // repositoryItemSearchControl1
            // 
            this.repositoryItemSearchControl1.AutoHeight = false;
            this.repositoryItemSearchControl1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Repository.ClearButton(),
            new DevExpress.XtraEditors.Repository.SearchButton()});
            this.repositoryItemSearchControl1.Name = "repositoryItemSearchControl1";
            // 
            // barSaveMarker
            // 
            this.barSaveMarker.Caption = "Guardar Marcador";
            this.barSaveMarker.Hint = "Guardar o marcador atual";
            this.barSaveMarker.Id = 24;
            this.barSaveMarker.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barSaveMarker.ImageOptions.Image")));
            this.barSaveMarker.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M));
            this.barSaveMarker.Name = "barSaveMarker";
            this.barSaveMarker.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarSaveMarker_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(709, 49);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 455);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(709, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 49);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 406);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(709, 49);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 406);
            // 
            // clientName
            // 
            this.clientName.Id = 1;
            this.clientName.Name = "clientName";
            // 
            // btnPopupRemoveMarker
            // 
            this.btnPopupRemoveMarker.Caption = "Remover";
            this.btnPopupRemoveMarker.Id = 4;
            this.btnPopupRemoveMarker.Name = "btnPopupRemoveMarker";
            this.btnPopupRemoveMarker.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnPopupRemoveMarker_ItemClick);
            // 
            // btnPopupSaveMarker
            // 
            this.btnPopupSaveMarker.Caption = "Guardar";
            this.btnPopupSaveMarker.Id = 5;
            this.btnPopupSaveMarker.Name = "btnPopupSaveMarker";
            this.btnPopupSaveMarker.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnPopupSaveMarker_ItemClick);
            // 
            // barStaticItem4
            // 
            this.barStaticItem4.Caption = "Pesquisa";
            this.barStaticItem4.Id = 22;
            this.barStaticItem4.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barStaticItem4.ImageOptions.SvgImage")));
            this.barStaticItem4.Name = "barStaticItem4";
            this.barStaticItem4.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 1;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barMapLabel),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Width, this.barMapServer, "", false, true, true, 155),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSearchLabel),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem4),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Width, this.barSearch, "", false, true, true, 260),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSaveMarker, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AutoPopupMode = DevExpress.XtraBars.BarAutoPopupMode.None;
            this.bar1.OptionsBar.DisableClose = true;
            this.bar1.OptionsBar.DisableCustomization = true;
            this.bar1.OptionsBar.DrawBorder = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.Text = "Tools";
            // 
            // controlContainer1
            // 
            this.controlContainer1.Location = new System.Drawing.Point(0, 0);
            this.controlContainer1.Name = "controlContainer1";
            this.controlContainer1.Size = new System.Drawing.Size(655, 564);
            this.controlContainer1.TabIndex = 0;
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPopupSaveMarker),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPopupRemoveMarker)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.mapControl);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 49);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(688, 406);
            this.panelControl1.TabIndex = 6;
            // 
            // mapControl
            // 
            this.mapControl.Bearing = 0F;
            this.mapControl.CanDragMap = true;
            this.mapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapControl.EmptyTileColor = System.Drawing.Color.Navy;
            this.mapControl.GrayScaleMode = false;
            this.mapControl.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.mapControl.LevelsKeepInMemory = 5;
            this.mapControl.Location = new System.Drawing.Point(2, 2);
            this.mapControl.MarkersEnabled = true;
            this.mapControl.MaxZoom = 2;
            this.mapControl.MinZoom = 2;
            this.mapControl.MouseWheelZoomEnabled = true;
            this.mapControl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.mapControl.Name = "mapControl";
            this.mapControl.NegativeMode = false;
            this.mapControl.PolygonsEnabled = true;
            this.mapControl.RetryLoadTile = 0;
            this.mapControl.RoutesEnabled = true;
            this.mapControl.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.mapControl.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.mapControl.ShowTileGridLines = false;
            this.mapControl.Size = new System.Drawing.Size(684, 402);
            this.mapControl.TabIndex = 2;
            this.mapControl.Zoom = 0D;
            this.mapControl.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.MapControl_OnMarkerClick);
            // 
            // TabMapBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.hideContainerRight);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "TabMapBase";
            this.Size = new System.Drawing.Size(709, 455);
            this.Load += new System.EventHandler(this.TabMapBase_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.hideContainerRight.ResumeLayout(false);
            this.markersPanel.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listRouteMarkers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listUserMarkers)).EndInit();
            this.directionsPanel.ResumeLayout(false);
            this.controlContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel markersPanel;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraEditors.ListBoxControl listUserMarkers;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer1;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarStaticItem clientName;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarButtonItem btnPopupRemoveMarker;
        private DevExpress.XtraBars.BarButtonItem btnPopupSaveMarker;
        private DevExpress.XtraBars.BarSubItem barRouteOptions;
        private DevExpress.XtraBars.BarButtonItem barDrawRoute;
        private DevExpress.XtraBars.BarButtonItem barSaveRoute;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarEditItem barSearch;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchControl repositoryItemSearchControl1;
        private DevExpress.XtraBars.BarStaticItem barRouteLabel;
        private DevExpress.XtraBars.BarStaticItem barStaticItem4;
        private DevExpress.XtraBars.BarButtonItem barSaveMarker;
        private DevExpress.XtraBars.BarButtonItem barClearRoute;
        private DevExpress.XtraBars.Docking.DockPanel directionsPanel;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer2;
        private DevExpress.XtraBars.BarCheckItem barAvoidHighways;
        private DevExpress.XtraBars.BarCheckItem barWalkingMode;
        private DevExpress.XtraEditors.ListBoxControl listRouteMarkers;
        private DevExpress.XtraBars.BarStaticItem barMapLabel;
        private DevExpress.XtraBars.BarStaticItem barSearchLabel;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraBars.BarEditItem barMapServer;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraBars.BarEditItem barChooseRoute;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private DevExpress.XtraBars.BarButtonItem barDeleteRoute;
        private DevExpress.XtraEditors.SimpleButton btnRemoveMapMarker;
        private DevExpress.XtraEditors.SimpleButton btnAddMapMarker;
        private DevExpress.XtraBars.Bar barTools;
        private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerRight;
        private DevExpress.XtraEditors.SimpleButton btnDeleteUserMarkerr;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private GMap.NET.WindowsForms.GMapControl mapControl;
    }
}

