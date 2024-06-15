using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLB_App.Models
{
    public class ProbableStartingPitchers
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty("away")]
        public string Away { get; set; }
        [JsonProperty("home")]
        public string Home { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}