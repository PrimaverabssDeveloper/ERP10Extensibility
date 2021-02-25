using System;
using System.Text;
using Primavera.Extensibility.CustomForm;
using PRISDK100;
using StdBE100;
using UpgradeHelpers.Spread;

namespace PrimaveraSDK
{
    public partial class frmDemoGrid : CustomForm
    {
        private const string GridVersion = "01.00";
        private const string GridName = "GridDemo";

        // Column name consts
        // Normal columns
        private const string colModuloDesc = "ModuloDesc = ''";
        private const string colEntidade = "Entidade";
        private const string colTipoDoc = "TipoDoc";
        private const string colSerie = "Serie";
        private const string colNumDocInt = "NumDocInt";
        private const string colDataDoc = "DataDoc";
        private const string colDataVenc = "DataVenc";
        private const string colValorTotal = "ValorTotal";
        private const string colValorPendente = "ValorPendente";
        // Calculated column
        private const string colDiasAtraso = "DiasAtraso = DATEDIFF(d, GETDATE(), DataVenc)";
        // Hidden column
        private const string colModulo = "Modulo";
        private const string colTipoEntidade = "TipoEntidade";
        private const string colFilial = "Filial";
        private const string colIdHistorico = "IdHistorico";

        private bool controlsInitialized;
        private string categoriaEntidade = "mntTabClientes";

        /// <summary>
        /// Private constructor
        /// </summary>
        public frmDemoGrid()
        {
            InitializeComponent();
        }

        #region Events        

        /// <summary>
        /// Form load
        /// </summary>
        private void frmDemoGrid_Load(object sender, EventArgs e)
        {
            // Initialize the SDK context
            PriSDKContext.Initialize(BSO, PSO);

            // Initialize SDK controls
            InitializeSDKControls();

            // Initialize the grid
            InitializeGrid();
        }

        private void frmDemoGrid_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            try
            {
                //Save the current configuration.
                priGrelha1.GravaXML();

                //Ensure that resources released.
                tiposEntidade1.Termina();
                f41.Termina();
                priGrelha1.Termina();

                controlsInitialized = false;
            }
            catch { }
        }

        /// <summary>
        /// Displays the accounts associated with the entity.
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void tiposEntidade1_TextChange(object Sender, TiposEntidade.TextChangeEventArgs e)
        {
            SelecionaCategoriaF4();
        }

        private void buttonLoadGrid_Click(object sender, System.EventArgs e)
        {
            try
            {
                LoadGrid();
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErroSimples("", StdPlatBS100.StdBSTipos.IconId.PRI_Critico, ex.Message);
            }
        }

        private void priGrelha1_ActualizaDados(object Sender, EventArgs e)
        {
            LoadGrid();
        }

        private void priGrelha1_DataFill(object Sender, PriGrelha.DataFillEventArgs e)
        {
            // Set modulo description
            SetModuloDesc(e.Row);

            // Set days of delay
            int daysOfDelay = PSO.Utils.FInt(priGrelha1.GetGRID_GetValorCelula(e.Row, colDiasAtraso));
            daysOfDelay = daysOfDelay < 0 ? Math.Abs(daysOfDelay) : 0;
            priGrelha1.SetGRID_SetValorCelula(e.Row, colDiasAtraso, daysOfDelay);
        }

        private void priGrelha1_FormatacaoAlterada(object Sender, PriGrelha.FormatacaoAlteradaEventArgs e)
        {
            priGrelha1.LimpaGrelha();
        }

