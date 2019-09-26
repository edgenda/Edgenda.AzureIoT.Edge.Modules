using Edgenda.AzureIoT.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Edgenda.AzureIoT.Edge.CameraModule
{
    class ExtendedCameraProperties : CameraProperties
    {
        public Predictions Predictions { get; set; }
        public ExtendedCameraProperties(CameraProperties source) 
        {
            base.BuroughId = source.BuroughId;
            base.CameraId = source.CameraId;
            base.Description = source.Description;
            base.EastWestStreet = source.EastWestStreet;
            base.Id = source.Id;
            base.ImageData = source.ImageData;
            base.ImageType = source.ImageType;
            base.LiveImageUrl = source.LiveImageUrl;
            base.LiveVideoUrl = source.LiveVideoUrl;
            base.NorthSouthStreet = source.NorthSouthStreet;
            base.Title = source.Title;
            base.Url = source.Url;
        }
    }
}
