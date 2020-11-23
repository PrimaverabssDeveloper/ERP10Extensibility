using System;
using System.Text;
using System.Windows.Forms;
using BasBE100;
using Primavera.Extensibility.CustomForm;
using PRISDK100;
using StdBE100;
using UpgradeHelpers.Spread;

namespace PrimaveraSDK
{
    public partial class frmDemoGridEdit : CustomForm
    {
        // Column name consts
        private const string col = "";

        private const string colData = "Data";

        private const string colArtigo = "Artigo";
        private const string colDescArtigo = "DescArtigo";
        private const string colLote = "Lote";
        private const string colNumSerie = "NumSerie";

        private const string colTipoEntidade = "TipoEntidade";
        private const string colEntidade = "Entidade";
        private const string colNomeEntidade = "NomeEntidade";

        private const string colTipoAnomalia = "TipoAnomalia";
        private const string colDescricao = "Descricao";
        private const string colQuantidade = "Quantidade";
        private const string colUnidade = "Unidade";

        private const string colArmazem = "Armazem";
        private const string colLocalizacao = "Localizacao";
        private const string colEstado = "Estado";

        private bool controlsInitialized;

        /// <summary>
        /// Private constructor
        /// </summary>
        public frmDemoGridEdit()
        {
            InitializeComponent();
        }

        #region Events        

        /// <summary>
        /// Form load
        /// </summary>
        private void frmDemoGridEdit_Load(object sender, EventArgs e)
        {
            try
            {
                // Initialize the SDK context
                PriSDKContext.Initialize(BSO, PSO);

                // Initialize SDK controls
                InitializeSDKControls();

                // Initialize the grid
                InitializeGrid();

                // Sets the form to a new record
                NewRecord();
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErroSimples("Erro ao carregar o formulário!", StdPlatBS100.StdBSTipos.IconId.PRI_Critico, ex.Message);
            }
        }

        private void frmDemoGridEdit_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            try
            {
                //Ensure that resources released.
                priGrelha1.Termina();

                controlsInitialized = false;
            }
            catch { }
        }

        private void numericUpDownNumber_Validated(object sender, EventArgs e)
        {
            try
            {
                Edit();
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErroSimples("Erro na editar!", StdPlatBS100.StdBSTipos.IconId.PRI_Critico, ex.Message);
                NewRecord();
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            try
            {
                NewRecord();
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErroSimples("Erro na executar operação!", StdPlatBS100.StdBSTipos.IconId.PRI_Critico, ex.Message);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (PSO.Dialogos.MostraPerguntaSimples("Confirma a remoção do registo?"))
                {
                    Remove();
                    NewRecord();
                }
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErroSimples("Erro na remover!", StdPlatBS100.StdBSTipos.IconId.PRI_Critico, ex.Message);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
                NewRecord();
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErroSimples("Erro na gravar!", StdPlatBS100.StdBSTipos.IconId.PRI_Critico, ex.Message);
            }
        }

        private void priGrelha1_FormatacaoAlterada(object Sender, PriGrelha.FormatacaoAlteradaEventArgs e)
        {
            //------------------------------------------------------------------------------------------------
            //   Span à 1ª linha dos títulos
            //------------------------------------------------------------------------------------------------

            try
            {
                // Artigo
                priGrelha1.Grelha.AddCellSpan(priGrelha1.Cols.GetEdita(colArtigo).Number, StdSpread.SS_SPREADHEADER, 4, 1);

                // Entidade
                priGrelha1.Grelha.AddCellSpan(priGrelha1.Cols.GetEdita(colTipoEntidade).Number, StdSpread.SS_SPREADHEADER, 3, 1);

                // Anomalia
                priGrelha1.Grelha.AddCellSpan(priGrelha1.Cols.GetEdita(colTipoAnomalia).Number, StdSpread.SS_SPREADHEADER, 4, 1);

                // Localização
                priGrelha1.Grelha.AddCellSpan(priGrelha1.Cols.GetEdita(colArmazem).Number, StdSpread.SS_SPREADHEADER, 3, 1);

                priGrelha1.LimpaGrelha();
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErroSimples("Erro na formatação da grelha!", StdPlatBS100.StdBSTipos.IconId.PRI_Critico, ex.Message);
            }
        }

        private void priGrelha1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F4 && !e.Control && !e.Shift)
                {
                    TrataF4Linhas(priGrelha1.Grelha.ActiveCol, priGrelha1.Grelha.ActiveRow);
                }

