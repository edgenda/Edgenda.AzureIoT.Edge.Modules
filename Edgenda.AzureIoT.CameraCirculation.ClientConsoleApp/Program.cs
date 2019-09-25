using Edgenda.AzureIoT.Common;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Edgenda.AzureIoT.CameraCirculation.ClientConsoleApp
{
    class Program
    {
        async static Task<IRestResponse> doFetch(string hostname, string portNumber)
        {
            var client = new RestClient($"http://{hostname}:{portNumber}/api/version");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", $"{hostname}:{portNumber}");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            IRestResponse response = await client.ExecuteGetTaskAsync(request);
            Console.WriteLine(response.Content);
            return response;
        }
        async static Task doTask(RequestSocket clientSocket)
        {
            GetByCoordinatesCommand cmd = new GetByCoordinatesCommand() { Name = Command.GET_BY_COORDINATES_COMMAND, Parameters = new double[] { -73.61579, 45.46438 } };
            clientSocket.SendFrame(JsonConvert.SerializeObject(cmd));
            var response = clientSocket.ReceiveFrameString();
            var retVal = JsonConvert.DeserializeObject<CameraProperties[]>(response);
        }
        async static Task Main(string[] args)
        {
            using (var clientSocket = new RequestSocket($"tcp://{Environment.GetEnvironmentVariable("HOSTNAME") ?? "localhost"}:{Environment.GetEnvironmentVariable("BASEPORT") ?? "15000"}"))
            {
                GetByCoordinatesCommand cmd = new GetByCoordinatesCommand() { Name = Command.GET_BY_COORDINATES_COMMAND, Parameters = new double[] { -73.61579, 45.46438 } };
                clientSocket.SendFrame(JsonConvert.SerializeObject(cmd));
                var response = clientSocket.ReceiveFrameString();
                var retVal = JsonConvert.DeserializeObject<CameraProperties[]>(response);
            }
        }
    }
}
