using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using StdBE100;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraBars.Docking;
using Primavera.Extensibility.Integration;
using static Primavera.Platform.Geolocation.MapHelper;
using Primavera.Extensibility.Integration.Context;

namespace Primavera.Platform.Geolocation
{
    /// <summary>
    /// Classe que configura o separador.
    /// </summary>
    public partial class TabMapBase : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        internal dynamic Entity { get; set; }

        private EditorContext contextService { get; set; }

        /// <summary>
        /// Propriedade referente a lista de servidores de mapa.
        /// </summary>
        private List<Map> listMapServers;

        /// <summary>
        /// Propriedade referente à lista de marcadores do mapa.
        /// </summary>
        private readonly List<Marker> mapMarkers = new List<Marker>();

        /// <summary>
        /// Propriedade referente à lista de marcadores do utilizador.
        /// </summary>
        private List<Marker> userMarkers;

        /// <summary>
        /// Propriedade referente à lista de rotas do utilizador.
        /// </summary>
        private List<Route> userRoutes;

        /// <summary>
        /// Propriedade referente ao mapa.
        /// </summary>
        private MapManager map;

        /// <summary>
        /// Construtor da classe sem parâmetros onde é inicializada a lista de marcadores do mapa.
        /// </summary>
        public TabMapBase()
        {
            InitializeComponent();
        }

        public TabMapBase(EditorContext context, dynamic entity) : this()
        {
            this.Entity = entity;
            this.contextService = context;
        }

        #region Eventos do User Control

        /// <summary>
        /// Método responsável pelo evento: carregar o mapa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabMapBase_Load(object sender, EventArgs e)
        {
            LoadMapServerSettings();

            ReadMapPreferences(string.Empty);

            LoadUserMarkers(((IProductContext)contextService).Aplicacao.Utilizador.Utilizador);

            LoadMapMarkers();

            LoadUserRoutes(contextService.Aplicacao.Utilizador.Utilizador);

            if (!(Entity is null))
            {
                ShowEntityLocationOnMap();
            }

            if (barMapServer.EditValue is Map selectedMap)
            {
                SetMapSource(selectedMap.Name, selectedMap.Key);
            }

            SetupMapControl();

            listRouteMarkers.Refresh();
            RefreshMarkersOverlay();

            SetZoomOnLoadMap();
        }

        /// <summary>
        /// Método responsável pelas alterações aquando da escolha da Entidade.
        /// </summary>
        internal void Loading()
        {
            if (barMapServer.EditValue is Map selectedMap)
            {
                bool selectedMapChanged = ReadMapPreferences(selectedMap.Name);
                if (!selectedMapChanged)
                {
                    mapMarkers.Clear();
                    RefreshUnusedUserMarkers();

                    mapControl.Overlays.Clear();

                    if (!(Entity is null))
                    {
                        ShowEntityLocationOnMap();
                    }

                    listRouteMarkers.Refresh();
                    RefreshMarkersOverlay();

                    SetZoomOnLoadMap();
                }
            }
            else
            {
                ReadMapPreferences(string.Empty);
            }
        }

        /// <summary>
        /// Método responsável pelo evento: apagar marcador do utilizador.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteUserMarker_Click(object sender, EventArgs e)
        {
            if (listUserMarkers.SelectedValue is Marker selectedMarker)
            {
                bool isRouteMarker = CheckRouteMarker(selectedMarker.Id);
                string message = string.Format(isRouteMarker ? Properties.Resources.RES_DeleteUserRouteMarker : Properties.Resources.RES_DeleteUserMarker, selectedMarker.Description);

                if (contextService.PSO.MensagensDialogos.MostraPerguntaSimples(message))
                {
                    DeleteUserMarker(selectedMarker.Id);
                    LoadUserMarkers(contextService.Aplicacao.Utilizador.Utilizador);

                    mapMarkers.RemoveAll(p => p.Id == selectedMarker.Id);
                    RefreshUnusedUserMarkers();

                    if (mapControl.Overlays.Any(a => a.Id == "routes"))
                    {
                        ClearMapRoutes();

                        List<MapRoute> route = CalculateRoute(barAvoidHighways.Checked, barWalkingMode.Checked);
                        if (route?.Count > 0)
                        {
                            ShowRoute(route);
                            ShowDirections(route);
                        }
                    }

                    listRouteMarkers.Refresh();
                    RefreshMarkersOverlay();
                    SetZoomOnLoadMap();
                }
            }
        }

