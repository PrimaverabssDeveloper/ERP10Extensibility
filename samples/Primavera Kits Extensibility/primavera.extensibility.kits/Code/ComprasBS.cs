using System;
using System.Collections.Generic;
using System.Linq;
using BasBE100;
using CmpBE100;
using ErpBS100;
using Primavera.Platform.Collections;
using StdPlatBS100;

namespace primavera.extensibility.kits.Code
{
    public class ComprasBS
    {
        private string avisos = string.Empty;

        private ErpBS BSO = null;
        private StdPlatBS Plataforma = null;

        public ComprasBS(ErpBS bso, StdPlatBS plataforma)
        {
            this.BSO = bso;
            this.Plataforma = plataforma;
        }

        public string KIT_Compras_Documento(string tipoDoc, string serie, string artigo = "", double qtd = 1)
        {

            avisos = string.Empty;

            try
            {
                var doc = new CmpBE100.CmpBEDocumentoCompra()
                {
                    Tipodoc = tipoDoc,
                    Serie = serie,
                    TipoEntidade = "F",
                    Entidade = "F0001",
                    NumDocExterno = $"{tipoDoc}_{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}_{DateTime.Now.Year}",
                };

                BSO.Compras.Documentos.PreencheDadosRelacionados(doc);

                BSO.Compras.Documentos.AdicionaLinha(doc, artigo, ref qtd);

                var linhaPai = doc.Linhas.GetEdita(1);

                //adicionar outros artigos ao KIT
                BSO.Compras.Documentos.AdicionaLinhaEspecial(doc, BasBETiposGcp.compTipoLinhaEspecial.compLinha_Comentario, 0, "## KIT ##");
                doc.Linhas.GetEdita(doc.Linhas.NumItens).IdLinhaPai = linhaPai.IdLinha;
                
                BSO.Compras.Documentos.AdicionaLinha(doc, "A0001", ref qtd);
                var linha = doc.Linhas.GetEdita(doc.Linhas.NumItens);
                linha.IdLinhaPai = linhaPai.IdLinha;

                BSO.Compras.Documentos.Actualiza(doc, ref avisos);
                Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Criado Com Sucesso  {doc.Tipodoc} {doc.Serie}/{doc.NumDoc}.\n{avisos}", StdBSTipos.IconId.PRI_Informativo);

                return $"Compra Criada Com Sucesso  {doc.Tipodoc} {doc.Serie}/{doc.NumDoc}.\n{avisos}";

            }
            catch (Exception ex)
            {
                Plataforma.MensagensDialogos.MostraErroSimples(ex.Message, StdBSTipos.IconId.PRI_Critico);

                return $"ERRO: {ex.Message}";
            }
        }

        public string KIT_Compras_Transforma(string tipoDocOrig, string serieOrig, string tipoDocDest, string serieDest, string artigo = "", double qtd = 1)
        {
            avisos = string.Empty;

            try
            {
                BSO.DSO.IniciaTransaccao();

                KIT_Compras_Documento(tipoDocOrig, serieOrig, artigo);

                var NovoDocumento = new CmpBE100.CmpBEDocumentoCompra()
                {
                    Tipodoc = tipoDocDest,
                    Serie = serieDest,
                    TipoEntidade = "F",
                    Entidade = "F0001",
                    NumDocExterno = $"{tipoDocDest}_{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}_{DateTime.Now.Year}",
                };

                BSO.Compras.Documentos.PreencheDadosRelacionados(NovoDocumento);

                var numdoc = 0;
                var objLista = BSO.Consulta($"SELECT NUMDOC = MAX(NUMDOC) FROM CABECCOMPRAS WHERE TIPODOC = '{tipoDocOrig}' AND SERIE = '{serieOrig}'");
                if (objLista?.NoFim() == false)
                    numdoc = BSO.DSO.Plat.Utils.FInt(objLista.Valor("NUMDOC"));

                List<CmpBEDocumentoCompra> documentos = new List<CmpBEDocumentoCompra>();
                var docOrig = BSO.Compras.Documentos.Edita("000", tipoDocOrig, serieOrig, numdoc);
                documentos.Add(docOrig);

                //outras opções
                //BSO.Compras.Documentos.AdicionaLinhaTransformada(doc, "ECF", numdoc, 1, ref filial, ref SerieECF, -1);
                //var docFinal = BSO.Compras.Documentos.AdicionaConversaoDocumento(doc, "ECF", numdoc, ref filial, ref SerieECF, ref inclui);

                BSO.Compras.Documentos.TransformaDocumento(documentos.ToArray(), ref NovoDocumento, true, ref avisos);

                BSO.Compras.Documentos.Actualiza(NovoDocumento, ref avisos);

                Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Transformado com sucesso. {NovoDocumento.Documento} \n {avisos}", StdBSTipos.IconId.PRI_Informativo);

                BSO.DSO.TerminaTransaccao();

                return $"Transformado com sucesso. {NovoDocumento.Documento} \n {avisos}";

            }
            catch (Exception ex)
            {
                BSO.DSO.DesfazTransaccao();

                Plataforma.MensagensDialogos.MostraErroSimples(ex.Message, StdBSTipos.IconId.PRI_Critico);

                return $"ERRO: {ex.Message}";
            }
        }
        
