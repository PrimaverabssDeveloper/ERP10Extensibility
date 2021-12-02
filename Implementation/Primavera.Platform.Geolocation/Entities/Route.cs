using System;

namespace Primavera.Platform.Geolocation
{
    internal class Route
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Route(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}