namespace CustomSmartAction
{
    partial class formImportacaoArtigos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formImportacaoArtigos));
            this.buttonCriaArtigos = new System.Windows.Forms.Button();
            this.textBoxCSV = new System.Windows.Forms.TextBox();
            this.labelResultado = new System.Windows.Forms.Label();
            this.textBoxImportacao = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonCriaArtigos
            // 
            resources.ApplyResources(this.buttonCriaArtigos, "buttonCriaArtigos");
            this.buttonCriaArtigos.Name = "buttonCriaArtigos";
            this.buttonCriaArtigos.UseVisualStyleBackColor = true;
            this.buttonCriaArtigos.Click += new System.EventHandler(this.buttonCriaArtigos_Click);
            // 
            // textBoxCSV
            // 
            resources.ApplyResources(this.textBoxCSV, "textBoxCSV");
            this.textBoxCSV.Name = "textBoxCSV";
            // 
            // labelResultado
            // 
            resources.ApplyResources(this.labelResultado, "labelResultado");
            this.labelResultado.Name = "labelResultado";
            // 
            // textBoxImportacao
            // 
            resources.ApplyResources(this.textBoxImportacao, "textBoxImportacao");
            this.textBoxImportacao.Name = "textBoxImportacao";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // formImportacaoArtigos
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxImportacao);
            this.Controls.Add(this.labelResultado);
            this.Controls.Add(this.textBoxCSV);
            this.Controls.Add(this.buttonCriaArtigos);
            this.Name = "formImportacaoArtigos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCriaArtigos;
        private System.Windows.Forms.TextBox textBoxCSV;
        private System.Windows.Forms.Label labelResultado;
        private System.Windows.Forms.TextBox textBoxImportacao;
        private System.Windows.Forms.Label label1;
    }
}