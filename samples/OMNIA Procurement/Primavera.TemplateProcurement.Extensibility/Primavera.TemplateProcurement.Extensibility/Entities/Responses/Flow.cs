using Newtonsoft.Json;

namespace Primavera.TemplateProcurement.Extensibility.Entities.Responses
{
    class Flow
    {
        [JsonProperty(PropertyName = "approvalprofile")]
        public string approvalProfile { get; set; }

        [JsonProperty(PropertyName = "order")]
        public int order { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public decimal amount { get; set; }
    }
}
