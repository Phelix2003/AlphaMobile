using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaMobile.Configuration
{
    public static class AppConfiguration
    {
        public const string CloudServer_URI = "https://alpha-easio.azurewebsites.net";
        public const string APIServer_URI = CloudServer_URI + "/api";
        public const string ItemPictureRender_URI = CloudServer_URI + "/Menu/RenderItemPhoto";
    }
}