        public string KIT_Compras_ConverteDocs(string tipoDocOrig, string serieOrig, string tipoDocDest, string serieDest, string artigo = "", double qtd = 1)
        {
            var DocsCompra = new PrimaveraOrderedDictionary();
            var ColNumsExternos = new PrimaveraOrderedDictionary();
            var TipoDocDestino = new PrimaveraOrderedDictionary();
            var strSerieDocDestino = new PrimaveraOrderedDictionary();
            var DataDocDestino = new PrimaveraOrderedDictionary();
            var strCamposAgrupamento = new PrimaveraOrderedDictionary();
            var colIdProjectos = new PrimaveraOrderedDictionary();

            bool bAgruparObjectos = true;
            bool bLinhaSeparadora = true;
            bool bLinhaBranco = true;
            string DocumentosGerados = "";
            int intAccaoRupturaStk = 1;
            bool bIncluiComentarios = true;

            var numdoc = 0;

            try
            {
                BSO.DSO.IniciaTransaccao();

                KIT_Compras_Documento(tipoDocOrig, serieOrig, artigo);

                var objLista = BSO.Consulta($"SELECT NUMDOC = MAX(NUMDOC) FROM CABECCOMPRAS WHERE TIPODOC = '{tipoDocOrig}' AND SERIE = '{serieOrig}'");
                if (objLista?.NoFim() == false)
                    numdoc = BSO.DSO.Plat.Utils.FInt(objLista.Valor("NUMDOC"));

                var ecfCompra = BSO.Compras.Documentos.Edita("000", "ECF", "A", numdoc);

                // Adiciona documento para transformação.
                DocsCompra.Add(Guid.NewGuid().ToString(), ecfCompra);
                ColNumsExternos.Add(Guid.NewGuid().ToString(), $"VFA_{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}_{DateTime.Now.Year}");
                TipoDocDestino.Add(Guid.NewGuid().ToString(), tipoDocDest);
                strSerieDocDestino.Add(Guid.NewGuid().ToString(), serieDest);

                BSO.Compras.Documentos.ConverteDocs(
                    DocsCompra,
                    ColNumsExternos,
                    TipoDocDestino,
                    strSerieDocDestino,
                    DataDocDestino,
                    bAgruparObjectos,
                    strCamposAgrupamento,
                    bLinhaSeparadora,
                    bLinhaBranco,
                    ref DocumentosGerados,
                    colIdProjectos);

                Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Criado Com Sucesso  {DocumentosGerados}.\n{avisos}", StdBSTipos.IconId.PRI_Informativo);

                BSO.DSO.TerminaTransaccao();

                return $"Convertido com sucesso: {DocumentosGerados} \n {avisos}";

            }
            catch (Exception ex)
            {
                BSO.DSO.DesfazTransaccao();
                Plataforma.MensagensDialogos.MostraErroSimples(ex.Message, StdBSTipos.IconId.PRI_Critico);

                return $"ERRO: {ex.Message}";
            }
        }
        
        public string KIT_Compras_EstornaDocumento(string tipoDoc, string serie, string artigo = "", double qtd = 1)
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

                KIT_Compras_Documento(tipoDoc, serie, artigo);

                //doc a transformar
                var objLista = BSO.Consulta($"SELECT NUMDOC = MAX(NUMDOC) FROM CABECCOMPRAS WHERE TIPODOC = '{tipoDoc}' AND SERIE = '{serie}'");
                if (objLista?.NoFim() == false)
                    numdoc = BSO.DSO.Plat.Utils.FInt(objLista.Valor("NUMDOC"));

                CmpBEDocumentoCompra clsCompra = BSO.Compras.Documentos.Edita("000", tipoDoc, serie, numdoc);

                var docEstorno = new CmpBEDocumentoCompra()
                {
                    ID = Guid.NewGuid().ToString(),
                    Serie = "A",
                    Tipodoc = "VNC",
                    TipoEntidade = clsCompra.TipoEntidade,
                    Entidade = clsCompra.Entidade,
                    NumDocExterno = $"NC_{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}_{DateTime.Now.Year}",
                };

                BSO.Compras.Documentos.PreencheDadosRelacionados(docEstorno);

                var docNovo = BSO.Compras.Documentos.EstornaDocumentoCompra(
                      clsCompra.ID,
                      MotivoEstorno,
                      Observacoes,
                      ref DataDocumentoEstorno,
                      ref DataIntroducao,
                      ref docEstorno,
                      GravaDocumentoEstorno,
                      ref Avisos);

                Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Estornado com sucesso. {docEstorno.Documento} \n {avisos}", StdBSTipos.IconId.PRI_Informativo);
                
                var novoDocumento = BSO.DSO.Plat.Utils.FBool(BSO.Base.MotivosEstorno.DaValorAtributo(MotivoEstorno, "NovoDocumento"));
                if (novoDocumento)
                {
                    if (docNovo?.Linhas?.Any() ?? false)
                    {
                        BSO.Compras.Documentos.Actualiza(docNovo, ref avisos);
                        Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Criado Com Sucesso  {docNovo.Tipodoc} {docNovo.Serie}/{docNovo.NumDoc}.\n{avisos}", StdBSTipos.IconId.PRI_Informativo);

                    }
                    else
                    {
                        Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Documento Sem Linhas.\n{avisos}", StdBSTipos.IconId.PRI_Informativo);
                    }
                }

                BSO.DSO.TerminaTransaccao();

                return $"Estornado com sucesso. {docEstorno.Documento} \n {avisos}";

            }
            catch (Exception ex)
            {
                BSO.DSO.DesfazTransaccao();
                Plataforma.MensagensDialogos.MostraErroSimples(ex.Message, StdBSTipos.IconId.PRI_Critico);

                return $"ERRO: {ex.Message}";
            }
        }

    }
}
