using ErpBS100;
using StdPlatBS100;
using System;
using System.Windows.Forms;

namespace CustomSmartAction
{
    public partial class formImportacaoArtigos : Form
    {
        private ErpBS _BSO;
        private StdBSInterfPub _PSO;

        public formImportacaoArtigos()
        {
            InitializeComponent();
        }

        public formImportacaoArtigos(ErpBS bSO, StdBSInterfPub pSO)
        {
            this._BSO = bSO;
            this._PSO = pSO;

            InitializeComponent();
        }

        private void buttonCriaArtigos_Click(object sender, EventArgs e)
        {
            string resultadoCSV = FilePdfExcelReader();

            this.Cursor = Cursors.WaitCursor;

            textBoxCSV.Text = resultadoCSV;

            CriaArtigos criaArtigos = new CriaArtigos(_BSO, _PSO, resultadoCSV);
            criaArtigos.Executar(textBoxImportacao);

            this.Cursor = Cursors.Default;
        }

        public static string FilePdfExcelReader()
        {
            string filePath = PromptFilePath();
            if (filePath == null)
            {
                return string.Empty;
            }

            try
            {
                string csvContent = PriceTableConverter.Convert(filePath);

                // Preview first few lines of the CSV string
                return csvContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

        }

        // ── UI helpers ───────────────────────────────────────────────────────

        private static string PromptFilePath()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Seleccione a tabela de preços";
                dlg.Filter = "Tabelas de preços (*.pdf;*.xlsx)|*.pdf;*.xlsx" +
                             "|Ficheiros PDF (*.pdf)|*.pdf" +
                             "|Ficheiros Excel (*.xlsx)|*.xlsx";
                dlg.FilterIndex = 1;      // start with combined filter
                dlg.RestoreDirectory = true;
                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                    return dlg.FileName;
            }

            return null;  // user cancelled
        }
    }
}
