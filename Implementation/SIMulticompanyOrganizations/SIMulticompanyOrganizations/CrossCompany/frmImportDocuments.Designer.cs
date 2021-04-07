namespace SUGIMPL_OME.CrossCompany
{
    partial class frmImportDocuments
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImportDocuments));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.grdDocuments = new PRISDK100.PriGrelha();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDocuments)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnImport,
            this.toolStripSeparator1,
            this.btnClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(853, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnImport
            // 
            this.btnImport.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(73, 22);
            this.btnImport.Text = "Importar";
            this.btnImport.ToolTipText = "Importar";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClose
            // 
            this.btnClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(62, 22);
            this.btnClose.Text = "Fechar";
            this.btnClose.ToolTipText = "Fechar";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grdDocuments
            // 
            this.grdDocuments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdDocuments.BackColor = System.Drawing.Color.White;
            this.grdDocuments.BandaMenuContexto = "";
            this.grdDocuments.BotaoConfigurarActiveBar = true;
            this.grdDocuments.BotaoProcurarActiveBar = false;
            this.grdDocuments.CaminhoTemplateImpressao = "";
            this.grdDocuments.Cols = null;
            this.grdDocuments.ColsFrozen = -1;
            this.grdDocuments.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdDocuments.Location = new System.Drawing.Point(12, 28);
            this.grdDocuments.Name = "grdDocuments";
            this.grdDocuments.NumeroMaxRegistosSemPag = 150000;
            this.grdDocuments.NumeroRegistos = 0;
            this.grdDocuments.NumLinhasCabecalho = 1;
            this.grdDocuments.OrientacaoMapa = PRISDK100.clsSDKTypes.OrientacaoImpressao.oiDefault;
            this.grdDocuments.ParentFormModal = false;
            this.grdDocuments.PermiteActiveBar = false;
            this.grdDocuments.PermiteActualizar = true;
            this.grdDocuments.PermiteAgrupamentosUser = true;
            this.grdDocuments.PermiteConfigurarDetalhes = false;
            this.grdDocuments.PermiteContextoVazia = false;
            this.grdDocuments.PermiteDataFill = true;
            this.grdDocuments.PermiteDetalhes = true;
            this.grdDocuments.PermiteEdicao = false;
            this.grdDocuments.PermiteFiltros = true;
            this.grdDocuments.PermiteGrafico = true;
            this.grdDocuments.PermiteGrandeTotal = true;
            this.grdDocuments.PermiteOrdenacao = true;
            this.grdDocuments.PermitePaginacao = false;
            this.grdDocuments.PermiteScrollBars = true;
            this.grdDocuments.PermiteStatusBar = true;
            this.grdDocuments.PermiteVistas = true;
            this.grdDocuments.PosicionaColunaSeguinte = true;
            this.grdDocuments.Size = new System.Drawing.Size(829, 318);
            this.grdDocuments.TabIndex = 7;
            this.grdDocuments.TituloGrelha = "Posição Global no Grupo";
            this.grdDocuments.TituloMapa = "";
            this.grdDocuments.TypeNameLinha = "";
            this.grdDocuments.TypeNameLinhas = "";
            this.grdDocuments.ActualizaDados += new PRISDK100.PriGrelha.ActualizaDadosHandler(this.grdDocuments_ActualizaDados);
            this.grdDocuments.Load += new System.EventHandler(this.grdDocuments_Load);
            // 
            // frmImportDocuments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 358);
            this.Controls.Add(this.grdDocuments);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmImportDocuments";
            this.Text = "Importar Documentos";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDocuments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnClose;
        private PRISDK100.PriGrelha grdDocuments;
    }
}