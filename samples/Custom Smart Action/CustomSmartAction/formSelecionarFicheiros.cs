using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CustomSmartAction
{
    public partial class formSelecionarFicheiros : Form
    {
        // Full paths of the confirmed selection
        public List<string> SelectedFiles { get; private set; } = new List<string>();

        public formSelecionarFicheiros()
        {
            InitializeComponent();
            UpdateButtons();
        }

        // ── Event handlers ────────────────────────────────────────────────────

        private void buttonAdicionar_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Selecionar Ficheiros PDF";
                dlg.Filter = "Ficheiros PDF (*.pdf)|*.pdf";
                dlg.Multiselect = true;
                dlg.RestoreDirectory = true;
                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                foreach (var path in dlg.FileNames)
                {
                    if (!SelectedFiles.Contains(path))
                    {
                        SelectedFiles.Add(path);
                        listBoxFicheiros.Items.Add(Path.GetFileName(path));
                    }
                }
            }

            UpdateButtons();
        }

        private void buttonRemover_Click(object sender, EventArgs e)
        {
            int idx = listBoxFicheiros.SelectedIndex;
            if (idx < 0)
                return;

            SelectedFiles.RemoveAt(idx);
            listBoxFicheiros.Items.RemoveAt(idx);
            UpdateButtons();
        }

        private void listBoxFicheiros_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void buttonContinuar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private void UpdateButtons()
        {
            buttonContinuar.Enabled = SelectedFiles.Count > 0;
            buttonRemover.Enabled = listBoxFicheiros.SelectedIndex >= 0;
        }
    }
}
