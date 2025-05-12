using System;
using System.Collections.Generic;
using System.Linq;
using ErpBS100;
using IntBE100;
using InvBE100;
using StdPlatBS100;

namespace primavera.extensibility.kits.Code
{
    public class InternalBS
    {
        private string avisos = string.Empty;

        private ErpBS BSO = null;
        private StdPlatBS Plataforma = null;

        private VendasBS _vendasBs = null;
        private ComprasBS _comprasBS = null;
        private InventarioBS _inventarioBS = null;

        public InternalBS(ErpBS bso, StdPlatBS plataforma)
        {
            this.BSO = bso;
            this.Plataforma = plataforma;

            _vendasBs = new VendasBS(bso, plataforma);
            _comprasBS = new ComprasBS(bso, plataforma);
            _inventarioBS = new InventarioBS(bso, plataforma);
        }

        public string KIT_Grava_Interno(string tipoDoc, string serie, string artigo = "", double qtd = 1)
        {
            avisos = string.Empty;

            try
            {
                var docInterno = new IntBEDocumentoInterno
                {
                    Tipodoc = tipoDoc,
                    Serie = serie,
                    Data = DateTime.Now,
                    DataVencimento = DateTime.Now
                };

                BSO.Internos.Documentos.PreencheDadosRelacionados(docInterno);
                BSO.Internos.Documentos.AdicionaLinha(docInterno, artigo, "A1", "A1", "", 0, 0, qtd);
                
                BSO.Internos.Documentos.AdicionaLinha(docInterno, "A0001", "A1", "A1", "", 0, 0, qtd);
                BSO.Internos.Documentos.AdicionaLinha(docInterno, "TAPETE", "A1", "A1", "", 0, 0, qtd);

                BSO.Internos.Documentos.Actualiza(docInterno, ref avisos);

                Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Criado Com Sucesso  {docInterno.Tipodoc} {docInterno.Serie}/{docInterno.NumDoc}.\n{avisos}", StdBSTipos.IconId.PRI_Informativo);

                return $"Interno Criado Com Sucesso  {docInterno.Tipodoc} {docInterno.Serie}/{docInterno.NumDoc}.\n{avisos}";

            }
            catch (Exception ex)
            {
                Plataforma.MensagensDialogos.MostraErroSimples(ex.Message, StdBSTipos.IconId.PRI_Critico);

                return $"ERRO: {ex.Message}";
            }
        }

        public string KIT_Internos_EstornaDocumento(string tipoDoc, string serie, string artigo = "", double qtd = 1)
        {
            string Avisos = string.Empty;

            bool GravaDocumentoEstorno = true;
            DateTime DataDocumentoEstorno = DateTime.Now;
            DateTime DataIntroducao = DateTime.Now;
            bool PreencheDadosRelacionados = true;
            string MotivoEstorno = "C01";

            var numdoc = 0;
            string Observacoes = "[Teste a EstornaDocumentoVenda]";

            try
            {
                BSO.DSO.IniciaTransaccao();

                KIT_Grava_Interno(tipoDoc, serie, artigo);

                //doc a transformar
                var objLista = BSO.Consulta($"SELECT NUMDOC = MAX(NUMDOC) FROM CABECINTERNOS WHERE TIPODOC = '{tipoDoc}' AND SERIE = '{serie}'");
                if (objLista?.NoFim() == false)
                    numdoc = BSO.DSO.Plat.Utils.FInt(objLista.Valor("NUMDOC"));

                IntBEDocumentoInterno clsInternos = BSO.Internos.Documentos.Edita(tipoDoc, numdoc,serie, "000");

                var docEstorno = new IntBEDocumentoInterno()
                {
                    ID = Guid.NewGuid().ToString(),
                    Serie = "A",
                    Tipodoc = "SS",
                    TipoEntidade = clsInternos.TipoEntidade,
                    Entidade = clsInternos.Entidade
                };

                BSO.Internos.Documentos.PreencheDadosRelacionados(docEstorno);

                BSO.Internos.TabInternos.ActualizaValorAtributo("ES", "PermiteEstorno", true);
                BSO.Internos.TabInternos.ActualizaValorAtributo("ES", "DocumentoEstorno", "SS");
                BSO.Internos.TabInternos.ActualizaValorAtributo("ES", "SerieDocEstorno", "A");

                var docNovo = BSO.Internos.Documentos.EstornaDocumentoInterno(
                    clsInternos.ID,
                      MotivoEstorno,
                      Observacoes,
                      ref DataDocumentoEstorno,
                      ref DataIntroducao,
                      ref docEstorno,
                      GravaDocumentoEstorno,
                      ref Avisos);

                Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Estornado com sucesso: {docEstorno.Documento} \n {avisos}", StdBSTipos.IconId.PRI_Informativo);


                var novoDocumento = BSO.DSO.Plat.Utils.FBool(BSO.Base.MotivosEstorno.DaValorAtributo(MotivoEstorno, "NovoDocumento"));
                if (novoDocumento)
                {
                    if (docNovo?.Linhas?.Any() ?? false)
                    {
                        BSO.Internos.Documentos.Actualiza(docNovo, ref avisos);
                        Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Criado Com Sucesso  {docNovo.Tipodoc} {docNovo.Serie}/{docNovo.NumDoc}.\n{avisos}", StdBSTipos.IconId.PRI_Informativo);

                    }
                    else
                    {
                        Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Documento Sem Linhas.\n{avisos}", StdBSTipos.IconId.PRI_Informativo);
                    }
                }

                BSO.DSO.TerminaTransaccao();

                return $"Estornado com sucesso: {docEstorno.Documento} \n {avisos}";

            }
            catch (Exception ex)
            {
                BSO.DSO.DesfazTransaccao();
                Plataforma.MensagensDialogos.MostraErroSimples(ex.Message, StdBSTipos.IconId.PRI_Critico);

                return $"ERRO: {ex.Message}";
            }
        }
        

