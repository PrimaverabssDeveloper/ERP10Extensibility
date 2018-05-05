namespace Primavera.Base.CustomTabs.Customer
{
    partial class GoogleMaps
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
            this.MapsBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // MapsBrowser
            // 
            this.MapsBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapsBrowser.Location = new System.Drawing.Point(0, 0);
            this.MapsBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.MapsBrowser.Name = "MapsBrowser";
            this.MapsBrowser.Size = new System.Drawing.Size(564, 331);
            this.MapsBrowser.TabIndex = 0;
            // 
            // GoogleMaps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MapsBrowser);
            this.Name = "GoogleMaps";
            this.Size = new System.Drawing.Size(564, 331);
            this.TabCaption = "Google Maps";
            this.Loading += new Primavera.Extensibility.Patterns.CustomTab.EventDelegate(this.GoogleMaps_Loading);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser MapsBrowser;
    }
}
