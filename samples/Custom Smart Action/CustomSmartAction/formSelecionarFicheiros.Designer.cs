namespace CustomSmartAction
{
    partial class formSelecionarFicheiros
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelTitulo       = new System.Windows.Forms.Label();
            this.listBoxFicheiros  = new System.Windows.Forms.ListBox();
            this.buttonAdicionar   = new System.Windows.Forms.Button();
            this.buttonRemover     = new System.Windows.Forms.Button();
            this.buttonContinuar   = new System.Windows.Forms.Button();
            this.buttonCancelar    = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // ── labelTitulo ──────────────────────────────────────────────────
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Location = new System.Drawing.Point(12, 14);
            this.labelTitulo.Name     = "labelTitulo";
            this.labelTitulo.Text     = "Ficheiros selecionados:";

            // ── listBoxFicheiros ─────────────────────────────────────────────
            this.listBoxFicheiros.Location           = new System.Drawing.Point(12, 36);
            this.listBoxFicheiros.Size               = new System.Drawing.Size(420, 134);
            this.listBoxFicheiros.Name               = "listBoxFicheiros";
            this.listBoxFicheiros.SelectionMode      = System.Windows.Forms.SelectionMode.One;
            this.listBoxFicheiros.IntegralHeight      = false;
            this.listBoxFicheiros.HorizontalScrollbar = true;
            this.listBoxFicheiros.SelectedIndexChanged += new System.EventHandler(this.listBoxFicheiros_SelectedIndexChanged);

            // ── buttonAdicionar ──────────────────────────────────────────────
            this.buttonAdicionar.Location = new System.Drawing.Point(443, 36);
            this.buttonAdicionar.Size     = new System.Drawing.Size(100, 28);
            this.buttonAdicionar.Name     = "buttonAdicionar";
            this.buttonAdicionar.Text     = "Adicionar...";
            this.buttonAdicionar.UseVisualStyleBackColor = true;
            this.buttonAdicionar.Click += new System.EventHandler(this.buttonAdicionar_Click);

            // ── buttonRemover ────────────────────────────────────────────────
            this.buttonRemover.Location = new System.Drawing.Point(443, 72);
            this.buttonRemover.Size     = new System.Drawing.Size(100, 28);
            this.buttonRemover.Name     = "buttonRemover";
            this.buttonRemover.Text     = "Remover";
            this.buttonRemover.Enabled  = false;
            this.buttonRemover.UseVisualStyleBackColor = true;
            this.buttonRemover.Click += new System.EventHandler(this.buttonRemover_Click);

            // ── buttonContinuar ──────────────────────────────────────────────
            this.buttonContinuar.Location = new System.Drawing.Point(260, 192);
            this.buttonContinuar.Size     = new System.Drawing.Size(130, 32);
            this.buttonContinuar.Name     = "buttonContinuar";
            this.buttonContinuar.Text     = "Continuar ▶";
            this.buttonContinuar.Enabled  = false;
            this.buttonContinuar.UseVisualStyleBackColor = true;
            this.buttonContinuar.Click += new System.EventHandler(this.buttonContinuar_Click);

            // ── buttonCancelar ───────────────────────────────────────────────
            this.buttonCancelar.Location = new System.Drawing.Point(400, 192);
            this.buttonCancelar.Size     = new System.Drawing.Size(100, 32);
            this.buttonCancelar.Name     = "buttonCancelar";
            this.buttonCancelar.Text     = "Cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);

            // ── formSelecionarFicheiros ──────────────────────────────────────
            this.Text             = "Selecionar Ficheiros PDF";
            this.ClientSize       = new System.Drawing.Size(558, 240);
            this.FormBorderStyle  = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox      = false;
            this.MinimizeBox      = false;
            this.StartPosition    = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name             = "formSelecionarFicheiros";
            this.Controls.Add(this.labelTitulo);
            this.Controls.Add(this.listBoxFicheiros);
            this.Controls.Add(this.buttonAdicionar);
            this.Controls.Add(this.buttonRemover);
            this.Controls.Add(this.buttonContinuar);
            this.Controls.Add(this.buttonCancelar);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label  labelTitulo;
        private System.Windows.Forms.ListBox listBoxFicheiros;
        private System.Windows.Forms.Button  buttonAdicionar;
        private System.Windows.Forms.Button  buttonRemover;
        private System.Windows.Forms.Button  buttonContinuar;
        private System.Windows.Forms.Button  buttonCancelar;
    }
}
