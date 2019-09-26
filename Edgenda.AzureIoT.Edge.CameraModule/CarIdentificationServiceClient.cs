using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Edgenda.AzureIoT.Edge.CameraModule
{
    public partial class Predictions
    {
        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("iteration")]
        public string Iteration { get; set; }
        [JsonProperty("predictions")]
        public Prediction[] PredictionsPredictions { get; set; }
        [JsonProperty("project")]
        public string Project { get; set; }
    }

    public partial class Prediction
    {
        [JsonProperty("boundingBox")]
        public BoundingBox BoundingBox { get; set; }
        [JsonProperty("probability")]
        public double Probability { get; set; }
        [JsonProperty("tagId")]
        public long TagId { get; set; }
        [JsonProperty("tagName")]
        public string TagName { get; set; }
    }

    public partial class BoundingBox
    {
        [JsonProperty("height")]
        public double Height { get; set; }
        [JsonProperty("left")]
        public double Left { get; set; }
        [JsonProperty("top")]
        public double Top { get; set; }
        [JsonProperty("width")]
        public double Width { get; set; }
    }

    public partial class Predictions
    {
        public static Predictions FromJson(string json) => JsonConvert.DeserializeObject<Predictions>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Predictions self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
    public class CarIdentificationServiceClient
    {
        private readonly string _serviceHostname;
        private readonly int _serviceBasePort;

        public CarIdentificationServiceClient(string hostname = "localhost", int basePort = 8080)
        {
            this._serviceHostname = hostname;
            this._serviceBasePort = basePort;
        }

        public Predictions GetPredictions(byte[] data, Uri liveImageUrl)
        {
            try
            {
                Console.WriteLine($"http://{this._serviceHostname}:{this._serviceBasePort}/image");
                var client = new RestClient($"http://{this._serviceHostname}:{this._serviceBasePort}/image");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Content-Length", data.Length.ToString());
                request.AddHeader("Accept-Encoding", "gzip, deflate");
                request.AddHeader("Host", $"{this._serviceHostname}:{this._serviceBasePort}");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("Content-Type", "application/octet-stream");
                request.AddParameter("application/octet-stream", data, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Content);
                Console.WriteLine(response.ErrorMessage);
                Console.WriteLine(response.ErrorException);
                var predictions = Predictions.FromJson(response.Content);
                return predictions;
            }
            catch (Exception ex) {
                Console.Error.WriteLine(ex);
                return null;
            }

        }
    }
}
