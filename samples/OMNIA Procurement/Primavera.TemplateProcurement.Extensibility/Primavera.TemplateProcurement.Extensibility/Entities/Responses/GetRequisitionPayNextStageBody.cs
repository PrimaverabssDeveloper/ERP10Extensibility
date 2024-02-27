using System.Collections.Generic;
using Newtonsoft.Json;

namespace Primavera.TemplateProcurement.Extensibility.Entities.Responses
{
    class GetRequisitionPayNextStageBody
    {
        [JsonProperty(PropertyName = "ultimaetapa")]
        public bool? ultimaEtapa { get; set; }

        [JsonProperty(PropertyName = "order")]
        public int? order { get; set; }

        [JsonProperty(PropertyName = "approvalprofile")]
        public string approvalProfile { get; set; }

        [JsonProperty(PropertyName = "approvalprofilename")]
        public string approvalProfileName { get; set; }

        [JsonProperty(PropertyName = "approverusername")]
        public string approverUsername { get; set; }

        [JsonProperty(PropertyName = "approvercontactemail")]
        public string approverContactEmail { get; set; }

        [JsonProperty(PropertyName = "liquidationdoctype")]
        public string liquidationDocType { get; set; }

        [JsonProperty(PropertyName = "flow")]
        public List<Flow> flow { get; set; }
    }
}
