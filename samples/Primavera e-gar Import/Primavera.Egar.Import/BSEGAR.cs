using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ErpBS100;
using StdPlatBS100;
using TTEBE100;
using TTEBS100.egarVersion2;
using static TTEBS100.TTEBSEgar;
using static TTEBE100.TTEBEDocumentoEgar;

namespace Primavera.Egar.Import
{
    public class BSEGAR
    {
        private ErpBS BSO = new ErpBS();
        private StdPlatBS PSO = new StdPlatBS();
        public BSEGAR(ErpBS motor, StdPlatBS plataforma)
        {
            BSO  = motor;
            PSO = plataforma;
        }

        private bool m_IniciouDadosPrimavera = false;
        private BEDadosPrimavera m_ObjDadosPrimavera = new BEDadosPrimavera();

        public void Egar(List<(string, string)> listaEgar, ref string strErrosEx, ref string imporados, ref string nImportados)
        {
            StdPlatBS100.StdBSDialogoEspera objDialogo = null;
            try
            {
                TTEBEEgarParametros parametros = BSO.TransaccoesElectronicas.EgarParametros.Edita();
                
                objDialogo = PSO.MensagensDialogos.MostraDialogoEsperaEx("A Processar  ...");
                foreach (var item in listaEgar)
                {
                    if (!BSO.DSO.TransaccoesElectronicas.Egar.Existe(item.Item1, item.Item2))
                    {
                        string strErros = string.Empty;
                        guia guiaDoc = BSO.TransaccoesElectronicas.Egar.ConsultarGuia(item.Item1, item.Item2);

                        if (guiaDoc!=null)
                        {
                            var nifInterveniente = parametros.Utilizador;

                            var interveniente = "P"; // P : Produtor | T : Transportador | D: Destinatário 

                            if (guiaDoc.transportadoresField.AsEnumerable().FirstOrDefault()?.nifField == nifInterveniente)
                            {
                                interveniente = "T";
                            }

                            if (guiaDoc.destinatarioField.nifField == nifInterveniente)
                            {
                                interveniente = "D";
                            }

                            RegistaGuiasRecebidas(guiaDoc, interveniente, nifInterveniente, ref strErros);

                            if (!string.IsNullOrEmpty(strErros))
                            {
                                strErrosEx += strErros + Environment.NewLine;
                                nImportados += guiaDoc.numeroGuiaField + Environment.NewLine;
                            }
                            else
                            {
                                imporados += guiaDoc.numeroGuiaField + Environment.NewLine;
                            }
                        }
                        else
                        {
                            nImportados += item.Item1 + Environment.NewLine;
                        }
                    }
                    else
                    {
                        nImportados += item.Item1 + Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                strErrosEx += ex.Message + Environment.NewLine;
            }
            finally
            {    
                //Termina o dialogo   
                objDialogo?.Termina();}
            }

        private void RegistaGuiasRecebidas(TTEBS100.egarVersion2.guia guia, string interveniente, string nifInterveniente, ref string strErros)
        {
            try
            {
                TTEBEDocumentoEgar egarDoc = null;
                if (!BSO.DSO.TransaccoesElectronicas.Egar.Existe(guia.numeroGuiaField, guia.codigoGuiaField))
                {
                    egarDoc = RegistaGuiaRecebida(guia, interveniente, nifInterveniente, ref strErros);

                    if (egarDoc != null)
                    {
                        BSO.DSO.TransaccoesElectronicas.Egar.Actualiza(egarDoc);
                    }
                }
            }
            catch (Exception ex)
            {
                strErros += ex.Message;
            }
        }

        private TTEBEDocumentoEgar RegistaGuiaRecebida(TTEBS100.egarVersion2.guia guia, string interveniente, string nifInterveniente, ref string erros, bool Autorizada = false)
        {
            try
            {
                string entidade = string.Empty;
                string tipoEntidade = string.Empty;
                TTEBEDocumentoEgar egar = new TTEBEDocumentoEgar
                {
                    Id = Guid.NewGuid().ToString(),
                    DataDoc = DateTime.Now,
                    DataComunicacao = guia.dataEstadoField,
                    CodigoVerificacao = guia.codigoGuiaField,
                    TipoProdutor = guia.tipoProdutorField,
                    Estado = DaCodigoEstado(guia.codigoEstadoField),
                    Autorizada = !guia.pendenteAutorizacaoField,
                    NumDoc = guia.numeroGuiaField,
                    //PDF
                    PDF = GetPDF(DadosPrimavera, guia.urlField),
                    ComentarioDestinatario = guia.comentarioDestinatarioField == null ? string.Empty : guia.comentarioDestinatarioField,
                    ComentarioRemetente = guia.comentarioRemetenteField == null ? string.Empty : guia.comentarioRemetenteField,
                    NifIntervenienteCriacao = interveniente.ToUpper() == "P" ? nifInterveniente : guia.nifIntervinienteCriacaoField,
                    Transportadores = PreencheTransportadores(guia.transportadoresField),
                    DataFimTransporte = guia.dataFimTransporteField,
                    DataInicioTransporte = guia.transportadoresField.First().dataHoraInicioTransporteField,
                    Designacao = guia.residuoTransportadoField.descricaoResiduoField,
                    CodigoGrupo = guia.residuoTransportadoField.codigoGrupoField == null ? string.Empty : guia.residuoTransportadoField.codigoGrupoField,
                    CodigoOperacao = guia.residuoTransportadoField.codigoOperacaoField,
                    CodigoLer = guia.residuoTransportadoField.codigoResiduoLerField,
                    Quantidade = guia.residuoTransportadoField.quantidadeField,
                    NumPGL = guia.residuoTransportadoField.numPglField == null ? string.Empty : guia.residuoTransportadoField.numPglField,
                    CodigoAPADestinatario = "",
                    CodigoAPAProdutor = "",
                    TipoInterveniente = interveniente // P : Produtor | T : Transportador | D: Destinatário 
                };

                if (guia.remetenteField.estabelecimentoField != null)
                {
                    egar.CodigoAPAProdutor = guia.remetenteField.estabelecimentoField.codigoAPAField;
                }

                if (guia.remetenteField.localRecolhaRetomaField != null)
                {
                    egar.CodigoInterno = guia.remetenteField.localRecolhaRetomaField.codigoInternoField;
                }

                if (guia.remetenteField.nifField == BSO.Contexto.IFNIF)
                {
                    if (!string.IsNullOrEmpty(egar.CodigoAPAProdutor))
                    {
                        entidade = DaArmazem(egar.CodigoAPAProdutor);
                    }
                }
                else
                {
                    DaDadosEntidadeDocumentoRecebido(guia.remetenteField.nifField, egar.CodigoAPAProdutor, ref tipoEntidade, ref entidade, true);
                }

                egar.Produtor = entidade;
                egar.NomeProdutor = guia.remetenteField.nomeField;
                egar.TipoEntidadeProdutor = tipoEntidade;
                egar.TipoRemetente = guia.remetenteField.tipoRemetenteField;

                egar.CodigoAPADestinatario = guia.destinatarioField.estabelecimentoField.codigoAPAField;
                if (guia.destinatarioField.nifField == BSO.Contexto.IFNIF)
                {
                    entidade = "";
                    tipoEntidade = "";
                    if (!string.IsNullOrEmpty(egar.CodigoAPADestinatario))
                    {
                        entidade = DaArmazem(egar.CodigoAPADestinatario);
                    }
                }
                else
                {
                    DaDadosEntidadeDocumentoRecebido(guia.destinatarioField.nifField, egar.CodigoAPADestinatario, ref tipoEntidade, ref entidade, false);
                }
                egar.Destinatario = entidade;
                egar.TipoEntidadeDestinatario = tipoEntidade;

                if (guia.residuoTransportadoCorrigidoField != null)
                {
                    egar.CodigoGrupoCorrigido = guia.residuoTransportadoCorrigidoField.codigoGrupoCorrigidoField;
                    egar.CodigoOperacaoCorrigido = guia.residuoTransportadoCorrigidoField.codigoOperacaoCorrigidoField;
                    egar.CodigoLerCorrigido = guia.residuoTransportadoCorrigidoField.codigoResiduoLerCorrigidoField;
                    egar.QuantidadeCorrigido = guia.residuoTransportadoCorrigidoField.quantidadeCorrigidoField;
                    egar.NumPGLCorrigido = guia.residuoTransportadoCorrigidoField.numPglCorrigidoField;
                }

                return egar;
            }
            catch (Exception ex)
            {
                erros += ex.Message;
            }
            return null;
        }

        private TTEBEEgarTransportadores PreencheTransportadores(List<TTEBS100.egarVersion2.transportador> transportadores)
        {
            TTEBEEgarTransportadores transportadoresResult = new TTEBEEgarTransportadores();
            foreach (TTEBS100.egarVersion2.transportador transportadorEgar in transportadores)
            {
                TTEBEEgarTransportador transportador = new TTEBEEgarTransportador();

                transportador.DataInicioTransporte = transportadorEgar.dataHoraInicioTransporteField;
                transportador.Matricula = transportadorEgar.matriculaField;
                transportador.Transportador = transportadorEgar.nifField;
                transportador.Ordem = transportador.Ordem;
                transportador.NomeTransportador = transportadorEgar.nomeField;

                transportadoresResult.Insere(transportador);
            }

            return transportadoresResult;
        }


        private string DaArmazem(string codApa)
        {
            string sql = BSO.DSO.Plat.Sql.FormatSQL("SELECT [Armazem] FROM [Armazens] A WHERE A.eGAR_CodigoAPA = '@1@'", codApa);

            var lista = BSO.DSO.Consulta(sql);

            if (lista != null)
            {
                if (lista.NumLinhas() > 0)
                {
                    return lista.Valor("Armazem");
                }
            }

            throw new Exception($"Armazem não encontrado para o codigo APA {codApa}");
        }

        private void DaDadosEntidadeDocumentoRecebido(string nif, string codApa, ref string tipoEntidade, ref string entidade, bool blnProdutor)
        {
            string sql = "";

            if (blnProdutor)
            {
                //Sync BID 54873 : Se for "Produtor", procurar pela ordem Fornecedor/OutroCredor/Cliente/OutroDevedor
                sql = @"SELECT '1' AS Tipo, F.Fornecedor  AS Entidade, 'F' AS TipoEntidade, F.DataUltimaActualizacao 
                        FROM Fornecedores F 
                        LEFT JOIN MoradasAlternativasFornecedores MF ON MF.Fornecedor = F.Fornecedor 
                        WHERE F.NumContrib = '@1@' AND F.eGAR_Isenta = 0 
                        AND (ISNULL(F.eGAR_CodigoAPA, '') = '@2@' OR ISNULL(MF.eGAR_CodigoAPA, '')= '@2@') 
                        
                        UNION ALL 
                        SELECT '2' AS Tipo, OC.Terceiro AS Entidade, OC.TipoEntidade, OC.DataUltimaActualizacao 
                        FROM OutrosTerceiros OC 
                        WHERE OC.DevCred = 'C' AND OC.NumContrib = '@1@' 
                        AND ISNULL(OC.eGAR_CodigoAPA, '') = '@2@' AND OC.eGAR_Isenta = 0 
                        
                        UNION ALL 
                        SELECT '3' AS Tipo, C.Cliente  AS Entidade, 'C' AS TipoEntidade, C.DataUltimaActualizacao 
                        FROM Clientes C 
                        LEFT JOIN MoradasAlternativasClientes MC ON MC.Cliente = C.Cliente 
                        WHERE C.NumContrib = '@1@' AND C.eGAR_Isenta = 0 
                        AND (ISNULL(C.eGAR_CodigoAPA, '') = '@2@' OR  ISNULL(MC.eGAR_CodigoAPA, '') = '@2@') 
                        
                        UNION ALL 
                        SELECT '4' AS Tipo, OD.Terceiro AS Entidade, OD.TipoEntidade, OD.DataUltimaActualizacao 
                        FROM OutrosTerceiros OD 
                        WHERE OD.DevCred = 'D' AND OD.NumContrib = '@1@' 
                        AND ISNULL(OD.eGAR_CodigoAPA, '') = '@2@' AND OD.eGAR_Isenta = 0 
                        ORDER BY Tipo, TipoEntidade, DataUltimaActualizacao DESC";
            }
            else
            {
                //Sync BID 54873 : Se for "Destinatário", procurar pela ordem Cliente/OutroDevedor/Fornecedor/OutroCredor
                sql = @"SELECT '1' AS Tipo, C.Cliente  AS Entidade, 'C' AS TipoEntidade, C.DataUltimaActualizacao 
                        FROM Clientes C 
                        LEFT JOIN MoradasAlternativasClientes MC ON MC.Cliente = C.Cliente 
                        WHERE C.NumContrib = '@1@' AND 
                              C.eGAR_Isenta = 0    AND 
                              (ISNULL(C.eGAR_CodigoAPA, '') = '@2@' OR  ISNULL(MC.eGAR_CodigoAPA, '') = '@2@')

                        UNION ALL 

                        SELECT '2' AS Tipo, OD.Terceiro AS Entidade, OD.TipoEntidade, OD.DataUltimaActualizacao 
                        FROM OutrosTerceiros OD 
                        WHERE OD.DevCred = 'D' AND OD.NumContrib = '@1@' 
                        AND ISNULL(OD.eGAR_CodigoAPA, '') = '@2@' AND OD.eGAR_Isenta = 0 
                        
                        UNION ALL 
                        
                        SELECT '3' AS Tipo, F.Fornecedor  AS Entidade, 'F' AS TipoEntidade, F.DataUltimaActualizacao
                        FROM Fornecedores F 
                        LEFT JOIN MoradasAlternativasFornecedores MF ON MF.Fornecedor = F.Fornecedor 
                        WHERE F.NumContrib = '@1@' AND F.eGAR_Isenta = 0 
                        AND (ISNULL(F.eGAR_CodigoAPA, '') = '@2@' OR ISNULL(MF.eGAR_CodigoAPA, '')= '@2@') 
                        
                        UNION ALL 
                        
                        SELECT '4' AS Tipo, OC.Terceiro AS Entidade, OC.TipoEntidade, OC.DataUltimaActualizacao 
                        FROM OutrosTerceiros OC 
                        WHERE OC.DevCred = 'C' AND OC.NumContrib = '@1@' 
                        AND ISNULL(OC.eGAR_CodigoAPA, '') = '@2@' AND OC.eGAR_Isenta = 0 
                        ORDER BY Tipo, TipoEntidade, DataUltimaActualizacao DESC";
            }

            sql = BSO.DSO.Plat.Sql.FormatSQL(sql, nif, codApa);
            var lista = BSO.DSO.Consulta(sql);

            if (lista != null)
            {
                if (lista.NumLinhas() == 0)
                {
                    tipoEntidade = string.Empty;
                    entidade = nif;
                }
                else
                {
                    tipoEntidade = lista.Valor("TipoEntidade");
                    entidade = lista.Valor("Entidade");
                }
            }
            else
            {
                throw new Exception($"Entidade não encontrada para o codigo APA {codApa}");
            }

        }


        private EnumEstadoEGar DaCodigoEstado(string Codigo)
        {
            EnumEstadoEGar result = EnumEstadoEGar.Ignorada;
            switch (Codigo)
            {
                case "EM":
                    result = EnumEstadoEGar.Emitida;
                    break;
                case "AC":
                    result = EnumEstadoEGar.Aceite;
                    break;
                case "CO":
                    result = EnumEstadoEGar.Corrigida;
                    break;
                case "RJ":
                    result = EnumEstadoEGar.Rejeitada;
                    break;
                case "CN":
                    result = EnumEstadoEGar.CorrecaoNegada;
                    break;
                case "CC":
                    result = EnumEstadoEGar.Concluida;
                    break;
                case "AN":
                    result = EnumEstadoEGar.Anulada;
                    break;
            }

            return result;
        }

        private byte[] GetPDF(BEDadosPrimavera dadosPrimavera, string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadData(url);
            }

        }

        private BEDadosPrimavera DadosPrimavera
        {
            get
            {
                if (!m_IniciouDadosPrimavera)
                {
                    m_ObjDadosPrimavera = new BEDadosPrimavera();
                    m_ObjDadosPrimavera.EgarPassword = BSO.DSO.Plat.Criptografia.Descripta(BSO.TransaccoesElectronicas.EgarParametros.Edita().Password, 21);
                    m_ObjDadosPrimavera.EgarUsername = BSO.TransaccoesElectronicas.EgarParametros.Edita().Utilizador;
                    m_ObjDadosPrimavera.PrimaveraProductionEndpoint = !DaValorUrlPrimavera();
                    m_ObjDadosPrimavera.PrimaveraLicenseUserId = BSO.Licenca.Utilizador;
                    m_ObjDadosPrimavera.PrimaveraUser = BSO.Contexto.UtilizadorActual;
                    m_ObjDadosPrimavera.PrimaveraProductCode = "PrimaveraERP100";
                    m_ObjDadosPrimavera.EgarProductionEndpoint = !DaValorUrl();
                    m_ObjDadosPrimavera.PrimaveraMachineName = BSO.DSO.Plat.UtilAPIs.DaNomeComputador();

                    m_IniciouDadosPrimavera = true;
                }

                return m_ObjDadosPrimavera;
            }
        }

        private bool DaValorUrl()
        {
            int result = BSO.DSO.Plat.IniFiles.IniLeLong("EGARRequest", "TestURL", 0, StdBE100.StdBETipos.TipoIni.inGlobalSistema);

            return result == 1;
        }

        private bool DaValorUrlPrimavera()
        {
            int result = BSO.DSO.Plat.IniFiles.IniLeLong("DaValorUrlPrimavera", "TestURL", 0, StdBE100.StdBETipos.TipoIni.inGlobalSistema);

            return result == 1;
        }

    }
}
