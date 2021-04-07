using ErpBS100;
using PRISDK100;
using StdBE100;
using SUGIMPL_OME.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace SUGIMPL_OME.CrossCompany
{
    class PayablesReceivables
    {

        /// <summary>
        /// Returns the global position of a specified company (and, optionally, its related companies) all over the group companies.
        /// </summary>
        /// <param name="ERPContext"></param>
        /// <param name="EntityType"></param>
        /// <param name="EntityCode"></param>
        /// <param name="IncludeRelatedEntities"></param>
        /// <returns>A StdBELista containing the information.</returns>
        internal static StdBELista GetGlobalPosition(ERPContext ERPContext, String EntityType, String EntityCode, Boolean IncludeRelatedEntities)
        {

            StringBuilder sqlQRY = new StringBuilder();
            Dictionary<String, String> GroupCompanies = CrossCompany.Platform.GetGroupCompanies(ERPContext);
            StdBELista retValue = new StdBELista();

            try
            {

                //Prepare the quary template to be used in each company
                sqlQRY.Append("select Grp1= '', Empresa = '{0}', E.TipoEntidade , E.Entidade ");
                sqlQRY.Append("     ,Orcamentos = isnull((select sum(totalmerc - totaldesc + totaloutros) from {1}..cabecdoc cd left join {1}..cabecdocstatus cds on cd.id = cds.idcabecdoc where cd.tipodoc = 'ORC' and cds.estado = 'G' and cd.entidade = E.CodEntidade and E.TipoEntidade IN('C', 'D')), 0) ");
                sqlQRY.Append("          +isnull((select - sum(totalmerc - totaldesc + totaloutros) from {1}..cabeccompras cc left join {1}..cabeccomprasstatus ccs on cc.id = ccs.idcabeccompras where cc.tipodoc = 'PCO' and ccs.estado = 'G' and cc.entidade = E.CodEntidade and E.TipoEntidade IN('F', 'R')),0) ");
                sqlQRY.Append("     ,Encomendas = isnull((select EncomendasPendentes from {1}..Clientes where Cliente = E.CodEntidade and E.TipoEntidade IN('C', 'D')),0) ");
                sqlQRY.Append("          +isnull((select - EncomendasPendentes from {1}..fornecedores where fornecedor = E.CodEntidade and E.TipoEntidade IN('F', 'R')),0)");
                sqlQRY.Append("     ,TotalVencido = isnull((select sum(valorpendente) from {1}..pendentes where entidade = E.CodEntidade and E.TipoEntidade IN('C', 'D') and datavenc < getdate() and tipoconta = 'CCC' group by entidade),0) ");
                sqlQRY.Append("          +isnull((select sum(valorpendente) from {1}..pendentes where entidade = E.CodEntidade and E.TipoEntidade IN('F', 'R') and datavenc < getdate() and tipoconta = 'CCF' group by entidade),0) ");
                sqlQRY.Append("     ,TotalDebito = isnull((select totaldeb from {1}..clientes where cliente = E.CodEntidade and E.TipoEntidade IN('C', 'D')),0) ");
                sqlQRY.Append("          +isnull((select - totaldeb from {1}..fornecedores where fornecedor = E.CodEntidade and E.TipoEntidade IN('F', 'R')),0)");
                sqlQRY.Append("from( ");
                sqlQRY.Append("     select '{2}' as TipoEntidade, Cliente as CodEntidade, Nome as Entidade, '0' as Associada from {1}..Clientes where Cliente = '{3}' and '{2}' = 'C' ");
                sqlQRY.Append("     UNION ALL ");
                sqlQRY.Append("     select '{2}' as TipoEntidade, Fornecedor as CodEntidade, Nome as Entidade, '0' as Associada from {1}..Fornecedores where Fornecedor = '{3}' and '{2}' = 'F' ");
                sqlQRY.Append("     UNION ALL ");
                sqlQRY.Append("     select '{2}' as TipoEntidade, Terceiro as CodEntidade, Nome as Entidade, '0' as Associada from {1}..OutrosTerceiros where Terceiro = '{3}' and TipoEntidade = '{2}' ");
                sqlQRY.Append("     UNION ALL ");
                sqlQRY.Append("     select TipoEntidadeAssociada as TipoEntidade, EntidadeAssociada as CodEntidade, coalesce(C.Nome, F.Nome, T.Nome) as Entidade, 1 as Associada ");
                sqlQRY.Append("     from {1}..EntidadesAssociadas EA ");
                sqlQRY.Append("         left join {1}..Clientes C on C.Cliente = EA.EntidadeAssociada and EA.TipoEntidadeAssociada = 'C' ");
                sqlQRY.Append("         left join {1}..Fornecedores F on F.Fornecedor = EA.EntidadeAssociada and EA.TipoEntidadeAssociada = 'F' ");
                sqlQRY.Append("         left join {1}..OutrosTerceiros T on T.Terceiro = EA.EntidadeAssociada and T.TipoTerceiro = EA.TipoEntidadeAssociada ");
                sqlQRY.Append("     where Entidade = '{3}' and EA.TipoEntidade = '{2}' ");
                sqlQRY.Append("     ) as E ");
                sqlQRY.Append("Where Associada = 0 or Associada = {4}");

                //Prepare de final query (having all companies)
                StringBuilder sqlQRYtoRun = new StringBuilder();
                String strUnion = string.Empty;
                foreach (String Company in GroupCompanies.Keys)
                {
                    sqlQRYtoRun.Append(strUnion);
                    sqlQRYtoRun.AppendFormat(sqlQRY.ToString(), Company, string.Format("PRI{0}", Company), EntityType, EntityCode, IncludeRelatedEntities ? 1 : 0);
                    strUnion = "UNION ALL ";
                }

                //Execute the query to retrieve the information
                retValue = ERPContext.BSO.Consulta(sqlQRYtoRun.ToString());

            }
            catch
            {
                //DO NOTHING
            }

            return retValue;
        }


        /// <summary>
        /// Return a list of companies where the credit limit of the given customer is exceeded orelse has been blocked.
        /// </summary>
        /// <param name="strCustomer">Customer to be analised</param>
        /// <returns>List<String></String></returns>
        internal static List<String> CreditLimitExceeded(ERPContext ERPContext, String strCustomer)
        {
            List<String> companiesList = new List<String>();
            Dictionary<String, String> groupCompanies = CrossCompany.Platform.GetGroupCompanies(ERPContext);

            foreach (string company in groupCompanies.Keys)
            {
                ErpBS currentCompany = new ErpBS();

                currentCompany.AbreEmpresaTrabalho(
                    StdBETipos.EnumTipoPlataforma.tpEmpresarial, 
                    company, 
                    Properties.Settings.Default.User, //ERPContext.BSO.Contexto.ObjUtilizador.Codigo,
                    Properties.Settings.Default.Password //ERPContext.BSO.Contexto.ObjUtilizador.Password
                    );

                if ((currentCompany.Base.Clientes.DaValorAtributo(strCustomer, "TipoCred") == "2")
                    || (currentCompany.Base.Clientes.DaValorAtributo(strCustomer, "limitecred") < currentCompany.Base.Clientes.DaValorAtributo(strCustomer, "totaldeb")))
                {
                    companiesList.Add(company);
                }

                currentCompany.FechaEmpresaTrabalho();
            }

            return companiesList;
        }
    }
}
