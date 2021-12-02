using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Collections.Generic;

namespace Primavera.Platform.Geolocation
{
    /// <summary>
    /// Classe abstrata que agrupa os métodos e propriedades a ser utiizados 
    /// pelos diferentes servidores de mapa. Aqui é definido 
    /// como se cria o mapa, como se adicionam os marcadores e se calculam rotas.
    /// </summary>
    public abstract class MapManager
    {
        /// <summary>
        /// Propriedade que obtém e define o servidor de mapa.
        /// </summary>
        public GMapProvider MapProvider { get; set; }
        /// <summary>
        /// Propriedade que identifica se o servidor de mapa precisa de chave.
        /// </summary>
        public bool NeedsKey { get; set; }
        /// <summary>
        /// Propriedade que identifica se o servidor de mapa disponibiliza o cálculo de rotas.
        /// </summary>
        public bool ProvideRoutes { get; set; }
        /// <summary>
        /// Propriedade que identifica se o servidor de mapa disponibiliza opções de rota.
        /// </summary>
        public bool HasRouteOptions { get; set; }
        /// <summary>
        /// Propriedade que identifica se o servidor de mapa disponibiliza direções de rota.
        /// </summary>
        public bool ProvideDirections { get; set; }

        /// <summary>
        /// Método para carregar mapa. 
        /// </summary>
        /// <param name="key"></param>
        public virtual void CreateMap(string key = null)
        {
            //MapProvider.BypassCache = true;
            MapProvider.OnInitialized();
        }

        /// <summary>
        /// Método que cria uma camada de marcadores, identifiando marcador inicial, intermédio e final assim como informações sobre os mesmos.
        /// </summary>
        /// <param name="markers"></param>
        /// <returns>Uma camada de marcadores</returns>
        public virtual GMapOverlay PlaceMarkers(List<Marker> markers)
        {      
            GMapOverlay markersOverlay = new GMapOverlay("markers");
            for (int i = 0; i < markers.Count; i++)
            {
                Marker marker = markers[i];
                PointLatLng point = new PointLatLng(marker.Latitude, marker.Longitude);

                GMarkerGoogleType markerType = GMarkerGoogleType.blue_pushpin;
                if (i == 0)
                    markerType = GMarkerGoogleType.green_pushpin;
                if (i == markers.Count - 1)
                    markerType = GMarkerGoogleType.red_pushpin;

                GMapMarker mapMarker = new GMarkerGoogle(point, markerType)
                {
                    Tag = marker,
                    ToolTipMode = MarkerTooltipMode.OnMouseOver,
                    ToolTipText = marker.Description
                };

                markersOverlay.Markers.Add(mapMarker);
            }
            return markersOverlay;
        }

        /// <summary>
        /// Método para obter rota com moradas inicial e final. 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="avoidHighways"></param>
        /// <param name="walkingMode"></param>
        /// <param name="zoom"></param>
        /// <returns>Camada de marcadores</returns>
        public virtual MapRoute GetRoute(string start, string end, bool avoidHighways, bool walkingMode, int zoom)
        {
            return null;
        }

        /// <summary>
        /// Método para obter rota com pontos de localização.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="avoidHighways"></param>
        /// <param name="walkingMode"></param>
        /// <param name="zoom"></param>
        /// <returns>Devolve uma rota <see cref="MapRoute"/> entre os pontos fornecidos</returns>
        public virtual MapRoute GetRoute(PointLatLng start, PointLatLng end, bool avoidHighways, bool walkingMode, int zoom)
        {
            return null;
        }

        /// <summary>
        /// Método para obter rota com lista de pontos.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="avoidHighways"></param>
        /// <param name="walkingMode"></param>
        /// <param name="zoom"></param>
        /// <returns>Devolve uma rota <see cref="MapRoute"/> entre os pontos fornecidos na lista</returns>
        public virtual MapRoute GetRoute(List<PointLatLng> list, bool avoidHighways, bool walkingMode, int zoom)
        {
            return null;
        }


        /// <summary>
        /// Método que faz pedido ao servidor de mapa para obter as coordenadas da pesquisa feita.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="pointLatLng"></param>
        /// <returns>Status do pedido para obter localização.</returns>
        public virtual GeoCoderStatusCode GetLocation(string address, out PointLatLng? pointLatLng)
        {
            pointLatLng = null;
            return GeoCoderStatusCode.UNKNOWN_ERROR;
        }

        /// <summary>
        /// Método que faz pedido ao servidor de mapa para obter as direções da rota com coordenadas.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="avoidHighways"></param>
        /// <param name="avoidTolls"></param>
        /// <param name="walkingMode"></param>
        /// <param name="sensor"></param>
        /// <param name="metric"></param>
        /// <returns>Status do pedido</returns>
        public virtual DirectionsStatusCode GetDirections(out GDirections direction, PointLatLng start, PointLatLng end, bool avoidHighways, bool avoidTolls, bool walkingMode, bool sensor, bool metric)
        {
            direction = null;
            return DirectionsStatusCode.UNKNOWN_ERROR;
        }
        /// <summary>
        /// Método que faz pedido ao servidor de mapa para obter as direções da rota com moradas.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="avoidHighways"></param>
        /// <param name="avoidTolls"></param>
        /// <param name="walkingMode"></param>
        /// <param name="sensor"></param>
        /// <param name="metric"></param>
        /// <returns>Status do pedido</returns>
        public virtual DirectionsStatusCode GetDirections(out GDirections direction, string start, string end, bool avoidHighways, bool avoidTolls, bool walkingMode, bool sensor, bool metric)
        {
            direction = null;
            return DirectionsStatusCode.UNKNOWN_ERROR;
        }

    }
}
