using System;

namespace Primavera.Platform.Geolocation
{
    internal class RouteMarker
    {
        public Guid Id { get; set; }
        public Guid IdMarker { get; set; }
        public Guid IdRoute { get; set; }
        public RouteMarker(Guid id, Guid idMarker, Guid idRoute)
        {
            Id = id;
            IdMarker = idMarker;
            IdRoute = idRoute;
        }
    }
}