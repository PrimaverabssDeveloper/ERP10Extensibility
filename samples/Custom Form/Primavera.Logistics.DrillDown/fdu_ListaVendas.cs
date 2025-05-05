using Primavera.Extensibility.CustomForm;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Primavera.Logistics.DrillDown
{
    public partial class fdu_ListaVendas : CustomForm
    {
        #region Public Methods
        public fdu_ListaVendas()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Methods
        private void LoadSalesInvoices()
        {
            try
            {
                string connectionString = this.PSO.BaseDados.DaConnectionStringNET(this.PSO.BaseDados.DaNomeBDdaEmpresa(this.BSO.Contexto.CodEmp), "Default");

                string query = "SELECT Entidade, Data,DataVencimento, TipoDoc, NumDoc, Serie, TotalMerc, TotalIva, TotalDesc,TotalOutros FROM CabecDoc Order by Data asc";

                SqlConnection con = new SqlConnection(connectionString);

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();

                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading information...");
            }
        }
        #endregion

        #region Toolbar Events
        private void toolrefresh_Click(object sender, EventArgs e)
        {
            LoadSalesInvoices();
        }

        private void toolClose_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        #endregion

        #region ContextMenuStrip
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DataGridViewRow viewRow = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex];

            switch (e.ClickedItem.Name)
            {
                case "drillDownEntidade":
                    DrillDownManager.drillDownEntidade(PSO,
                                                        "mnuTabClientes",
                                                        Convert.ToString(viewRow.Cells[0].Value));
                    break;

                case "drillDownDocumento":
                    DrillDownManager.drillDownDocumento(PSO,
                                                        "V",
                                                        Convert.ToString(viewRow.Cells[3].Value),
                                                        Convert.ToInt16(viewRow.Cells[4].Value),
                                                        Convert.ToString(viewRow.Cells[5].Value),
                                                        "000");
                    break;
            }
        }
        #endregion
    }
}
