using StdBE100;
using StdBESql100;
using StdPlatBS100;
using System;

namespace Primavera.Logistics.DrillDown
{
    internal static class DrillDownManager
    {
        internal static void drillDownDocumento(StdBSInterfPub PSO, string modulo, string tipoDoc, int numDoc, string serie, string filial)
        {
            StdBESqlCampoDrillDown campoDrillDown = new StdBESqlCampoDrillDown();
            StdBEValoresStr param = new StdBEValoresStr();

            campoDrillDown.ModuloNotificado = "GCP";
            campoDrillDown.Tipo = StdBESqlTipos.EnumTipoDrillDownListas.tddlEventoAplicacao;
            campoDrillDown.Evento = "GCP_EditarDocumento";

            // "V" ' Vendas
            // "C" ' Compras
            // "N" ' Internos
            // "S" ' Stocks
            // "M" ' Contas Correntes
            // "B" ' Tesouraria

            param.InsereNovo("Modulo", modulo);
            param.InsereNovo("Filial", filial);
            param.InsereNovo ("Tipodoc", tipoDoc);
            param.InsereNovo ("Serie", serie);
            param.InsereNovo ("NumDocInt", Convert.ToString(numDoc));

            PSO.DrillDownLista(campoDrillDown, param);
        }

        internal static void drillDownEntidade(StdBSInterfPub PSO, string categoria, string entidade)
        {
            StdBESqlCampoDrillDown campoDrillDown = new StdBESqlCampoDrillDown();
            StdBEValoresStr param = new StdBEValoresStr();

            campoDrillDown.ModuloNotificado = "GCP";
            campoDrillDown.Tipo = StdBESqlTipos.EnumTipoDrillDownListas.tddlEventoAplicacao;
            campoDrillDown.Evento = "GCP_MOSTRAMANUTENCAO";

            param.InsereNovo("Manutencao", categoria);
            param.InsereNovo("Chave", entidade);

            PSO.DrillDownLista(campoDrillDown, param);
        }
    }
}