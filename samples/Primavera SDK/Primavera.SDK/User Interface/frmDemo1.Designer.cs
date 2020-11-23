namespace PrimaveraSDK
{
    partial class frmDemo1
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
            this.tiposEntidade1 = new PRISDK100.TiposEntidade();
            this.statesCheckBox = new System.Windows.Forms.CheckBox();
            this.outstandingCheckBox = new System.Windows.Forms.CheckBox();
            this.restrictionButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.treeContasEstado1 = new PRISDK100.TreeContasEstado();
            this.textboxRestriction = new System.Windows.Forms.TextBox();
            this.f41 = new PRISDK100.F4();
            this.SuspendLayout();
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
            this.tiposEntidade1.Location = new System.Drawing.Point(11, 12);
            this.tiposEntidade1.Margin = new System.Windows.Forms.Padding(2);
            this.tiposEntidade1.Modulo = "";
            this.tiposEntidade1.MostraLink = false;
            this.tiposEntidade1.Name = "tiposEntidade1";
            this.tiposEntidade1.PermiteSemTipoEntidade = false;
            this.tiposEntidade1.ResourceID = 0;
            this.tiposEntidade1.Size = new System.Drawing.Size(270, 21);
            this.tiposEntidade1.TabIndex = 0;
            this.tiposEntidade1.Texto = "";
            this.tiposEntidade1.TipoContexto = PRISDK100.clsSDKTypes.enumContexto.Editor;
            this.tiposEntidade1.TipoEntidade = "";
            this.tiposEntidade1.TiposEntidade_TiposEntidade = null;
            this.tiposEntidade1.Titulo = "Tipo de Entidade:";
            this.tiposEntidade1.WidthLink = 0;
            this.tiposEntidade1.TextChange += new PRISDK100.TiposEntidade.TextChangeHandler(this.tiposEntidade1_TextChange);
            // 
            // statesCheckBox
            // 
            this.statesCheckBox.AutoSize = true;
            this.statesCheckBox.Location = new System.Drawing.Point(621, 39);
            this.statesCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.statesCheckBox.Name = "statesCheckBox";
            this.statesCheckBox.Size = new System.Drawing.Size(94, 17);
            this.statesCheckBox.TabIndex = 1;
            this.statesCheckBox.Text = "Include States";
            this.statesCheckBox.UseVisualStyleBackColor = true;
            this.statesCheckBox.CheckedChanged += new System.EventHandler(this.statesCheckBox_CheckedChanged);
            // 
            // outstandingCheckBox
            // 
            this.outstandingCheckBox.AutoSize = true;
            this.outstandingCheckBox.Location = new System.Drawing.Point(621, 60);
            this.outstandingCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.outstandingCheckBox.Name = "outstandingCheckBox";
            this.outstandingCheckBox.Size = new System.Drawing.Size(107, 17);
            this.outstandingCheckBox.TabIndex = 2;
            this.outstandingCheckBox.Text = "Only Outstanding";
            this.outstandingCheckBox.UseVisualStyleBackColor = true;
            this.outstandingCheckBox.CheckedChanged += new System.EventHandler(this.outstandingCheckBox_CheckedChanged);
            // 
            // restrictionButton
            // 
            this.restrictionButton.Location = new System.Drawing.Point(621, 96);
            this.restrictionButton.Margin = new System.Windows.Forms.Padding(2);
            this.restrictionButton.Name = "restrictionButton";
            this.restrictionButton.Size = new System.Drawing.Size(111, 25);
            this.restrictionButton.TabIndex = 4;
            this.restrictionButton.Text = "Restriction";
            this.restrictionButton.UseVisualStyleBackColor = true;
            this.restrictionButton.Click += new System.EventHandler(this.restrictionButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(621, 216);
            this.closeButton.Margin = new System.Windows.Forms.Padding(2);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(111, 25);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // treeContasEstado1
            // 
            this.treeContasEstado1.ApenasAdiantamentos = false;
            this.treeContasEstado1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.treeContasEstado1.Entidade = "";
            this.treeContasEstado1.EntidadesAssociadas = false;
            this.treeContasEstado1.FiltroTipoConta = "";
            this.treeContasEstado1.IncluirEstados = true;
            this.treeContasEstado1.Inicializado = false;
            this.treeContasEstado1.Location = new System.Drawing.Point(11, 39);
            this.treeContasEstado1.Margin = new System.Windows.Forms.Padding(2);
            this.treeContasEstado1.MostraContasFactoring = true;
            this.treeContasEstado1.MostraEntidadesAsocciadas = true;
            this.treeContasEstado1.Name = "treeContasEstado1";
            this.treeContasEstado1.Size = new System.Drawing.Size(606, 103);
            this.treeContasEstado1.SoPendentes = false;
            this.treeContasEstado1.TabIndex = 6;
            this.treeContasEstado1.TipoDoc = "";
            this.treeContasEstado1.TipoEntidade = "";
            // 
            // textboxRestriction
            // 
            this.textboxRestriction.Location = new System.Drawing.Point(12, 147);
            this.textboxRestriction.Multiline = true;
            this.textboxRestriction.Name = "textboxRestriction";
            this.textboxRestriction.ReadOnly = true;
            this.textboxRestriction.Size = new System.Drawing.Size(719, 64);
            this.textboxRestriction.TabIndex = 7;
            // 
            // f41
            // 
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
            this.f41.Size = new System.Drawing.Size(445, 22);
            this.f41.TabIndex = 8;
            this.f41.TextoDescricao = "";
            this.f41.WidthEspacamento = 60;
            this.f41.WidthF4 = 1590;
            this.f41.WidthLink = 1575;
            // 
            // frmDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 250);
            this.Controls.Add(this.f41);
            this.Controls.Add(this.textboxRestriction);
            this.Controls.Add(this.treeContasEstado1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.restrictionButton);
            this.Controls.Add(this.outstandingCheckBox);
            this.Controls.Add(this.statesCheckBox);
            this.Controls.Add(this.tiposEntidade1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDemo";
            this.Text = "PRIMAVERA SDK Sample";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDemo_FormClosed);
            this.Load += new System.EventHandler(this.frmDemo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PRISDK100.TiposEntidade tiposEntidade1;
        private System.Windows.Forms.CheckBox statesCheckBox;
        private System.Windows.Forms.CheckBox outstandingCheckBox;
        private System.Windows.Forms.Button restrictionButton;
        private System.Windows.Forms.Button closeButton;
        private PRISDK100.TreeContasEstado treeContasEstado1;
        private System.Windows.Forms.TextBox textboxRestriction;
        private PRISDK100.F4 f41;
    }
}