        private void priGrelha1_MenuContextoSeleccionado(object Sender, PriGrelha.MenuContextoSeleccionadoEventArgs e)
        {
            switch (e.Comando.ToUpper())
            {
                case "MNUSTDDRILLDOWN":
                    ExecuteDrillDown();
                    break;
                case "MNUCRIAENTIDADE":
                    PSO.Dialogos.MostraMensagem(
                            StdPlatBS100.StdBSTipos.TipoMsg.PRI_SimplesOk, "Create e new entity");
                    break;
                case "MNUEDITARENTIDADE":
                    PSO.Dialogos.MostraMensagem(
                        StdPlatBS100.StdBSTipos.TipoMsg.PRI_SimplesOk, 
                        PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(priGrelha1.Grelha.ActiveRow, colEntidade)));
                    break;
                default:
                    break;
            }
        }

        #endregion Events

        #region Private

        /// <summary>
        /// Initialize SDK controls.
        /// </summary>
        private void InitializeSDKControls()
        {
            //Initializes controls
            if (!controlsInitialized)
            {
                // Initialize the controls with the SDK context
                tiposEntidade1.Inicializa(PriSDKContext.SdkContext);
                f41.Inicializa(PriSDKContext.SdkContext);
                priGrelha1.Inicializa(PriSDKContext.SdkContext);

                controlsInitialized = true;
            }
        }

        private void InitializeGrid()
        {
            priGrelha1.BandaMenuContexto = "PopupGrelhasStd";
            priGrelha1.IniciaDadosConfig();

            // Number of groupings allowed (maximum of 4)
            priGrelha1.AddColAgrupa();
            priGrelha1.AddColAgrupa();
            priGrelha1.AddColAgrupa();
            priGrelha1.AddColAgrupa();

            // Add a custom comand to Context Menu
            priGrelha1.AddOpcaoActiveBar(0, "mnuCriaEntidade", "Novo", null,
                    StdBrandingInfo100.Properties.RibbonResourcesVND.novo_16,
                    strBandaDestino: "mnuContexto");

            priGrelha1.AddOpcaoActiveBar(1, "mnuEditarEntidade", "Editar", null,
                    StdBrandingInfo100.Properties.RibbonResourcesVND.clientes_16,
                    strBandaDestino: "mnuContexto");

            // Add a custom comand to active bar.
            priGrelha1.AddOpcaoActiveBar(0, "mnuCriaEntidadeC", "Novo", null,
                    StdBrandingInfo100.Properties.RibbonResourcesVND.novo_16);

            priGrelha1.AddOpcaoActiveBar(1, "mnuEditarEntidadeC", "Editar", null,
                    StdBrandingInfo100.Properties.RibbonResourcesVND.clientes_16);

            // Normal columns
            priGrelha1.AddColKey(colModuloDesc, FpCellType.CellTypeEdit, "Módulo", 10, true, strCamposBaseDados: colModulo);
            priGrelha1.AddColKey(colEntidade, FpCellType.CellTypeEdit, "Entidade", 24, true, strCamposBaseDados: colEntidade, blnDrillDown: true);
            priGrelha1.AddColKey(colTipoDoc, FpCellType.CellTypeEdit, "Tipo Doc.", 10, true, strCamposBaseDados: colTipoDoc);
            priGrelha1.AddColKey(colSerie, FpCellType.CellTypeEdit, "Série", 10, true, strCamposBaseDados: colSerie);
            priGrelha1.AddColKey(colNumDocInt, FpCellType.CellTypeInteger, "Núm.Doc.", 4, true, strCamposBaseDados: colNumDocInt, blnDrillDown: true);
            priGrelha1.AddColKey(colDataDoc, FpCellType.CellTypeDate, "Data Doc.", 8, true, strCamposBaseDados: colDataDoc);
            priGrelha1.AddColKey(colDataVenc, FpCellType.CellTypeDate, "Data Venc.", 8, true, strCamposBaseDados: colDataVenc);
            priGrelha1.AddColKey(colValorTotal, FpCellType.CellTypeFloat, "Valor Total", 8, true, strCamposBaseDados: colValorTotal, blnColunaTotalizador: true);
            priGrelha1.AddColKey(colValorPendente, FpCellType.CellTypeFloat, "Valor Pendente", 8, true, strCamposBaseDados: colValorPendente, blnColunaTotalizador: true);
            
            // Calculeted column
            priGrelha1.AddColKey(colModulo, FpCellType.CellTypeEdit, "Modulo", 2, true, false, strCamposBaseDados: colModulo);
            priGrelha1.AddColKey(colDiasAtraso, FpCellType.CellTypeInteger, "Dias Atraso", 8, true, strCamposBaseDados: colDiasAtraso, blnColunaTotalizador: true);
            // Hidden column
            priGrelha1.AddColKey(colTipoEntidade, FpCellType.CellTypeEdit, "TipoEntidade", 2, true, false, strCamposBaseDados: colTipoEntidade);
            priGrelha1.AddColKey(colFilial, FpCellType.CellTypeEdit, "Filial", 2, true, false, strCamposBaseDados: colFilial);
            priGrelha1.AddColKey(colIdHistorico, FpCellType.CellTypeEdit, "IdHistorico", 2, true, false, strCamposBaseDados: colIdHistorico);

            //Default grouping
            priGrelha1.AdicionaAgrupamento(priGrelha1.Cols.GetEdita(colEntidade).Number);
            priGrelha1.AdicionaAgrupamento(priGrelha1.Cols.GetEdita(colModuloDesc).Number);

            // Set greid default behavior
            priGrelha1.TituloGrelha = "Demo Grid 1";
            priGrelha1.PermiteAgrupamentosUser = true;
            priGrelha1.PermiteOrdenacao = true;
            priGrelha1.PermiteActualizar = true;
            priGrelha1.PermiteFiltros = true;
            priGrelha1.PermiteDetalhes = true;
            priGrelha1.PermiteStatusBar = true;
            priGrelha1.PermiteDataFill = true;
            priGrelha1.PermiteVistas = true;

            // Read the last grid layout for the current user.
            if (!priGrelha1.LeXML("GRIDDEMO", BSO.Contexto.UtilizadorActual, GridName, GridName, GridVersion))
            {
                priGrelha1.FormataGrelha(true);
            }


            priGrelha1.LimpaGrelha();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid()
        {
            StdBELista lista;

            StringBuilder query = new StringBuilder();
            query.AppendLine(string.Format("SELECT {0}", priGrelha1.DaCamposBDSelect()));
            query.AppendLine("FROM Pendentes");
            query.AppendLine(string.Format("WHERE TipoEntidade = '{0}'", tiposEntidade1.TipoEntidade));

            if (!string.IsNullOrWhiteSpace(f41.Text))
            {
                query.AppendLine(string.Format("AND Entidade = '{0}'", f41.Text));
            }

            query.AppendLine("ORDER BY DataDoc");

            lista = new StdBELista();
            lista = PriSDKContext.SdkContext.BSO.Consulta(query.ToString());

            priGrelha1.DataBind(lista);
        }

        private void SelecionaCategoriaF4()
        {
            f41.Enabled = true;
            f41.PermiteDrillDown = true;

            switch (tiposEntidade1.TipoEntidade)
            {
                case "C":
                    f41.Categoria = clsSDKTypes.EnumCategoria.Clientes;
                    categoriaEntidade = "mntTabClientes";
                    break;
                case "F":
                    f41.Categoria = clsSDKTypes.EnumCategoria.Fornecedores;
                    categoriaEntidade = "mntTabFornecedores";
                    break;
                default:
                    f41.Categoria = clsSDKTypes.EnumCategoria.NaoDefinida;
                    f41.Enabled = false;
                    f41.Limpa();
                    f41.PermiteDrillDown = false;
                    break;
            }

            f41.ResetText();
        }

        private void SetModuloDesc(int row)
        {
            string modulo = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colModulo));
            string moduloDesc = modulo;

            switch (modulo)
            {
                case "V":
                    moduloDesc = "Vendas";
                    break;
                case "C":
                    moduloDesc = "Compras";
                    break;
                case "M":
                    moduloDesc = "C/Corentes";
                    break;
                default:
                    break;
            }

            priGrelha1.SetGRID_SetValorCelula(row, colModuloDesc, moduloDesc);
        }

        private void ExecuteDrillDown()
        {
            int row = priGrelha1.Grelha.ActiveRowIndex;
            int col = priGrelha1.Grelha.ActiveColumnIndex;

            if (priGrelha1.Cols.GetEditaCol(col).ColKey == colEntidade)
            {
                string entidade = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colEntidade));

                DrillDownManager.DrillDownEntidade(PSO, categoriaEntidade, entidade);

                return;
            }

            if (priGrelha1.Cols.GetEditaCol(col).ColKey == colNumDocInt)
            {
                string modulo = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colModulo));
                string tipodoc = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colTipoDoc));
                string serie = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colSerie));
                int numdoc = PSO.Utils.FInt(priGrelha1.GetGRID_GetValorCelula(row, colNumDocInt));
                string filial = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colFilial));

                DrillDownManager.DrillDownDocumento(PSO, modulo, tipodoc, numdoc, serie, filial);

                return;
            }
        }

        #endregion Private
    }
}

