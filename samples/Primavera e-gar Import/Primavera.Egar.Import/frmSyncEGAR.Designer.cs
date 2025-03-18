namespace Primavera.Egar.Import
{
    partial class frmSyncEGAR
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBoxEmpresa = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.buttonAbre = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnFicheiro = new System.Windows.Forms.Button();
            this.btnSincronizacao = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtImportados = new System.Windows.Forms.TextBox();
            this.txtNImportados = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Instância";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Utilizador";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Empresa";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(122, 84);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(108, 20);
            this.textBox4.TabIndex = 16;
            this.textBox4.Text = "Default";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(122, 58);
            this.textBox3.Name = "textBox3";
            this.textBox3.PasswordChar = '*';
            this.textBox3.Size = new System.Drawing.Size(108, 20);
            this.textBox3.TabIndex = 15;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(122, 32);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(108, 20);
            this.textBox2.TabIndex = 14;
            // 
            // textBoxEmpresa
            // 
            this.textBoxEmpresa.Location = new System.Drawing.Point(122, 6);
            this.textBoxEmpresa.Name = "textBoxEmpresa";
            this.textBoxEmpresa.Size = new System.Drawing.Size(108, 20);
            this.textBoxEmpresa.TabIndex = 13;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(122, 110);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(112, 17);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "Linha Professional";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // buttonAbre
            // 
            this.buttonAbre.Location = new System.Drawing.Point(15, 133);
            this.buttonAbre.Name = "buttonAbre";
            this.buttonAbre.Size = new System.Drawing.Size(219, 40);
            this.buttonAbre.TabIndex = 18;
            this.buttonAbre.Text = "Abrir Empresa";
            this.buttonAbre.UseVisualStyleBackColor = true;
            this.buttonAbre.Click += new System.EventHandler(this.buttonAbre_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(15, 194);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(228, 20);
            this.txtFilePath.TabIndex = 19;
            // 
            // btnFicheiro
            // 
            this.btnFicheiro.Location = new System.Drawing.Point(15, 220);
            this.btnFicheiro.Name = "btnFicheiro";
            this.btnFicheiro.Size = new System.Drawing.Size(228, 35);
            this.btnFicheiro.TabIndex = 20;
            this.btnFicheiro.Text = "Carregar Ficheiro";
            this.btnFicheiro.UseVisualStyleBackColor = true;
            this.btnFicheiro.Click += new System.EventHandler(this.btnFicheiro_Click);
            // 
            // btnSincronizacao
            // 
            this.btnSincronizacao.Location = new System.Drawing.Point(293, 9);
            this.btnSincronizacao.Name = "btnSincronizacao";
            this.btnSincronizacao.Size = new System.Drawing.Size(228, 35);
            this.btnSincronizacao.TabIndex = 21;
            this.btnSincronizacao.Text = "Sincronização";
            this.btnSincronizacao.UseVisualStyleBackColor = true;
            this.btnSincronizacao.Click += new System.EventHandler(this.btnSincronizacao_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(290, 281);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(142, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Documentos não importados";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(15, 310);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox5.Size = new System.Drawing.Size(228, 179);
            this.textBox5.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 281);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Log";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(290, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(176, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Documentos Importados importados";
            // 
            // txtImportados
            // 
            this.txtImportados.Location = new System.Drawing.Point(293, 84);
            this.txtImportados.Multiline = true;
            this.txtImportados.Name = "txtImportados";
            this.txtImportados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtImportados.Size = new System.Drawing.Size(228, 179);
            this.txtImportados.TabIndex = 28;
            // 
            // txtNImportados
            // 
            this.txtNImportados.Location = new System.Drawing.Point(293, 310);
            this.txtNImportados.Multiline = true;
            this.txtNImportados.Name = "txtNImportados";
            this.txtNImportados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNImportados.Size = new System.Drawing.Size(228, 179);
            this.txtNImportados.TabIndex = 29;
            // 
            // frmSyncEGAR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 501);
            this.Controls.Add(this.txtNImportados);
            this.Controls.Add(this.txtImportados);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnSincronizacao);
            this.Controls.Add(this.btnFicheiro);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.buttonAbre);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBoxEmpresa);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmSyncEGAR";
            this.Text = "Importa Doc eGar";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBoxEmpresa;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button buttonAbre;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnFicheiro;
        private System.Windows.Forms.Button btnSincronizacao;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtImportados;
        private System.Windows.Forms.TextBox txtNImportados;
    }
}