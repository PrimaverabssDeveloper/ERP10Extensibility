using System;
using System.Collections.Generic;
using InvBE100;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Inventory.Services;
using Primavera.TemplateProcurement.Extensibility.Ext;
using Primavera.TemplateProcurement.Extensibility.Resource;
using StdBE100;

namespace Primavera.TemplateProcurement.Extensibility.Inventory
{
    public class BSTransferencias : InvBSTransferencias
    {
        public override void DepoisDeGravar(InvBEDocumentoTransf Documento, ref string strErros, ExtensibilityEventArgs e)
        {

            e.HandleExceptions = true;

            foreach (InvBELinhaOrigemTransf linha in Documento.LinhasOrigem)
            {
                if (linha.ModuloOrigemCopia == "N" && !string.IsNullOrEmpty(linha.IdLinhaOrigemCopia))
                {
                    try
                    {
                        string getCodOmnia = PSO.Sql.FormatSQLExt(Sql.GetInternos_CodOmniaByLineId, new object[] { linha.IdLinhaOrigemCopia });

                        StdBELista resultCodOmnia = BSO.Consulta(getCodOmnia);

                        if (resultCodOmnia != null)
                        {
                            if (!resultCodOmnia.Vazia())
                            {
                                string omniaCode = resultCodOmnia.Valor("CDU_CodOmnia").ToString();

                                if (!string.IsNullOrEmpty(omniaCode))
                                {
                                    InvBETabTransferencia docConfig = BSO.Inventario.TabTransferencias.Edita(Documento.Tipodoc);

                                    if (docConfig.TransfereArmazem && docConfig.TransfereLocalizacao && Helper.IsRISatisfied(omniaCode, BSO, PSO))
                                    {
                                        Dictionary<string, object> requestResult = ApiClient.ValidateRequisition(omniaCode, 2).GetAwaiter().GetResult();

                                        if (requestResult.ContainsKey("errorMessage"))
                                            strErros += Environment.NewLine + requestResult["errorMessage"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        strErros += Environment.NewLine + ex.Message;
                    }
                }
            }
        }
    }
}
