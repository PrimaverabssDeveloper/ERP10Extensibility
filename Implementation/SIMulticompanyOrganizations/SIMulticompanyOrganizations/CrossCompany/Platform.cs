using BasBE100;
using ErpBS100;
using StdBE100;
using StdPlatBS100;
using SUGIMPL_OME.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

namespace SUGIMPL_OME.CrossCompany
{
    class Platform
    {
        /// <summary>
        /// Get the list of companies.
        /// </summary>
        /// <returns>A dictionary containing [string CompanyCode, string CompanyDescription]</returns>
        public static Dictionary<string, string> GetGroupCompanies(ERPContext oERPContext)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();

            StringBuilder sqlQry = new StringBuilder();
            sqlQry.AppendLine("select e.Codigo, e.IDNome as Descricao, ce.Descricao as Categoria ");
            sqlQry.AppendLine("from {0}..empresas e ");
            sqlQry.AppendLine("     inner join {0}..categoriasempresas ce on e.categoria=ce.categoria ");
            sqlQry.AppendLine("     inner join {0}..empresas ea on ea.Categoria = ce.Categoria ");
            sqlQry.AppendFormat("where ea.codigo = '{0}' and e.TipoExercicio <> 'CONSO'", oERPContext.BSO.Contexto.CodEmp);

            string sqlQRYRun = string.Format(sqlQry.ToString(), Properties.Settings.Default.PRIEMPRE_DBNAME);

            StdBELista compList = oERPContext.BSO.Consulta(sqlQRYRun.ToString());

            if (!compList.Vazia())
            {
                while (!compList.NoFim())
                {
                    Result.Add(compList.Valor("Codigo"), compList.Valor("Descricao"));
                    compList.Seguinte();
                }
            }

            return Result;
        }

        internal static bool CheckPassword(ERPContext oERPContext, string password)
        {
            bool result = false;


            frmPassword frmPWD = new frmPassword();
            frmPWD.ShowDialog();

            if (frmPWD.Password == password)
                result = true;

            frmPWD.Close();

            return result;
        }


        /// <summary>
        /// Processes before open the company
        /// </summary>
        /// <param name="Cancel"></param>
        internal static void BeforeOpenCompany(ERPContext oERPContext, ref Boolean Cancel)
        {

            String groupCategory = GetCompanyCategory(ref oERPContext);
            StdBSDialogoEspera oDialog;

            //Validation: the current company belongs to a category
            if (string.IsNullOrEmpty(groupCategory))
            {
                Cancel = true;
                oERPContext.PSO.Dialogos.MostraAviso(
                    "A empresa actual não está inserida em nenhuma categoria no administrador.",
                    StdPlatBS100.StdBSTipos.IconId.PRI_Critico,
                    "É necessário associar esta empresa à categoria onde estão inseridas as restantes empresas do grupo.");
                return;
            }

            oDialog = oERPContext.PSO.Dialogos.MostraDialogoEspera(
                sMensagem: "A processar manutenções nas empresas do grupo.",
                iNumProgressBars: 2,
                eIcon: StdBSTipos.IconId.PRI_Informativo,
                eAnim: StdBSTipos.AnimId.PRI_AviCalculos,
                sLabel1: "Calcular lista de empresas...",
                sLabel2: "Analisar lista de operações...");


            //Validation: there are further companies in the group. If not nothing should happen.
            Dictionary<String, String> groupCompanies = GetGroupCompanies(oERPContext);
            if (groupCompanies.Count == 0)
            {
                return; // No message is needed
            }


            //Validations by company
            int processedCompanies = 0;
            foreach (String groupCompany in groupCompanies.Keys)
            {

                oDialog.SetCaption(string.Format("A processar: {0}.", groupCompany), 1);

                ErpBS oCompany = new ErpBS();
                oCompany.AbreEmpresaTrabalho(
                    StdBETipos.EnumTipoPlataforma.tpEmpresarial,
                    groupCompany,
                    Properties.Settings.Default.User,
                    Properties.Settings.Default.Password);
                oDialog.ProgressBar2 = 2;

                //Check DATA MODEL
                oDialog.SetCaption("A verificar o modelo de dados...", 2);
                DataUpgrade(ref oCompany);
                oDialog.ProgressBar2 = 50;

                //Check Projects (not implemented - example for further entity validations)
                oDialog.SetCaption("A actualizar Projectos...", 2);
                //Do something
                oDialog.ProgressBar2 = 80;

                //Check default data (not implemented - example for defaults mastering)
                oDialog.SetCaption("A actualizar dados predefinidos...", 2);
                //Do something
                oDialog.ProgressBar2 = 100;


                //Final by company operations
                oCompany.FechaEmpresaTrabalho();
                oDialog.SetCaption("", 2);
                processedCompanies += 1;
                oDialog.ProgressBar1 = Convert.ToInt32(((processedCompanies * 100) / groupCompanies.Count));
                oDialog.ProgressBar2 = 0;

            }

            oDialog.Termina();
        }


        #region DATA MODEL

        /// <summary>
        /// Check for available Data Model updates in resources and apply them, if any.
        /// </summary>
        /// <param name="companyObject"></param>
        private static void DataUpgrade(ref ErpBS companyObject)
        {
            try
            {
                StdBELista dbVersion = companyObject.Consulta("select Versao from VersaoModulo where Modulo = 'XME'");

                string currentVersion = dbVersion.Vazia() ? "0" : dbVersion.DaValor<String>("Versao");
                int intVersion = Convert.ToInt32(currentVersion) + 1;

                //Apply the upgrade scripts from resources
                string sqlUPG = Properties.Resources.ResourceManager.GetString(String.Format("UPG_{0}", intVersion.ToString()), CultureInfo.InvariantCulture);
                while (!String.IsNullOrEmpty(sqlUPG))
                {
                    companyObject.DSO.ExecuteSQL(sqlUPG);

                    intVersion += 1;
                    sqlUPG = Properties.Resources.ResourceManager.GetString(String.Format("UPG_{0}", intVersion.ToString()), CultureInfo.InvariantCulture);
                }

                //Update the version
                intVersion -= 1;
                if (intVersion > Convert.ToInt32(currentVersion))
                {
                    string sqlQRY = string.Format("" +
                        "if exists(select * from VersaoModulo where Modulo = 'XME') " +
                        "   update VersaoModulo set Versao = '{0}' where Modulo = 'XME' " +
                        "else " +
                        "   insert into VersaoModulo(Modulo, Versao) values('XME', '{0}')"
                        , intVersion.ToString());

                    companyObject.DSO.ExecuteSQL(sqlQRY);
                }
            }
            catch
            {
                //Do nothing
            }
        }

        #endregion

        #region PRIVATE FUNCTIONS

        private static String GetCompanyCategory(ref ERPContext oERPContext)
        {
            String retValue = "";

            String sqlQRY = string.Format("" +
                "select ce.descricao Categoria " +
                "from {0}..empresas e " +
                "   left join {0}..categoriasempresas ce on e.categoria=ce.categoria " +
                "where e.codigo='{1}'", Properties.Settings.Default.PRIEMPRE_DBNAME, oERPContext.BSO.Contexto.CodEmp);
            StdBELista listQueryResults = oERPContext.BSO.Consulta(sqlQRY);

            if (!listQueryResults.Vazia())
            {
                retValue = listQueryResults.Valor("Categoria");
            }

            return retValue;
        }

        #endregion


    }
}