        public string KIT_CopiaLinhas(string artigo, string moduloOrig, string tipoDocOrig, string serieOrig, string moduloDest, string tipoDocDest, string serieDest)
        {
            avisos = string.Empty;

            bool blnSugereDadosEntidade = true;
            bool blnCopiaPrecoUnitario = true;
            string strTipoEntidade = "";
            string strEntidade = "";
            bool blnCopiaQuantidadeTotal = false;
            bool Estorno = false;
            
            var listaNumLinhas = moduloOrig == ConstantesPrimavera100.Modulos.Transferencias ? new List<Int32> { 10001 } : new List<Int32> { 1 };
            var listaQuantidades = new List<double> { 1 };

            var numdoc = 0;

            dynamic objDocOrigem = null;
            dynamic objDocDestino = null;

            try
            {
                switch (moduloOrig)
                {
                    case ConstantesPrimavera100.Modulos.Internos:

                        KIT_Grava_Interno(tipoDocOrig, serieOrig, artigo);
                        numdoc = DaUltimoNumDoc(moduloOrig, tipoDocOrig, serieOrig);
                        objDocOrigem = BSO.Internos.Documentos.Edita(tipoDocOrig, numdoc, serieOrig, "000");
                        break;

                    case ConstantesPrimavera100.Modulos.Vendas:
                        _vendasBs.KIT_Vendas_Documento(tipoDocOrig, serieOrig, artigo);
                        numdoc = DaUltimoNumDoc(moduloOrig, tipoDocOrig, serieOrig);
                        objDocOrigem = BSO.Vendas.Documentos.Edita("000",tipoDocOrig, serieOrig, numdoc);
                        break;

                    case ConstantesPrimavera100.Modulos.Compras:
                       _comprasBS.KIT_Compras_Documento(tipoDocOrig, serieOrig, artigo);
                        numdoc = DaUltimoNumDoc(moduloOrig, tipoDocOrig, serieOrig);
                        objDocOrigem = BSO.Compras.Documentos.Edita("000",tipoDocOrig, serieOrig,numdoc);
                        break;

                    case ConstantesPrimavera100.Modulos.Transferencias:
                        _inventarioBS.KIT_Grava_Trasnferencia(tipoDocOrig, serieOrig, artigo);
                        numdoc = DaUltimoNumDoc(moduloOrig, tipoDocOrig, serieOrig);
                        objDocOrigem = BSO.Inventario.Transferencias.Edita(tipoDocOrig, numdoc, "000",serieOrig);
                        break;
                }

                switch (moduloDest)
                {
                    case ConstantesPrimavera100.Modulos.Internos:
                        objDocDestino = new IntBEDocumentoInterno
                        {
                            Tipodoc = tipoDocDest,
                            Serie = serieDest,
                            TipoEntidade = "C",
                            Entidade = "",
                            Data = DateTime.Now
                        };

                        BSO.Internos.Documentos.PreencheDadosRelacionados(objDocDestino);
                        objDocDestino.DataVencimento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        break;
                    case ConstantesPrimavera100.Modulos.Vendas:
                        objDocDestino = new VndBE100.VndBEDocumentoVenda()
                        {
                            Tipodoc = tipoDocDest,
                            Serie = serieDest,
                            TipoEntidade = "C",
                            Entidade = "SOFRIO"
                        };

                        BSO.Vendas.Documentos.PreencheDadosRelacionados(objDocDestino);
                        break;

                    case ConstantesPrimavera100.Modulos.Compras:
                        objDocDestino  = new CmpBE100.CmpBEDocumentoCompra()
                        {
                            Tipodoc = tipoDocDest,
                            Serie = serieDest,
                            TipoEntidade = "F",
                            Entidade = "F0001",
                            NumDocExterno = $"VFA_{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}_{DateTime.Now.Year}"
                        };

                        BSO.Compras.Documentos.PreencheDadosRelacionados(objDocDestino);
                        break;

                    case ConstantesPrimavera100.Modulos.Transferencias:
                        objDocDestino = new InvBEDocumentoTransf()
                        {
                            Tipodoc = tipoDocDest,
                            Serie = serieDest,
                            Data = DateTime.Now
                        };
                        BSO.Inventario.Transferencias.PreencheDadosRelacionados(objDocDestino);
                        break;
                }

                strTipoEntidade = objDocDestino.TipoEntidade;
                strEntidade = objDocDestino.Entidade;

                return CopiaLinhasInterno(
                    objDocOrigem, 
                    objDocDestino, 
                    moduloOrig, 
                    moduloDest, 
                    listaNumLinhas, 
                    listaQuantidades, 
                    blnSugereDadosEntidade, 
                    blnCopiaPrecoUnitario, 
                    strTipoEntidade, 
                    strEntidade, 
                    tipoDocDest, 
                    serieDest, 
                    Estorno);
            }
            catch (Exception ex)
            {
                Plataforma.MensagensDialogos.MostraErroSimples(ex.Message, StdBSTipos.IconId.PRI_Critico);

                return $"ERRO: {ex.Message}";
            }
        }

