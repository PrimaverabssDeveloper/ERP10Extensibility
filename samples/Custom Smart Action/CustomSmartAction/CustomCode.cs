using System;
using System.Windows.Forms;

namespace CustomSmartAction
{
    public class CustomCode : Primavera.Extensibility.CustomCode.CustomCode
    {
        public void ImportaArtigosCsvPdf()
        {
            try
            {
                formImportacaoArtigos form = new formImportacaoArtigos(this.BSO, this.PSO);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }
    }
}
