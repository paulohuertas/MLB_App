using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLB_App.Models.Interfaces
{
    public interface IPlayerRoot<T> where T : class
    {
        [JsonProperty("body")]
        T player { get; set; }
    }
}
