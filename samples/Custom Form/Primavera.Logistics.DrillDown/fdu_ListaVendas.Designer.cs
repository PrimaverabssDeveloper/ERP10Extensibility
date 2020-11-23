namespace Primavera.Logistics.DrillDown
{
    partial class fdu_ListaVendas
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fdu_ListaVendas));
            this.barDockControlActiveBarTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlActiveBarBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlActiveBarLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlActiveBarRight = new DevExpress.XtraBars.BarDockControl();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.drillDownDocumento = new System.Windows.Forms.ToolStripMenuItem();
            this.drillDownEntidade = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolrefresh = new System.Windows.Forms.ToolStripButton();
            this.toolClose = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // barDockControlActiveBarTop
            // 
            this.barDockControlActiveBarTop.CausesValidation = false;
            this.barDockControlActiveBarTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlActiveBarTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlActiveBarTop.Size = new System.Drawing.Size(674, 0);
            // 
            // barDockControlActiveBarBottom
            // 
            this.barDockControlActiveBarBottom.CausesValidation = false;
            this.barDockControlActiveBarBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlActiveBarBottom.Location = new System.Drawing.Point(0, 378);
            this.barDockControlActiveBarBottom.Size = new System.Drawing.Size(674, 0);
            // 
            // barDockControlActiveBarLeft
            // 
            this.barDockControlActiveBarLeft.CausesValidation = false;
            this.barDockControlActiveBarLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlActiveBarLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlActiveBarLeft.Size = new System.Drawing.Size(0, 378);
            // 
            // barDockControlActiveBarRight
            // 
            this.barDockControlActiveBarRight.CausesValidation = false;
            this.barDockControlActiveBarRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlActiveBarRight.Location = new System.Drawing.Point(674, 0);
            this.barDockControlActiveBarRight.Size = new System.Drawing.Size(0, 378);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(0, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(670, 349);
            this.dataGridView1.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drillDownDocumento,
            this.drillDownEntidade});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(193, 70);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // drillDownDocumento
            // 
            this.drillDownDocumento.Name = "drillDownDocumento";
            this.drillDownDocumento.Size = new System.Drawing.Size(192, 22);
            this.drillDownDocumento.Text = "DrillDown Documento";
            // 
            // drillDownEntidade
            // 
            this.drillDownEntidade.Name = "drillDownEntidade";
            this.drillDownEntidade.Size = new System.Drawing.Size(192, 22);
            this.drillDownEntidade.Text = "DrillDown Entidade";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolrefresh,
            this.toolClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(674, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolrefresh
            // 
            this.toolrefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolrefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolrefresh.Image")));
            this.toolrefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolrefresh.Name = "toolrefresh";
            this.toolrefresh.Size = new System.Drawing.Size(50, 22);
            this.toolrefresh.Text = "Refresh";
            this.toolrefresh.Click += new System.EventHandler(this.toolrefresh_Click);
            // 
            // toolClose
            // 
            this.toolClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolClose.Image = ((System.Drawing.Image)(resources.GetObject("toolClose.Image")));
            this.toolClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolClose.Name = "toolClose";
            this.toolClose.Size = new System.Drawing.Size(40, 22);
            this.toolClose.Text = "Close";
            this.toolClose.Click += new System.EventHandler(this.toolClose_Click);
            // 
            // fdu_ListaVendas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 378);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "fdu_ListaVendas";
            this.Text = "Lista de Faturas";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolrefresh;
        private System.Windows.Forms.ToolStripButton toolClose;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem drillDownDocumento;
        private System.Windows.Forms.ToolStripMenuItem drillDownEntidade;
        private DevExpress.XtraBars.BarDockControl barDockControlActiveBarTop;
        private DevExpress.XtraBars.BarDockControl barDockControlActiveBarBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlActiveBarLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlActiveBarRight;
    }
}