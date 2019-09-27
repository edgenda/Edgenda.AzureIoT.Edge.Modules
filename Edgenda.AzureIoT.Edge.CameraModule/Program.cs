namespace Edgenda.AzureIoT.Edge.CameraModule
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Runtime.Loader;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;
    using Microsoft.Azure.Devices.Shared;
    using Newtonsoft.Json;

    class Program
    {
        private static volatile DesiredPropertiesData desiredPropertiesData;

        static void Main(string[] args)
        {
            Init().Wait();

            // Wait until the app unloads or is cancelled
            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Initializes the ModuleClient and sets up the callback to receive
        /// messages containing temperature information
        /// </summary>
        static async Task Init()
        {
            AmqpTransportSettings amqpSetting = new AmqpTransportSettings(TransportType.Amqp_Tcp_Only);
            ITransportSettings[] settings = { amqpSetting };

            // Open a connection to the Edge runtime
            ModuleClient ioTHubModuleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);
            await ioTHubModuleClient.OpenAsync();
            Console.WriteLine("IoT Hub module client initialized.");

            var moduleTwin = await ioTHubModuleClient.GetTwinAsync();
            var moduleTwinCollection = moduleTwin.Properties.Desired;
            Console.WriteLine("Got Device Twin configuration.");
            desiredPropertiesData = new DesiredPropertiesData(moduleTwinCollection);

            // callback for updating desired properties through the portal or rest api
            await ioTHubModuleClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertiesUpdate, null);

            await ioTHubModuleClient.SetMethodHandlerAsync("capture", CaptureMethod, ioTHubModuleClient);

        }
        private static Task OnDesiredPropertiesUpdate(TwinCollection twinCollection, object userContext)
        {
            desiredPropertiesData = new DesiredPropertiesData(twinCollection);
            return Task.CompletedTask;
        }

        private async static Task<MethodResponse> CaptureMethod(MethodRequest request, object userContext)
        {
            Console.WriteLine("Received capture method call");
            try
            {
                var client = new CameraServerClient(desiredPropertiesData.ServerHostname, desiredPropertiesData.ServerBasePort);
                var data = client.GetCameraProperties(desiredPropertiesData.DeviceLongitude, desiredPropertiesData.DeviceLatitude);
                Console.WriteLine($"Received camera data {JsonConvert.SerializeObject(data)}");
                if (data != null && data.Length > 0)
                {
                    Console.WriteLine("Calling image detection api");
                    var carIdService = new CarIdentificationServiceClient(desiredPropertiesData.CarDetectionServerHostname, desiredPropertiesData.CarDetectionServerBasePort);
                    var camData = new ExtendedCameraProperties(data.First());
                    camData.Predictions = carIdService.GetPredictions(Convert.FromBase64String(camData.ImageData), camData.LiveImageUrl);
                    var serializedCamData = JsonConvert.SerializeObject(camData);
                    var camDataAsBytes = Encoding.UTF8.GetBytes(serializedCamData);
                    var response = new MethodResponse(camDataAsBytes, (int)HttpStatusCode.OK);

                    var message = new Message(camDataAsBytes);
                    message.ContentEncoding = "utf-8";
                    message.ContentType = "application/json";
                    var deviceClient = userContext as ModuleClient;
                    await deviceClient.SendEventAsync("outputs", message);

                    return response;
                }
                return new MethodResponse((int)HttpStatusCode.NotFound);
            }
            catch(Exception error)
            {
                Console.Error.WriteLine(error);
                throw;
            }
        }
    }
}
