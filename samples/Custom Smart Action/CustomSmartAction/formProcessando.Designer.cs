namespace CustomSmartAction
{
    partial class formProcessando
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
            this.labelMensagem  = new System.Windows.Forms.Label();
            this.progressBar    = new System.Windows.Forms.ProgressBar();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.timerTimeout   = new System.Windows.Forms.Timer();
            this.SuspendLayout();

            // labelMensagem
            this.labelMensagem.Location  = new System.Drawing.Point(20, 18);
            this.labelMensagem.Size      = new System.Drawing.Size(360, 28);
            this.labelMensagem.Name      = "labelMensagem";
            this.labelMensagem.Text      = "A processar os documentos, aguarde...";
            this.labelMensagem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelMensagem.Font      = new System.Drawing.Font("Segoe UI", 9.5F);

            // progressBar
            this.progressBar.Location              = new System.Drawing.Point(20, 54);
            this.progressBar.Size                  = new System.Drawing.Size(360, 16);
            this.progressBar.Name                  = "progressBar";
            this.progressBar.Style                 = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.MarqueeAnimationSpeed = 25;

            // buttonCancelar
            this.buttonCancelar.Location               = new System.Drawing.Point(145, 82);
            this.buttonCancelar.Size                   = new System.Drawing.Size(110, 28);
            this.buttonCancelar.Name                   = "buttonCancelar";
            this.buttonCancelar.Text                   = "Cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);

            // timerTimeout — intervalo e handler configurados no construtor

            // formProcessando
            this.Text            = "A processar...";
            this.ClientSize      = new System.Drawing.Size(400, 124);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.ControlBox      = false;
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name            = "formProcessando";
            this.Controls.Add(this.labelMensagem);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonCancelar);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label       labelMensagem;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button      buttonCancelar;
        private System.Windows.Forms.Timer       timerTimeout;
    }
}