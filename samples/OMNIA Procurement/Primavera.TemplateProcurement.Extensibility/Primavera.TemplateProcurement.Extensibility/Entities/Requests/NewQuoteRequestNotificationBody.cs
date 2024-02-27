using Newtonsoft.Json;

namespace Primavera.TemplateProcurement.Extensibility.Entities.Requests
{
    class NewQuoteRequestNotificationBody
    {
        [JsonProperty(PropertyName = "quoterequests")]
        public string quoterequests { get; set; }

        [JsonProperty(PropertyName = "supplier")]
        public string supplier { get; set; }
    }
}
