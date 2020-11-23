namespace PrimaveraSDK
{
    partial class frmDemoGridEdit
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
            this.labelDate = new System.Windows.Forms.Label();
            this.labelNumber = new System.Windows.Forms.Label();
            this.numericUpDownNumber = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.priGrelha1 = new PRISDK100.PriGrelha();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonNew = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumber)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.priGrelha1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dateTimePickerDate);
            this.panel1.Controls.Add(this.labelDate);
            this.panel1.Controls.Add(this.labelNumber);
            this.panel1.Controls.Add(this.numericUpDownNumber);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(759, 45);
            this.panel1.TabIndex = 5;
            // 
            // dateTimePickerDate
            // 
            this.dateTimePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerDate.Location = new System.Drawing.Point(200, 12);
            this.dateTimePickerDate.Name = "dateTimePickerDate";
            this.dateTimePickerDate.Size = new System.Drawing.Size(102, 21);
            this.dateTimePickerDate.TabIndex = 3;
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(160, 14);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(34, 13);
            this.labelDate.TabIndex = 2;
            this.labelDate.Text = "Data:";
            // 
            // labelNumber
            // 
            this.labelNumber.AutoSize = true;
            this.labelNumber.Location = new System.Drawing.Point(12, 14);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(48, 13);
            this.labelNumber.TabIndex = 1;
            this.labelNumber.Text = "Número:";
            // 
            // numericUpDownNumber
            // 
            this.numericUpDownNumber.Location = new System.Drawing.Point(66, 12);
            this.numericUpDownNumber.Name = "numericUpDownNumber";
            this.numericUpDownNumber.Size = new System.Drawing.Size(77, 21);
            this.numericUpDownNumber.TabIndex = 0;
            this.numericUpDownNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownNumber.Validated += new System.EventHandler(this.numericUpDownNumber_Validated);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.priGrelha1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 45);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(759, 411);
            this.panel2.TabIndex = 6;
            // 
            // priGrelha1
            // 
            this.priGrelha1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.priGrelha1.BackColor = System.Drawing.Color.White;
            this.priGrelha1.BandaMenuContexto = "";
            this.priGrelha1.BotaoConfigurarActiveBar = true;
            this.priGrelha1.BotaoProcurarActiveBar = false;
            this.priGrelha1.CaminhoTemplateImpressao = "";
            this.priGrelha1.Cols = null;
            this.priGrelha1.ColsFrozen = -1;
            this.priGrelha1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.priGrelha1.Location = new System.Drawing.Point(12, 6);
            this.priGrelha1.Name = "priGrelha1";
            this.priGrelha1.NumeroMaxRegistosSemPag = 150000;
            this.priGrelha1.NumeroRegistos = 0;
            this.priGrelha1.NumLinhasCabecalho = 1;
            this.priGrelha1.OrientacaoMapa = PRISDK100.clsSDKTypes.OrientacaoImpressao.oiDefault;
            this.priGrelha1.ParentFormModal = false;
            this.priGrelha1.PermiteActiveBar = false;
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
            this.priGrelha1.Size = new System.Drawing.Size(733, 363);
            this.priGrelha1.TabIndex = 5;
            this.priGrelha1.TituloGrelha = "";
            this.priGrelha1.TituloMapa = "";
            this.priGrelha1.TypeNameLinha = "";
            this.priGrelha1.TypeNameLinhas = "";
            this.priGrelha1.FormatacaoAlterada += new PRISDK100.PriGrelha.FormatacaoAlteradaHandler(this.priGrelha1_FormatacaoAlterada);
            this.priGrelha1.MenuContextoSeleccionado += new PRISDK100.PriGrelha.MenuContextoSeleccionadoHandler(this.priGrelha1_MenuContextoSeleccionado);
            this.priGrelha1.EditMode += new PRISDK100.PriGrelha.EditModeHandler(this.priGrelha1_EditMode);
            this.priGrelha1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.priGrelha1_KeyDown);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonRemove);
            this.panel3.Controls.Add(this.buttonNew);
            this.panel3.Controls.Add(this.buttonSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 421);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(759, 35);
            this.panel3.TabIndex = 7;
            // 
            // buttonNew
            // 
            this.buttonNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNew.Location = new System.Drawing.Point(509, 3);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(75, 23);
            this.buttonNew.TabIndex = 9;
            this.buttonNew.Text = "Novo";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(671, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "Gravar";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemove.Location = new System.Drawing.Point(590, 3);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 10;
            this.buttonRemove.Text = "Remover";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // frmDemoGridEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 456);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(759, 411);
            this.Name = "frmDemoGridEdit";
            this.Text = "Registo de Anomalias";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDemoGridEdit_FormClosed);
            this.Load += new System.EventHandler(this.frmDemoGridEdit_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumber)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.priGrelha1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private PRISDK100.PriGrelha priGrelha1;
        private System.Windows.Forms.DateTimePicker dateTimePickerDate;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Label labelNumber;
        private System.Windows.Forms.NumericUpDown numericUpDownNumber;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonRemove;
    }
}