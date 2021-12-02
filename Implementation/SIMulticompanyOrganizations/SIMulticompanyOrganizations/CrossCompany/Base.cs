using BasBE100;
using CblBE100;
using CrmBE100;
using ErpBS100;
using ICblBS100;
using ICrmBS100;
using Primavera.Extensibility.Base.Services;
using SUGIMPL_OME.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace SUGIMPL_OME.CrossCompany
{
    class Base
    {

        /// <summary>
        /// Update the Item in the group companies
        /// </summary>
        /// <param name="oERPContext"></param>
        /// <param name="Artigo"></param>
        /// <returns>List<String> of the updated companies</returns>
        internal static List<String> UpdateItem_GroupCompanies(ERPContext oERPContext, String Item)
        {

            Dictionary<String, String> groupCompanies = CrossCompany.Platform.GetGroupCompanies(oERPContext);
            List<String> updatedCompanies = new List<string>();

            //Exit if no companies where found 
            if (groupCompanies.Count == 0)
                return updatedCompanies;

            groupCompanies.Remove(oERPContext.BSO.Contexto.CodEmp);
            foreach (string company in groupCompanies.Keys)
            {
                ErpBS oCompany = new ErpBS();

                oCompany.AbreEmpresaTrabalho(
                    StdBE100.StdBETipos.EnumTipoPlataforma.tpEmpresarial,
                    company,
                    Properties.Settings.Default.User,
                    Properties.Settings.Default.Password);

                //Create or update the item
                BasBEArtigo oItem = oERPContext.BSO.Base.Artigos.Edita(Item);
                BasBEArtigoMoeda oItemPrices = oERPContext.BSO.Base.ArtigosPrecos.Edita(Item, "EUR", oERPContext.BSO.Base.Artigos.DaValorAtributo(Item, "UnidadeBase"));
                if (!oCompany.Base.Artigos.Existe(Item))
                {
                    oItem.EmModoEdicao = false;
                    oItemPrices.EmModoEdicao = false;
                }
                oCompany.Base.Artigos.Actualiza(oItem);
                oCompany.Base.ArtigosPrecos.Actualiza(oItemPrices);
                updatedCompanies.Add(company);

                oCompany.FechaEmpresaTrabalho();
            }

            return updatedCompanies;
        }

        internal static List<String> UpdateEntity(ERPContext oERPContext, string EntityType, string Entity)
        {
            Dictionary<String, String> groupCompanies = CrossCompany.Platform.GetGroupCompanies(oERPContext);
            List<String> updatedCompanies = new List<string>();

            //Exit if no companies where found 
            if (groupCompanies.Count == 0)
                return updatedCompanies;

            groupCompanies.Remove(oERPContext.BSO.Contexto.CodEmp);
            foreach (string groupCompany in groupCompanies.Keys)
            {
                ErpBS oCompany = new ErpBS();

                oCompany.AbreEmpresaTrabalho(
                    StdBE100.StdBETipos.EnumTipoPlataforma.tpEmpresarial,
                    groupCompany,
                    Properties.Settings.Default.User,
                    Properties.Settings.Default.Password);

                //get the last exercise (to create the entity accounts for each exercise)
                int lastYear = oERPContext.BSO.Contabilidade.ExerciciosCBL.DaUltimoAno();

                switch (EntityType)
                {
                    case "C":
                        if (Convert.ToBoolean(oERPContext.BSO.Base.Clientes.DaValorAtributo(Entity, "CDU_EntidadeGrupo")))
                        {
                            //Entity
                            BasBECliente objNewEntity = oERPContext.BSO.Base.Clientes.Edita(Entity);
                            if (!oCompany.Base.Clientes.Existe(Entity))
                            {
                                objNewEntity.EmModoEdicao = false;
                            }
                            oCompany.Base.Clientes.Actualiza(objNewEntity);
                            updatedCompanies.Add(groupCompany);

                            //Connection to CBL
                            for (int currentYear = DateTime.Now.Year; currentYear <= lastYear; currentYear++)
                            {
                                CblBECnfTabLinhaLigCBL objNewLinhaCnfTabLigCBL = oERPContext.BSO.Contabilidade.ConfiguracaoTabCBL.Edita(CblBE100.CblBECnfTabLinhaLigCBL.TETipoTabela.GCPClientes, currentYear, "001", Entity, 1);
                                if (objNewLinhaCnfTabLigCBL != null)
                                {
                                    if (oCompany.Contabilidade.ConfiguracaoTabCBL.ExisteID(objNewLinhaCnfTabLigCBL.Id))
                                    {
                                        oCompany.Contabilidade.ConfiguracaoTabCBL.ActualizaValorAtributoID(objNewLinhaCnfTabLigCBL.Id, "Conta", objNewLinhaCnfTabLigCBL.Conta);
                                    }
                                    else
                                    {
                                        objNewLinhaCnfTabLigCBL.EmModoEdicao = false;
                                        CblBECnfTabLigCBL objNewCnfTabLigCBL = oCompany.Contabilidade.ConfiguracaoTabCBL.EditaTabela(CblBECnfTabLinhaLigCBL.TETipoTabela.GCPClientes);
                                        objNewCnfTabLigCBL.PlanoExercicios.GetEdita(1).Linhas.Insere(objNewLinhaCnfTabLigCBL);
                                        oCompany.Contabilidade.ConfiguracaoTabCBL.Actualiza(objNewCnfTabLigCBL);
                                    }
                                }
                            }
                        }
                        break;
                    case "F":
                        if (Convert.ToBoolean(oERPContext.BSO.Base.Fornecedores.DaValorAtributo(Entity, "CDU_EntidadeGrupo")))
                        {
                            //Entity
                            BasBEFornecedor objNewEntity = oERPContext.BSO.Base.Fornecedores.Edita(Entity);
                            if (!oCompany.Base.Fornecedores.Existe(Entity))
                            {
                                objNewEntity.EmModoEdicao = false;
                            }
                            oCompany.Base.Fornecedores.Actualiza(objNewEntity);
                            updatedCompanies.Add(groupCompany);

                            //Connection to CBL
                            for (int currentYear = DateTime.Now.Year; currentYear <= lastYear; currentYear++)
                            {
                                CblBECnfTabLinhaLigCBL objNewLinhaCnfTabLigCBL = oERPContext.BSO.Contabilidade.ConfiguracaoTabCBL.Edita(CblBE100.CblBECnfTabLinhaLigCBL.TETipoTabela.GCPFornecedores, currentYear, "001", Entity, 1);
                                if (objNewLinhaCnfTabLigCBL != null)
                                {
                                    if (oCompany.Contabilidade.ConfiguracaoTabCBL.ExisteID(objNewLinhaCnfTabLigCBL.Id))
                                    {
                                        oCompany.Contabilidade.ConfiguracaoTabCBL.ActualizaValorAtributoID(objNewLinhaCnfTabLigCBL.Id, "Conta", objNewLinhaCnfTabLigCBL.Conta);
                                    }
                                    else
                                    {
                                        objNewLinhaCnfTabLigCBL.EmModoEdicao = false;
                                        CblBECnfTabLigCBL objNewCnfTabLigCBL = oCompany.Contabilidade.ConfiguracaoTabCBL.EditaTabela(CblBECnfTabLinhaLigCBL.TETipoTabela.GCPFornecedores);
                                        objNewCnfTabLigCBL.PlanoExercicios.GetEdita(1).Linhas.Insere(objNewLinhaCnfTabLigCBL);
                                        oCompany.Contabilidade.ConfiguracaoTabCBL.Actualiza(objNewCnfTabLigCBL);
                                    }
                                }
                            }
                        }
                        break;
                    case "R":
                    case "D":
                        if (Convert.ToBoolean(oERPContext.BSO.Base.OutrosTerceiros.DaValorAtributo(Entity, EntityType, "CDU_EntidadeGrupo")))
                        {
                            //Entity
                            BasBEOutroTerceiro objNewEntity = oERPContext.BSO.Base.OutrosTerceiros.Edita(Entity);
                            if (!oCompany.Base.OutrosTerceiros.Existe(Entity))
                            {
                                objNewEntity.EmModoEdicao = false;
                            }
                            oCompany.Base.OutrosTerceiros.Actualiza(objNewEntity);
                            updatedCompanies.Add(groupCompany);

                            //Connection to CBL
                            for (int currentYear = DateTime.Now.Year; currentYear <= lastYear; currentYear++)
                            {
                                CblBECnfTabLinhaLigCBL objNewLinhaCnfTabLigCBL = oERPContext.BSO.Contabilidade.ConfiguracaoTabCBL.Edita(CblBE100.CblBECnfTabLinhaLigCBL.TETipoTabela.GCPOutrosTerceiros, currentYear, "001", Entity, 1);
                                if (objNewLinhaCnfTabLigCBL != null)
                                {
                                    if (oCompany.Contabilidade.ConfiguracaoTabCBL.ExisteID(objNewLinhaCnfTabLigCBL.Id))
                                    {
                                        oCompany.Contabilidade.ConfiguracaoTabCBL.ActualizaValorAtributoID(objNewLinhaCnfTabLigCBL.Id, "Conta", objNewLinhaCnfTabLigCBL.Conta);
                                    }
                                    else
                                    {
                                        objNewLinhaCnfTabLigCBL.EmModoEdicao = false;
                                        CblBECnfTabLigCBL objNewCnfTabLigCBL = oCompany.Contabilidade.ConfiguracaoTabCBL.EditaTabela(CblBECnfTabLinhaLigCBL.TETipoTabela.GCPOutrosTerceiros);
                                        objNewCnfTabLigCBL.PlanoExercicios.GetEdita(1).Linhas.Insere(objNewLinhaCnfTabLigCBL);
                                        oCompany.Contabilidade.ConfiguracaoTabCBL.Actualiza(objNewCnfTabLigCBL);
                                    }
                                }
                            }
                        }
                        break;
                    case "E":
                        if (Convert.ToBoolean(oERPContext.BSO.CRM.EntidadesExternas.DaValorAtributo(Entity, "CDU_EntidadeGrupo")))
                        {
                            //Entity
                            CrmBEEntidadeExterna objNewEntity = oERPContext.BSO.CRM.EntidadesExternas.Edita(Entity);
                            if (!oCompany.CRM.EntidadesExternas.Existe(Entity))
                            {
                                objNewEntity.EmModoEdicao = false;
                            }
                            oCompany.CRM.EntidadesExternas.Actualiza(objNewEntity);
                            updatedCompanies.Add(groupCompany);
                        }
                        break;
                    default:
                        break;
                }

                oCompany.FechaEmpresaTrabalho();
            }

            return updatedCompanies;
        }


        internal static List<String> RemoveEntity(ERPContext oERPContext, string EntityType, string Entity)
        {
            Dictionary<String, String> groupCompanies = CrossCompany.Platform.GetGroupCompanies(oERPContext);
            List<String> updatedCompanies = new List<string>();

            //Exit if no companies where found 
            if (groupCompanies.Count == 0)
                return updatedCompanies;

            groupCompanies.Remove(oERPContext.BSO.Contexto.CodEmp);
            foreach (string groupCompany in groupCompanies.Keys)
            {
                ErpBS oCompany = new ErpBS();

                oCompany.AbreEmpresaTrabalho(
                    StdBE100.StdBETipos.EnumTipoPlataforma.tpEmpresarial,
                    groupCompany,
                    Properties.Settings.Default.User,
                    Properties.Settings.Default.Password);

                switch (EntityType)
                {
                    case "C":
                        if (Convert.ToBoolean(oERPContext.BSO.Base.Clientes.DaValorAtributo(Entity, "CDU_EntidadeGrupo")))
                        {
                            oCompany.Base.Clientes.Remove(Entity);
                            updatedCompanies.Add(groupCompany);
                        }
                        break;
                    case "F":
                        if (Convert.ToBoolean(oERPContext.BSO.Base.Fornecedores.DaValorAtributo(Entity, "CDU_EntidadeGrupo")))
                        {
                            oCompany.Base.Fornecedores.Remove(Entity);
                            updatedCompanies.Add(groupCompany);
                        }
                        break;
                    case "R":
                    case "D":
                        if (Convert.ToBoolean(oERPContext.BSO.Base.OutrosTerceiros.DaValorAtributo(Entity, EntityType, "CDU_EntidadeGrupo")))
                        {
                            oCompany.Base.OutrosTerceiros.Remove(Entity);
                            updatedCompanies.Add(groupCompany);
                        }
                        break;
                    case "E":
                        if (Convert.ToBoolean(oERPContext.BSO.CRM.EntidadesExternas.DaValorAtributo(Entity, "CDU_EntidadeGrupo")))
                        {
                            oCompany.CRM.EntidadesExternas.Remove(Entity);
                            updatedCompanies.Add(groupCompany);
                        }
                        break;
                    default:
                        break;
                }

                oCompany.FechaEmpresaTrabalho();
            }

            return updatedCompanies;
        }
    }
}
