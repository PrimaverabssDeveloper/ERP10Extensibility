namespace Primavera.Platform.Geolocation
{
    /// <summary>
    /// Represents the <see cref="MapServer"/> class.
    /// </summary>
    internal class MapServer
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public MapServer(string name, string key)
        {
            Name = name;
            Key = key;
        }
    }
}