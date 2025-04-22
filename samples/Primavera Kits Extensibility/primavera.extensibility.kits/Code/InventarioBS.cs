using System;
using ErpBS100;
using InvBE100;
using StdPlatBS100;

namespace primavera.extensibility.kits.Code
{
    public class InventarioBS
    {
        private string avisos = string.Empty;

        private ErpBS BSO = null;
        private StdPlatBS Plataforma = null;

        public InventarioBS(ErpBS bso, StdPlatBS plataforma)
        {
            this.BSO = bso;
            this.Plataforma = plataforma;
        }

        public string KIT_Grava_Trasnferencia(string tipoDoc, string serie, string artigo = "", double qtd = 1)
        {

            avisos = string.Empty;

            try
            {
                var doc = new InvBEDocumentoTransf()
                {
                    Tipodoc = tipoDoc,
                    Serie = serie,
                    Data = DateTime.Now
                };
                BSO.Inventario.Transferencias.PreencheDadosRelacionados(doc);

                BSO.Inventario.Transferencias.AdicionaLinhaOrigem(doc, artigo, "A1", "A1", "DISP", qtd);

                var linhaPai = doc.LinhasOrigem.GetEdita(1);

                //adicionar outros artigos ao KIT
                doc.LinhasOrigem.Insere(new InvBELinhaOrigemTransf
                {
                    IdLinha = Guid.NewGuid().ToString(),
                    Armazem = "A1",
                    Localizacao = "A1",
                    Lote = "",
                    Descricao = "## KIT ##",
                    TipoLinha = ConstantesPrimavera100.Documentos.TipoLinComentario,
                    IdLinhaPai = linhaPai.IdLinha

                });

                BSO.Inventario.Transferencias.AdicionaLinhaOrigem(doc, "TAPETE", "A1", "A1", "DISP", 1);
                var linha = doc.LinhasOrigem.GetEdita(doc.LinhasOrigem.NumItens);
                linha.IdLinhaPai = linhaPai.IdLinha;

                foreach (InvBELinhaOrigemTransf linhaOrigem in doc.LinhasOrigem)
                {
                    foreach (InvBELinhaDestinoTransf linhaDestino in linhaOrigem.LinhasDestino)
                    {
                        linhaDestino.Armazem = "A2";
                        linhaDestino.Localizacao = "A2";
                    }
                }


                BSO.Inventario.Transferencias.Actualiza(doc, ref avisos);
                Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Criado Com Sucesso  {doc.Tipodoc} {doc.Serie}/{doc.NumDoc}.\n{avisos}", StdBSTipos.IconId.PRI_Informativo);

                return $"Transf. Criada Com Sucesso  {doc.Tipodoc} {doc.Serie}/{doc.NumDoc}.\n{avisos}";
            }
            catch (Exception ex)
            {
                Plataforma.MensagensDialogos.MostraErroSimples(ex.Message, StdBSTipos.IconId.PRI_Critico);

                return $"ERRO: {ex.Message}";
            }
        }

        public string KIT_Grava_Composicao(string tipoDoc, string serie, string composto = "" , string componente = "", double qtd = 1)
        {
            avisos = string.Empty;

            try
            {
                InvBEDocumentoComposicao doc = new InvBEDocumentoComposicao
                {
                    TipoQtdComposicaoTotal = false,
                    TipoPreco = "P",//V - cMovimentoVTotal  | P - cMovimentoVUnitario
                    Tipodoc = tipoDoc,
                    Data = DateTime.Now,
                    Serie = serie
                };

                BSO.Inventario.Composicoes.PreencheDadosRelacionados(doc);
                BSO.Inventario.Composicoes.AdicionaLinhaComposto(doc, "CMP", qtd, "A1");
                var linhaComposto = doc.LinhasCompostos.GetEdita(1);

                BSO.Inventario.Composicoes.AdicionaLinhaComponente(doc, linhaComposto, componente, qtd);

                BSO.Inventario.Composicoes.Actualiza(doc, ref avisos);
                Plataforma.MensagensDialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, $"Criado Com Sucesso  {doc.Tipodoc} {doc.Serie}/{doc.NumDoc}.\n{avisos}", StdBSTipos.IconId.PRI_Informativo);

                return $"Comp. Criada Com Sucesso  {doc.Tipodoc} {doc.Serie}/{doc.NumDoc}.\n{avisos}";


            }
            catch (Exception ex)
            {
                Plataforma.MensagensDialogos.MostraErroSimples(ex.Message, StdBSTipos.IconId.PRI_Critico);

                return $"ERRO: {ex.Message}";
            }
        }


    }
}
