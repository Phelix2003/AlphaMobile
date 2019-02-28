using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaMobile.Models.APIModels
{

    public class ResponseHeaderAPIModel
    {       

        public string SpecVersion { get; set; }
        public string RequestId { get; set; }
    }

    public class TokenRequestResponseAPIModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string userName { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }
    }

    public class ErrorResponseAPIModel
    {
        public string error { get; set; }
        public string error_description { get; set; }
    }
}