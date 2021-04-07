namespace SUGIMPL_OME.CrossCompany
{
    partial class frmPosicaoGlobal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPosicaoGlobal));
            this.ctlEntity = new PRISDK100.FiltroEntidades();
            this.grdMainGrid = new PRISDK100.PriGrelha();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAtualizar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnFechar = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdMainGrid)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctlEntity
            // 
            this.ctlEntity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctlEntity.AplContextoAudit = "";
            this.ctlEntity.CCorrenteEstendida = true;
            this.ctlEntity.ContextoAudit = "";
            this.ctlEntity.EntidadesAssociadas = true;
            this.ctlEntity.F4Modal = false;
            this.ctlEntity.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.ctlEntity.Location = new System.Drawing.Point(12, 28);
            this.ctlEntity.Modulo = "";
            this.ctlEntity.MostraEntidadesAsocciadas = true;
            this.ctlEntity.MultiSelecao = false;
            this.ctlEntity.Name = "ctlEntity";
            this.ctlEntity.PermiteSemTipoEntidade = false;
            this.ctlEntity.Size = new System.Drawing.Size(815, 53);
            this.ctlEntity.TabIndex = 2;
            this.ctlEntity.TipoContexto = PRISDK100.clsSDKTypes.enumContexto.Exploracao;
            this.ctlEntity.TipoEntidadeCombo = "";
            this.ctlEntity.TiposEntidade = null;
            this.ctlEntity.ValorRestricao = "";
            // 
            // grdMainGrid
            // 
            this.grdMainGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdMainGrid.BackColor = System.Drawing.Color.White;
            this.grdMainGrid.BandaMenuContexto = "";
            this.grdMainGrid.BotaoConfigurarActiveBar = true;
            this.grdMainGrid.BotaoProcurarActiveBar = false;
            this.grdMainGrid.CaminhoTemplateImpressao = "";
            this.grdMainGrid.Cols = null;
            this.grdMainGrid.ColsFrozen = -1;
            this.grdMainGrid.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdMainGrid.Location = new System.Drawing.Point(12, 87);
            this.grdMainGrid.Name = "grdMainGrid";
            this.grdMainGrid.NumeroMaxRegistosSemPag = 150000;
            this.grdMainGrid.NumeroRegistos = 0;
            this.grdMainGrid.NumLinhasCabecalho = 1;
            this.grdMainGrid.OrientacaoMapa = PRISDK100.clsSDKTypes.OrientacaoImpressao.oiDefault;
            this.grdMainGrid.ParentFormModal = false;
            this.grdMainGrid.PermiteActiveBar = false;
            this.grdMainGrid.PermiteActualizar = true;
            this.grdMainGrid.PermiteAgrupamentosUser = true;
            this.grdMainGrid.PermiteConfigurarDetalhes = false;
            this.grdMainGrid.PermiteContextoVazia = false;
            this.grdMainGrid.PermiteDataFill = true;
            this.grdMainGrid.PermiteDetalhes = true;
            this.grdMainGrid.PermiteEdicao = false;
            this.grdMainGrid.PermiteFiltros = true;
            this.grdMainGrid.PermiteGrafico = true;
            this.grdMainGrid.PermiteGrandeTotal = true;
            this.grdMainGrid.PermiteOrdenacao = true;
            this.grdMainGrid.PermitePaginacao = false;
            this.grdMainGrid.PermiteScrollBars = true;
            this.grdMainGrid.PermiteStatusBar = true;
            this.grdMainGrid.PermiteVistas = true;
            this.grdMainGrid.PosicionaColunaSeguinte = true;
            this.grdMainGrid.Size = new System.Drawing.Size(815, 354);
            this.grdMainGrid.TabIndex = 4;
            this.grdMainGrid.TituloGrelha = "Posição Global no Grupo";
            this.grdMainGrid.TituloMapa = "";
            this.grdMainGrid.TypeNameLinha = "";
            this.grdMainGrid.TypeNameLinhas = "";
            this.grdMainGrid.ActualizaDados += new PRISDK100.PriGrelha.ActualizaDadosHandler(this.grdMainGrid_ActualizaDados);
            this.grdMainGrid.DataFill += new PRISDK100.PriGrelha.DataFillHandler(this.grdMainGrid_DataFill);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAtualizar,
            this.toolStripSeparator1,
            this.btnFechar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(839, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAtualizar.Image = ((System.Drawing.Image)(resources.GetObject("btnAtualizar.Image")));
            this.btnAtualizar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(73, 22);
            this.btnAtualizar.Text = "Atualizar";
            this.btnAtualizar.ToolTipText = "Atualizar";
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnFechar
            // 
            this.btnFechar.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFechar.Image = ((System.Drawing.Image)(resources.GetObject("btnFechar.Image")));
            this.btnFechar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(62, 22);
            this.btnFechar.Text = "Fechar";
            this.btnFechar.ToolTipText = "Fechar";
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // frmPosicaoGlobal
            // 
            this.components = new System.ComponentModel.Container();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 453);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.grdMainGrid);
            this.Controls.Add(this.ctlEntity);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPosicaoGlobal";
            this.Text = "Posição Global (Grupo)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPosicaoGlobal_FormClosing);
            this.Load += new System.EventHandler(this.frmPosicaoGlobal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdMainGrid)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private PRISDK100.FiltroEntidades ctlEntity;
        private PRISDK100.PriGrelha grdMainGrid;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAtualizar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnFechar;
    }
}