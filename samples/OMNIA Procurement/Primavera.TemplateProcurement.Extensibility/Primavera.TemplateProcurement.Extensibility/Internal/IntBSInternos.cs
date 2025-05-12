using System;
using System.Collections.Generic;
using IntBE100;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.TemplateProcurement.Extensibility.Ext;
using Primavera.TemplateProcurement.Extensibility.Resource;
using StdBE100;

namespace Primavera.TemplateProcurement.Extensibility.Internal
{
    public class IntBSInternos : Primavera.Extensibility.Internal.Services.IntBSInternos
    {
        public override void AntesDeGravar(IntBEDocumentoInterno objBE, ref string strAvisos, ExtensibilityEventArgs e)
        {
            e.HandleExceptions = true;

            if (objBE.CamposUtil.Existe("CDU_CodOmnia") && (objBE.CamposUtil["CDU_CodOmnia"].Valor == null || string.IsNullOrEmpty(objBE.CamposUtil["CDU_CodOmnia"].Valor.ToString())) && objBE.Linhas.NumItens > 0)
            {
                foreach (IntBELinhaDocumentoInterno linha in objBE.Linhas)
                {
                    if (!string.IsNullOrEmpty(linha.IdLinhaOrigemCopia))
                    {
                        string getCodOmnia = PSO.Sql.FormatSQLExt(Sql.GetInternos_CodOmniaByLineId, new object[] { linha.IdLinhaOrigemCopia });

                        StdBELista resultCodOmnia = BSO.Consulta(getCodOmnia);

                        if (resultCodOmnia != null)
                        {
                            if (!resultCodOmnia.Vazia())
                            {
                                objBE.CamposUtil["CDU_CodOmnia"].Valor = resultCodOmnia.Valor("CDU_CodOmnia").ToString();
                                string sql = PSO.Sql.FormatSQLExt(Sql.GetInternos_CDU, objBE.CamposUtil["CDU_CodOmnia"].Valor);

                                StdBELista objLista = BSO.Consulta(sql);

                                if (objLista != null)
                                {
                                    if (!objLista.Vazia())
                                    {
                                        objBE.CamposUtil["CDU_OrganizationalUnit"].Valor = objLista.Valor("CDU_OrganizationalUnit").ToString();
                                        objBE.CamposUtil["CDU_Autor"].Valor = objLista.Valor("CDU_Autor").ToString();
                                        objBE.CamposUtil["CDU_AutorNome"].Valor = objLista.Valor("CDU_AutorNome").ToString();
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

        }

        public override void DepoisDeGravar(IntBEDocumentoInterno objBE, ref string strAvisos, ExtensibilityEventArgs e)
        {
            e.HandleExceptions = true;
            //CDU_CodOmnia has value?
            if (objBE.CamposUtil["CDU_CodOmnia"] != null && objBE.CamposUtil["CDU_CodOmnia"].Valor != null && !string.IsNullOrEmpty(objBE.CamposUtil["CDU_CodOmnia"].Valor.ToString()))
            {
                try
                {
                    string omniaCode = objBE.CamposUtil["CDU_CodOmnia"].Valor.ToString();

                    IntBETabInterno configDoc = BSO.Internos.TabInternos.Edita(objBE.Tipodoc);

                    if (configDoc.TipoDocumento == 1 && objBE.Estado == "F")
                    {
                        Dictionary<string, object> requestResult = new Dictionary<string, object>();

                        if (!ApiClient.Intialized())
                        {
                            ErpBS100.ErpBS _BSO = BSO;
                            StdPlatBS100.StdBSInterfPub _PSO = PSO;

                            ApiClient.InitializeApiClient(ref _BSO, ref _PSO);

                            if (!ApiClient.Intialized())
                                strAvisos += Environment.NewLine + string.Format(Properties.Resources.RES_NaoExisteConfi, BSO.Contexto.CodEmp);
                            else
                            {
                                requestResult = ApiClient.ValidateRequisition(omniaCode, 1).GetAwaiter().GetResult();
                            }
                        }
                        else
                            ApiClient.ValidateRequisition(omniaCode, 1).GetAwaiter().GetResult();

                        if (requestResult.ContainsKey("errorMessage"))
                            strAvisos += Environment.NewLine + requestResult["errorMessage"].ToString();
                    }

                    else if (configDoc.TipoDocumento == 4 || configDoc.TipoDocumento == 5)
                    {
                        if (Helper.IsRISatisfied(objBE.CamposUtil["CDU_CodOmnia"].Valor.ToString(), BSO, PSO))
                        {
                            Dictionary<string, object> requestResult = new Dictionary<string, object>();

                            if (!ApiClient.Intialized())
                            {
                                ErpBS100.ErpBS _BSO = BSO;
                                StdPlatBS100.StdBSInterfPub _PSO = PSO;

                                ApiClient.InitializeApiClient(ref _BSO, ref _PSO);

                                if (!ApiClient.Intialized())
                                    strAvisos += Environment.NewLine + string.Format(Properties.Resources.RES_NaoExisteConfi, BSO.Contexto.CodEmp);
                                else
                                    requestResult = ApiClient.ValidateRequisition(omniaCode, 2).GetAwaiter().GetResult();

                            }
                            else
                                requestResult = ApiClient.ValidateRequisition(omniaCode, 2).GetAwaiter().GetResult();

                            if (requestResult.ContainsKey("errorMessage"))
                                strAvisos += Environment.NewLine + requestResult["errorMessage"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    strAvisos += Environment.NewLine + ex.Message;
                }
            }
        }

    }
}