        /// <summary>
        /// Método responsável pelo evento: selecionar um marcador do utilizador e acrescentá-lo à rota.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListUserMarkers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            AddUserMarkerToRoute();
        }

        /// <summary>
        /// Método responsável pelo evento: mostrar descrição do marcador quando o seleciono.
        /// </summary>
        /// <param name="item">Marcador selcionado</param>
        /// <param name="e"></param>
        private void MapControl_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                popupMenu1.Tag = item;
                popupMenu1.ShowPopup(Control.MousePosition);
            }
        }

        /// <summary>
        /// Método responsável pelo evento: remover marcador quando o seleciono no mapa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPopupRemoveMarker_ItemClick(object sender, ItemClickEventArgs e)
        {
            GMapMarker marker = popupMenu1.Tag as GMapMarker;
            RemoveMarkerFromMap(marker);
        }

        /// <summary>
        /// Método responsável pelo evento: gravar marcador quando o seleciono no mapa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPopupSaveMarker_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(popupMenu1.Tag is GMapMarker mapMarker))
            {
                return;
            }

            if (!(mapMarker.Tag is Marker marker))
            {
                return;
            }

            string markerName = GetInfoForSaveMarker(string.Empty, out bool pontoInteresse);
            if (string.IsNullOrWhiteSpace(markerName))
            {
                return;
            }
            PointLatLng pointLatLng = new PointLatLng(marker.Latitude, marker.Longitude);
            SaveMarker(marker.Id, pointLatLng, markerName, pontoInteresse);
            LoadUserMarkers(contextService.Aplicacao.Utilizador.Utilizador);
        }

        /// <summary>
        /// Método responsável pelo evento: guardar marcador quando o seleciono o botão na barra main.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarSaveMarker_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (map is null)
            {
                return;
            }

            if (barSearch.EditValue is null)
            {
                return;
            }

            string searchText = barSearch.EditValue.ToString();
            PointLatLng? pointLatLng = AddMapMarker(searchText, out Guid markerId);

            if (!pointLatLng.HasValue)
            {
                return;
            }

            string markerName = GetInfoForSaveMarker(searchText, out bool pointOfInterest);
            if (string.IsNullOrWhiteSpace(markerName))
            {
                return;
            }

            SaveMarker(markerId, pointLatLng.Value, markerName, pointOfInterest);
            LoadUserMarkers(contextService.Aplicacao.Utilizador.Utilizador);

            listRouteMarkers.Refresh();
            RefreshMarkersOverlay();
            SetZoomOnLoadMap(true);
        }

        /// <summary>
        /// Método responsável pelo evento: limpar rota quando seleciono o botão na barra main.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarClearRoute_ItemClick(object sender, ItemClickEventArgs e)
        {
            barChooseRoute.EditValue = null;

            mapMarkers.Clear();
            RefreshUnusedUserMarkers();

            mapControl.Overlays.Clear();

            if (!(Entity is null))
            {
                ShowEntityLocationOnMap();
            }

            listRouteMarkers.Refresh();
            RefreshMarkersOverlay();

            SetZoomOnLoadMap();
        }

        /// <summary>
        /// Método responsável pelo evento: gravar rota quando seleciono o botão na barra main.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarSaveRoute_ItemClick(object sender, ItemClickEventArgs e)
        {
            bool isValidRoute = ValidateRouteBeforeSave(out List<Guid> routePoints, out string routeName);

            if (!isValidRoute)
            {
                return;
            }

            Guid routeGuid = Guid.NewGuid();
            SaveRoute(routeGuid, routeName);

            SaveRoutePoints(routeGuid, routePoints);

            LoadUserRoutes(contextService.Aplicacao.Utilizador.Utilizador);
            barChooseRoute.EditValue = userRoutes.First(r => r.Name == routeName);
        }

        /// <summary>
        /// Método responsável pelo evento: calcular e mostrar rota no mapa e mostrar direcões quando seleciono o botão na barra main.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarDrawRoute_ItemClick(object sender, ItemClickEventArgs e)
        {
            ClearMapRoutes();

            List<MapRoute> route = CalculateRoute(barAvoidHighways.Checked, barWalkingMode.Checked);
            if (route is null || route.Count == 0)
            {
                return;
            }
            ShowRoute(route);

            ShowDirections(route);
        }

        /// <summary>
        /// Método responsável pelo evento: pesquisar localização no mapa na barra main.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarSearch_ShownEditor(object sender, ItemClickEventArgs e) //???
        {
            var item = (BarEditItem)sender;
            item.Manager.ActiveEditor.KeyDown -= ActiveEditor_KeyDown;
            item.Manager.ActiveEditor.KeyDown += ActiveEditor_KeyDown;
        }

        /// <summary>
        /// Método responsável pelo evento: editar campo de pesquisa na barra main.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActiveEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (map is null)
            {
                return;
            }

            var editItem = sender as SearchControl;
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                AddMapMarker(editItem.EditValue.ToString(), out _);

                listRouteMarkers.Refresh();
                RefreshMarkersOverlay();
                SetZoomOnLoadMap(true);
            }
        }

        /// <summary>
        /// Método responsável pelo evento: escolher servidor de mapa da lista e aplicar as respetivas configurações.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarMapServer_EditValueChanged(object sender, EventArgs e)
        {
            if (!(sender is BarEditItem lookupMapServer))
            {
                return;
            }
            if (!(lookupMapServer.EditValue is Map selectedMap))
            {
                return;
            }

            SetMapSource(selectedMap.Name, selectedMap.Key);

            ConfigSeletedMapServerSettings(selectedMap);

            SetMapFunctionalities(selectedMap);

            SetupMapControl();

            listRouteMarkers.Refresh();

            if (!(Entity is null))
            {
                ShowEntityLocationOnMap();
            }

            RefreshMarkersOverlay();

            SetZoomOnLoadMap();
        }

        /// <summary>
        /// Método responsável pelo evento: escolher rota, carregar os marcadores que a constituem no mapa e obter direções.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarChooseRoute_EditValueChanged(object sender, EventArgs e)
        {
            if (!(sender is BarEditItem lookupRoute))
            {
                return;
            }
            if (!(lookupRoute.EditValue is Route selectedRoute))
            {
                return;
            }
            List<Marker> routeMarkers = GetRouteMarkers(selectedRoute.Id);
            if (routeMarkers.Count == 0)
            {
                return;
            }

            mapControl.Overlays.Clear();

            mapMarkers.Clear();
            foreach (var marker in routeMarkers)
            {
                mapMarkers.Add(marker);
            }
            RefreshUnusedUserMarkers();

            listRouteMarkers.Refresh();
            RefreshMarkersOverlay();

            List<MapRoute> route = CalculateRoute(barAvoidHighways.Checked, barWalkingMode.Checked);
            if (route is null || route.Count == 0)
            {
                SetZoomOnLoadMap();
                return;
            }

            ShowRoute(route);
            ShowDirections(route);
        }

        /// <summary>
        /// Método responsável pelo evento: apagar rota da lista e da base de dados na barra main.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarDeleteRoute_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(barChooseRoute.EditValue is Route selectedRoute))
            {
                return;
            }
            string message = string.Format(Properties.Resources.RES_DeleteRoute, selectedRoute.Name);
            bool delete = contextService.PSO.MensagensDialogos.MostraPerguntaSimples(message);
            if (delete)
            {
                DeleteRouteMarkers(selectedRoute.Id);
                DeleteRoute(selectedRoute.Id);
                LoadUserRoutes(contextService.Aplicacao.Utilizador.Utilizador);
                barChooseRoute.EditValue = null;

                ClearMapRoutes();

                mapMarkers.Clear();
                RefreshUnusedUserMarkers();

                listRouteMarkers.Refresh();
                RefreshMarkersOverlay();

                SetZoomOnLoadMap();
            }
        }

        /// <summary>
        /// Método responsável pelo evento: remover marcador do mapa e recalcular a rota se aplicável.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRemoveMapMarker_Click(object sender, EventArgs e)
        {
            if (!(listRouteMarkers.SelectedValue is Marker selectedMarker))
            {
                return;
            }

            string message = string.Format(Properties.Resources.RES_DeleteMapMarker, selectedMarker.Description);
            bool delete = contextService.PSO.MensagensDialogos.MostraPerguntaSimples(message);
            if (delete)
            {
                mapMarkers.Remove(selectedMarker);
                RefreshUnusedUserMarkers();

                listRouteMarkers.Refresh();
                RefreshMarkersOverlay();

                if (mapControl.Overlays.Any(a => a.Id == "routes"))
                {
                    ClearMapRoutes();

                    List<MapRoute> route = CalculateRoute(barAvoidHighways.Checked, barWalkingMode.Checked);
                    if (route?.Count > 0)
                    {
                        ShowRoute(route);
                        ShowDirections(route);
                    }
                }

                SetZoomOnLoadMap();
            }
        }

        /// <summary>
        /// Método responsável pelo evento: selecionar marcador para reordenar na lista.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListRouteMarkers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Point point = new Point(e.X, e.Y);
            int index = this.listRouteMarkers.IndexFromPoint(point);
            object markerToMove = this.listRouteMarkers.SelectedItem;

            if (index >= 0)
            {
                markerToMove = mapMarkers[index];
            }
            if (markerToMove is null)
            {
                return;
            }

            this.listRouteMarkers.SelectedIndex = index;
            this.listRouteMarkers.DoDragDrop(markerToMove, DragDropEffects.Move);
        }

        /// <summary>
        /// Método responsável pelo evento: mover marcador para reordenar a lista.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListRouteMarkers_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// Método responsável pelo evento: colocar o marcador na nova posição da lista.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListRouteMarkers_DragDrop(object sender, DragEventArgs e)
        {
            Point point = this.listRouteMarkers.PointToClient(new Point(e.X, e.Y));
            int index = this.listRouteMarkers.IndexFromPoint(point);
            if (index < 0 && mapMarkers.Count > 0)
            {
                index = mapMarkers.Count - 1;
            }

            if (!(e.Data.GetData(typeof(Marker)) is Marker data))
            {
                return;
            }

            mapMarkers.Remove(data);
            mapMarkers.Insert(index, data);
        }

        /// <summary>
        /// Método responsável pelo evento: adicionar o marcador do utilizador selecionado à rota.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddMapMarker_Click(object sender, EventArgs e)
        {
            AddUserMarkerToRoute();
        }
        #endregion

        #region Métodos relativos à Entidade escolhida
        /// <summary>
        /// Adiciona no mapa um marcador com a morada da Entidade.
        /// </summary>
        private void ShowEntityLocationOnMap()
        {
            if (map is null)
            {
                return;
            }

            StringBuilder entityLocation = new StringBuilder();

            entityLocation.Append(Entity.Morada + ", ");
            entityLocation.Append(Entity.Localidade + ", ");
            entityLocation.Append(Entity.LocalidadeCodigoPostal);

            if (map.GetLocation(entityLocation.ToString(), out PointLatLng? pointLatLng) == GeoCoderStatusCode.OK && pointLatLng.HasValue)
            {
                Guid entityMarkerGuid = new Guid();
                string entityMarkerName;

                if (Entity is BasBE100.BasBECliente)
                {
                    entityMarkerName = $"{Entity.Cliente}, {Entity.Nome}";
                }
                else if (Entity is BasBE100.BasBEFornecedor)
                {
                    entityMarkerName = $"{Entity.Fornecedor}, {Entity.Nome}";
                }
                else
                {
                    entityMarkerName = $"{Entity.OutroTerceiro}, {Entity.Nome}";
                }

                Marker entityMarker = new Marker(entityMarkerGuid, entityMarkerName, false, pointLatLng.Value.Lat, pointLatLng.Value.Lng);
                mapMarkers.Add(entityMarker);

                RefreshUnusedUserMarkers();
            }
        }
        #endregion

        #region Métodos do Mapa
        /// <summary>
        /// Método responsável por obter as preferências de mapa do utilizador.
        /// </summary>
        private bool ReadMapPreferences(string selectedMapName)
        {
            if (listMapServers is null)
            {
                return false;
            }

            string mapPreference = contextService.PSO.IniFiles.IniLeString("Geolocalizacao", "Mapa", string.Empty, StdBETipos.TipoIni.inGlobalUtiliz);

            if (string.IsNullOrWhiteSpace(mapPreference))
            {
                string title = Properties.Resources.RES_GeolocationWarningTitle;
                string alert = Properties.Resources.RES_GeolocationWarning;
                contextService.PSO.PainelNotificacoes.MostraInformacao(title, alert);
                return false;
            }

            if (mapPreference.Equals(selectedMapName))
            {
                return false;
            }

            barMapServer.EditValue = listMapServers.First(m => m.Name == mapPreference);
            return true;
        }

        /// <summary>
        /// Verifica a existência de chave caso seja necessária para o servidor de mapa escolhido.
        /// Caso não exista pede ao utilizador.
        /// </summary>
        /// <param name="selectedMap"></param>
        private void ConfigSeletedMapServerSettings(Map selectedMap)
        {
            if (!contextService.Aplicacao.Utilizador.Administrador)
            {
                return;
            }

            string name = selectedMap.Name;
            string key = selectedMap.Key;
            if (map.NeedsKey && string.IsNullOrWhiteSpace(key))
            {
                string title = Properties.Resources.RES_ServerKeyTitle;
                string message = string.Format(Properties.Resources.RES_ServerKeyName, name);
                contextService.PSO.MensagensDialogos.MostraDialogoInput(ref key, title, message, 64, true, string.Empty);

                if (string.IsNullOrWhiteSpace(key))
                {
                    return;
                }

                selectedMap.Key = key;

                StdBECamposChave campos = new StdBECamposChave();
                campos.AddCampoChave("Nome", name);

                contextService.PSO.Registos.ActualizaValorAtributo(MAPSERVER_TABLE, campos, "Chave", key);
            }
        }

        /// <summary>
        /// Efetua consulta à base de dados e obtém a lista de servidores.
        /// </summary>
        /// <returns>Lista de servidores de mapa</returns>
        private List<Map> GetMapServers()
        {
            if (contextService != null)
            {
                DataTable tableMapServers = contextService.PSO.Registos.ConsultaDataTable($"SELECT Nome, Chave FROM {MAPSERVER_TABLE}");

                return tableMapServers.Rows
                    .Cast<DataRow>()
                    .Select(m => new Map()
                    {
                        Key = m["Chave"].ToString(),
                        Name = m["Nome"].ToString()
                    })
                    .ToList();
            }

            return null;
        }

        /// <summary>
        /// Preenche na comboBox a lista de servidores de mapa existentes.
        /// </summary>
        private void LoadMapServerSettings()
        {
            listMapServers = GetMapServers();

            if (!(barMapServer.Edit is RepositoryItemLookUpEdit mapServerRepository))
            {
                return;
            }
            mapServerRepository.DataSource = listMapServers;
            mapServerRepository.PopulateColumns();
            mapServerRepository.Columns["Key"].Visible = false;
        }

        /// <summary>
        /// Define o mapa escolhido pelo utilizador.
        /// </summary>
        /// <param name="mapSource"></param>
        /// <param name="mapKey"></param>
        private void SetMapSource(string mapSource, string mapKey)
        {
            if (string.IsNullOrWhiteSpace(mapSource))
            {
                return;
            }

            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            MapFactory mapFactory = new MapFactory();
            map = mapFactory.GetMap(mapSource);
            if (map is null)
            {
                return;
            }
            map.CreateMap(mapKey);

            mapControl.MapProvider = map.MapProvider;
        }

        /// <summary>
        /// Configura o controlo do mapa.
        /// </summary>
        private void SetupMapControl()
        {
            mapControl.ShowCenter = false;
            mapControl.DragButton = MouseButtons.Left;
            mapControl.MinZoom = 5;
            mapControl.MaxZoom = 18;
        }

        /// <summary>
        /// Método que define as funcionalidades do mapa selecionado.
        /// </summary>
        /// <param name="selectedMap"></param>
        private void SetMapFunctionalities(Map selectedMap)
        {
            bool flag = map.ProvideRoutes && ((map.NeedsKey && !string.IsNullOrEmpty(selectedMap.Key)) || !map.NeedsKey);
            barSaveRoute.Enabled = flag;
            barDrawRoute.Enabled = flag;
            barClearRoute.Enabled = flag;
            barChooseRoute.Enabled = flag;
            barDeleteRoute.Enabled = flag;
            barRouteOptions.Enabled = flag && map.HasRouteOptions;

            if (directionsPanel.Created)
            {
                directionsPanel.Visibility = map.ProvideDirections ? DockVisibility.AutoHide : DockVisibility.Hidden;
            }
        }

        /// <summary>
        /// Define o zoom do mapa, quando existem rotas ou marcadores.
        /// </summary>
        /// <param name="preferMarkers"></param>
        private void SetZoomOnLoadMap(bool preferMarkers = false)
        {
            if (mapControl.Overlays.Any(a => a.Id == "routes") && !preferMarkers)
            {
                mapControl.ZoomAndCenterRoutes("routes");
            }
            else if (mapControl.Overlays.Any(a => a.Id == "markers"))
            {
                mapControl.ZoomAndCenterMarkers("markers");

                var markersOverlay = mapControl.Overlays?.FirstOrDefault(m => m.Id == "markers");
                if (markersOverlay.Markers.Count == 1)
                {
                    mapControl.Zoom += 6;
                }
            }
            else
            {
                mapControl.Position = new PointLatLng(40, -5);
                mapControl.Zoom = 6;
            }
        }

        /// <summary>
        /// Método responsável por gravar as preferências de mapa do utilizador.
        /// </summary>
        internal void SaveMapPreferences()
        {
            if (!(barMapServer.EditValue is Map selectedMap))
            {
                return;
            }

            contextService.PSO.IniFiles.IniGravaString("Geolocalizacao", "Mapa", selectedMap.Name, StdBETipos.TipoIni.inGlobalUtiliz);
        }
        #endregion

        #region Métodos dos Marcadores
        /// <summary>
        /// Atualiza a camada de marcadores do mapa.
        /// </summary>
        private void RefreshMarkersOverlay()
        {
            if (map is null)
            {
                return;
            }

            var existingMarkers = mapControl.Overlays?.FirstOrDefault(m => m.Id == "markers");
            if (!(existingMarkers is null))
            {
                mapControl.Overlays.Remove(existingMarkers);
            }

            var markers = map.PlaceMarkers(mapMarkers);

            if (markers.Markers.Count > 0)
            {
                mapControl.Overlays.Add(markers);
            }
        }

        /// <summary>
        /// Mostra os marcadores guardados do utilizador.
        /// </summary>
        /// <param name="utilizador">Login do utilizador</param>
        private void LoadUserMarkers(string utilizador)
        {
            userMarkers = GetUserMarkers(utilizador);

            if (mapMarkers.Count == 0)
            {
                listUserMarkers.DataSource = userMarkers;
            }
            else
            {
                RefreshUnusedUserMarkers();
            }
        }

        /// <summary>
        /// Mostra os marcadores do mapa.
        /// </summary>
        /// <param name="utilizador">Login do utilizador</param>
        private void LoadMapMarkers()
        {
            listRouteMarkers.DataSource = mapMarkers;
        }

        /// <summary>
        /// Obtém da base de dados os marcadores do utilizador
        /// </summary>
        /// <param name="utilizador">Login do utilizador</param>
        /// <returns>Dicionario com os marcadores do utilizador</returns>
        private List<Marker> GetUserMarkers(string utilizador)
        {
            DataTable dtMarkers = contextService.PSO.Registos.ConsultaDataTable(
                    $"SELECT Latitude, Longitude, PontoInteresse, Descricao, Id FROM {MARKER_TABLE} WHERE Utilizador = @Utilizador"
                    , new List<SqlParameter>()
                        {
                            new SqlParameter("Utilizador", utilizador)
                        }
                    );

            List<Marker> userMarkers = new List<Marker>();
            foreach (DataRow item in dtMarkers.Rows)
            {
                Marker marker = new Marker(
                    (Guid)item["Id"],
                    item["Descricao"].ToString(),
                    Convert.ToBoolean(item["PontoInteresse"]),
                    Convert.ToDouble(item["Latitude"]),
                    Convert.ToDouble(item["Longitude"])
                    );
                userMarkers.Add(marker);
            }

            return userMarkers;
        }

        /// <summary>
        /// Atualiza a lista de marcadores do utilizador.
        /// </summary>
        private void RefreshUnusedUserMarkers()
        {
            if (userMarkers is null)
            {
                return;
            }
            listUserMarkers.DataSource = userMarkers.Except(mapMarkers).ToList();
            listUserMarkers.Refresh();
        }

        /// <summary>
        /// Adiciona um marcador à lista de marcadores do mapa a partir do campo de pesquisa.
        /// </summary>
        /// <returns>Ponto Coordenada</returns>
        private PointLatLng? AddMapMarker(string searchText, out Guid mapMarkerGuid)
        {
            PointLatLng? pointLatLng;
            mapMarkerGuid = Guid.NewGuid();

            string[] stringParts = searchText.Split(';');

            if (stringParts.Length == 2
                && double.TryParse(stringParts[0].Trim(), out double lat)
                && double.TryParse(stringParts[1].Trim(), out double lng))
            {
                pointLatLng = new PointLatLng(lat, lng);
                if (mapMarkers.Any(p => p.Latitude == lat && p.Longitude == lng))
                {
                    return pointLatLng;
                }
            }
            else if (map.GetLocation(searchText, out pointLatLng) == GeoCoderStatusCode.OK && pointLatLng.HasValue)
            {
                if (mapMarkers.Any(p => p.Latitude == pointLatLng.Value.Lat && p.Longitude == pointLatLng.Value.Lng))
                {
                    return pointLatLng;
                }
            }

            if (pointLatLng.HasValue)
            {
                string mapMarkerName = searchText;
                Marker mapMarker = new Marker(mapMarkerGuid, mapMarkerName, false, pointLatLng.Value.Lat, pointLatLng.Value.Lng);
                mapMarkers.Add(mapMarker);
                RefreshUnusedUserMarkers();
            }

            return pointLatLng;
        }

        /// <summary>
        /// Guarda o marcador na base de dados.
        /// </summary>
        /// <param name="markerId"></param>
        /// <param name="pointLatLng"></param>
        /// <param name="markerName"></param>
        /// <param name="poi"></param>
        private void SaveMarker(Guid markerId, PointLatLng pointLatLng, string markerName, bool poi)
        {
            DataTable markerTable = new DataTable();

            markerTable.Columns.Add("Id", typeof(Guid));
            markerTable.Columns.Add("Descricao", typeof(string));
            markerTable.Columns.Add("Latitude", typeof(double));
            markerTable.Columns.Add("Longitude", typeof(double));
            markerTable.Columns.Add("PontoInteresse", typeof(bool));
            markerTable.Columns.Add("Utilizador", typeof(string));

            DataRow row = markerTable.NewRow();
            row["Id"] = markerId;
            row["Descricao"] = markerName;
            row["Latitude"] = pointLatLng.Lat;
            row["Longitude"] = pointLatLng.Lng;
            row["PontoInteresse"] = poi;
            row["Utilizador"] = contextService.Aplicacao.Utilizador.Utilizador;
            markerTable.Rows.Add(row);

            contextService.PSO.Registos.InsereRegistosBulk(MARKER_TABLE, markerTable);
        }

        /// <summary>
        /// Apaga da base de dados o marcador escolhido do utilizador.
        /// </summary>
        private void DeleteUserMarker(Guid markerId)
        {
            if (markerId == Guid.Empty)
            {
                return;
            }

            StdBECamposChave camposChave = new StdBECamposChave();
            camposChave.AddCampoChave("IdMarcador", markerId);

            contextService.PSO.Registos.Remove(ROUTEMARKER_TABLE, camposChave);

            camposChave.CamposChave.RemoveTodos();
            camposChave.AddCampoChave("Id", markerId);

            contextService.PSO.Registos.Remove(MARKER_TABLE, camposChave);
        }

        /// <summary>
        /// Remove o marcador do mapa e recalcula a rota, se aplicável.
        /// </summary>
        /// <param name="item"></param>
        private void RemoveMarkerFromMap(GMapMarker item)
        {
            if (!(item.Tag is Marker mapMarker))
            {
                return;
            }

            string message = string.Format(Properties.Resources.RES_DeleteMapMarker, mapMarker.Description);
            bool remove = contextService.PSO.MensagensDialogos.MostraPerguntaSimples(message);
            if (remove)
            {
                mapMarkers.RemoveAll(p => p.Id == mapMarker.Id);
                RefreshUnusedUserMarkers();

                listRouteMarkers.Refresh();
                RefreshMarkersOverlay();

                if (mapControl.Overlays.Any(a => a.Id == "routes"))
                {
                    ClearMapRoutes();

                    List<MapRoute> route = CalculateRoute(barAvoidHighways.Checked, barWalkingMode.Checked);
                    if (route?.Count > 0)
                    {
                        ShowRoute(route);
                        ShowDirections(route);
                    }
                }

                SetZoomOnLoadMap();
            }
        }

        /// <summary>
        /// Adiciona o marcador do utilizador à rota.
        /// </summary>
        private void AddUserMarkerToRoute()
        {
            if (!(listUserMarkers.SelectedValue is Marker selectedMarker))
            {
                return;
            }

            Guid selectedMarkerGuid = selectedMarker.Id;
            if (mapMarkers.Any(p => p.Id == selectedMarkerGuid))
            {
                return;
            }

            mapMarkers.Add(selectedMarker);
            RefreshUnusedUserMarkers();

            listRouteMarkers.Refresh();
            RefreshMarkersOverlay();
            SetZoomOnLoadMap(true);
        }

        /// <summary>
        /// Solicita ao utilizador informação sobre o marcador aquando da sua gravação.
        /// </summary>
        /// <param name="nameHint"></param>
        /// <param name="pointOfInterest"></param>
        /// <returns>Nome do marcador</returns>
        private string GetInfoForSaveMarker(string nameHint, out bool pointOfInterest)
        {
            string title = Properties.Resources.RES_SaveMarkerTitle;
            string message = Properties.Resources.RES_SaveMarker;
            string name = null;
            bool continueSave = contextService.PSO.MensagensDialogos.MostraDialogoInput(ref name, title, message, 200, true, nameHint);

            pointOfInterest = false;
            if (!continueSave || string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            string message2 = Properties.Resources.RES_MarkerPOI;
            pointOfInterest = contextService.PSO.MensagensDialogos.MostraPerguntaSimples(message2);

            return name;
        }

        /// <summary>
        /// Verifica se o marcador pertence a alguma rota
        /// </summary>
        /// <param name="idMarker"></param>
        /// <returns></returns>
        private bool CheckRouteMarker(Guid idMarker)
        {
            string whereClause = $"IdMarcador = '{idMarker}'";

            return contextService.PSO.Registos.ExisteWhere(ROUTEMARKER_TABLE, whereClause);
        }
        #endregion

        #region Métodos das Rotas
        /// <summary>
        /// Método que calcula a rota.
        /// </summary>
        /// <returns>Lista de rotas</returns>
        private List<MapRoute> CalculateRoute(bool avoidHighways, bool walkingMode)
        {
            if (map is null)
            {
                return null;
            }

            if (mapMarkers.Count < 2)
            {
                string message = Properties.Resources.RES_TwoMarkerWarning;
                contextService.PSO.MensagensDialogos.MostraAviso(message);
                return null;
            }

            List<PointLatLng> points = new List<PointLatLng>();
            foreach (var item in mapMarkers)
            {
                points.Add(new PointLatLng(item.Latitude, item.Longitude));
            }

            List<MapRoute> mapRoutes = new List<MapRoute>();

            var route = map.GetRoute(points, avoidHighways, walkingMode, 14);
            if (route is null)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    route = map.GetRoute(points[i], points[i + 1], avoidHighways, walkingMode, 14);
                    if (route is null)
                    {
                        mapRoutes = null;
                        break;
                    }
                    mapRoutes.Add(route);
                }
            }
            else
            {
                mapRoutes.Add(route);
            }

            return mapRoutes;
        }

        /// <summary>
        /// Método que mostra a rota no mapa.
        /// </summary>
        /// <param name="route"></param>
        private void ShowRoute(List<MapRoute> route)
        {
            var routes = new GMapOverlay("routes");
            foreach (var step in route)
            {
                var mapRoute = new GMapRoute(step.Points, step.Name);
                routes.Routes.Add(mapRoute);
            }
            mapControl.Overlays.Add(routes);
            mapControl.ZoomAndCenterRoutes(routes.Id);
        }

        /// <summary>
        /// Remove todas as camadas de marcadores e rotas do mapa.
        /// </summary>
        private void ClearMapRoutes()
        {
            if (mapControl.Overlays.Count == 0)
            {
                return;
            }

            var routesOverlay = mapControl.Overlays.Where(item => item.Id == "routes").ToList();
            foreach (GMapOverlay overlay in routesOverlay)
            {
                mapControl.Overlays.Remove(overlay);
            }
        }

        /// <summary>
        /// Apaga uma rota da base de dados.
        /// </summary>
        /// <param name="routeId"></param>
        private void DeleteRoute(Guid routeId)
        {
            StdBECamposChave camposChave = new StdBECamposChave();
            camposChave.AddCampoChave("Id", routeId);

            contextService.PSO.Registos.Remove(ROUTE_TABLE, camposChave);
        }

        /// <summary>
        /// Validações para gravar a rota.
        /// </summary>
        /// <param name="routePoints"></param>
        /// <param name="routeName"></param>
        /// <returns>Validade da rota</returns>
        private bool ValidateRouteBeforeSave(out List<Guid> routePoints, out string routeName)
        {
            routePoints = null;
            routeName = null;

            string title = Properties.Resources.RES_SaveRouteTitle;
            string message;

            if (mapControl.Overlays.All(a => a.Id != "routes"))
            {
                message = Properties.Resources.RES_SaveRouteCalculateWarning;
                contextService.PSO.MensagensDialogos.MostraAviso(message);
                return false;
            }

            var routesOverlay = mapControl.Overlays?.FirstOrDefault(m => m.Id == "routes");
            if (routesOverlay.Routes.Count == 0)
            {
                message = Properties.Resources.RES_SaveRouteCalculateWarning;
                contextService.PSO.MensagensDialogos.MostraAviso(message);
                return false;
            }

            routePoints = new List<Guid>();
            if (mapControl.Overlays.All(a => a.Id != "markers"))
            {
                message = Properties.Resources.RES_SaveRouteMarkersWarning;
                contextService.PSO.MensagensDialogos.MostraAviso(message);
                return false;
            }

            var markersOverlay = mapControl.Overlays?.FirstOrDefault(m => m.Id == "markers");
            if (markersOverlay.Markers.Count == 0)
            {
                message = Properties.Resources.RES_SaveRouteMarkersWarning;
                contextService.PSO.MensagensDialogos.MostraAviso(message);
                return false;
            }

            bool saveMarkers = false;
            foreach (var mapMarker in markersOverlay.Markers)
            {
                Marker marker = mapMarker.Tag as Marker;
                if (userMarkers.Any(m => m.Id == marker.Id))
                {
                    routePoints.Add(marker.Id);
                }
                else
                {
                    if (!saveMarkers)
                    {
                        message = Properties.Resources.RES_RouteUnsavedMarkers;
                        saveMarkers = contextService.PSO.MensagensDialogos.MostraPerguntaSimples(message);
                    }

                    if (saveMarkers)
                    {
                        PointLatLng markerPoint = new PointLatLng(marker.Latitude, marker.Longitude);
                        SaveMarker(marker.Id, markerPoint, marker.Description, false);
                        routePoints.Add(marker.Id);
                    }
                    else
                    {
                        message = string.Format(Properties.Resources.RES_RouteUnsavedMarker, marker.Description);
                        contextService.PSO.MensagensDialogos.MostraAviso(message);
                        return false;
                    }
                }
            }

            message = Properties.Resources.RES_SaveRoute;
            routeName = null;
            contextService.PSO.MensagensDialogos.MostraDialogoInput(ref routeName, title, message, 100, true, string.Empty);


            if (string.IsNullOrWhiteSpace(routeName))
            {
                message = Properties.Resources.RES_SaveRouteNameWarning;
                contextService.PSO.MensagensDialogos.MostraAviso(message);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Guarda uma rota na base de dados.
        /// </summary>
        /// <param name="routeGuid"></param>
        /// <param name="routeName"></param>
        private void SaveRoute(Guid routeGuid, string routeName)
        {
            DataTable route = new DataTable();

            route.Columns.Add("Id", typeof(Guid));
            route.Columns.Add("Nome", typeof(string));
            route.Columns.Add("Utilizador", typeof(string));

            DataRow row = route.NewRow();
            row["Id"] = routeGuid;
            row["Nome"] = routeName;
            row["Utilizador"] = contextService.Aplicacao.Utilizador.Utilizador;
            route.Rows.Add(row);

            contextService.PSO.Registos.InsereRegistosBulk(ROUTE_TABLE, route);
        }
        /// <summary>
        /// Método que guarda os pontos de uma rota na base de dados.
        /// </summary>
        /// <param name="routeGuid"></param>
        /// <param name="routePoints"></param>
        private void SaveRoutePoints(Guid routeGuid, List<Guid> routePoints)
        {
            DataTable records = new DataTable();

            records.Columns.Add("IdMarcador", typeof(Guid));
            records.Columns.Add("IdRota", typeof(Guid));
            records.Columns.Add("Ordem", typeof(byte));

            for (int i = 0; i < routePoints.Count; i++)
            {
                Guid pointGuid = routePoints[i];
                DataRow row = records.NewRow();
                row["IdMarcador"] = pointGuid;
                row["IdRota"] = routeGuid;
                row["Ordem"] = i;

                records.Rows.Add(row);
            }

            contextService.PSO.Registos.InsereRegistosBulk(ROUTEMARKER_TABLE, records);
        }
        /// <summary>
        /// Obtém da base de dados as rotas do utilizador.
        /// </summary>
        /// <param name="utilizador"></param>
        /// <returns>DataTable de rotas</returns>
        private List<Route> GetUserRoutes(string utilizador)
        {
            DataTable dtRoutes = contextService.PSO.Registos.ConsultaDataTable(
                    $"SELECT Nome, Id FROM {ROUTE_TABLE} WHERE Utilizador = @Utilizador"
                    , new List<SqlParameter>()
                        {
                            new SqlParameter("Utilizador", utilizador)
                        }
                    );

            List<Route> userRoutes = new List<Route>();
            foreach (DataRow item in dtRoutes.Rows)
            {
                Route route = new Route(
                    (Guid)item["Id"],
                    item["Nome"].ToString()
                    );
                userRoutes.Add(route);
            }

            return userRoutes;
        }
        /// <summary>
        /// Obtém da base de dados os marcadores da rota.
        /// </summary>
        /// <param name="idRota">Identificador da rota</param>
        /// <returns>Lista com os marcadores da rota</returns>
        private List<Marker> GetRouteMarkers(Guid idRota)
        {
            DataTable dtMarkers = contextService.PSO.Registos.ConsultaDataTable(
                   $@"  SELECT      m.Id, m.Latitude, m.Longitude, m.Utilizador, m.PontoInteresse, m.Descricao 
                        FROM        {MARKER_TABLE} m 
                        INNER JOIN  {ROUTEMARKER_TABLE} a ON a.IdMarcador = m.Id
                        INNER JOIN  {ROUTE_TABLE} r on r.Id = a.IdRota 
                        WHERE       r.Id = @Rota 
                        ORDER BY    a.Ordem"
                   , new List<SqlParameter>()
                        {
                            new SqlParameter("Rota", idRota)
                        }
                    );

            List<Marker> routeMarkers = new List<Marker>();
            foreach (DataRow item in dtMarkers.Rows)
            {
                Marker marker = new Marker(
                    (Guid)item["Id"],
                    item["Descricao"].ToString(),
                    Convert.ToBoolean(item["PontoInteresse"]),
                    Convert.ToDouble(item["Latitude"]),
                    Convert.ToDouble(item["Longitude"])
                    );
                routeMarkers.Add(marker);
            }

            return routeMarkers;
        }
        /// <summary>
        /// Método que mostra na combobox as rotas do <paramref name="utilizador"/>.
        /// </summary>
        /// <param name="utilizador"></param>
        private void LoadUserRoutes(string utilizador)
        {
            userRoutes = GetUserRoutes(utilizador);

            if (!(barChooseRoute.Edit is RepositoryItemLookUpEdit chooseRouteRepository))
            {
                return;
            }
            chooseRouteRepository.DataSource = userRoutes;
            chooseRouteRepository.PopulateColumns();
            chooseRouteRepository.Columns["Id"].Visible = false;
        }
        /// <summary>
        /// Mostra direções de rota
        /// </summary>
        /// <param name="route"></param>
        private void ShowDirections(List<MapRoute> route)
        {
            if (map is null)
            {
                return;
            }

            StringBuilder directionsText = new StringBuilder();

            foreach (var step in route)
            {
                if (step.Instructions.Count > 0)
                {
                    if (directionsText.Length > 0)
                    {
                        directionsText.AppendLine("<br />");
                    }

                    directionsText.AppendLine($"<div style='font-family:tahoma;font-size:8.25pt'>Por <b>{step.Name}</b>&nbsp;<span style='color:green'>{step.Duration}</span>&nbsp;<span style='color:grey'>({string.Format("{0:###,##0.0}", step.Distance)} km)</span></div><br />");

                    for (int i = 1; i <= step.Instructions.Count; i++)
                    {
                        var item = step.Instructions[i - 1];
                        directionsText.AppendLine($"<div style='font-family:tahoma;font-size:8.25pt'>&#8226;&nbsp;{item}</div>");
                    }
                }
            }

            if (directionsText.Length > 0)
            {
                webBrowser1.DocumentText = directionsText.ToString();
                return;
            }

            List<PointLatLng> points = new List<PointLatLng>();
            foreach (var item in mapMarkers)
            {
                points.Add(new PointLatLng(item.Latitude, item.Longitude));
            }
            for (int i = 0; i < points.Count - 1; i++)
            {
                var statusCode = map.GetDirections(out GDirections directions, points[i], points[i + 1], false, false, false, false, true);
                if (statusCode == DirectionsStatusCode.OK && directions != null)
                {
                    foreach (var item in directions.Steps)
                    {
                        directionsText.AppendLine($"<div style='font-family:tahoma;font-size:8.25pt'>&#8226;&nbsp;{item.HtmlInstructions}</div>");
                    }
                }
            }
            webBrowser1.DocumentText = directionsText.ToString();
        }
        /// <summary>
        /// Apaga da base de dados os marcadores da rota
        /// </summary>
        /// <param name="routeId"></param>
        private void DeleteRouteMarkers(Guid routeId)
        {
            DataTable tableRouteMarkers = contextService.PSO.Registos.ConsultaDataTable($"SELECT Id FROM {ROUTEMARKER_TABLE} WHERE IdRota = '{routeId}'");

            foreach (DataRow row in tableRouteMarkers.Rows)
            {
                StdBECamposChave camposChave = new StdBECamposChave();
                camposChave.AddCampoChave("Id", row["Id"]);

                contextService.PSO.Registos.Remove(ROUTEMARKER_TABLE, camposChave);
            }
        }
        #endregion
    }
}
