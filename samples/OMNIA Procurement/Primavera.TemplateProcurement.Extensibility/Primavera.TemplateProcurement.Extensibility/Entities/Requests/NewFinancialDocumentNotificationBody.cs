using Newtonsoft.Json;

namespace Primavera.TemplateProcurement.Extensibility.Entities.Requests
{
    class NewFinancialDocumentNotificationBody
    {
        [JsonProperty(PropertyName = "code")]
        public string code { get; set; }

        [JsonProperty(PropertyName = "entidade")]
        public string entidade { get; set; }

        [JsonProperty(PropertyName = "organizationalunit")]
        public string organizationalUnit { get; set; }

        [JsonProperty(PropertyName = "data")]
        public string data { get; set; }

        [JsonProperty(PropertyName = "author")]
        public string author { get; set; }

        [JsonProperty(PropertyName = "approver")]
        public string approver { get; set; }
    }
}
