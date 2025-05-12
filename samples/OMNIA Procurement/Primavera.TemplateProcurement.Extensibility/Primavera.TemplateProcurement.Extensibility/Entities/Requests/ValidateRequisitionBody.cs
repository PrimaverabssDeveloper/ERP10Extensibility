using Newtonsoft.Json;

namespace Primavera.TemplateProcurement.Extensibility.Entities.Requests
{
    class ValidateRequisitionBody
    {
        [JsonProperty(PropertyName = "omniaCode")]
        public string omniaCode { get; set; }

        [JsonProperty(PropertyName = "operation")]
        public int operation { get; set; }
    }
}
