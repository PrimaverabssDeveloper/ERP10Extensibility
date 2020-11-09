using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Purchases.Editors;
using StdBE100;

namespace Primavera.SIFitofarmaceuticos.Purchases
{
    public class UiEditorCompras : EditorCompras
    {
        public override void ValidaLinha(int NumLinha, ExtensibilityEventArgs e)
        {
            base.ValidaLinha(NumLinha, e);

            try
            {
                //Apenas valida documentos financeiros ou de transporte
                if ((BSO.Compras.TabCompras.DaValorAtributo(DocumentoCompra.Tipodoc, "TipoDocumento") < 3))
                    return;

                //Validações dos fitofarmacêuticos
                if ((bool)(BSO.Base.Artigos.DaValorAtributo(DocumentoCompra.Linhas.GetEdita(NumLinha).Artigo, "CDU_Fitofarmaceutico")))
                {

                    //Obrigatório indicar o número de autorização de venda
                    if (string.IsNullOrEmpty(DocumentoCompra.Linhas.GetEdita(NumLinha).CamposUtil["CDU_NumeroAutorizacao"].Valor.ToString()))
                        throw new Exception("O número de autorização de venda é obrigatório nos produtos fitofarmacêuticos.");

                    //Obrigatório indicar o lote
                    if (DocumentoCompra.Linhas.GetEdita(NumLinha).Lote.Equals("<L01>") || string.IsNullOrEmpty(DocumentoCompra.Linhas.GetEdita(NumLinha).Lote))
                        throw new Exception("É obrigatório indicar o lote dos produtos fitofarmacêuticos.");

                    //A compra a entidades não autorizadas implica a identificação do armazém origem
                    //O armazém origem tem de pertencer ao fornecedor
                    if (!(bool)(BSO.Base.Fornecedores.DaValorAtributo(DocumentoCompra.Entidade, "CDU_OperadorFitofarmaceuticos") ?? false))
                    {
                        //Se não foi definido nenhum armazém de proveniencia associa o primeiro do fornecedor (caso exista)
                        if (string.IsNullOrEmpty((DocumentoCompra.Linhas.GetEdita(NumLinha).CamposUtil["CDU_ArmazemProveniencia"].Valor ?? string.Empty).ToString()))
                        {
                            StdBELista LstArmProveniencia = BSO.Consulta(string.Format("select top(1) [CDU_Codigo] from TDU_ArmazensProveniencia where [CDU_CodFornecedor] = '{0}'", DocumentoCompra.Entidade));
                            if (!LstArmProveniencia.Vazia())
                            {
                                DocumentoCompra.Linhas.GetEdita(NumLinha).CamposUtil["CDU_ArmazemProveniencia"].Valor = LstArmProveniencia.Valor("CDU_Codigo");
                            }
                            else
                            {
                                throw new Exception("Não existe nenhum armazém de proveniência associado a este fornecedor. A compra de produtos fitofarmacêuticos a entidades isentas de autorização implica a identificação do armazém de proveniência.");
                            }
                        }
                        
                        //Valida se o armazém de proveniência pertence ao fornecedor
                        StdBECamposChave objCampoChave = new StdBECamposChave();
                        objCampoChave.AddCampoChave("CDU_Codigo", DocumentoCompra.Linhas.GetEdita(NumLinha).CamposUtil["CDU_ArmazemProveniencia"].Valor);

                        if (!BSO.TabelasUtilizador.DaValorAtributo("TDU_ArmazensProveniencia", objCampoChave, "CDU_CodFornecedor").Equals(DocumentoCompra.Entidade))
                            throw new Exception("O armazém de proveniência não pertence à entidade do documento.");
                    }
                }
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErro(ex.Message, StdPlatBS100.StdBSTipos.IconId.PRI_Exclama);
                DocumentoCompra.Linhas.Remove(NumLinha);
            }
        }
    }
}
