using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Edgenda.AzureIoT.Common
{
    public class GeometryFeatures
    {
        [JsonProperty("type")]
        public string DocumentType { get; set; }
        [JsonProperty("features")]
        public GeometryFeature[] Features { get; set; }
    }
}
