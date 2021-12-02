using System;

namespace Primavera.Platform.Geolocation
{
    public class Marker
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool PointOfInterest { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Marker(Guid id, string description, bool pointOfInterest, double latitude, double longitude)
        {
            Id = id;
            Description = description;
            PointOfInterest = pointOfInterest;
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}