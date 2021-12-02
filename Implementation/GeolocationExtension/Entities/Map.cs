namespace Primavera.Platform.Geolocation
{
    /// <summary>
    /// Represents the <see cref="Map"/> class.
    /// </summary>
    internal class Map
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the map key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the map name.
        /// </summary>
        public string Name { get; set; } 
        #endregion

        #region Constructors
        /// <summary>
        /// Simple <see cref="Map"/> constructor.
        /// </summary>
        public Map()
        {
            Key = string.Empty;
            Name = string.Empty;
        }

        /// <summary>
        /// <see cref="Map"/> constructor with map key and name.
        /// </summary>
        /// <param name="key">The map key.</param>
        /// <param name="name">The map name.</param>
        public Map(string key, string name)
        {
            Key = key;
            Name = name;
        }

        #endregion

        #region Overrides
        /// <summary>
        /// Overrides the ToString() method to return the map name.
        /// </summary>
        /// <returns>Map name.</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}