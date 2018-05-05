namespace Primavera.Base.CustomTabs.Customer
{
    partial class CustomerFields
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CDUCampoVar1 = new System.Windows.Forms.TextBox();
            this.CDUCampoVar2 = new System.Windows.Forms.TextBox();
            this.CDUCampoVar3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "CDU CampoVar1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "CDU CampoVar2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "CDU CampoVar3";
            // 
            // CDUCampoVar1
            // 
            this.CDUCampoVar1.Location = new System.Drawing.Point(113, 26);
            this.CDUCampoVar1.Name = "CDUCampoVar1";
            this.CDUCampoVar1.Size = new System.Drawing.Size(207, 20);
            this.CDUCampoVar1.TabIndex = 3;
            // 
            // CDUCampoVar2
            // 
            this.CDUCampoVar2.Location = new System.Drawing.Point(113, 53);
            this.CDUCampoVar2.Name = "CDUCampoVar2";
            this.CDUCampoVar2.Size = new System.Drawing.Size(207, 20);
            this.CDUCampoVar2.TabIndex = 4;
            // 
            // CDUCampoVar3
            // 
            this.CDUCampoVar3.Location = new System.Drawing.Point(113, 79);
            this.CDUCampoVar3.Name = "CDUCampoVar3";
            this.CDUCampoVar3.Size = new System.Drawing.Size(207, 20);
            this.CDUCampoVar3.TabIndex = 5;
            // 
            // CustomerFields
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CDUCampoVar3);
            this.Controls.Add(this.CDUCampoVar2);
            this.Controls.Add(this.CDUCampoVar1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CustomerFields";
            this.Size = new System.Drawing.Size(560, 330);
            this.TabCaption = "User Fields";
            this.Loading += new Primavera.Extensibility.Patterns.CustomTab.EventDelegate(this.CustomerFields_Loading);
            this.Saving += new Primavera.Extensibility.Patterns.CustomTab.EventDelegate(this.CustomerFields_Saving);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CDUCampoVar1;
        private System.Windows.Forms.TextBox CDUCampoVar2;
        private System.Windows.Forms.TextBox CDUCampoVar3;
    }
}