                return;
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErroSimples("Erro ao executar a operação!", StdPlatBS100.StdBSTipos.IconId.PRI_Critico, ex.Message);
            }
        }

        private void priGrelha1_MenuContextoSeleccionado(object Sender, PriGrelha.MenuContextoSeleccionadoEventArgs e)
        {
            try
            {
                switch (e.Comando.ToUpper())
                {
                    case "MNUSTDDRILLDOWN":
                        ExecuteDrillDown();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErroSimples("Erro ao executar a operação!", StdPlatBS100.StdBSTipos.IconId.PRI_Critico, ex.Message);
            }
        }

        private void priGrelha1_EditMode(object Sender, PriGrelha.EditModeEventArgs e)
        {
            try
            {
                if ((e.Mode == 0) && (e.ChangeMade))
                {
                    TrataColuna(e.Col, e.Row);
                }
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErroSimples("Erro ao executar a operação!", StdPlatBS100.StdBSTipos.IconId.PRI_Critico, ex.Message);
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
                priGrelha1.Inicializa(PriSDKContext.SdkContext);

                controlsInitialized = true;
            }
        }

        private void InitializeGrid()
        {
            priGrelha1.BandaMenuContexto = "PopupGrelhasStd";
            priGrelha1.IniciaDadosConfig();

            // columns
            priGrelha1.AddColKey(colArtigo, FpCellType.CellTypeEdit, "Artigo", 10, blnDrillDown: true);
            priGrelha1.AddColKey(colDescArtigo, FpCellType.CellTypeStaticText, "Descrição", 15, blnBloqueada: true);
            priGrelha1.AddColKey(colLote, FpCellType.CellTypeEdit, "Lote", 8);
            priGrelha1.AddColKey(colNumSerie, FpCellType.CellTypeEdit, "Núm.Série", 8, blnDrillDown: true);

            priGrelha1.AddTituloColuna(colArtigo, "Artigo");
            priGrelha1.AddTituloColuna(colDescArtigo, "Descrição");
            priGrelha1.AddTituloColuna(colLote, "Lote");
            priGrelha1.AddTituloColuna(colNumSerie, "Núm.Série");

            priGrelha1.AddColKey(colTipoEntidade, FpCellType.CellTypeComboBox, "Tipo Ent.", 8, strComboValores: DaValoresComboTipoEntidade());
            priGrelha1.AddColKey(colEntidade, FpCellType.CellTypeEdit, "Entidade", 10, blnDrillDown: true);
            priGrelha1.AddColKey(colNomeEntidade, FpCellType.CellTypeStaticText, "Nome", 10, blnBloqueada: true, blnDrillDown: true);

            priGrelha1.AddTituloColuna(colTipoEntidade, "Tipo");
            priGrelha1.AddTituloColuna(colEntidade, "Entidade");
            priGrelha1.AddTituloColuna(colNomeEntidade, "Nome");

            priGrelha1.AddColKey(colTipoAnomalia, FpCellType.CellTypeComboBox, "Anomalia", 8, strComboValores: DaValoresComboTipoAnomalia());
            priGrelha1.AddColKey(colDescricao, FpCellType.CellTypeEdit, "Descrição", 20);
            priGrelha1.AddColKey(colQuantidade, FpCellType.CellTypeFloat, "Quant.", 8, intCasasDecimais: 2);
            priGrelha1.AddColKey(colUnidade, FpCellType.CellTypeEdit, "Un.", 6, blnDrillDown: true);

            priGrelha1.AddTituloColuna(colTipoAnomalia, "Tipo");
            priGrelha1.AddTituloColuna(colDescricao, "Descrição");
            priGrelha1.AddTituloColuna(colQuantidade, "Quant.");
            priGrelha1.AddTituloColuna(colUnidade, "Un.");

            priGrelha1.AddColKey(colArmazem, FpCellType.CellTypeEdit, "Localização", 8, blnDrillDown: true);
            priGrelha1.AddColKey(colLocalizacao, FpCellType.CellTypeEdit, "Localização", 8);
            priGrelha1.AddColKey(colEstado, FpCellType.CellTypeEdit, "Estado", 8, blnDrillDown: true);

            priGrelha1.AddTituloColuna(colArmazem, "Armazém");
            priGrelha1.AddTituloColuna(colLocalizacao, "Localização");
            priGrelha1.AddTituloColuna(colEstado, "Estado");

            // Other properties
            priGrelha1.TituloGrelha = "Demo Grid Edit 1";
            priGrelha1.PermiteAgrupamentosUser = false;
            priGrelha1.PermiteDataFill = false;
            priGrelha1.PermiteEdicao = true;
            priGrelha1.ColsFrozen = 2;
            priGrelha1.NumLinhasCabecalho = 2;
            priGrelha1.PermiteOrdenacao = true;
            priGrelha1.PermiteFiltros = false;

            priGrelha1.FormataGrelha(true);
            priGrelha1.LimpaGrelha();
        }

        private string DaValoresComboTipoEntidade()
        {
            return
                "Cliente\t" +
                "Fornecedor\t" +
                "Outro Terceiro";
        }

        private string DaValoresComboTipoAnomalia()
        {
            return
                "Avaria\t" +
                "Quebra\t" +
                "Defeito";
        }

        public void F4RowFields(string Categoria, string NomeCampo, dynamic Valor)
        {
            try
            {
                if (Categoria == ConstantesPrimavera100.Categorias.Artigo)
                {
                    TrataColuna_Artigo(priGrelha1.Grelha.ActiveRow);
                }
                else if (Categoria == ConstantesPrimavera100.Categorias.Unidade)
                {
                    TrataColuna_Unidade(priGrelha1.Grelha.ActiveRow);
                }
                else if ((Categoria == ConstantesPrimavera100.Categorias.Cliente) || (Categoria == ConstantesPrimavera100.Categorias.Fornecedor) || (Categoria == ConstantesPrimavera100.Categorias.OutroTerceiro))
                {
                    TrataColuna_Entidade(priGrelha1.Grelha.ActiveRow);
                }
                else if (Categoria == ConstantesPrimavera100.Categorias.Armazem)
                {
                    TrataColuna_Armazem(priGrelha1.Grelha.ActiveRow);
                }
                else if (Categoria == ConstantesPrimavera100.Categorias.ArmazemLocalizacoes)
                {
                    TrataColuna_Localizacao(priGrelha1.Grelha.ActiveRow);
                }
                else if (Categoria == ConstantesPrimavera100.Categorias.EstadosInventario)
                {
                    TrataColuna_Estado(priGrelha1.Grelha.ActiveRow);
                }
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErro("Erro ao carregar o registo.", StdPlatBS100.StdBSTipos.IconId.PRI_Exclama, ex.Message, ex);
            }
        }

        private void ExecuteDrillDown()
        {
            int row = priGrelha1.Grelha.ActiveRowIndex;
            int col = priGrelha1.Grelha.ActiveColumnIndex;

            string colKey = priGrelha1.Cols.GetEditaCol(col).ColKey;

            if ((colKey == colArtigo) || (colKey == colDescArtigo))
            {
                string artigo = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colArtigo));
                DrillDownManager.DrillDownEntidade(PSO, ConstantesPrimavera100.Audit.TAB_ARTIGOS, artigo);
            }
            else if (colKey == colUnidade)
            {
                string unidade = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colUnidade));
                DrillDownManager.DrillDownEntidade(PSO, ConstantesPrimavera100.Audit.TAB_UNIDADES, unidade);
            }
            else if ((colKey == colEntidade) || (colKey == colNomeEntidade))
            {
                string entidade = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colEntidade));
                DrillDownManager.DrillDownEntidade(PSO, DaCategoriaEntidadeDrillDown(row), entidade);
            }
            else if (colKey == colArmazem)
            {
                string armazem = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colArmazem));
                DrillDownManager.DrillDownEntidade(PSO, ConstantesPrimavera100.Audit.TAB_ARMAZENS, armazem);
            }
            else if (colKey == colEstado)
            {
                string estado = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colEstado));
                DrillDownManager.DrillDownEntidade(PSO, ConstantesPrimavera100.Audit.TAB_ESTADOS_INVENTARIO, estado);
            }
        }

        private string DaCategoriaEntidadeDrillDown(int row)
        {
            string result = string.Empty;

            int tipoentidade = priGrelha1.GetGRID_GetValorCelula(row, colTipoEntidade);

            switch (tipoentidade)
            {
                case 0:
                    result = ConstantesPrimavera100.Audit.TAB_CLIENTES;
                    break;
                case 1:
                    result = ConstantesPrimavera100.Audit.TAB_FORNECEDORES;
                    break;
                case 2:
                    result = ConstantesPrimavera100.Audit.TAB_OUTROSTERC;
                    break;
                default:
                    break;
            }

            return result;
        }

        private void TrataColuna(int col, int row)
        {
            if ((row > priGrelha1.Grelha.DataRowCnt) || row == 0)
            {
                return;
            }

            if (col == priGrelha1.Cols.GetEdita(colArtigo).Number)
            {
                TrataColuna_Artigo(row);
            }
            else if (col == priGrelha1.Cols.GetEdita(colLote).Number)
            {
                TrataColuna_Lote(row);
            }
            else if (col == priGrelha1.Cols.GetEdita(colUnidade).Number)
            {
                TrataColuna_Unidade(row);
            }
            else if (col == priGrelha1.Cols.GetEdita(colEntidade).Number)
            {
                TrataColuna_Entidade(row);
            }
            else if (col == priGrelha1.Cols.GetEdita(colArmazem).Number)
            {
                TrataColuna_Armazem(row);
            }
            else if (col == priGrelha1.Cols.GetEdita(colLocalizacao).Number)
            {
                TrataColuna_Localizacao(row);
            }
            else if (col == priGrelha1.Cols.GetEdita(colEstado).Number)
            {
                TrataColuna_Estado(row);
            }
        }

        private void TrataColuna_Artigo(int row)
        {
            try
            {
                bool artigoInjectado = false;
                string artigo = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colArtigo));

                if (!string.IsNullOrWhiteSpace(artigo))
                {
                    if (BSO.Base.Artigos.Existe(artigo))
                    {
                        BasBEArtigo beArtigo = BSO.Base.Artigos.Consulta(artigo);

                        priGrelha1.SetGRID_SetValorCelula(row, colArtigo, beArtigo.Artigo.ToUpper());
                        priGrelha1.SetGRID_SetValorCelula(row, colDescArtigo, beArtigo.Descricao.ToUpper());

                        priGrelha1.SetGRID_SetValorCelula(row, colTipoEntidade, "Cliente");

                        priGrelha1.SetGRID_SetValorCelula(row, colTipoAnomalia, "Avaria");

                        priGrelha1.SetGRID_SetValorCelula(row, colQuantidade, 1);
                        priGrelha1.SetGRID_SetValorCelula(row, colUnidade, beArtigo.UnidadeBase.ToUpper());

                        priGrelha1.SetGRID_SetValorCelula(row, colArmazem, beArtigo.ArmazemSugestao);
                        TrataColuna_Armazem(row);

                        artigoInjectado = true;
                    }
                    else
                    {
                        PSO.Dialogos.MostraErroSimples("Artigo inexistente!");                        
                    }
                }

                if (!artigoInjectado)
                {
                    LimpaCamposGrelhaGrupoArtigo(row);
                }
            }
            catch (Exception ex)
            {
                LimpaCamposGrelhaGrupoArtigo(row);
                throw ex;
            }
        }

        private void TrataColuna_Lote(int row)
        {
            try
            {
                string artigo = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colArtigo));
                string lote = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colLote));

                if (!string.IsNullOrWhiteSpace(artigo) && !string.IsNullOrWhiteSpace(lote))
                {
                    if (BSO.Inventario.ArtigosLotes.Existe(artigo, lote))
                    {
                        priGrelha1.SetGRID_SetValorCelula(row, colLote, lote.ToUpper());
                    }
                    else
                    {
                        PSO.Dialogos.MostraErroSimples("Lote inexistente!");
                        priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colLote).ColKey, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colLote).ColKey, string.Empty);
                throw ex;
            }
        }

        private void TrataColuna_Unidade(int row)
        {
            try
            {
                string unidade = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colUnidade));

                if (!string.IsNullOrWhiteSpace(unidade))
                {
                    if (BSO.Base.Unidades.Existe(unidade))
                    {
                        priGrelha1.SetGRID_SetValorCelula(row, colUnidade, unidade.ToUpper());
                    }
                    else
                    {
                        PSO.Dialogos.MostraErroSimples("Unidade inexistente!");
                        priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colUnidade).ColKey, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colUnidade).ColKey, string.Empty);
                throw ex;
            }
        }

        private void TrataColuna_Entidade(int row)
        {
            int tipoEntidade = 0;
            string entidade = string.Empty;
            bool entidadeInjetada = false;

            try
            {
                tipoEntidade = PSO.Utils.FInt(priGrelha1.GetGRID_GetValorCelula(row, colTipoEntidade));
                entidade = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colEntidade));

                if (!string.IsNullOrWhiteSpace(entidade))
                {
                    if (EntidadeExiste(tipoEntidade, entidade))
                    {
                        priGrelha1.SetGRID_SetValorCelula(row, colEntidade, entidade.ToUpper());
                        priGrelha1.SetGRID_SetValorCelula(row, colNomeEntidade, GetNomeEntidade(tipoEntidade, entidade));

                        entidadeInjetada = true;
                    }
                    else
                    {
                        PSO.Dialogos.MostraErroSimples("Entidade inexistente!");
                    }
                }

                if (!entidadeInjetada)
                {
                    LimpaCamposGrelhaGrupoEntidade(row);
                }
            }
            catch (Exception ex)
            {
                LimpaCamposGrelhaGrupoEntidade(row);
                throw ex;
            }
        }

        private void TrataColuna_Armazem(int row)
        {
            string armazem = string.Empty;
            bool armazemInjetado = false;

            try
            {
                armazem = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colArmazem));

                if (!string.IsNullOrWhiteSpace(armazem))
                {
                    if (BSO.Inventario.Armazens.Existe(armazem))
                    {
                        priGrelha1.SetGRID_SetValorCelula(row, colArmazem, armazem.ToUpper());
                        priGrelha1.SetGRID_SetValorCelula(row, colLocalizacao, armazem.ToUpper());

                        armazemInjetado = true;
                    }
                    else
                    {
                        PSO.Dialogos.MostraErroSimples("Amazém inexistente!");
                    }
                }

                if (!armazemInjetado)
                {
                    LimpaCamposGrelhaGrupoArmazem(row);
                }
            }
            catch (Exception ex)
            {
                LimpaCamposGrelhaGrupoArmazem(row);
                throw ex;
            }
        }

        private void TrataColuna_Localizacao(int row)
        {
            try
            {
                string armazem = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colArmazem));
                string localizacao = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colLocalizacao));

                if (!string.IsNullOrWhiteSpace(armazem) && !string.IsNullOrWhiteSpace(localizacao))
                {
                    if (BSO.Inventario.ArmazemLocalizacao.ExisteLocArmazem(localizacao, armazem))
                    {
                        priGrelha1.SetGRID_SetValorCelula(row, colLocalizacao, localizacao.ToUpper());
                    }
                    else
                    {
                        PSO.Dialogos.MostraErroSimples("Localização inexistente!");
                        priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colLocalizacao).ColKey, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colLocalizacao).ColKey, string.Empty);
                throw ex;
            }
        }

        private void TrataColuna_Estado(int row)
        {
            try
            {
                string estado = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colEstado));

                if (!string.IsNullOrWhiteSpace(estado))
                {
                    if (BSO.Inventario.EstadosInventario.Existe(estado))
                    {
                        priGrelha1.SetGRID_SetValorCelula(row, colEstado, estado.ToUpper());
                    }
                    else
                    {
                        PSO.Dialogos.MostraErroSimples("Estado inexistente!");
                        priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colEstado).ColKey, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colEstado).ColKey, string.Empty);
                throw ex;
            }
        }

        private bool EntidadeExiste(int tipoEntidade, string entidade)
        {
            bool result = false;

            switch (tipoEntidade)
            {
                case 0:
                    result = BSO.Base.Clientes.Existe(entidade);
                    break;
                case 1:
                    result = BSO.Base.Fornecedores.Existe(entidade);
                    break;
                case 2:
                    result = BSO.Base.OutrosTerceiros.Existe(entidade);
                    break;
                default:
                    break;
            }

            return result;
        }

        private string GetNomeEntidade(int tipoEntidade, string entidade)
        {
            string result = string.Empty;

            switch (tipoEntidade)
            {
                case 0:
                    result = BSO.Base.Clientes.DaNome(entidade);
                    break;
                case 1:
                    result = BSO.Base.Fornecedores.DaNome(entidade);
                    break;
                case 2:
                    result = BSO.Base.OutrosTerceiros.DaNome(entidade);
                    break;
                default:
                    break;
            }

            return result;
        }

        private void LimpaCamposGrelhaGrupoArtigo(int row)
        {
            priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colArtigo).ColKey, string.Empty);
            priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colDescArtigo).ColKey, string.Empty);
            priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colLote).ColKey, string.Empty);
            priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colNumSerie).ColKey, string.Empty);
        }

        private void LimpaCamposGrelhaGrupoEntidade(int row)
        {
            priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colTipoEntidade).ColKey, 0);
            priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colEntidade).ColKey, string.Empty);
            priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colNomeEntidade).ColKey, string.Empty);
        }

        private void LimpaCamposGrelhaGrupoArmazem(int row)
        {
            priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colArmazem).ColKey, string.Empty);
            priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colLocalizacao).ColKey, string.Empty);
            priGrelha1.SetGRID_SetValorCelula(row, priGrelha1.Cols.GetEdita(colEstado).ColKey, string.Empty);
        }

        private void TrataF4Linhas(int col, int row)
        {
            // Artigos
            if (col == priGrelha1.Cols.GetEdita(colArtigo).Number)
            {
                PSO.AbreLista(1, ConstantesPrimavera100.Categorias.Artigo, "Artigo", this.ParentForm, priGrelha1.Grelha, "mnuTabArtigo", row, col, false, "(ArtigoAnulado = 0)");
            }
            else
            {
                string artigo = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colArtigo));

                if (!string.IsNullOrWhiteSpace(artigo))
                {
                    // Lotes
                    if (col == priGrelha1.Cols.GetEdita(colLote).Number)
                    {
                        PSO.AbreLista(1, ConstantesPrimavera100.Categorias.ArtigoLote, "Lote", this.ParentForm, priGrelha1.Grelha, ConstantesPrimavera100.Audit.TAB_ARTIGOS, row, col, false, string.Format("([ARTIGOLOTE].Artigo = '{0}')", artigo));
                    }
                    
                    // Entidade
                    if (col == priGrelha1.Cols.GetEdita(colEntidade).Number)
                    {
                        string categoria = ConstantesPrimavera100.Categorias.Cliente;
                        string audit = ConstantesPrimavera100.Audit.TAB_CLIENTES;
                        string campo = "Cliente";
                        string where = "(ClienteAnulado = 0)";

                        switch (PSO.Utils.FInt(priGrelha1.GetGRID_GetValorCelula(row, colTipoEntidade)))
                        {
                            case 1:
                                categoria = ConstantesPrimavera100.Categorias.Fornecedor;
                                audit = ConstantesPrimavera100.Audit.TAB_FORNECEDORES;
                                campo = "Fornecedor";
                                where = "(FornecedorAnulado = 0)";
                                break;
                            case 2:
                                categoria = ConstantesPrimavera100.Categorias.OutroTerceiro;
                                audit = ConstantesPrimavera100.Audit.TAB_OUTROSTERC;
                                campo = "Terceiro";
                                where = "(Anulado = 0)";
                                break;
                            default:
                                break;
                        }
                        PSO.AbreLista(1, categoria, campo, this.ParentForm, priGrelha1.Grelha, audit, row, col, false, where);
                    }

                    // Armazém
                    if (col == priGrelha1.Cols.GetEdita(colArmazem).Number)
                    {
                        PSO.AbreLista(1, ConstantesPrimavera100.Categorias.Armazem, "Armazem", this.ParentForm, priGrelha1.Grelha, ConstantesPrimavera100.Audit.TAB_ARMAZENS, row, col, false);
                    }

                    // Localização
                    if (col == priGrelha1.Cols.GetEdita(colLocalizacao).Number)
                    {
                        string armazem = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colArmazem));
                        PSO.AbreLista(1, ConstantesPrimavera100.Categorias.ArmazemLocalizacoes, "Localizacao", this.ParentForm, priGrelha1.Grelha, ConstantesPrimavera100.Audit.TAB_ARMAZEMLOCALIZACAO, row, col, false, string.Format("(Armazem = '{0}')", armazem));
                    }

                    // Estado
                    if (col == priGrelha1.Cols.GetEdita(colEstado).Number)
                    {
                        PSO.AbreLista(1, ConstantesPrimavera100.Categorias.EstadosInventario, "Estado", this.ParentForm, priGrelha1.Grelha, ConstantesPrimavera100.Audit.TAB_ESTADOS_INVENTARIO, row, col, false, "Disponivel = 0 AND EstadoReserva = 0 AND Previsto = 0 AND Transito = 0");
                    }
                }
            }
        }

        private void NewRecord()
        {
            int nextNumber = GetMaxNumber() + 1;

            numericUpDownNumber.Minimum = 1;
            numericUpDownNumber.Maximum = nextNumber;
            numericUpDownNumber.Value = nextNumber;

            dateTimePickerDate.Value = DateTime.Now;

            priGrelha1.LimpaGrelha();
        }

        private int GetMaxNumber()
        {
            int result = 0;

            try
            {
                string sql = "SELECT MaxNumero = MAX(CDU_Numero) FROM TDU_CabecAnomalias";
                result = (int)BSO.Consulta(sql).Valor("MaxNumero");
            }
            catch
            {
                result = 0;
            }

            return result;
        }

        private void Edit()
        {
            try
            {
                int number = (int)numericUpDownNumber.Value;

                // Load values from cabec
                bool exists = EditCabec(number);

                if (exists)
                {
                    // Load values from rows
                    EditRows(number);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool EditCabec(int number)
        {
            bool result = false;

            string sql = PSO.Sql.FormatSQL("SELECT * FROM TDU_CabecAnomalias WHERE CDU_Numero = @1@", number);
            StdBELista list = BSO.Consulta(sql);

            if (list != null)
            {
                if (list.NumLinhas() == 1)
                {
                    result = true;

                    numericUpDownNumber.Value = list.Valor("CDU_Numero");
                    dateTimePickerDate.Value = list.Valor("CDU_Data");
                }
            }

            return result;
        }

        private void EditRows(int number)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT		linhas.*, DescArtigo = art.Descricao, ent.Nome");
            sql.AppendLine("FROM		TDU_LinhasAnomalias	linhas");
            sql.AppendLine("INNER JOIN	Artigo				art		ON linhas.CDU_Artigo = art.Artigo");
            sql.AppendLine("LEFT JOIN	(");
            sql.AppendLine("						SELECT TipoEntidade = 'C', Entidade = Cliente, Nome FROM Clientes");
            sql.AppendLine("				UNION	SELECT TipoEntidade = 'F', Entidade = Fornecedor, Nome FROM Fornecedores");
            sql.AppendLine("				UNION	SELECT TipoEntidade = 'O', Entidade = Terceiro, Nome FROM OutrosTerceiros");
            sql.AppendLine("			)					ent		ON linhas.CDU_TipoEntidade = ent.TipoEntidade AND linhas.CDU_Entidade = ent.Entidade");
            sql.AppendLine(PSO.Sql.FormatSQL("WHERE       linhas.CDU_Numero = @1@", number));

            StdBELista list = BSO.Consulta(sql.ToString());

            if (list != null)
            {
                int row = 1;

                while (!list.NoFim())
                {
                    // Grupo Artigo
                    priGrelha1.SetGRID_SetValorCelula(row, colArtigo, list.Valor("CDU_Artigo"));
                    priGrelha1.SetGRID_SetValorCelula(row, colDescArtigo, list.Valor("DescArtigo"));
                    priGrelha1.SetGRID_SetValorCelula(row, colLote, list.Valor("CDU_Lote"));
                    priGrelha1.SetGRID_SetValorCelula(row, colNumSerie, list.Valor("CDU_NumSerie"));

                    // Grupo Entidade
                    string tipoEntidade = list.Valor("CDU_TipoEntidade");
                    SetValorComboboxGrelha(colTipoEntidade, row, GetDescTipoEntidade(tipoEntidade));

                    priGrelha1.SetGRID_SetValorCelula(row, colEntidade, list.Valor("CDU_Entidade"));
                    priGrelha1.SetGRID_SetValorCelula(row, colNomeEntidade, list.Valor("Nome"));

                    // Grupo Anomalia
                    int tipoAnomalia = list.Valor("CDU_TipoAnomalia");
                    priGrelha1.SetGRID_SetValorCelula(row, colTipoAnomalia, GetDescTipoAnomalia(tipoAnomalia));
                    priGrelha1.SetGRID_SetValorCelula(row, colDescricao, list.Valor("CDU_Descricao"));
                    priGrelha1.SetGRID_SetValorCelula(row, colQuantidade, list.Valor("CDU_Quantidade"));
                    priGrelha1.SetGRID_SetValorCelula(row, colUnidade, list.Valor("CDU_Unidade"));

                    // Grupo Localização
                    priGrelha1.SetGRID_SetValorCelula(row, colArmazem, list.Valor("CDU_Armazem"));
                    priGrelha1.SetGRID_SetValorCelula(row, colLocalizacao, list.Valor("CDU_Localizacao"));
                    priGrelha1.SetGRID_SetValorCelula(row, colEstado, list.Valor("CDU_Estado"));

                    list.Seguinte();
                    row++;
                }
            }
        }

        private void SetValorComboboxGrelha(string col, int row, string value)
        {
            //Actualiza o valor da combo na grelha.
            priGrelha1.Grelha.SetText(priGrelha1.Cols.GetEdita(col).Number, row, value);
        }

        private void Remove()
        {
            try
            {
                int number = (int)numericUpDownNumber.Value;

                // Start a transaction
                BSO.IniciaTransaccao();

                // Delete the rows
                RemoveRows(number);

                // Delete the cabec
                RemoveCabec(number);

                // Finish the transaction
                BSO.TerminaTransaccao();
            }
            catch (Exception ex)
            {
                // Abort the transaction
                BSO.DesfazTransaccao();
                throw ex;
            }
        }

        private void RemoveRows(int number)
        {
            string sql = PSO.Sql.FormatSQL("DELETE FROM TDU_LinhasAnomalias WHERE CDU_Numero = @1@", number);
            BSO.DSO.ExecuteSQL(sql);
        }

        private void RemoveCabec(int number)
        {
            string sql = PSO.Sql.FormatSQL("DELETE FROM TDU_CabecAnomalias WHERE CDU_Numero = @1@", number);
            BSO.DSO.ExecuteSQL(sql);
        }

        private void Save()
        {
            try
            {
                int numero = 0;

                // Start a transaction
                BSO.IniciaTransaccao();

                // Save the cabec and returns the record number
                SaveCabec(ref numero);

                // Save the rows with the returned record number
                SaveRows(numero);

                // Finish the transaction
                BSO.TerminaTransaccao();
            }
            catch (Exception ex)
            {
                // Abort the transaction
                BSO.DesfazTransaccao();
                throw ex;
            }
        }

        private void SaveCabec(ref int number)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("INSERT INTO TDU_CabecAnomalias (CDU_Data, CDU_Utilizador)");
            sql.AppendLine(PSO.Sql.FormatSQL("VALUES (@1@, '@2@')", dateTimePickerDate.Value, BSO.Contexto.UtilizadorActual));

            BSO.DSO.ExecuteSQL(sql.ToString());

            number = GetMaxNumber();
        }

        private void SaveRows(int number)
        {
            StringBuilder sql = new StringBuilder();

            for (int i = 1; i <= priGrelha1.Grelha.DataRowCnt; i++)
            {
                sql.Clear();

                sql.AppendLine("INSERT INTO TDU_LinhasAnomalias");
                sql.AppendLine("(CDU_Numero, CDU_Artigo, CDU_Lote, CDU_NumSerie, CDU_TipoEntidade, CDU_Entidade, CDU_TipoAnomalia, CDU_Descricao, CDU_Quantidade, CDU_Unidade, CDU_Armazem, CDU_Localizacao, CDU_Estado)");
                sql.AppendLine(PSO.Sql.FormatSQL("VALUES (@1@, '@2@', '@3@', '@4@', '@5@', '@6@', @7@, '@8@', @9@, '@10@', '@11@', '@12@', '@13@')",
                    number,
                    PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(i, colArtigo)),
                    PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(i, colLote)),
                    PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(i, colNumSerie)),
                    GetTipoEntidadeFromGrid(i),
                    PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(i, colEntidade)),
                    PSO.Utils.FInt(priGrelha1.GetGRID_GetValorCelula(i, colTipoAnomalia)),
                    PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(i, colDescricao)),
                    PSO.Utils.FDbl(priGrelha1.GetGRID_GetValorCelula(i, colQuantidade)),
                    PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(i, colUnidade)),
                    PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(i, colArmazem)),
                    PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(i, colLocalizacao)),
                    PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(i, colEstado))));

                BSO.DSO.ExecuteSQL(sql.ToString());
            }
        }

        private string GetTipoEntidadeFromGrid(int row)
        {
            string result = "C";

            switch (PSO.Utils.FInt(priGrelha1.GetGRID_GetValorCelula(row, colTipoEntidade)))
            {
                case 1:
                    result = "F";
                    break;
                case 2:
                    result = "O";
                    break;
                default:
                    break;
            }

            return result;
        }

        private string GetDescTipoEntidade(string tipoEntidade)
        {
            string result = string.Empty;

            switch (tipoEntidade)
            {
                case "C":
                    result = "Cliente";
                    break;
                case "F":
                    result = "Fornecedor";
                    break;
                case "O":
                    result = "Outro Terceiro";
                    break;
                default:
                    break;
            }

            return result;
        }

        private string GetDescTipoAnomalia(int tipoAnomalia)
        {
            string result = string.Empty;

            switch (tipoAnomalia)
            {
                case 0:
                    result = "Avaria";
                    break;
                case 1:
                    result = "Quebra";
                    break;
                case 2:
                    result = "Defeito";
                    break;
                default:
                    break;
            }

            return result;
        }

        #endregion Private       
    }
}

