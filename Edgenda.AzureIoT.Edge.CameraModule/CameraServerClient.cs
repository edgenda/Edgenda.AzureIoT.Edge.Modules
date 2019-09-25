using System;
using System.Collections.Generic;
using System.Text;
using Edgenda.AzureIoT.Common;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;

namespace Edgenda.AzureIoT.Edge.CameraModule
{
    class CameraServerClient : ICameraServerClient
    {
        private readonly string _hostname;
        private readonly int _basePort;

        public CameraServerClient(string hostname = "localhost", int basePort = 15000)
        {
            //this._hostname = Environment.GetEnvironmentVariable("CAMERA_EDGEMODULE_SERVER_HOSTNAME") ?? "host.docker.internal";
            this._hostname = hostname;
            this._basePort = basePort;
        }
        public CameraProperties[] GetCameraProperties(double longitude, double latitude)
        {
            using (var clientSocket = new RequestSocket($"tcp://{_hostname}:{_basePort}"))
            {
                GetByCoordinatesCommand cmd = new GetByCoordinatesCommand() { Name = Command.GET_BY_COORDINATES_COMMAND, Parameters = new double[] { longitude, latitude } };
                clientSocket.SendFrame(JsonConvert.SerializeObject(cmd));
                var response = clientSocket.ReceiveFrameString();
                var retVal = JsonConvert.DeserializeObject<CameraProperties[]>(response);
                return retVal;
            }
        }
    }
}
