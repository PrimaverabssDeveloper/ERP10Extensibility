using StdBE100;
using StdPlatBS100;
using SUGIMPL_OME.Helpers;
using System;
using System.Threading;
using System.Windows.Forms;

namespace SUGIMPL_OME.CrossCompany
{
    public partial class frmImportDocuments:Form
    {

        #region DECLARE
        //Constants
        private const string colIdDoc = "IDDoc";
        private const string colSelection = "Sel";
        private const string colMovType = "DocType";
        private const string colCompany = "Company";
        private const string colDocument = "Document";
        private const string colDate = "Date";
        private const string colTotal = "Total";
        private const string colTargetDoc = "TargetDoc";
        private const string colImportNotes = "ImportNotes";
        //Private
        private ERPContext ERPContext;
        StdBELista lstDocuments;
        #endregion

        public frmImportDocuments(ERPContext oERPContext)
        {
            InitializeComponent();

            ERPContext = oERPContext;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

            if (lstDocuments != null && lstDocuments.NumLinhas() > 0)
            {
                bool occurredErrors = false;
                bool occurredSomeIntegrations = false;

                DialogResult oDialog = ERPContext.PSO.Dialogos.MostraDialogoEsperaAsync(
                    this,
                    "A processar documentos...",
                    new Action<Progress<int>, CancellationToken>((progress, cancel) =>
                    {
                        int i = 0;
                        lstDocuments.Inicio();
                        while (!lstDocuments.NoFim())
                        {
                            ((IProgress<int>)progress).Report(Convert.ToInt32((decimal)i / (decimal)lstDocuments.NumLinhas() * 100));

                            if (lstDocuments.DaValor<bool>("Sel"))
                            {
                                Tuple<string, string> retValue = new Tuple<string, string>(String.Empty, String.Empty);
                                if (lstDocuments.DaValor<string>("DocType").Equals("Compra"))
                                {
                                    retValue = BusinessProcesses.ImportSalesDocument(ERPContext, lstDocuments.DaValor<string>("Company"), lstDocuments.DaValor<string>("IDDoc"));
                                }
                                else if (lstDocuments.DaValor<string>("DocType").Equals("Encomenda"))
                                {
                                    retValue = BusinessProcesses.ImportPurchasesDocument(ERPContext, lstDocuments.DaValor<string>("Company"), lstDocuments.DaValor<string>("IDDoc"));
                                }
                                else
                                {
                                    retValue = new Tuple<string, string>(String.Empty, String.Empty);
                                }

                                if (retValue.Item1.Equals("ERRO"))
                                    occurredErrors = true;
                                else
                                    occurredSomeIntegrations = true;

                                SetGridText(lstDocuments.DaValor<string>("IDDoc"), retValue);
                            }

                            i++;
                            lstDocuments.Seguinte();
                        }
                    }));

                // Erros
                if (occurredErrors)
                {
                    ERPContext.PSO.Dialogos.MostraMensagem(
                        StdBSTipos.TipoMsg.PRI_SimplesOk,
                        "Ocorreram erros na importação. Verifique as notas.",
                        StdBSTipos.IconId.PRI_Exclama);
                }
                else if (occurredSomeIntegrations)
                {
                    ERPContext.PSO.Dialogos.MostraMensagem(
                        StdBSTipos.TipoMsg.PRI_SimplesOk,
                        "Integração terminada com sucesso.",
                        StdBSTipos.IconId.PRI_Informativo);
                }
                else
                {
                    ERPContext.PSO.Dialogos.MostraMensagem(
                        StdBSTipos.TipoMsg.PRI_SimplesOk,
                        "Não foram efetuadas quaisquer integrações.",
                        StdBSTipos.IconId.PRI_Informativo);
                }
            }
            else
            {
                ERPContext.PSO.Dialogos.MostraMensagem(
                    StdPlatBS100.StdBSTipos.TipoMsg.PRI_SimplesOk,
                    "Não há documentos para processar.",
                    StdBSTipos.IconId.PRI_Informativo);
            }

        }

        private void SetGridText(string IdDoc, Tuple<string, string> setValue)
        {
            bool continueLooping = true;
            int index = 1;
            int currentRecord = 1;
            while (continueLooping && currentRecord <= grdDocuments.NumeroRegistos)
            {
                if (!((bool)grdDocuments.GetLinhaAgrupamento(index)))
                {
                    if (string.Compare(grdDocuments.GetGRID_GetValorCelula(index, colIdDoc), IdDoc) == 0)
                    {
                        grdDocuments.SetGRID_SetValorCelula(index, colTargetDoc, setValue.Item1);
                        grdDocuments.SetGRID_SetValorCelula(index, colImportNotes, setValue.Item2);
                    }
                    currentRecord++;
                }
                index++;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdDocuments_Format()
        {

            grdDocuments.TituloGrelha = "Posição Global no Grupo";

            grdDocuments.AddColAgrupa();

            grdDocuments.AddColKey(colIdDoc, UpgradeHelpers.Spread.FpCellType.CellTypeStaticText, "IdDoc", 10, true, false);
            grdDocuments.AddColKey(colSelection, UpgradeHelpers.Spread.FpCellType.CellTypeCheckBox, "Sel.", 5);
            grdDocuments.AddColKey(colMovType, UpgradeHelpers.Spread.FpCellType.CellTypeStaticText, "Tipo Movimento", 15);
            grdDocuments.AddColKey(colCompany, UpgradeHelpers.Spread.FpCellType.CellTypeStaticText, "Empresa", 10);
            grdDocuments.AddColKey(colDocument, UpgradeHelpers.Spread.FpCellType.CellTypeStaticText, "Documento", 10);
            grdDocuments.AddColKey(colDate, UpgradeHelpers.Spread.FpCellType.CellTypeDate, "Data", 10);
            grdDocuments.AddColKey(colTotal, UpgradeHelpers.Spread.FpCellType.CellTypeCurrency, "Total", 10);
            grdDocuments.AddColKey(colTargetDoc, UpgradeHelpers.Spread.FpCellType.CellTypeStaticText, "Doc. Gerado", 10);
            grdDocuments.AddColKey(colImportNotes, UpgradeHelpers.Spread.FpCellType.CellTypeStaticText, "Notas Importação", 50);

            grdDocuments.ColsAgrupamento.Insere(grdDocuments.Cols.GetEdita(colMovType));

            grdDocuments.ColsTotais.Insere(grdDocuments.Cols.GetEdita(colTotal));

            grdDocuments.BandaMenuContexto = "PopupGrelhasStd";
        }

        private void grdDocuments_Load(object sender, EventArgs e)
        {
            grdDocuments.Inicializa(ERPContext.sdkContext);
            grdDocuments_Format();
            grdDocuments_ActualizaDados(sender, e);
        }

        private void grdDocuments_ActualizaDados(object Sender, EventArgs e)
        {
            lstDocuments = CrossCompany.BusinessProcesses.GetDocumentsToImport(ERPContext);

            if (lstDocuments!= null && !lstDocuments.Vazia())
                grdDocuments.DataBind(lstDocuments);
        }
    }
}
