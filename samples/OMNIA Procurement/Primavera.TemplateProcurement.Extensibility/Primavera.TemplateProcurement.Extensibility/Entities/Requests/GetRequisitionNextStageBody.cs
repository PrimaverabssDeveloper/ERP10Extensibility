using Newtonsoft.Json;

namespace Primavera.TemplateProcurement.Extensibility.Entities.Requests
{
    class GetRequisitionNextStageBody
    {
        [JsonProperty(PropertyName = "requisition")]
        public string Requisition { get; set; }

        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "families")]
        public string Families { get; set; }
    }
}
