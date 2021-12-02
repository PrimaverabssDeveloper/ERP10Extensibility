using GMap.NET;
using GMap.NET.MapProviders;

namespace Primavera.Platform.Geolocation
{
    /// <summary>
    /// Esta Classe herda da Classe Map e redefine os métodos para criar o mapa
    /// e calcular rota usando o servidor Google.
    /// </summary>
    public class GoogleMap : MapManager
    {
        /// <summary>
        /// Construtor de define as funcionalidades do servidor de mapa Google.
        /// </summary>
        public GoogleMap()
        {
            NeedsKey = true;
            ProvideRoutes = true;
            ProvideDirections = true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"></param>
        public override void CreateMap(string key)
        {
            //GMaps.Instance.UseUrlCache = false;
            GMapProviders.GoogleMap.ApiKey = key;
            MapProvider = GMapProviders.GoogleMap;
            base.CreateMap(key);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="avoidHighways"></param>
        /// <param name="walkingMode"></param>
        /// <param name="zoom"></param>
        /// <returns>Rota no mapa</returns>
        public override MapRoute GetRoute(PointLatLng start, PointLatLng end, bool avoidHighways, bool walkingMode, int zoom)
        {
            return GoogleMapProvider.Instance.GetRoute(start, end, avoidHighways, walkingMode, zoom);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="avoidHighways"></param>
        /// <param name="walkingMode"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public override MapRoute GetRoute(string start, string end, bool avoidHighways, bool walkingMode, int zoom)
        {
            return GoogleMapProvider.Instance.GetRoute(start, end, avoidHighways, walkingMode, zoom);
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="pointLatLng"></param>
        /// <returns></returns>
        public override GeoCoderStatusCode GetLocation(string address, out PointLatLng? pointLatLng)
        {
            pointLatLng = GoogleMapProvider.Instance.GetPoint(address, out GeoCoderStatusCode geoCoderStatusCode);
            return geoCoderStatusCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="avoidHighways"></param>
        /// <param name="avoidTolls"></param>
        /// <param name="walkingMode"></param>
        /// <param name="sensor"></param>
        /// <param name="metric"></param>
        /// <returns></returns>
        public override DirectionsStatusCode GetDirections(out GDirections direction, PointLatLng start, PointLatLng end, bool avoidHighways, bool avoidTolls, bool walkingMode, bool sensor, bool metric)
        {
            return GoogleMapProvider.Instance.GetDirections(out direction, start, end, avoidHighways, avoidTolls, walkingMode, sensor, metric);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="avoidHighways"></param>
        /// <param name="avoidTolls"></param>
        /// <param name="walkingMode"></param>
        /// <param name="sensor"></param>
        /// <param name="metric"></param>
        /// <returns></returns>
        public override DirectionsStatusCode GetDirections(out GDirections direction, string start, string end, bool avoidHighways, bool avoidTolls, bool walkingMode, bool sensor, bool metric)
        {
            return GoogleMapProvider.Instance.GetDirections(out direction, start, end, avoidHighways, avoidTolls, walkingMode, sensor, metric);
        }
    }
}
