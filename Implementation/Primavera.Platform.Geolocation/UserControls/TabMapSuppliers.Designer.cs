
namespace Primavera.Platform.Geolocation
{
    partial class TabMapSuppliers
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
            this.SuspendLayout();
            // 
            // TabMapClients
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TabMapSuppliers";
            this.Size = new System.Drawing.Size(709, 455);
            this.TabCaption = "Geolocalização";
            this.Load += new System.EventHandler(this.TabMapSuppliers_Load);
            this.Loading += new Primavera.Extensibility.Patterns.CustomTab.EventDelegate(this.TabMapSuppliers_Loading);
            this.Saving += new Primavera.Extensibility.Patterns.CustomTab.EventDelegate(this.TabMapSuppliers_Saving);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

