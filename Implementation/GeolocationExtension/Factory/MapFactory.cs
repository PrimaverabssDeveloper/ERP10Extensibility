namespace Primavera.Platform.Geolocation
{
    /// <summary>
    /// Classe para instanciar os mapas.
    /// </summary>
    public class MapFactory
    {
        /// <summary>
        /// Método que devolve instâncias das classes concretas (Google, Bing, OSM).
        /// </summary>
        /// <param name="mapType"></param>
        /// <returns>Instância da classe concreta</returns>
        public MapManager GetMap(string mapType)
        {
            switch (mapType)
            {
                case MapHelper.MAP_GOOGLE:
                    return new GoogleMap();
                case MapHelper.MAP_BING:
                    return new BingMap();
                case MapHelper.MAP_OPENSTREETMAP:
                    return new OSMMap();
                default:
                    return null;
            }
        }

    }
}
