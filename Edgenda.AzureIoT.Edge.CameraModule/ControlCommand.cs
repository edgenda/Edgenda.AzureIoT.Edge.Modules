using Newtonsoft.Json;

namespace Edgenda.AzureIoT.Edge.CameraModule
{
    public enum ControlCommandEnum
    {
        Capture = 0
    };

    public class ControlCommand
    {
        [JsonProperty("command")]
        public ControlCommandEnum Command { get; set; }
    }
}