        private int DaUltimoNumDoc(string strModuloDest, string tipoDoc, string serie)
        {
            var strSQL = string.Empty;
            int numdoc = 0;

            switch (strModuloDest)
            {
                case ConstantesPrimavera100.Modulos.Internos:
                    strSQL = $"SELECT NUMDOC = MAX(NUMDOC) FROM CABECINTERNOS (NOLOCK) WHERE TIPODOC = '{tipoDoc}' AND SERIE = '{serie}'";
                    break;

                case ConstantesPrimavera100.Modulos.Vendas:
                    strSQL = $"SELECT NUMDOC = MAX(NUMDOC) FROM CABECDOC (NOLOCK) WHERE TIPODOC = '{tipoDoc}' AND SERIE = '{serie}'";
                    break;

                case ConstantesPrimavera100.Modulos.Compras:
                    strSQL = $"SELECT NUMDOC = MAX(NUMDOC) FROM CABECCOMPRAS (NOLOCK) WHERE TIPODOC = '{tipoDoc}' AND SERIE = '{serie}'";
                    break;

                case ConstantesPrimavera100.Modulos.Transferencias:
                    strSQL = $"SELECT NUMDOC = MAX(NUMDOC) FROM INV_CABECTRANSFERENCIAS (NOLOCK) WHERE TIPODOC = '{tipoDoc}' AND SERIE = '{serie}'";
                    break;
            }

            var objLista = BSO.Consulta(strSQL);
            if (objLista?.NoFim() == false)
                numdoc = BSO.DSO.Plat.Utils.FInt(objLista.Valor("NUMDOC"));

            return numdoc;
        }
        
        private string CopiaLinhasInterno(dynamic objDocOrigem, dynamic objDocDestino, string strModuloOrig, string strModuloDest, List<int> listaNumLinhas, List<double> listaQuantidades, bool blnSugereDadosEntidade, bool blnCopiaPrecoUnitario, string strTipoEntidade, string strEntidade, string strTipDocDestino, string strSerie, bool Estorno)
        {
            avisos = string.Empty;

            BSO.Internos.Documentos.CopiaLinhas(
                strModuloOrig,
                objDocOrigem,
                ref strModuloDest,
                ref objDocDestino,
                listaNumLinhas.ToArray(),
                listaQuantidades.ToArray(),
                blnSugereDadosEntidade,
                blnCopiaPrecoUnitario,
                ref strTipoEntidade,
                ref strEntidade,
                strTipDocDestino,
                strSerie,
                Estorno);

            switch (strModuloDest)
            {
                case ConstantesPrimavera100.Modulos.Internos:
                    BSO.Internos.Documentos.Actualiza(objDocDestino, ref avisos);
                    break;

                case ConstantesPrimavera100.Modulos.Vendas:
                    BSO.Vendas.Documentos.Actualiza(objDocDestino, ref avisos);
                    break;

                case ConstantesPrimavera100.Modulos.Compras:
                    BSO.Compras.Documentos.Actualiza(objDocDestino, ref avisos);
                    break;

                case ConstantesPrimavera100.Modulos.Transferencias:

                    //Forçar Origem/destino diferente, tambel é possivel configurar na serie
                    foreach (InvBELinhaOrigemTransf linhaOrigem in ((InvBEDocumentoTransf)objDocDestino).LinhasOrigem)
                    {
                        linhaOrigem.Armazem = "A1";
                        linhaOrigem.Localizacao = "A1";

                        foreach (InvBELinhaDestinoTransf linhaDestino in linhaOrigem.LinhasDestino)
                        {
                            linhaDestino.Armazem = "A2";
                            linhaDestino.Localizacao = "A2";
                        }

                    }

                    BSO.Inventario.Transferencias.Actualiza(objDocDestino, ref avisos);
                    break;
            }

            Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Criado Com Sucesso  {objDocDestino.Tipodoc} {objDocDestino.Serie}/{objDocDestino.NumDoc}.\n{avisos}", StdBSTipos.IconId.PRI_Informativo);

            return $"Copia de Linhas Criado Com Sucesso  {objDocDestino.Tipodoc} {objDocDestino.Serie}/{objDocDestino.NumDoc}.\n{avisos}";
        }


    }
}
