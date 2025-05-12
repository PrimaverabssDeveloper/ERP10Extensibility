using ErpBS100;
using StdBE100;
using StdPlatBS100;

namespace Primavera.TemplateProcurement.Extensibility
{
    public static class Helper
    {
        public static string TABELA_TDU_PARAMETROS = "TDU_ParametrosOMNIA";

        public static bool IsRISatisfied(string CodOmnia, ErpBS BSO, StdBSInterfPub PSO)
        {
            bool result = false,
                SatisfyOnInventoryOut = false;

            //Validar se APIClient foi inicializado, se não inicializar
            if (!ApiClient.Intialized())
            {
                ErpBS100.ErpBS _BSO = BSO;
                StdPlatBS100.StdBSInterfPub _PSO = PSO;

                ApiClient.InitializeApiClient(ref _BSO, ref _PSO);
            }
            //Valida novamente se foi inicializada, se não estiver não vai fazer a chamada e assumir o valor default para o cálculo
            if (!ApiClient.Intialized())
            {
                try
                {
                    var companyConfig = ApiClient.GetCompanyConfig(BSO.Contexto.CodEmp).GetAwaiter().GetResult();
                    SatisfyOnInventoryOut = bool.Parse(companyConfig["riSatisfactionBasedOnStkMovements"].ToString());

                }
                catch
                {
                    SatisfyOnInventoryOut = false;
                }
            }

            try
            {
                StdBELista resultSP = BSO.Consulta($"GCP_INT_ProcessamentoReqInt @Restricoes = 'CI.CDU_CodOmnia = ''{CodOmnia}''', @VerSaidasStock={(SatisfyOnInventoryOut ? 1 : 0)}");

                if (resultSP != null && !resultSP.Vazia())
                {
                    result = true;
                    while (!resultSP.NoFim())
                    {
                        if (resultSP.Valor("QtdAberto") > 0)
                        {
                            result = false;
                            break;
                        }
                        resultSP.Seguinte();
                    }
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static bool ExisteTabela(ErpBS100.ErpBS _BSO, string tabela)
        {
            var existeTabela = false;
            var strSQL = $@"IF NOT EXISTS (SELECT 1 [Exists] FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tabela}') 
                                   SELECT Existe =  '0'
                                ELSE
                                   SELECT Existe =  '1' ";
            var objLista = _BSO.Consulta(strSQL);
            if (objLista?.NoFim() == false)
                existeTabela = _BSO.DSO.Plat.Utils.FBool(objLista.Valor("Existe"));
            return existeTabela;
        }



    }
}
