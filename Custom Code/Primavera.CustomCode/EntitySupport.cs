using System.Collections.Generic;

namespace Primavera.CustomNifService.RootEntity
{
    internal class EntitySupport
    {
        public string result { get; set; }
        public Dictionary<int, Location> records { get; set; }
        public bool nif_validation { get; set; }
        public bool is_nif { get; set; }
    }

    internal class Location
    {
        public int nif { get; set; }
        public string seo_url { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public string pc4 { get; set; }
        public string pc3 { get; set; }
        public string city { get; set; }
        public string activity { get; set; }
        public string status { get; set; }
        public string cae { get; set; }
        public Contacts contacts { get; set; }
        public Structure structure { get; set; }
        public Geo geo { get; set; }
        public Place place { get; set; }
        public string racius { get; set; }
        public string alias { get; set; }
        public string portugalio { get; set; }
    }

    internal class Contacts
    {
        public object email { get; set; }
        public string phone { get; set; }
        public object website { get; set; }
        public string fax { get; set; }
    }

    internal class Structure
    {
        public string nature { get; set; }
        public string capital { get; set; }
        public string capital_currency { get; set; }
    }

    internal class Geo
    {
        public string region { get; set; }
        public string county { get; set; }
        public string parish { get; set; }
    }

    internal class Place
    {
        public string address { get; set; }
        public string pc4 { get; set; }
        public string pc3 { get; set; }
        public string city { get; set; }
    }
}
