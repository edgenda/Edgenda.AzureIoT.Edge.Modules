using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Edgenda.AzureIoT.Common
{
    public class Geometry
    {
        [JsonProperty("type")]
        public string DocumentType { get; set; }
        [JsonProperty("coordinates")]
        public double[] Coordinates { get; set; }
    }
    public class CameraProperties
    {
        [JsonProperty("nid")]
        public int Id { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("titre")]
        public string Title { get; set; }
        [JsonProperty("id-camera")]
        public int CameraId { get; set; }
        [JsonProperty("id-arrondissement")]
        public int BuroughId { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("axe-routier-nord-sud")]
        public string NorthSouthStreet { get; set; }
        [JsonProperty("axe-routier-est-ouest")]
        public string EastWestStreet { get; set; }
        [JsonProperty("url-image-en-direct")]
        public Uri LiveImageUrl { get; set; }
        [JsonProperty("url-video-en-direct")]
        public Uri LiveVideoUrl { get; set; }
        [JsonProperty("image-type", Required = Required.Default)]
        public string ImageType { get; set; }
        [JsonProperty("image-data", Required = Required.Default)]
        public string ImageData { get; set; }

    }
    public class GeometryFeature
    {
        [JsonProperty("type")]
        public string DocumentType { get; set; }
        public Geometry Geometry { get; set; }
        [JsonProperty("properties")]
        public CameraProperties Properties { get; set; }
    }
}
