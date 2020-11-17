using StdBE100;
using SUGIMPL_OME.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SUGIMPL_OME.CrossCompany
{
    public partial class frmPosicaoGlobal : Form
    {

        #region DECLARE
        //Public Properties
        public string EntityType { get; set; }
        public string Entity { get; set; }
        public bool IncludeRelatedEntities { get; set; }

        //Constants
        private const string colEmpresa = "Empresa";
        private const string colTipoEntidade = "TipoEntidade";
        private const string colEntidade = "Entidade";
        private const string colOrcamentos = "Orcamentos";
        private const string colEncomendas = "Encomendas";
        private const string colTotalVencido = "TotalVencido";
        private const string colTotalDebito = "TotalDebito";
        //Private
        private ERPContext ERPContext;
        #endregion

        public frmPosicaoGlobal(ERPContext oERPContext, string strTipoEntidade, string strCodigoEntidade)
        {
            InitializeComponent();

            ERPContext = oERPContext;

            EntityType = strTipoEntidade == "" ? "C" : strTipoEntidade;
            Entity = strCodigoEntidade;
            IncludeRelatedEntities = true; 
        }

        private void grdMainGrid_Format()
        {

            grdMainGrid.TituloGrelha = "Posição Global no Grupo";

            grdMainGrid.AddColAgrupa();

            grdMainGrid.AddColKey(colEmpresa, UpgradeHelpers.Spread.FpCellType.CellTypeStaticText, "Empresa", 10);
            grdMainGrid.AddColKey(colTipoEntidade, UpgradeHelpers.Spread.FpCellType.CellTypeStaticText, "Tipo Entidade", 10);
            grdMainGrid.AddColKey(colEntidade, UpgradeHelpers.Spread.FpCellType.CellTypeStaticText, "Entidade", 30);
            grdMainGrid.AddColKey(colOrcamentos, UpgradeHelpers.Spread.FpCellType.CellTypeCurrency, "Orçamentos", 10);
            grdMainGrid.AddColKey(colEncomendas, UpgradeHelpers.Spread.FpCellType.CellTypeCurrency, "Encomendas", 10);
            grdMainGrid.AddColKey(colTotalVencido, UpgradeHelpers.Spread.FpCellType.CellTypeCurrency, "Total Vencido", 10);
            grdMainGrid.AddColKey(colTotalDebito, UpgradeHelpers.Spread.FpCellType.CellTypeCurrency, "Total Debito", 10);

            grdMainGrid.ColsAgrupamento.Insere(grdMainGrid.Cols.GetEdita(colEmpresa));

            grdMainGrid.ColsTotais.Insere(grdMainGrid.Cols.GetEdita(colOrcamentos));
            grdMainGrid.ColsTotais.Insere(grdMainGrid.Cols.GetEdita(colEncomendas));
            grdMainGrid.ColsTotais.Insere(grdMainGrid.Cols.GetEdita(colTotalVencido));
            grdMainGrid.ColsTotais.Insere(grdMainGrid.Cols.GetEdita(colTotalDebito));

            grdMainGrid.BandaMenuContexto = "PopupGrelhasStd";            
        }

        private void frmPosicaoGlobal_Load(object sender, EventArgs e)
        {
            ctlEntity.Inicializa(ERPContext.sdkContext);
            
            //grdMailGrid Initializations
            grdMainGrid.Inicializa(ERPContext.sdkContext);
            ctlEntity.TipoEntidadeCombo = EntityType;
            ctlEntity.ValorRestricao = Entity;
            IncludeRelatedEntities = Convert.ToBoolean(ERPContext.PSO.IniFiles.IniLeString("SUGIMPLOME", "IncludeRelatedEntities", "true"));
            ctlEntity.EntidadesAssociadas = IncludeRelatedEntities;
            grdMainGrid_Format();
            grdMainGrid_ActualizaDados(sender, e);
        }

        private void grdMainGrid_DataFill(object Sender, PRISDK100.PriGrelha.DataFillEventArgs e)
        {
            // Set EntityType description
            string tipoEntidade = ERPContext.PSO.Utils.FStr(grdMainGrid.GetGRID_GetValorCelula(e.Row, colTipoEntidade));
            string tipoEntidadeDescription = tipoEntidade;

            switch (tipoEntidade)
            {
                case "C":
                    tipoEntidadeDescription = "Cliente";
                    break;
                case "D":
                    tipoEntidadeDescription = "Outro Devedor";
                    break;
                case "F":
                    tipoEntidadeDescription = "Fornecedor";
                    break;
                case "R":
                    tipoEntidadeDescription = "Outro Credor";
                    break;
                default:
                    break;
            }

            grdMainGrid.SetGRID_SetValorCelula(e.Row, colTipoEntidade, tipoEntidadeDescription);
        }

        private void grdMainGrid_ActualizaDados(object Sender, EventArgs e)
        {
            StdBELista lstGlobalPosition = CrossCompany.PayablesReceivables.GetGlobalPosition(ERPContext, ctlEntity.TipoEntidadeCombo, ctlEntity.ValorRestricao, ctlEntity.EntidadesAssociadas);

            if (!lstGlobalPosition.Vazia())
                grdMainGrid.DataBind(lstGlobalPosition);
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            grdMainGrid_ActualizaDados(sender, e);
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPosicaoGlobal_FormClosing(object sender, FormClosingEventArgs e)
        {
            ERPContext.PSO.IniFiles.IniGravaString("SUGIMPLOME", "IncludeRelatedEntities", Convert.ToString(ctlEntity.EntidadesAssociadas));
        }
    }
}
