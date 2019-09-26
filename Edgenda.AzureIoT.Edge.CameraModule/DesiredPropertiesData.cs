using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;

namespace Edgenda.AzureIoT.Edge.CameraModule
{
    public class DesiredPropertiesData
    {
        private bool _sendData = true;
        private double _deviceLongitude = -73.532344350311;
        private double _deviceLatitude = 45.600982799511;
        private string _serverHostname = string.Empty;
        private int _serverBasePort = 15000;
        private string _carDetectionServerHostname = string.Empty;
        private int _carDetectionServerBasePort = 8080;

        public DesiredPropertiesData(TwinCollection twinCollection)
        {
            Console.WriteLine($"Updating desired properties {twinCollection.ToJson(Formatting.Indented)}");
            try
            {
                if (twinCollection.Contains("SendData") && twinCollection["SendData"] != null)
                {
                    _sendData = twinCollection["SendData"];
                }
                if (twinCollection.Contains("DeviceLongitude") && twinCollection["DeviceLongitude"] != null)
                {
                    _deviceLongitude = twinCollection["DeviceLongitude"];
                }
                if (twinCollection.Contains("DeviceLatitude") && twinCollection["DeviceLatitude"] != null)
                {
                    _deviceLatitude = twinCollection["DeviceLatitude"];
                }
                if (twinCollection.Contains("ServerHostName") && twinCollection["ServerHostName"] != null)
                {
                    _serverHostname = twinCollection["ServerHostName"];
                }
                if (twinCollection.Contains("ServerBasePort") && twinCollection["ServerBasePort"] != null)
                {
                    _serverBasePort = twinCollection["ServerBasePort"];
                }
                if (twinCollection.Contains("CarDetectionServerHostname") && twinCollection["CarDetectionServerHostname"] != null)
                {
                    _carDetectionServerHostname = twinCollection["CarDetectionServerHostname"];
                }
                if (twinCollection.Contains("CarDetectionServerBasePort") && twinCollection["CarDetectionServerBasePort"] != null)
                {
                    _carDetectionServerBasePort = twinCollection["CarDetectionServerBasePort"];
                }
            }
            catch (AggregateException aexc)
            {
                foreach (var exception in aexc.InnerExceptions)
                {
                    Console.WriteLine($"[ERROR] Could not retrieve desired properties {aexc.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Reading desired properties failed with {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"Value for SendData = {_sendData}");
                Console.WriteLine($"Value for DeviceLatitude = {_deviceLatitude}");
                Console.WriteLine($"Value for DeviceLongitude = {_deviceLongitude}");
                Console.WriteLine($"Value for ServerHostname = {_serverHostname}");
                Console.WriteLine($"Value for ServerBasePort = {_serverBasePort}");
                Console.WriteLine($"Value for CarDetectionServerHostname = {_carDetectionServerHostname}");
                Console.WriteLine($"Value for CarDetectionServerHostname = {_carDetectionServerBasePort}");
            }
        }
        public bool SendData => _sendData;
        public double DeviceLatitude => _deviceLatitude;
        public double DeviceLongitude => _deviceLongitude;
        public string ServerHostname => _serverHostname;
        public int ServerBasePort => _serverBasePort;
        public string CarDetectionServerHostname => _carDetectionServerHostname;
        public int CarDetectionServerBasePort => _carDetectionServerBasePort;
    }
}