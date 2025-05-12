using System;
using System.Collections.Generic;
using System.Linq;
using BasBE100;
using ErpBS100;
using Primavera.Platform.Collections;
using StdPlatBS100;
using VndBE100;

namespace primavera.extensibility.kits.Code
{
    public class VendasBS
    {
        private string avisos = string.Empty;

        private string cliente = "SOFRIO";

        private ErpBS BSO = null;
        private StdPlatBS Plataforma = null;

        public VendasBS(ErpBS bso, StdPlatBS plataforma)
        {
            this.BSO = bso;
            this.Plataforma = plataforma;
        }

        public string KIT_Vendas_Documento(string tipoDoc, string serie, string artigo = "", double qtd = 1)
        {
            avisos = string.Empty;

            try
            {
                var doc = new VndBE100.VndBEDocumentoVenda()
                {
                    Tipodoc = tipoDoc,
                    Serie = serie,
                    TipoEntidade = "C",
                    Entidade = cliente
                };

                BSO.Vendas.Documentos.PreencheDadosRelacionados(doc);

                BSO.Vendas.Documentos.AdicionaLinha(doc, artigo, ref qtd);

                var linhaPai = doc.Linhas.GetEdita(1);

                //adicionar outros artigos ao KIT
                BSO.Vendas.Documentos.AdicionaLinhaEspecial(doc, BasBETiposGcp.vdTipoLinhaEspecial.vdLinha_Comentario, 0, "## KIT ##");
                doc.Linhas.GetEdita(doc.Linhas.NumItens).IdLinhaPai = linhaPai.IdLinha;
                
                BSO.Vendas.Documentos.AdicionaLinha(doc, "A0001", ref qtd);
                var linha = doc.Linhas.GetEdita(doc.Linhas.NumItens);
                linha.IdLinhaPai = linhaPai.IdLinha;

                BSO.Vendas.Documentos.Actualiza(doc, ref avisos);
                Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Criado Com Sucesso  {doc.Tipodoc} {doc.Serie}/{doc.NumDoc}.\n{avisos}", StdBSTipos.IconId.PRI_Informativo);

                return $"Venda Criado Com Sucesso  {doc.Tipodoc} {doc.Serie}/{doc.NumDoc}.\n{avisos}";

            }
            catch (Exception ex)
            {
                Plataforma.MensagensDialogos.MostraErroSimples(ex.Message, StdBSTipos.IconId.PRI_Critico);

                return $"ERRO: {ex.Message}";
            }
        }
        
