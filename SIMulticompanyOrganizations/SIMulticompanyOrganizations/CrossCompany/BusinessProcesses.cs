using SUGIMPL_OME.Helpers;
using ErpBS100;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using StdBE100;
using VndBE100;
using CmpBE100;
using BasBE100;
using DevExpress.Utils.DirectXPaint;

namespace SUGIMPL_OME.CrossCompany
{
    class BusinessProcesses
    {
        internal static Dictionary<String, int> CheckPendingDocuments(ERPContext oERPContext)
        {
            Dictionary<String, int> result = new Dictionary<string, int>();
            Dictionary<String, String> groupCompanies = CrossCompany.Platform.GetGroupCompanies(oERPContext);

            //Exit if no companies where found 
            if (groupCompanies.Count == 0)
                return result;

            result.Add("Purchases", 0);
            result.Add("Sales", 0);

            //Load the documents to import from all the group companies
            groupCompanies.Remove(oERPContext.BSO.Contexto.CodEmp);
            foreach (string groupCompany in groupCompanies.Keys)
            {
                ErpBS oCompany = new ErpBS();

                oCompany.AbreEmpresaTrabalho(
                    StdBE100.StdBETipos.EnumTipoPlataforma.tpEmpresarial,
                    groupCompany,
                    Properties.Settings.Default.User,
                    Properties.Settings.Default.Password);


                String strSQL = String.Format(
                "select sum(pur) Purchases, sum(sls) Sales " +
                "from(" +
                "   select count(*) pur, 0 sls from cabecdoc cd inner join CabecDocStatus cds on cds.IdCabecDoc=cd.Id left join documentosvenda dv on cd.tipodoc = dv.documento " +
                "   where dv.cdu_exportagrupo = 1 AND cd.cdu_exportado = 0  AND cds.Anulado=0 AND cd.entidade = '{0}' " +
                "   UNION ALL " +
                "   select 0 pur, count(*) pur from cabeccompras cc inner join CabecComprasStatus ccs on ccs.IdCabecCompras=cc.Id left join documentoscompra dc on cc.tipodoc = dc.documento " +
                "   where dc.cdu_exportagrupo = 1 AND isnull(cc.cdu_exportado, 0) = 0 AND ccs.Anulado=0 AND cc.entidade = '{0}' " +
                "   ) as tmp"
                , oERPContext.BSO.Contexto.CodEmp);

                StdBELista lstPendDocs = oCompany.Consulta(strSQL);

                if (!lstPendDocs.Vazia())
                {
                    if (lstPendDocs.DaValor<int>("Purchases") > 0)
                    {
                        result["Purchases"] += lstPendDocs.DaValor<int>("Purchases");
                    }

                    if (lstPendDocs.DaValor<int>("Sales") > 0)
                    {
                        result["Sales"] += lstPendDocs.DaValor<int>("Sales");
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Get the list of documents pending to import.
        /// </summary>
        /// <param name="oERPContext"></param>
        /// <returns>STDBELista containing all the documents pendin to import.</returns>
        internal static StdBELista GetDocumentsToImport(ERPContext oERPContext)
        {

            Dictionary<String, String> groupCompanies = CrossCompany.Platform.GetGroupCompanies(oERPContext);

            //Exit if no companies where found 
            if (groupCompanies.Count == 0)
                return null;

            //Load the documents to import from all the group companies
            groupCompanies.Remove(oERPContext.BSO.Contexto.CodEmp);
            String strQRY = string.Empty;
            String strUNION = string.Empty;
            foreach (string groupCompany in groupCompanies.Keys)
            {
                //Union
                strQRY += strUNION;
                strUNION = "UNION ALL ";

                //Purchases
                strQRY += string.Format(
                    "select " +
                    "   Grp1        = ''," +
                    "   IDDoc       = cd.id," +
                    "   Sel         = 0," +
                    "   DocType     = 'Compra'," +
                    "   Company     = '{0}'," +
                    "   Document    = cd.tipodoc + ' ' + cd.Serie + '/' + cast(cd.numdoc as nvarchar)," +
                    "   Date        = cast(cd.data as smalldatetime)," +
                    "   Total       = totalmerc + totaloutros - totaldesc + totaliva," +
                    "   TargetDoc   = '', " +
                    "   ImportNotes = '' " +
                    "from PRI{0}..cabecdoc cd " +
                    "   inner join PRI{0}..CabecDocStatus cds ON cds.IdCabecDoc = cd.id " +
                    "   left join PRI{0}..documentosvenda dv on cd.tipodoc=dv.documento " +
                    "where dv.cdu_exportagrupo=1 and cds.Anulado = 0 " +
                    "   AND cd.cdu_exportado=0 " +
                    "   AND cd.entidade='{1}'",
                    groupCompany, oERPContext.BSO.Contexto.CodEmp);

                //Union
                strQRY += strUNION;

                //Orders
                strQRY += string.Format(
                    "select " +
                    "   Grp1        = ''," +
                    "   IDDoc       = cc.id," +
                    "   Sel         = 0," +
                    "   DocType     = 'Encomenda'," +
                    "   Company     = '{0}'," +
                    "   Document    = cc.tipodoc + ' ' + cc.Serie + '/' + cast(cc.numdoc as nvarchar)," +
                    "   Date        = cast(cc.datadoc as smalldatetime)," +
                    "   Total       = totalmerc + totaloutros - totaldesc + totaliva," +
                    "   TargetDoc   = '', " +
                    "   ImportNotes = '' " +
                    "from PRI{0}..cabeccompras cc " +
                    "   inner join PRI{0}..CabecComprasStatus ccs ON ccs.IdCabecCompras = cc.ID " +
                    "   left join PRI{0}..documentoscompra dc on cc.tipodoc=dc.documento " +
                    "where dc.cdu_exportagrupo=1 and ccs.Anulado = 0 " +
                    "   AND cc.cdu_exportado=0 " +
                    "   AND cc.entidade='{1}'",
                    groupCompany, oERPContext.BSO.Contexto.CodEmp);
            }

            if (!string.IsNullOrEmpty(strQRY))
                return oERPContext.BSO.Consulta(strQRY);
            else
                return null;
        }


        /// <summary>
        /// Imports a sales document.
        /// </summary>
        /// <param name="oERPContext"></param>
        /// <param name="Company"></param>
        /// <param name="IdDoc"></param>
        /// <returns>The reference of the created document or the error text.</returns>
        internal static Tuple<string, string> ImportSalesDocument(ERPContext oERPContext, String Company, String IdDoc)
        {
            Tuple<string, string> retValue = new Tuple<string, string>(String.Empty, String.Empty);
            ErpBS oCompany = new ErpBS();
            string strErrWarn = string.Empty;

            try
            {
                oCompany.AbreEmpresaTrabalho(
                    StdBE100.StdBETipos.EnumTipoPlataforma.tpEmpresarial,
                    Company,
                    Properties.Settings.Default.User,
                    Properties.Settings.Default.Password);

                VndBEDocumentoVenda sourceDocument = oCompany.Vendas.Documentos.EditaID(IdDoc);
                String targetDocumentType = oCompany.Vendas.TabVendas.DaValorAtributo(sourceDocument.Tipodoc, "CDU_DocDestino");


                //Error if the target document not exists in the target company
                if (!oERPContext.BSO.Compras.TabCompras.Existe(targetDocumentType))
                {
                    throw new Exception(String.Format("O tipo de documento {0} não existe na empresa atual.", targetDocumentType));
                }
                CmpBETabCompra purchasesTable = oERPContext.BSO.Compras.TabCompras.Edita(targetDocumentType);


                //NEW DOCUMENT
                CmpBEDocumentoCompra targetDocument = new CmpBEDocumentoCompra();
                targetDocument.Filial = "000";
                targetDocument.Serie = oERPContext.BSO.Base.Series.DaSerieDefeito("C", targetDocumentType);
                targetDocument.Tipodoc = targetDocumentType;
                targetDocument.TipoEntidade = "F";
                targetDocument.DataIntroducao = DateTime.Now;

                //Error if the entity doesnt exists in the target company
                if (!oERPContext.BSO.Base.Fornecedores.Existe(Company))
                {
                    throw new Exception(String.Format("O fornecedor {0} não existe na empresa atual.", Company));
                }
                targetDocument.Entidade = Company;

                targetDocument.DataDoc = sourceDocument.DataDoc;
                int preencheDadosTodos = (int)BasBETiposGcp.PreencheRelacaoCompras.compDadosTodos;
                targetDocument = oERPContext.BSO.Compras.Documentos.PreencheDadosRelacionados(targetDocument, ref preencheDadosTodos);
                if (targetDocument.DataVenc <= new DateTime(2000, 01, 01))
                    targetDocument.DataVenc = sourceDocument.DataVenc;
                if (string.IsNullOrEmpty(targetDocument.CondPag))
                    targetDocument.CondPag = sourceDocument.CondPag;
                if (string.IsNullOrEmpty(targetDocument.ModoPag))
                    targetDocument.ModoPag = sourceDocument.ModoPag;
                targetDocument.NumDocExterno = targetDocument.NumDoc.ToString();
                targetDocument.DescFinanceiro = sourceDocument.DescFinanceiro;
                targetDocument.DescFornecedor = sourceDocument.DescEntidade;
                targetDocument.CamposUtil["CDU_Exportado"].Valor = 1;

                //NEW DOCUMENT DETAILS
                foreach (VndBELinhaDocumentoVenda detailSourceDocument in sourceDocument.Linhas)
                {
                    double quantity = detailSourceDocument.Quantidade;
                    string targetWarehouse = oERPContext.BSO.Base.Artigos.DaValorAtributo(detailSourceDocument.Artigo, "ArmazemSugestao") ?? detailSourceDocument.Armazem;
                    string targetWarehouseLocation = oERPContext.BSO.Base.Artigos.DaValorAtributo(detailSourceDocument.Artigo, "LocalizacaoSugestao") ?? detailSourceDocument.Localizacao;
                    targetDocument = oERPContext.BSO.Compras.Documentos.AdicionaLinha(
                        targetDocument,
                        detailSourceDocument.Artigo,
                        ref quantity,
                        ref targetWarehouse,
                        ref targetWarehouseLocation,
                        detailSourceDocument.PrecUnit,
                        detailSourceDocument.Desconto1);
                }

                //SAVE
                if (!oERPContext.BSO.Compras.Documentos.ValidaActualizacao(targetDocument, purchasesTable, ref strErrWarn))
                {
                    throw new Exception(String.Format("Erro: {0}.", strErrWarn));
                }
                else
                {
                    oERPContext.BSO.Compras.Documentos.Actualiza(targetDocument, ref strErrWarn);

                    retValue = Tuple.Create<string, string>(
                        string.Format("{0} {1}/{2}",targetDocument.Tipodoc, targetDocument.Serie, targetDocument.NumDoc.ToString()), 
                        strErrWarn);

                    oCompany.DSO.ExecuteSQL(string.Format("UPDATE CabecDoc SET CDU_Exportado=1 WHERE ID='{0}'", sourceDocument.ID));
                    //TODO: Eliminar (foi adicionado porque o objeto não estava a gravar os valores dos CDUs)
                    oERPContext.BSO.DSO.ExecuteSQL(string.Format("UPDATE CabecCompras SET CDU_Exportado=1 WHERE ID='{0}'", targetDocument.ID));
                }
            }
            catch (Exception e)
            {
                retValue = Tuple.Create<string, string>("ERRO", e.Message);
            }
            finally
            {
                if (oCompany != null)
                    oCompany.FechaEmpresaTrabalho();
            }

            return retValue;
        }


        /// <summary>
        /// Import a purchases document.
        /// </summary>
        /// <param name="oERPContext"></param>
        /// <param name="Company"></param>
        /// <param name="IdDoc"></param>
        /// <returns>The reference of the created document or the error text.</returns>
        internal static Tuple<string, string> ImportPurchasesDocument(ERPContext oERPContext, String Company, String IdDoc)
        {
            Tuple<string, string> retValue = new Tuple<string, string>(String.Empty, String.Empty);
            ErpBS oCompany = new ErpBS();
            string strErrWarn = string.Empty;

            try
            {
                oCompany.AbreEmpresaTrabalho(
                    StdBE100.StdBETipos.EnumTipoPlataforma.tpEmpresarial,
                    Company,
                    Properties.Settings.Default.User,
                    Properties.Settings.Default.Password);

                CmpBEDocumentoCompra sourceDocument = oCompany.Compras.Documentos.EditaID(IdDoc);
                String targetDocumentType = oCompany.Compras.TabCompras.DaValorAtributo(sourceDocument.Tipodoc, "CDU_DocDestino");


                //Error if the target document not exists in the target company
                if (!oERPContext.BSO.Vendas.TabVendas.Existe(targetDocumentType))
                {
                    throw new Exception(String.Format("O tipo de documento {0} não existe na empresa atual.", targetDocumentType));
                }
                VndBETabVenda salesTable = oERPContext.BSO.Vendas.TabVendas.Edita(targetDocumentType);


                //NEW DOCUMENT
                VndBEDocumentoVenda targetDocument = new VndBEDocumentoVenda();
                targetDocument.Filial = "000";
                targetDocument.Serie = oERPContext.BSO.Base.Series.DaSerieDefeito("V", targetDocumentType);
                targetDocument.Tipodoc = targetDocumentType;
                targetDocument.TipoEntidade = "C";

                //Error if the entity doesnt exists in the target company
                if (!oERPContext.BSO.Base.Clientes.Existe(Company))
                {
                    throw new Exception(String.Format("O cliente {0} não existe na empresa atual.", Company));
                }
                targetDocument.Entidade = Company;

                targetDocument.DataDoc = sourceDocument.DataDoc;
                int preencheDadosTodos = (int)BasBETiposGcp.PreencheRelacaoVendas.vdDadosTodos;
                targetDocument = oERPContext.BSO.Vendas.Documentos.PreencheDadosRelacionados(targetDocument, ref preencheDadosTodos);
                if (targetDocument.DataVenc == default(DateTime))
                    targetDocument.DataVenc = sourceDocument.DataVenc;
                if (string.IsNullOrEmpty(targetDocument.CondPag))
                    targetDocument.CondPag =  sourceDocument.CondPag;
                if (string.IsNullOrEmpty(targetDocument.ModoPag))
                    targetDocument.ModoPag = sourceDocument.ModoPag;
                targetDocument.DescFinanceiro = sourceDocument.DescFinanceiro;
                targetDocument.DescEntidade = sourceDocument.DescFornecedor;
                targetDocument.CamposUtil["CDU_Exportado"].Valor = 1;

                //NEW DOCUMENT DETAILS
                foreach (CmpBELinhaDocumentoCompra detailSourceDocument in sourceDocument.Linhas)
                {
                    double quantity = detailSourceDocument.Quantidade;
                    string targetWarehouse = oERPContext.BSO.Base.Artigos.DaValorAtributo(detailSourceDocument.Artigo, "ArmazemSugestao") ?? detailSourceDocument.Armazem;
                    string targetWarehouseLocation = oERPContext.BSO.Base.Artigos.DaValorAtributo(detailSourceDocument.Artigo, "LocalizacaoSugestao") ?? detailSourceDocument.Localizacao;
                    targetDocument = oERPContext.BSO.Vendas.Documentos.AdicionaLinha(
                        targetDocument,
                        detailSourceDocument.Artigo,
                        ref quantity,
                        ref targetWarehouse,
                        ref targetWarehouseLocation,
                        detailSourceDocument.PrecUnit,
                        detailSourceDocument.Desconto1);
                }

                //SAVE
                //string settlementSeries = string.Empty;
                //if (!oERPContext.BSO.Vendas.Documentos.ValidaActualizacao(targetDocument, salesTable, ref settlementSeries, ref strErrWarn))
                //{
                //    throw new Exception(strErrWarn);
                //}
                //else
                //{
                    oERPContext.BSO.Vendas.Documentos.Actualiza(targetDocument, ref strErrWarn);

                    retValue = Tuple.Create<string, string>(
                        string.Format("{0} {1}/{2}", targetDocument.Tipodoc, targetDocument.Serie, targetDocument.NumDoc.ToString()),
                        strErrWarn);

                    oCompany.DSO.ExecuteSQL(string.Format("UPDATE CabecCompras SET CDU_Exportado=1 WHERE ID='{0}'", sourceDocument.ID));
                    //TODO: Eliminar (foi adicionado porque o objeto não estava a gravar os valores dos CDUs)
                    oERPContext.BSO.DSO.ExecuteSQL(string.Format("UPDATE CabecDoc SET CDU_Exportado=1 WHERE ID='{0}'", targetDocument.ID));
                //}
            }
            catch (Exception e)
            {
                retValue = Tuple.Create<string, string>("ERRO", e.Message);
            }
            finally
            {
                if (oCompany != null)
                    oCompany.FechaEmpresaTrabalho();
            }

            return retValue;
        }

    }
}
