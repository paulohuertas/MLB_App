using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLB_App.Models
{
    public abstract class Root
    {
        [JsonProperty("statusCode")]
        public string StatusCode;
    }
}