        public string KIT_Vendas_Transforma(string tipoDocOrig, string serieOrig, string tipoDocDest, string serieDest, string artigo = "", double qtd = 1)
        {
            avisos = string.Empty;

            try
            {
                BSO.DSO.IniciaTransaccao();
                
                KIT_Vendas_Documento(tipoDocOrig, serieOrig, artigo);
                
                var NovoDocumento = new VndBEDocumentoVenda
                {
                    Tipodoc = tipoDocDest,
                    Serie = serieDest,
                    Entidade = cliente,
                    TipoEntidade = ("C"),
                };
                BSO.Vendas.Documentos.PreencheDadosRelacionados(NovoDocumento);

                var numdoc = 0;

                var objLista = BSO.Consulta($"SELECT NUMDOC = MAX(NUMDOC) FROM CABECDOC WHERE TIPODOC = '{tipoDocOrig}' AND SERIE = '{serieOrig}'");
                if (objLista?.NoFim() == false)
                    numdoc = BSO.DSO.Plat.Utils.FInt(objLista.Valor("NUMDOC"));

                List<VndBEDocumentoVenda> documentos = new List<VndBEDocumentoVenda>();
                var docOrig = BSO.Vendas.Documentos.Edita("000", tipoDocOrig, serieOrig, numdoc);
                documentos.Add(docOrig);

                //outras opções
                //BSO.Vendas.Documentos.AdicionaLinhaTransformada(doc, "ECL", numdoc, 1, ref filial, ref SerieECL, -1);
                //var docFinal = BSO.Vendas.Documentos.AdicionaConversaoDocumento(doc, "ECL", numdoc, ref filial, ref SerieECL, ref inclui);

                BSO.Vendas.Documentos.TransformaDocumento(documentos.ToArray(), ref NovoDocumento, true, ref avisos, true);

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
        
        public string KIT_Vendas_ConverteDocs(string tipoDocOrig, string serieOrig, string tipoDocDest, string serieDest, string artigo = "", double qtd = 1)
        {
            var DocsVenda = new PrimaveraOrderedDictionary();
            var TipoDocDestino = new PrimaveraOrderedDictionary();
            var SerieDestino = new PrimaveraOrderedDictionary();

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

                KIT_Vendas_Documento(tipoDocOrig, serieOrig, artigo);
                
                var objLista = BSO.Consulta($"SELECT NUMDOC = MAX(NUMDOC) FROM CABECDOC WHERE TIPODOC = '{tipoDocOrig}' AND SERIE = '{serieOrig}'");
                if (objLista?.NoFim() == false)
                    numdoc = BSO.DSO.Plat.Utils.FInt(objLista.Valor("NUMDOC"));

                var clsDocumentoVenda = BSO.Vendas.Documentos.Edita("000", tipoDocOrig, serieOrig, numdoc);
                
                VndBE100.VndBETipos.tpDocsConverter DocsvendaConv = new VndBE100.VndBETipos.tpDocsConverter
                {
                    IDDoc = clsDocumentoVenda.ID,
                    DataHoraCarga = DateTime.FromOADate(0),
                    DataHoraDescarga = DateTime.FromOADate(0),
                    DescricaoMotivoEmissao = string.Empty,
                    Matricula = string.Empty,
                    MotivoEmissao = string.Empty,
                    RefDocOrig = string.Empty,
                    RefSerieDocOrig = string.Empty,
                    RefTipoDocOrig = string.Empty
                };
                DocsVenda.Add(DocsvendaConv);

                TipoDocDestino.Add(tipoDocDest);
                SerieDestino.Add(serieDest);

                BSO.Vendas.Documentos.ConverteDocs(
                    DocsVenda,
                    TipoDocDestino,
                    SerieDestino,
                    bAgruparObjectos,
                    null,
                    bLinhaSeparadora,
                    bLinhaBranco,
                    ref DocumentosGerados,
                    intAccaoRupturaStk);

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

        public string KIT_Vendas_EstornaDocumento(string tipoDoc, string serie, string artigo = "", double qtd = 1)
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

                KIT_Vendas_Documento(tipoDoc, serie, artigo);

                //doc a transformar
                var objLista = BSO.Consulta($"SELECT NUMDOC = MAX(NUMDOC) FROM CABECDOC WHERE TIPODOC = '{tipoDoc}' AND SERIE = '{serie}'");
                if (objLista?.NoFim() == false)
                    numdoc = BSO.DSO.Plat.Utils.FInt(objLista.Valor("NUMDOC"));

                VndBEDocumentoVenda clsVenda = BSO.Vendas.Documentos.Edita("000", tipoDoc, serie, numdoc);

                var docEstorno = new VndBEDocumentoVenda()
                {
                    ID = Guid.NewGuid().ToString(),
                    Serie = "C",
                    CondPag = "1",
                    Tipodoc = "NC",
                    TipoEntidade = "C",
                    Entidade = clsVenda.Entidade
                };

                BSO.Vendas.Documentos.PreencheDadosRelacionados(docEstorno);

                var docNovo = BSO.Vendas.Documentos.EstornaDocumentoVenda(
                      clsVenda.ID,
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
                        BSO.Vendas.Documentos.Actualiza(docNovo, ref avisos);
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
    }
}
