using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Platform.Services;
using Primavera.Platform.Helpers;
using Primavera.TemplateProcurement.Extensibility.Resource;
using StdPlatBS100;

namespace Primavera.TemplateProcurement.Extensibility.Platform
{
    public class ApiPlatform : Plataforma
    {
        #region Constants

        private static readonly string CompanyDbNameFormat = "PRI{0}";
        private static readonly string VersaoModulo = "10.01";//alteração dos CDUS , incrementar a versão

        #endregion

        #region Public

        public override void AntesDeAbrirEmpresa(ref bool Cancel, ExtensibilityEventArgs e)
        {
            try
            {
                if (NeedUpgrade())
                    ExecUpgrade();

            }
            catch (Exception ex)
            {
                PSO.MensagensDialogos.MostraErroSimples(ex.Message, StdBSTipos.IconId.PRI_Critico);
            }
        }

        public override void DepoisDeAbrirEmpresa(ExtensibilityEventArgs e)
        {
            e.HandleExceptions = true;

            try
            {
                ErpBS100.ErpBS _BSO = BSO;
                StdPlatBS100.StdBSInterfPub _PSO = PSO;
                
                if (Helper.ExisteTabela(_BSO, Helper.TABELA_TDU_PARAMETROS))
                    ApiClient.InitializeApiClient(ref _BSO, ref _PSO);

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Properties.Resources.RES_ErroInicializacao, ex.Message));
            }
        }

    

        #endregion

        #region Private
        private bool NeedUpgrade()
        {
            try
            {
                using (DbConnection conn = PSO.BaseDados.DaConnection(string.Format(CompanyDbNameFormat, BSO.Contexto.CodEmp), BSO.Contexto.Instancia, string.Empty))
                {
                    if (conn is SqlConnection sqlConnection)
                    {
                        SqlCommand command = new SqlCommand("SELECT Versao FROM VersaoModulo WHERE Modulo = 'CRI'", sqlConnection);
                        command.Connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (!reader.HasRows)
                        {
                            reader.Close();
                            command = new SqlCommand($"INSERT INTO VersaoModulo (Modulo, Versao) VALUES ('CRI', '{VersaoModulo}')", sqlConnection);
                            command.ExecuteNonQuery();
                            return true;
                        }
                        else
                        {
                            float versao = float.Parse(VersaoModulo, CultureInfo.InvariantCulture);

                            while (reader.Read())
                            {
                                versao = float.Parse(reader.GetSqlString(0).Value, CultureInfo.InvariantCulture);
                            }

                            reader.Close();
                            if (float.Parse(VersaoModulo, CultureInfo.InvariantCulture) > versao)
                            {
                                reader.Close();
                                command = new SqlCommand($"UPDATE VersaoModulo SET Versao = '{VersaoModulo}' WHERE Modulo = 'CRI'", sqlConnection);
                                command.ExecuteNonQuery();
                                return true;
                            }
                            return false;
                        }
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ExecUpgrade()
        {
            try
            {
                using (DbConnection dbConnection = PSO.BaseDados.DaConnection(string.Format(CompanyDbNameFormat, BSO.Contexto.CodEmp), BSO.Contexto.Instancia, String.Empty))
                {
                    if (dbConnection is SqlConnection sqlConnection)
                    {
                        SqlCommand command = new SqlCommand(Sql.StdUp, sqlConnection);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                StdUpgCDUs100.StdUpgCDUs objUpgCDUs = new StdUpgCDUs100.StdUpgCDUs();

                if (PSO != null && BSO.Contexto != null)
                {
                    int timeout = int.MaxValue;
                    DbConnection dbConnection = PSO.BaseDados.DaConnection(string.Format(CompanyDbNameFormat, BSO.Contexto.CodEmp), BSO.Contexto.Instancia, String.Empty, timeout, false);

                    objUpgCDUs.Inicializa(
                        (StdBE100.StdBETipos.EnumGlobalCultures)Convert.ToInt32(BSO.Contexto.CulturaUtilizadorActual),
                        dbConnection,
                        Convert.ToString(PSO.RegistryPrimavera.DaPercursoDadosDef));

                    StdLoggingHandler.FileTrace("[CDUs] Rebuild all dependencies");
                    objUpgCDUs.ReconstroiTodasDependencias();
                    objUpgCDUs.Termina();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
