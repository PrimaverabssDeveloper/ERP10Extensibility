using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ErpBS100;
using Primavera.Extensibility.Constants.ExtensibilityService;
using Primavera.Extensibility.Patterns;
using StdBE100;
using StdPlatBS100;
using UpgradeHelpers.SupportHelper;

namespace Primavera.Egar.Import
{
    public partial class frmSyncEGAR : Form
    {
        string empresa;
        string user;
        string password;
        bool login = false;
        ErpBS motor = new ErpBS();
        StdPlatBS plataforma = new StdPlatBS();

        StdBSConfApl objAplConf = new StdBSConfApl();

        StdBE100.StdBETipos.EnumTipoPlataforma objTipoPlataforma;

        public frmSyncEGAR()
        {
            InitializeComponent();
            textBoxEmpresa.Text = "DEMO";
            textBox2.Text = "USER";
            textBox3.Text = "PWD";

        }

        private void buttonAbre_Click(object sender, EventArgs e)
        {
            this.textBox5.Text = "";
            Cursor.Current = Cursors.WaitCursor;
            empresa = textBoxEmpresa.Text;
            user = textBox2.Text;
            password = textBox3.Text;

            if (!this.checkBox1.Checked)
            {
                objTipoPlataforma = StdBE100.StdBETipos.EnumTipoPlataforma.tpEmpresarial;
            }
            else
            {
                objTipoPlataforma = StdBE100.StdBETipos.EnumTipoPlataforma.tpProfissional;
            }

            objAplConf.Instancia = this.textBox4.Text;
            objAplConf.AbvtApl = "ERP";
            objAplConf.PwdUtilizador = password;
            objAplConf.Utilizador = user;

            objAplConf.LicVersaoMinima = "10.00";

            StdBETransaccao objStdTransac = new StdBE100.StdBETransaccao();

            try
            {
                plataforma.AbrePlataformaEmpresa(empresa, null, objAplConf, objTipoPlataforma);

            }
            catch (Exception ex)
            {
                this.textBox5.AppendText(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
            }

            if (plataforma.Inicializada)
            {
                this.textBox5.AppendText("Plataforma inicializada" + Environment.NewLine);
                this.textBox5.AppendText(plataforma.Contexto.Utilizador.Utilizador + Environment.NewLine);


                motor.Plataforma = plataforma;

                try
                {
                    motor.AbreEmpresaTrabalho(objTipoPlataforma, empresa, plataforma.Contexto.Utilizador.Utilizador, plataforma.Contexto.Utilizador.Password, strInstancia: this.textBox4.Text);
                    InicializaExtensibilidade(motor);
                    login = true;
                    this.textBox5.AppendText("Empresa aberta" + Environment.NewLine);

                }
                catch (Exception ex)
                {
                    this.textBox5.AppendText(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
                }

            }
            Cursor.Current = Cursors.Default;
        }

        private void InicializaExtensibilidade(ErpBS erpbs)
        {
            // Primavera Extensibility PEX
            bool initialized = false;

            if (Support.CreateObject("Primavera.Extensibility.Engine.ExtensibilityService", string.Empty) is IExtensibilityService service)
            {
                // The line below is an example for external services that want to use the default provided Logger but doesn't support UI interaction
                //StdUSMain.Plataforma.ExtensibilityLogger.AllowInteractivity = false;
                service.Initialize(erpbs);

                // Check if service is operational
                if (service.IsOperational)
                {
                    // Initialize and load implementations
                    service.LoadExtensions();
                    initialized = true;
                }
                else
                {
                    // Service started but isn't operational due to no extensions available or no license
                    if (service.PEXLicense.Equals(PEXLicense.None) || service.ExtensionsNotLoaded.Count == 0)
                    {
                        initialized = true;
                    }
                }
            }

            if (!initialized)
            {
                throw new Exception("---------------------------------------------------" + Environment.NewLine +
                                    "-----Error initializing extensibility service.-----" + Environment.NewLine +
                                    "---------------------------------------------------");
            }
        }

        private void btnFicheiro_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\"; // Define o diretório inicial (opcional)
            openFileDialog1.Filter = "Arquivos CSV (*.csv)|*.csv"; // Define os filtros de ficheiro (opcional)
            openFileDialog1.FilterIndex = 1; // Define o filtro padrão (opcional)
            openFileDialog1.RestoreDirectory = true; // Restaura o diretório anterior (opcional)

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    txtFilePath.Text = openFileDialog1.FileName;
                }
                catch (Exception ex)
                {
                    this.textBox5.AppendText("Erro: Não foi possível ler o ficheiro. Erro original: " + Environment.NewLine +ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
                }
            }
        }

        private List<(string, string)> ReadCsvToList(string filePath)
        {
            List<(string, string)> data = new List<(string, string)>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);


                foreach (string line in lines.Skip(0))
                {
                    string[] values = line.Split(';');

                    if (values.Length >= 2)
                    {
                        data.Add((values[0], values[1]));
                    }
                }
            }
            catch (FileNotFoundException)
            {
                this.textBox5.AppendText("Arquivo não encontrado." + Environment.NewLine);
            }
            catch (Exception ex)
            {
                this.textBox5.AppendText($"Ocorreu um erro: "+ Environment.NewLine + ex.Message+ Environment.NewLine);
            }

            return data;
        }

        private void btnSincronizacao_Click(object sender, EventArgs e)
        {
            if (login)
            {
                if (!string.IsNullOrEmpty(txtFilePath.Text))
                {
                    List<(string, string)> result = ReadCsvToList(txtFilePath.Text);

                    this.textBox5.AppendText($"Vão ser importados {result.Count.ToString()} documentos" + Environment.NewLine);

                    BSEGAR bsEgar = new BSEGAR(motor, plataforma);
                    string strErrosEx = string.Empty;
                    string imporados = string.Empty;
                    string nImportados = string.Empty;

                    bsEgar.Egar(result, ref strErrosEx, ref imporados, ref nImportados);

                    if(!string.IsNullOrEmpty(strErrosEx))
                    {
                        this.textBox5.AppendText(strErrosEx + Environment.NewLine);
                    }
                    if (!string.IsNullOrEmpty(imporados))
                    {
                        this.txtImportados.AppendText(imporados + Environment.NewLine);
                    }
                    if (!string.IsNullOrEmpty(nImportados))
                    {
                        this.txtNImportados.AppendText(nImportados + Environment.NewLine);
                    }
                }
                else
                {
                    this.textBox5.AppendText("Deve indicar o caminho do ficheiro!" + Environment.NewLine);
                }
            }
            else
            {
                this.textBox5.AppendText("Não efectuou login!" + Environment.NewLine);
            }
            
        }
    }
}
