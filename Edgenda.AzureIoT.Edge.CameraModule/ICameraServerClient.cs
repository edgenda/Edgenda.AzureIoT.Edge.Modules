using Edgenda.AzureIoT.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Edgenda.AzureIoT.Edge.CameraModule
{
    interface ICameraServerClient
    {
        CameraProperties[] GetCameraProperties(double longitude, double latitude);
    }
}
