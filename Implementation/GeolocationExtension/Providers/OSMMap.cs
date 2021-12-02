using GMap.NET;
using GMap.NET.MapProviders;

namespace Primavera.Platform.Geolocation
{
    /// <summary>
    /// Esta Classe herda da Classe MapManager e redefine os métodos para criar o mapa
    /// e calcular rota usando o servidor OSM.
    /// </summary>
    public class OSMMap : MapManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public override void CreateMap(string key = null)
        {            
            MapProvider = GMapProviders.OpenCycleMap;          
            base.CreateMap();
        }    
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="pointLatLng"></param>
        /// <returns></returns>
        public override GeoCoderStatusCode GetLocation(string address, out PointLatLng? pointLatLng)
        {
            pointLatLng = OpenStreet4UMapProvider.Instance.GetPoint(address, out GeoCoderStatusCode geoCoderStatusCode);
            return geoCoderStatusCode;
        }
    }
}
