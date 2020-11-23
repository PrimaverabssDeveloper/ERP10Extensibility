using System;
using System.Windows.Forms;
using Primavera.Extensibility.CustomForm;
using PRISDK100;

namespace PrimaveraSDK
{
    public partial class frmDemo1 : CustomForm
    {
        private bool controlsInitialized;

        public frmDemo1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the form and initialize the SDK context and the SDK controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDemo_Load(object sender, EventArgs e)
        {
            //Initializes context 
            PriSDKContext.Initialize(BSO, PSO);

            //Initializes controls
            if (!controlsInitialized)
            {
                //Initializes the components with the ERP context
                treeContasEstado1.Inicializa(PriSDKContext.SdkContext);
                tiposEntidade1.Inicializa(PriSDKContext.SdkContext);
                f41.Inicializa(PriSDKContext.SdkContext);

                controlsInitialized = true;
            }
        }

        /// <summary>
        /// Closes the form and terminate the SDK controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDemo_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Ensure that resources released.
            treeContasEstado1.Termina();
            tiposEntidade1.Termina();
            f41.Termina();

            controlsInitialized = false;
        }

        /// <summary>
        /// Displays the accounts associated with the entity.
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void tiposEntidade1_TextChange(object Sender, TiposEntidade.TextChangeEventArgs e)
        {
            treeContasEstado1.TipoEntidade = tiposEntidade1.TipoEntidade;
            textboxRestriction.Text = string.Empty;

            SelecionaCategoriaF4();
        }

        /// <summary>
        /// Extract the restriction from the tree control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restrictionButton_Click(object sender, EventArgs e)
        {
            string tabelaBD = "P";
            textboxRestriction.Text = treeContasEstado1.Restricao(false, ref tabelaBD, false).Replace("'", "''");
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void statesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            treeContasEstado1.IncluirEstados = statesCheckBox.Checked;
            textboxRestriction.Text = string.Empty;
        }

        private void outstandingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            treeContasEstado1.SoPendentes = outstandingCheckBox.Checked;
            textboxRestriction.Text = string.Empty;
        }

        /// <summary>
        /// Selects the F4 category according to the entity type selected.
        /// </summary>
        private void SelecionaCategoriaF4()
        {
            f41.Enabled = true;
            f41.PermiteDrillDown = true;

            switch (tiposEntidade1.TipoEntidade)
            {
                case "C":
                    f41.Categoria = clsSDKTypes.EnumCategoria.Clientes;
                    break;
                case "F":
                    f41.Categoria = clsSDKTypes.EnumCategoria.Fornecedores;
                    break;
                default:
                    f41.Categoria = clsSDKTypes.EnumCategoria.NaoDefinida;
                    f41.Enabled = false;
                    f41.Limpa();
                    f41.PermiteDrillDown = false;
                    break;
            }

            f41.ResetText();
        }
    }
}
