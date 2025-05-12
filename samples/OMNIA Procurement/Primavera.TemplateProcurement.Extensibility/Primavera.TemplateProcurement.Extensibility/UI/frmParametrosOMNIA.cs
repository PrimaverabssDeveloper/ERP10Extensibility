using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Primavera.Extensibility.Engine.Containers.Modules;
using StdPlatBS100;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Primavera.TemplateProcurement.Extensibility.Ext.UI
{
    public partial class frmParametrosOMNIA : Form
    {
        #region Constants

        private const string CompanyDbNameFormat = "PRI{0}";
        private const string dllName = "Primavera.TemplateProcurement.Extensibility";

        #endregion

        #region Fields

        public ErpBS100.ErpBS BSO;
        public StdPlatBS100.StdBSInterfPub PSO;

        private readonly StdPlatBS plataforma;
        //private readonly AdmEngine100.clsFichaEmpresa erpEnterprise;

        //public BaseManager baseManager = null;

        private readonly string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private readonly string connstr;

        private bool EmModoEdicao = false;

        #endregion

        public frmParametrosOMNIA(ErpBS100.ErpBS bso, StdBSInterfPub pso)
        {
            BSO = bso;
            PSO = pso;

            InitializeComponent();

            IntegraOmniacheckBox.Visible = false;
            IntegraOmniacheckBox.Checked = true;

            InicializaDados();

        }

        private void InicializaDados()
        {
            var parametros = GetParametros();

            if (parametros is null)
            {
                //IntegraOmniacheckBox.Checked = false;
            }
            else
            {
                EmModoEdicao = true;

                //IntegraOmniacheckBox.Checked = true;

                fonteEndpointTextBox.Text = parametros.EndpointOmnia;
                fonteTenantTextBox.Text = parametros.TenantOmnia;

                endpointApiTextBox.Text = parametros.EndpointApi;
                endpointIdentityTextBox.Text = parametros.EndpointIdentity;

                omniaBDTextBox.Text = parametros.OmniaDB;

                clienteIdTextBox.Text = parametros.ClientIdOmnia;
                clienteSecretTextBox.Text = parametros.ClientSecretOmnia;
            }
        }

        public ParametrosOMNIA GetParametros()
        {
            MigraTabelaParametrosGCP();

            if (!Helper.ExisteTabela(BSO, Helper.TABELA_TDU_PARAMETROS)) return null;

            var strSQL = "select CDU_EmpresaOmnia,  CDU_EndpointOmnia, CDU_TenantOmnia, CDU_ClientIdOmnia, CDU_ClientSecretOmnia, CDU_OmniaDB, CDU_EndpointIdentity, CDU_EndpointApi from TDU_ParametrosOMNIA";
            DataTable result = BSO.ConsultaDataTable(strSQL);
            if (result.Rows.Count > 0)
            {
                var empresaOmnia = BSO.DSO.Plat.Utils.FStr(result.Rows[0]["CDU_EmpresaOmnia"]);
                var endpointOmnia = BSO.DSO.Plat.Utils.FStr(result.Rows[0]["CDU_EndpointOmnia"]);
                var tenantOmnia = BSO.DSO.Plat.Utils.FStr(result.Rows[0]["CDU_TenantOmnia"]);
                var clientIdOmnia = BSO.DSO.Plat.Utils.FStr(result.Rows[0]["CDU_ClientIdOmnia"]);
                var clientSecretOmnia = BSO.DSO.Plat.Utils.FStr(result.Rows[0]["CDU_ClientSecretOmnia"]);
                var omniaDB = BSO.DSO.Plat.Utils.FStr(result.Rows[0]["CDU_OmniaDB"]);
                var endpointIdentity = BSO.DSO.Plat.Utils.FStr(result.Rows[0]["CDU_EndpointIdentity"]);
                var endpointApi = BSO.DSO.Plat.Utils.FStr(result.Rows[0]["CDU_EndpointApi"]);

                return new ParametrosOMNIA
                {
                    EndpointApi = endpointApi,
                    TenantOmnia = tenantOmnia,
                    ClientIdOmnia = clientIdOmnia,
                    ClientSecretOmnia = clientSecretOmnia,
                    OmniaDB = omniaDB,
                    EndpointIdentity = endpointIdentity,
                    EndpointOmnia = endpointOmnia,
                };
            }

            return null;
        }

        private string CriaTabelaTDU(bool executeSQL = true)
        {
            var strSQL = @"IF NOT EXISTS (SELECT 1 [Exists] FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TDU_ParametrosOMNIA')
                               BEGIN
                                 CREATE TABLE [dbo].[TDU_ParametrosOMNIA](
                                           CDU_EmpresaOmnia      [varchar](50) NULL,
                                           CDU_EndpointOmnia     [varchar](200) NULL, 
	                                       CDU_TenantOmnia       [varchar](50) NULL,
	                                       CDU_ClientIdOmnia     [varchar](100) NULL,
	                                       CDU_ClientSecretOmnia [varchar](100) NULL,
	                                       CDU_OmniaDB           [varchar](100) NULL, 
	                                       CDU_EndpointIdentity  [varchar](200) NULL, 
	                                       CDU_EndpointApi       [varchar](200) NULL

                                    ) ON [PRIMARY]
                               END;  ";
            if (executeSQL)
                BSO.DSO.ExecuteSQL(strSQL);

            return strSQL;
        }

        private void MigraTabelaParametrosGCP()
        {
            if (!Helper.ExisteTabela(BSO, Helper.TABELA_TDU_PARAMETROS))
            {
               var strSQL = $@"IF  EXISTS (SELECT 1 [Exists] FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ParametrosBAS' and COLUMN_NAME ='EmpresaOmnia')
                            BEGIN

	                            DECLARE @P_EmpresaOmnia AS NVARCHAR(200),
			                            @P_EndpointOmnia  AS NVARCHAR(200),
			                            @P_TenantOmnia  AS NVARCHAR(200),
			                            @P_ClientIdOmnia  AS NVARCHAR(200),
			                            @P_ClientSecretOmnia  AS NVARCHAR(200),
			                            @P_OmniaDB  AS NVARCHAR(200),
			                            @P_EndpointIdentity  AS NVARCHAR(200),
			                            @P_EndpointApi  AS NVARCHAR(200)

	                            SELECT TOP 1  @P_EmpresaOmnia = [EmpresaOmnia],
				                              @P_EndpointOmnia = [EndpointOmnia],
				                              @P_TenantOmnia = [TenantOmnia],
				                              @P_ClientIdOmnia = [ClientIdOmnia],
				                              @P_ClientSecretOmnia = [ClientSecretOmnia],
				                              @P_OmniaDB = [OmniaDB],
				                              @P_EndpointIdentity  = [EndpointIdentity],
				                              @P_EndpointApi = [EndpointApi]			  
	                            FROM ParametrosBAS

                                {CriaTabelaTDU(executeSQL: false)}

                                INSERT INTO [dbo].[TDU_ParametrosOMNIA] ([CDU_EmpresaOmnia]
                                                                       ,[CDU_EndpointOmnia]
                                                                       ,[CDU_TenantOmnia]
                                                                       ,[CDU_ClientIdOmnia]
                                                                       ,[CDU_ClientSecretOmnia]
                                                                       ,[CDU_OmniaDB]
                                                                       ,[CDU_EndpointIdentity]
                                                                       ,[CDU_EndpointApi])
                                                                 VALUES
                                                                       ( @P_EmpresaOmnia
                                                                       , @P_EndpointOmnia
                                                                       , @P_TenantOmnia
                                                                       , @P_ClientIdOmnia
                                                                       , @P_ClientSecretOmnia
                                                                       , @P_OmniaDB
                                                                       , @P_EndpointIdentity
                                                                       , @P_EndpointApi)
                            END";

                try
                {
                    BSO.DSO.ExecuteSQL(strSQL);
                }
                catch (Exception e)
                {
                    // ignored
                }

            }
        }

        private void IntegraOmniacheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            //fonteGroupBox.Enabled = IntegraOmniacheckBox.Checked;
            //clienteGroupBox.Enabled = IntegraOmniacheckBox.Checked;
        }

        private void cmdGravar_Click(object sender, System.EventArgs e)
        {
            try
            {
                (bool valido, string msg) = ValidarGravar();
                if (valido)
                {

                    CriaTabelaTDU();

                    var strSQL = string.Empty;
                    if (EmModoEdicao)
                    {
                        strSQL = $@" UPDATE [dbo].[TDU_ParametrosOMNIA]
                                       SET [CDU_EmpresaOmnia] = '{BSO.Contexto.CodEmp}'
                                          ,[CDU_EndpointOmnia] = '{fonteEndpointTextBox.Text}'
                                          ,[CDU_TenantOmnia] = '{fonteTenantTextBox.Text}'
                                          ,[CDU_ClientIdOmnia] = '{clienteIdTextBox.Text}'
                                          ,[CDU_ClientSecretOmnia] = '{clienteSecretTextBox.Text}'
                                          ,[CDU_OmniaDB] = '{omniaBDTextBox.Text}'
                                          ,[CDU_EndpointIdentity] = '{endpointIdentityTextBox.Text}'
                                          ,[CDU_EndpointApi] = '{endpointApiTextBox.Text}' ";
                    }
                    else
                    {
                        strSQL = $@" INSERT INTO [dbo].[TDU_ParametrosOMNIA]
                                           ([CDU_EmpresaOmnia]
                                           ,[CDU_EndpointOmnia]
                                           ,[CDU_TenantOmnia]
                                           ,[CDU_ClientIdOmnia]
                                           ,[CDU_ClientSecretOmnia]
                                           ,[CDU_OmniaDB]
                                           ,[CDU_EndpointIdentity]
                                           ,[CDU_EndpointApi])
                                     VALUES
                                           ('{BSO.Contexto.CodEmp}'
                                           ,'{fonteEndpointTextBox.Text}'
                                           ,'{fonteTenantTextBox.Text}'
                                           ,'{clienteIdTextBox.Text}'
                                           ,'{clienteSecretTextBox.Text}'
                                           ,'{omniaBDTextBox.Text}'
                                           ,'{endpointIdentityTextBox.Text}'
                                           ,'{endpointApiTextBox.Text}')";

                    }


                    BSO.DSO.ExecuteSQL(strSQL);

                    MessageBox.Show(Properties.Resources.RES_Sucesso, "", MessageBoxButtons.OK);
                    this.Close();

                }
                else
                {
                    PSO.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, Properties.Resources.RES_CamposInvalidos, StdBSTipos.IconId.PRI_Exclama, msg);
                }

            }
            catch (Exception ex)
            {
                PSO.MensagensDialogos.MostraErroSimples(ex.Message, StdBSTipos.IconId.PRI_Critico);
            }
        }

        private (bool, string) ValidarGravar()
        {
            //if (!IntegraOmniacheckBox.Checked)
            //    return (true, "");

            string camposInvalidos = string.Empty;
            bool valido = true;



            if (string.IsNullOrEmpty(this.fonteEndpointTextBox.Text) || string.IsNullOrWhiteSpace(this.fonteEndpointTextBox.Text))
            {
                valido = false;
                camposInvalidos += Properties.Resources.RES_EndpointVazio + Environment.NewLine;

            }
            else if (!Uri.IsWellFormedUriString(this.fonteEndpointTextBox.Text, UriKind.Absolute))
            {
                valido = false;
                camposInvalidos += string.Format(Properties.Resources.RES_EndpointInvalido, this.fonteEndpointTextBox.Text) + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(this.endpointApiTextBox.Text) || string.IsNullOrWhiteSpace(this.endpointApiTextBox.Text))
            {
                valido = false;
                camposInvalidos += Properties.Resources.RES_EndpointApiVazio + Environment.NewLine;

            }
            else if (!Uri.IsWellFormedUriString(this.endpointApiTextBox.Text, UriKind.Relative))
            {
                valido = false;
                camposInvalidos += string.Format(Properties.Resources.RES_EndpointInvalido, this.endpointApiTextBox.Text) + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(this.endpointIdentityTextBox.Text) || string.IsNullOrWhiteSpace(this.endpointIdentityTextBox.Text))
            {
                valido = false;
                camposInvalidos += Properties.Resources.RES_EndpointIdentityVazio + Environment.NewLine;

            }
            else if (!Uri.IsWellFormedUriString(this.endpointIdentityTextBox.Text, UriKind.Relative))
            {
                valido = false;
                camposInvalidos += string.Format(Properties.Resources.RES_EndpointInvalido, this.endpointIdentityTextBox.Text) + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(this.fonteTenantTextBox.Text) || string.IsNullOrWhiteSpace(this.fonteTenantTextBox.Text))
            {
                valido = false;
                camposInvalidos += Properties.Resources.RES_TenantVazio + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(this.clienteIdTextBox.Text) || string.IsNullOrWhiteSpace(this.clienteIdTextBox.Text))
            {
                valido = false;
                camposInvalidos += Properties.Resources.RES_ClientIdVazio + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(this.clienteSecretTextBox.Text) || string.IsNullOrWhiteSpace(this.clienteSecretTextBox.Text))
            {
                valido = false;
                camposInvalidos += Properties.Resources.RES_ClientSecretVazio + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(this.omniaBDTextBox.Text) || string.IsNullOrWhiteSpace(this.omniaBDTextBox.Text))
            {
                valido = false;
                camposInvalidos += Properties.Resources.RES_OmniaDBVazio + Environment.NewLine;
            }

            //if (!File.Exists($"{rootPath}\\{Assembly.GetExecutingAssembly().GetName().Name}.dll"))
            //{
            //    valido = false;
            //    camposInvalidos += Properties.Resources.RES_ExtensibilidadeNaoEncontrado + Environment.NewLine;
            //}


            return (valido, camposInvalidos);
        }


    }

    public class ParametrosOMNIA
    {
        public string EndpointOmnia { get; set; } = string.Empty;
        public string TenantOmnia { get; set; } = string.Empty;
        public string ClientIdOmnia { get; set; } = string.Empty;
        public string ClientSecretOmnia { get; set; } = string.Empty;
        public string OmniaDB { get; set; } = string.Empty;
        public string EndpointIdentity { get; set; } = string.Empty;
        public string EndpointApi { get; set; } = string.Empty;
    }


}
