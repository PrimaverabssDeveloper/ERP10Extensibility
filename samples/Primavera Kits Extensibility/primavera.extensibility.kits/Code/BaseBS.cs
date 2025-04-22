using System;
using System.Collections.Generic;
using BasBE100;
using ErpBS100;
using StdPlatBS100;

namespace primavera.extensibility.kits.Code
{
    public class BaseBS
    {
        public static string ARTIGO_KIT = "COZ.A";

        private ErpBS BSO = null;
        private StdPlatBS Plataforma = null;

        public BaseBS(ErpBS bso, StdPlatBS plataforma)
        {
            this.BSO = bso;
            this.Plataforma = plataforma;
        }

        private List<(string Artigo, string Descricao, double Qtd, double PrecUnit)> GetComponentes(string sufixo = "A") =>
            new List<(string Artigo, string Descricao, double Qtd, double PrecUnit)>()
            {
                (Artigo: $"COZ.ARMARIO.BRANCO_{sufixo}", Descricao: "Armário cozinha (Branco)" , Qtd: 2.0d , PrecUnit: 400d ),
                (Artigo: $"COZ.LAVALOUCA.BRANCO_{sufixo}", Descricao: "Lava louça cozinha (Branco)" , Qtd: 1.0d , PrecUnit: 200d),
                (Artigo: $"COZ.MESA.BRANCO_{sufixo}", Descricao: "Mesa de Cozinha (Branco)" , Qtd: 1.0d , PrecUnit: 699.99d ),
                (Artigo: $"COZ.EXAUSTOR_{sufixo}", Descricao: "Exaustor Cozinha" , Qtd: 1.0d , PrecUnit: 249.99d )

            };

        public BasBEArtigo CriaArtigo(string codArtigo, string descricao, string iva = "23", string unidadeBase = "UN", string movStock = "S", string armazemSugestao = "A1",
                                      string localizacaoSugestao = "A1", string moeda = "EUR", double PVP1 = 100, string unidade = "UN", bool trataLotes = false, bool trataNumerosSerie = false,
                                      double stkMinimo = 0, string grupoCenariosCompras = "", bool sujeitoRetencao = false)
        {
            BasBEArtigo artigo = null;

            /* Cria Novo Artigo */
            if (!BSO.Base.Artigos.Existe(codArtigo))
            {
                try
                {
                    artigo = new BasBEArtigo();

                    artigo.Artigo = codArtigo.ToUpper();
                    artigo.Descricao = descricao;
                    artigo.IVA = string.IsNullOrEmpty(iva) ? "23" : iva;

                    artigo.UnidadeBase = unidadeBase;
                    artigo.UnidadeCompra = unidadeBase;
                    artigo.UnidadeEntrada = unidadeBase;
                    artigo.UnidadeSaida = unidadeBase;
                    artigo.UnidadeVenda = unidadeBase;

                    artigo.MovStock = movStock;
                    artigo.ArmazemSugestao = armazemSugestao;
                    artigo.LocalizacaoSugestao = localizacaoSugestao;
                    artigo.STKMinimo = stkMinimo;
                    artigo.DeduzIVA = true;
                    artigo.PercIvaDedutivel = 100;
                    artigo.SujeitoDevolucao = true;
                    artigo.SujeitoRetencao = sujeitoRetencao;


                    if (!string.IsNullOrEmpty(grupoCenariosCompras))
                        artigo.GrupoCenariosCompras = grupoCenariosCompras;


                    artigo.TrataLotes = trataLotes;
                    artigo.TrataNumerosSerie = trataNumerosSerie;


                    BSO.Base.Artigos.Actualiza(artigo);

                    BasBEArtigoMoeda artigoMoeda = new BasBEArtigoMoeda();
                    artigoMoeda.Artigo = codArtigo;
                    artigoMoeda.Moeda = moeda;
                    artigoMoeda.PVP1 = PVP1;
                    artigoMoeda.Unidade = unidade;

                    BSO.Base.ArtigosPrecos.Actualiza(artigoMoeda);

                    //já foi criado
                    artigo.EmModoEdicao = true;

                    if (!BSO.Base.Artigos.Existe(codArtigo))
                        return null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                artigo = BSO.Base.Artigos.Edita(codArtigo);
            }

            return artigo;
        }

        public BasBEArtigo CriaArtigosKitTeste(string artigo = "COZ.A", string descricao = "Cozinha Tipologia A (KIT)", string armazem = "A1") =>
            CriaArtigosKit(artigo, descricao, GetComponentes(), armazem);

        public BasBEArtigo CriaArtigosKit(string artigo, string descricao, List<(string Artigo, string Descricao, double Qtd, double PrecUnit)> listaCompontes, string armazem)
        {
            if (!BSO.Base.Artigos.Existe(artigo))
            {
                // Cria o artigo kit
                var artigoKit = CriaArtigo(artigo, descricao);
                artigoKit.Classe = ConstantesPrimavera100.Artigos.Kit;
                BSO.Base.Artigos.Actualiza(artigoKit);

                var ordem = 1;
                foreach (var componente in listaCompontes)
                {
                    var artigoFilho = CriaArtigo(componente.Artigo, componente.Descricao, PVP1: componente.PrecUnit * 1.3);

                    // Cria os componentes do kit
                    var filho = new BasBEArtigoComponente
                    {
                        Artigo = artigoKit.Artigo,
                        Componente = componente.Artigo,
                        Armazem = armazem,
                        Localizacao = armazem,
                        Ordem = ordem,
                        Quantidade = componente.Qtd,
                        TipoCusto = BasBETipos.EnumTipoCustoComponentesArtigos.tcPCUnitario

                    };
                    BSO.Base.ArtigosComponentes.Actualiza(ref filho);

                    ordem += 1;
                }

                return BSO.Base.Artigos.Edita(artigo);
            }
            else
            {
                return BSO.Base.Artigos.Edita(artigo);
            }

        }

    }
}
