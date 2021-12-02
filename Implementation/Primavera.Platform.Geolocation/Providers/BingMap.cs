using GMap.NET;
using GMap.NET.MapProviders;
using System.Collections.Generic;

namespace Primavera.Platform.Geolocation
{
    /// <summary>
    /// Esta Classe herda da Classe Map e redefine os métodos para criar o mapa
    /// e calcular rota usando o servidor Bing.
    /// </summary>
    public class BingMap : MapManager
    {
        /// <summary>
        /// Construtor da classe que define as funcionalidades disponíveis com o Bing.
        /// </summary>
        public BingMap()
        {
            NeedsKey = true;
            ProvideRoutes = true;
            HasRouteOptions = true;
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public override void CreateMap(string key = null)
        {
            GMapProviders.BingMap.ClientKey = key;
            MapProvider = GMapProviders.BingMap;           
            base.CreateMap();
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
        public override MapRoute GetRoute(PointLatLng start, PointLatLng end, bool avoidHighways, bool walkingMode, int zoom)
        {
            return BingMapProvider.Instance.GetRoute(start, end, avoidHighways, walkingMode, zoom);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="pointLatLng"></param>
        /// <returns></returns>
        public override GeoCoderStatusCode GetLocation(string address, out PointLatLng? pointLatLng)
        {
            pointLatLng = BingMapProvider.Instance.GetPoint(address, out GeoCoderStatusCode geoCoderStatusCode);
            return geoCoderStatusCode;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="list"></param>
        ///// <param name="avoidHighways"></param>
        ///// <param name="walkingMode"></param>
        ///// <param name="zoom"></param>
        ///// <returns></returns>
        //public override MapRoute GetRoute(List<PointLatLng> list, bool avoidHighways, bool walkingMode, int zoom)
        //{
        //    return BingMapProvider.Instance.GetRoute(list, avoidHighways, walkingMode, zoom);
        //}
    }
}
