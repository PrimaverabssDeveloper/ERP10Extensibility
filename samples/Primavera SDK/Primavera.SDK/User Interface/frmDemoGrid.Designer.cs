namespace PrimaveraSDK
{
    partial class frmDemoGrid
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
            StdBase100.StdBETipoEntidade stdBETipoEntidade1 = new StdBase100.StdBETipoEntidade();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonLoadGrid = new System.Windows.Forms.Button();
            this.f41 = new PRISDK100.F4();
            this.tiposEntidade1 = new PRISDK100.TiposEntidade();
            this.panel2 = new System.Windows.Forms.Panel();
            this.priGrelha1 = new PRISDK100.PriGrelha();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.priGrelha1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonLoadGrid);
            this.panel1.Controls.Add(this.f41);
            this.panel1.Controls.Add(this.tiposEntidade1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(759, 44);
            this.panel1.TabIndex = 5;
            // 
            // buttonLoadGrid
            // 
            this.buttonLoadGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadGrid.Location = new System.Drawing.Point(665, 11);
            this.buttonLoadGrid.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLoadGrid.Name = "buttonLoadGrid";
            this.buttonLoadGrid.Size = new System.Drawing.Size(86, 23);
            this.buttonLoadGrid.TabIndex = 10;
            this.buttonLoadGrid.Text = "Load Grid";
            this.buttonLoadGrid.UseVisualStyleBackColor = true;
            this.buttonLoadGrid.Click += new System.EventHandler(this.buttonLoadGrid_Click);
            // 
            // f41
            // 
            this.f41.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.f41.Audit = "mnuTabClientes";
            this.f41.AutoComplete = false;
            this.f41.BackColorLocked = System.Drawing.SystemColors.ButtonFace;
            this.f41.CampoChave = "Cliente";
            this.f41.CampoChaveFisica = "";
            this.f41.CampoDescricao = "Nome";
            this.f41.Caption = "Cliente:";
            this.f41.CarregarValoresEdicao = false;
            this.f41.Categoria = PRISDK100.clsSDKTypes.EnumCategoria.Clientes;
            this.f41.ChaveFisica = "";
            this.f41.ChaveNumerica = false;
            this.f41.F4Modal = true;
            this.f41.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.f41.IDCategoria = "Clientes";
            this.f41.Location = new System.Drawing.Point(286, 12);
            this.f41.MaxLengthDescricao = 0;
            this.f41.MaxLengthF4 = 50;
            this.f41.MinimumSize = new System.Drawing.Size(37, 21);
            this.f41.Modulo = "BAS";
            this.f41.MostraDescricao = true;
            this.f41.MostraLink = true;
            this.f41.Name = "f41";
            this.f41.PainesInformacaoRelacionada = false;
            this.f41.PainesInformacaoRelacionadaMultiplasChaves = false;
            this.f41.PermiteDrillDown = true;
            this.f41.PermiteEnabledLink = true;
            this.f41.PodeEditarDescricao = false;
            this.f41.ResourceID = 669;
            this.f41.ResourcePersonalizada = false;
            this.f41.Restricao = "";
            this.f41.SelectionFormula = "";
            this.f41.Size = new System.Drawing.Size(374, 22);
            this.f41.TabIndex = 9;
            this.f41.TextoDescricao = "";
            this.f41.WidthEspacamento = 60;
            this.f41.WidthF4 = 1590;
            this.f41.WidthLink = 1575;
            // 
            // tiposEntidade1
            // 
            this.tiposEntidade1.Activo = true;
            this.tiposEntidade1.Caption = "Tipo de Entidade:";
            stdBETipoEntidade1.AcedeOperacao = false;
            stdBETipoEntidade1.Audit = "";
            stdBETipoEntidade1.Campo_Chave = "";
            stdBETipoEntidade1.Categoria = "";
            stdBETipoEntidade1.Descricao = "";
            stdBETipoEntidade1.DescricaoPlural = "";
            stdBETipoEntidade1.Modulo = "";
            stdBETipoEntidade1.Natureza = "";
            stdBETipoEntidade1.Tabela = "";
            stdBETipoEntidade1.TipoEntidade = "";
            stdBETipoEntidade1.Visivel = false;
            this.tiposEntidade1.Contexto = stdBETipoEntidade1;
            this.tiposEntidade1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.tiposEntidade1.LarguraLink = 0;
            this.tiposEntidade1.Location = new System.Drawing.Point(11, 11);
            this.tiposEntidade1.Margin = new System.Windows.Forms.Padding(2);
            this.tiposEntidade1.Modulo = "";
            this.tiposEntidade1.MostraLink = false;
            this.tiposEntidade1.Name = "tiposEntidade1";
            this.tiposEntidade1.PermiteSemTipoEntidade = false;
            this.tiposEntidade1.ResourceID = 0;
            this.tiposEntidade1.Size = new System.Drawing.Size(270, 21);
            this.tiposEntidade1.TabIndex = 1;
            this.tiposEntidade1.Texto = "";
            this.tiposEntidade1.TipoContexto = PRISDK100.clsSDKTypes.enumContexto.Editor;
            this.tiposEntidade1.TipoEntidade = "";
            this.tiposEntidade1.TiposEntidade_TiposEntidade = null;
            this.tiposEntidade1.Titulo = "Tipo de Entidade:";
            this.tiposEntidade1.WidthLink = 0;
            this.tiposEntidade1.TextChange += new PRISDK100.TiposEntidade.TextChangeHandler(this.tiposEntidade1_TextChange);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.priGrelha1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 44);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(759, 367);
            this.panel2.TabIndex = 6;
            // 
            // priGrelha1
            // 
            this.priGrelha1.BackColor = System.Drawing.Color.White;
            this.priGrelha1.BandaMenuContexto = "";
            this.priGrelha1.BotaoConfigurarActiveBar = true;
            this.priGrelha1.BotaoProcurarActiveBar = false;
            this.priGrelha1.CaminhoTemplateImpressao = "";
            this.priGrelha1.Cols = null;
            this.priGrelha1.ColsFrozen = -1;
            this.priGrelha1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.priGrelha1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.priGrelha1.Location = new System.Drawing.Point(0, 0);
            this.priGrelha1.Name = "priGrelha1";
            this.priGrelha1.NumeroMaxRegistosSemPag = 150000;
            this.priGrelha1.NumeroRegistos = 0;
            this.priGrelha1.NumLinhasCabecalho = 1;
            this.priGrelha1.OrientacaoMapa = PRISDK100.clsSDKTypes.OrientacaoImpressao.oiDefault;
            this.priGrelha1.ParentFormModal = false;
            this.priGrelha1.PermiteActiveBar = true;
            this.priGrelha1.PermiteActualizar = true;
            this.priGrelha1.PermiteAgrupamentosUser = true;
            this.priGrelha1.PermiteConfigurarDetalhes = false;
            this.priGrelha1.PermiteContextoVazia = false;
            this.priGrelha1.PermiteDataFill = false;
            this.priGrelha1.PermiteDetalhes = true;
            this.priGrelha1.PermiteEdicao = false;
            this.priGrelha1.PermiteFiltros = true;
            this.priGrelha1.PermiteGrafico = true;
            this.priGrelha1.PermiteGrandeTotal = false;
            this.priGrelha1.PermiteOrdenacao = true;
            this.priGrelha1.PermitePaginacao = false;
            this.priGrelha1.PermiteScrollBars = true;
            this.priGrelha1.PermiteStatusBar = true;
            this.priGrelha1.PermiteVistas = true;
            this.priGrelha1.PosicionaColunaSeguinte = true;
            this.priGrelha1.Size = new System.Drawing.Size(759, 367);
            this.priGrelha1.TabIndex = 5;
            this.priGrelha1.TituloGrelha = "";
            this.priGrelha1.TituloMapa = "";
            this.priGrelha1.TypeNameLinha = "";
            this.priGrelha1.TypeNameLinhas = "";
            this.priGrelha1.FormatacaoAlterada += new PRISDK100.PriGrelha.FormatacaoAlteradaHandler(this.priGrelha1_FormatacaoAlterada);
            this.priGrelha1.MenuContextoSeleccionado += new PRISDK100.PriGrelha.MenuContextoSeleccionadoHandler(this.priGrelha1_MenuContextoSeleccionado);
            this.priGrelha1.ActualizaDados += new PRISDK100.PriGrelha.ActualizaDadosHandler(this.priGrelha1_ActualizaDados);
            this.priGrelha1.DataFill += new PRISDK100.PriGrelha.DataFillHandler(this.priGrelha1_DataFill);
            // 
            // frmDemoGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(759, 411);
            this.Name = "frmDemoGrid";
            this.Size = new System.Drawing.Size(759, 411);
            this.Text = "Pendentes";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDemoGrid_FormClosed);
            this.Load += new System.EventHandler(this.frmDemoGrid_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.priGrelha1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonLoadGrid;
        private PRISDK100.F4 f41;
        private PRISDK100.TiposEntidade tiposEntidade1;
        private System.Windows.Forms.Panel panel2;
        private PRISDK100.PriGrelha priGrelha1;
    }
}