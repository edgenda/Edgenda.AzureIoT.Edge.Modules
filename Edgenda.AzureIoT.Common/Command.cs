using System;
using System.Collections.Generic;
using System.Text;

namespace Edgenda.AzureIoT.Common
{
    public class Command
    {
        public const string GET_BY_COORDINATES_COMMAND = "get-by-coordinates";
        public const string SHUTDOWN_COMMAND = "shutdown";

        public string Name { get; set; }
    }

    public class GetByCoordinatesCommand : Command
    {
        public double[] Parameters { get; set; }
    }
}
