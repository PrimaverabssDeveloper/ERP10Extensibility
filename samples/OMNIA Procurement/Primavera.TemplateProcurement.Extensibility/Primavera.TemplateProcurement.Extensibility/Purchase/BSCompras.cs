using System;
using System.Collections.Generic;
using System.Linq;
using CmpBE100;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Purchases.Services;
using Primavera.TemplateProcurement.Extensibility.Entities.Responses;
using Primavera.TemplateProcurement.Extensibility.Resource;
using StdBE100;

namespace Primavera.TemplateProcurement.Extensibility.Purchase
{
    public class BSCompras : CmpBSCompras
    {
        private string strErrors = string.Empty;
        public override void AntesDeGravar(CmpBEDocumentoCompra clsDocCompra, ref string strAvisos, ref string IdDocLiqRet, ref string IdDocLiqRetGar, ExtensibilityEventArgs e)
        {
            e.HandleExceptions = true;

            BSO.Compras.Documentos.CalculaValoresTotais(clsDocCompra);

            CmpBETabCompra docConfig = BSO.Compras.TabCompras.Edita(clsDocCompra.Tipodoc);

            try
            {
                if (clsDocCompra.CamposUtil.NumItens == 0)
                    clsDocCompra.CamposUtil = BSO.Compras.Documentos.DaCamposUtil();

                if (clsDocCompra.CamposUtil.Existe("CDU_CodOmnia") && (clsDocCompra.CamposUtil["CDU_CodOmnia"].Valor == null || string.IsNullOrEmpty(clsDocCompra.CamposUtil["CDU_CodOmnia"].Valor.ToString())))
                {

                    if (clsDocCompra.Linhas.NumItens > 0)
                    {
                        foreach (CmpBELinhaDocumentoCompra linha in clsDocCompra.Linhas)
                        {
                            if (!string.IsNullOrEmpty(linha.IdLinhaOrigemCopia))
                            {
                                string getCodOmnia = PSO.Sql.FormatSQLExt(Sql.GetInternos_CodOmniaByLineId, new object[] { linha.IdLinhaOrigemCopia });

                                StdBELista resultCodOmnia = BSO.Consulta(getCodOmnia);

                                if (resultCodOmnia != null)
                                {
                                    if (!resultCodOmnia.Vazia())
                                    {
                                        clsDocCompra.CamposUtil["CDU_CodOmnia"].Valor = resultCodOmnia.Valor("CDU_CodOmnia").ToString();
                                        string sql = PSO.Sql.FormatSQLExt(Sql.GetInternos_CDU, clsDocCompra.CamposUtil["CDU_CodOmnia"].Valor);

                                        StdBELista objLista = BSO.Consulta(sql);

                                        if (objLista != null)
                                        {
                                            if (!objLista.Vazia())
                                            {
                                                clsDocCompra.CamposUtil["CDU_OrganizationalUnit"].Valor = objLista.Valor("CDU_OrganizationalUnit").ToString();
                                                clsDocCompra.CamposUtil["CDU_Autor"].Valor = objLista.Valor("CDU_Autor").ToString();
                                                clsDocCompra.CamposUtil["CDU_AutorNome"].Valor = objLista.Valor("CDU_AutorNome").ToString();
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (clsDocCompra.CamposUtil.Existe("CDU_CodOmnia") && !string.IsNullOrEmpty(clsDocCompra.CamposUtil["CDU_CodOmnia"].Valor.ToString()))
                {
                    if (docConfig.TipoDocumento == 4 && (clsDocCompra.CamposUtil["CDU_Estado"].Valor == null || string.IsNullOrEmpty(clsDocCompra.CamposUtil["CDU_Estado"].Valor.ToString())))
                    {
                        Dictionary<string, object> requestResult = new Dictionary<string, object>();

                        var familias = string.Join("##", clsDocCompra.Linhas
                            .Select(linha => linha.Artigo)
                            .Distinct()
                            .Select(artigo =>
                            {
                                var _artigo = BSO.Base.Artigos.Consulta(artigo, false);
                                return _artigo is null ? "" : _artigo.Familia;
                            }).Distinct());

                        if (!ApiClient.Intialized())
                        {
                            ErpBS100.ErpBS _BSO = BSO;
                            StdPlatBS100.StdBSInterfPub _PSO = PSO;

                            ApiClient.InitializeApiClient(ref _BSO, ref _PSO);

                            if (!ApiClient.Intialized())
                                strErrors += string.Format(Properties.Resources.RES_NaoExisteConfi, BSO.Contexto.CodEmp) + Environment.NewLine;
                            else
                                requestResult = ApiClient.GetRequisitionPayNextStage(clsDocCompra.CamposUtil["CDU_CodOmnia"].Valor.ToString(), clsDocCompra.TotalDocumento.ToString(), familias).GetAwaiter().GetResult();

                        }
                        else
                            requestResult = ApiClient.GetRequisitionPayNextStage(clsDocCompra.CamposUtil["CDU_CodOmnia"].Valor.ToString(), clsDocCompra.TotalDocumento.ToString(), familias).GetAwaiter().GetResult();

                        GetRequisitionPayNextStageBody result = null;

                        if (requestResult.ContainsKey("errorMessage"))
                        {
                            strErrors += requestResult["errorMessage"].ToString() + Environment.NewLine;
                        }
                        else
                        {
                            result = (GetRequisitionPayNextStageBody)requestResult["result"];
                        }

                        if (result != null && result.ultimaEtapa.HasValue && !result.ultimaEtapa.Value)
                        {
                            if (!string.IsNullOrEmpty(result.approvalProfile))
                                clsDocCompra.CamposUtil["CDU_Atribuido"].Valor = Convert.ToInt32(result.approvalProfile);

                            if (!string.IsNullOrEmpty(result.order.ToString()))
                                clsDocCompra.CamposUtil["CDU_Etapa"].Valor = result.order;

                            if (clsDocCompra.CamposUtil["CDU_Estado"].Valor == null || string.IsNullOrEmpty(clsDocCompra.CamposUtil["CDU_Estado"].Valor.ToString()))
                                clsDocCompra.CamposUtil["CDU_Estado"].Valor = "OnApproval";

                            if (result.flow.Count > 0)
                            {
                                foreach (var flow in result.flow)
                                {
                                    string sql = Sql.InsertIntoFinancialDocumentPaymentApprovalFlow;

                                    sql = PSO.Sql.FormatSQLExt(sql, clsDocCompra.ID, flow.approvalProfile, flow.order, flow.amount);

                                    BSO.DSO.ExecuteSQL(sql);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strErrors += ex.Message + Environment.NewLine;
            }
        }

        public override void DepoisDeGravar(CmpBEDocumentoCompra clsDocCompra, ref string strAvisos, ref string IdDocLiqRet, ref string IdDocLiqRetGar, ExtensibilityEventArgs e)
        {
            if (clsDocCompra.CamposUtil["CDU_CodOmnia"] != null && clsDocCompra.CamposUtil["CDU_CodOmnia"].Valor != null && !string.IsNullOrEmpty(clsDocCompra.CamposUtil["CDU_CodOmnia"].Valor.ToString()))
            {
                string omniaCode = clsDocCompra.CamposUtil["CDU_CodOmnia"].Valor.ToString();
                CmpBETabCompra docConfig = BSO.Compras.TabCompras.Edita(clsDocCompra.Tipodoc);

                try
                {
                    if (docConfig.TipoDocumento == 0 && !clsDocCompra.EmModoEdicao)
                    {
                        Dictionary<string, object> requestResult = new Dictionary<string, object>();

                        if (!ApiClient.Intialized())
                        {
                            ErpBS100.ErpBS _BSO = BSO;
                            StdPlatBS100.StdBSInterfPub _PSO = PSO;

                            ApiClient.InitializeApiClient(ref _BSO, ref _PSO);

                            if (!ApiClient.Intialized())
                                strErrors += string.Format(Properties.Resources.RES_NaoExisteConfi, BSO.Contexto.CodEmp) + Environment.NewLine;
                            else
                                requestResult = ApiClient.NewQuoteRequestNotification(clsDocCompra.Tipodoc, clsDocCompra.Serie, clsDocCompra.NumDoc, clsDocCompra.Entidade).GetAwaiter().GetResult();

                        }
                        else
                            requestResult = ApiClient.NewQuoteRequestNotification(clsDocCompra.Tipodoc, clsDocCompra.Serie, clsDocCompra.NumDoc, clsDocCompra.Entidade).GetAwaiter().GetResult();

                        if (requestResult.ContainsKey("errorMessage"))
                            strErrors += requestResult["errorMessage"].ToString() + Environment.NewLine;
                    }

                    if (docConfig.TipoDocumento == 1)
                    {
                        Dictionary<string, object> requestResult = new Dictionary<string, object>();

                        if (!ApiClient.Intialized())
                        {
                            ErpBS100.ErpBS _BSO = BSO;
                            StdPlatBS100.StdBSInterfPub _PSO = PSO;

                            ApiClient.InitializeApiClient(ref _BSO, ref _PSO);

                            if (!ApiClient.Intialized())
                                strErrors += string.Format(Properties.Resources.RES_NaoExisteConfi, BSO.Contexto.CodEmp) + Environment.NewLine;
                            else
                                requestResult = ApiClient.ValidateRequisition(omniaCode, 0).GetAwaiter().GetResult();

                        }
                        else
                            requestResult = ApiClient.ValidateRequisition(omniaCode, 0).GetAwaiter().GetResult();

                        if (requestResult.ContainsKey("errorMessage"))
                            strErrors += requestResult["errorMessage"].ToString() + Environment.NewLine;
                    }

                    if (docConfig.TipoDocumento == 2)
                    {
                        Dictionary<string, object> requestResult = new Dictionary<string, object>();

                        if (!ApiClient.Intialized())
                        {
                            ErpBS100.ErpBS _BSO = BSO;
                            StdPlatBS100.StdBSInterfPub _PSO = PSO;

                            ApiClient.InitializeApiClient(ref _BSO, ref _PSO);

                            if (!ApiClient.Intialized())
                                strErrors += string.Format(Properties.Resources.RES_NaoExisteConfi, BSO.Contexto.CodEmp) + Environment.NewLine;
                            else
                                requestResult = ApiClient.ValidateRequisition(omniaCode, 3).GetAwaiter().GetResult();

                        }
                        else
                            requestResult = ApiClient.ValidateRequisition(omniaCode, 3).GetAwaiter().GetResult();

                        if (requestResult.ContainsKey("errorMessage"))
                            strErrors += requestResult["errorMessage"].ToString() + Environment.NewLine;
                    }

                    if (docConfig.TipoDocumento == 4 && !clsDocCompra.EmModoEdicao && string.IsNullOrEmpty(strErrors))
                    {
                        string user = string.Empty;
                        string sqlGetApproverUser = Sql.GetFromPRIOMNIA_Approver;
                        sqlGetApproverUser = sqlGetApproverUser.Replace("@1@", clsDocCompra.CamposUtil["CDU_CodOmnia"].Valor.ToString());
                        sqlGetApproverUser = sqlGetApproverUser.Replace("@2@", clsDocCompra.Tipodoc);
                        sqlGetApproverUser = sqlGetApproverUser.Replace("@3@", "OMNIA_" + ApiClient.GetOmniaDB());

                        StdBELista userResult = BSO.Consulta(sqlGetApproverUser);

                        if (userResult != null)
                        {
                            if (!userResult.Vazia())
                            {
                                user = userResult.Valor("Approver").ToString();
                            }
                        }

                        Dictionary<string, object> requestResult = new Dictionary<string, object>();

                        if (!ApiClient.Intialized())
                        {
                            ErpBS100.ErpBS _BSO = BSO;
                            StdPlatBS100.StdBSInterfPub _PSO = PSO;

                            ApiClient.InitializeApiClient(ref _BSO, ref _PSO);

                            if (!ApiClient.Intialized())
                                strErrors += string.Format(Properties.Resources.RES_NaoExisteConfi, BSO.Contexto.CodEmp) + Environment.NewLine;
                            else
                                requestResult = ApiClient.NewFinancialDocumentNotification(clsDocCompra.Tipodoc, clsDocCompra.Serie, clsDocCompra.NumDoc, clsDocCompra.Entidade,
                                    clsDocCompra.CamposUtil["CDU_OrganizationalUnit"].Valor.ToString(), clsDocCompra.DataDoc.Date.ToString(), clsDocCompra.CamposUtil["CDU_Autor"].Valor.ToString(), user).GetAwaiter().GetResult();

                        }
                        else
                            requestResult = ApiClient.NewFinancialDocumentNotification(clsDocCompra.Tipodoc, clsDocCompra.Serie, clsDocCompra.NumDoc, clsDocCompra.Entidade,
                                    clsDocCompra.CamposUtil["CDU_OrganizationalUnit"].Valor.ToString(), clsDocCompra.DataDoc.Date.ToString(), clsDocCompra.CamposUtil["CDU_Autor"].Valor.ToString(), user).GetAwaiter().GetResult();

                        if (requestResult.ContainsKey("errorMessage"))
                            strErrors += requestResult["errorMessage"].ToString() + Environment.NewLine;
                    }


                    if (!string.IsNullOrEmpty(strErrors))
                    {
                        strAvisos += Environment.NewLine + strErrors;
                    }

                }
                catch (Exception ex)
                {
                    strErrors += Environment.NewLine + ex.Message;
                }
            }
        }
    }
}
