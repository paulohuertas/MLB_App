using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MLB_App.Models
{
    public class DailyScoreboard : Root
    {
        public Dictionary<string, GameDetails> body { get; set; }
    }

    public class GameDetails
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty]
        public string away { get; set; }
        [JsonProperty]
        public string home { get; set; }
        [JsonProperty]
        public string teamIDAway { get; set; }
        [JsonProperty]
        public string teamIDHome { get; set; }
        [JsonProperty]
        public string gameTime { get; set; }
        [JsonProperty]
        public string gameTime_epoch { get; set; }
        [JsonProperty]
        [ForeignKey("lineScore_Id")]
        public lineScore lineScore { get; set; }
        [JsonProperty]
        public string currentInning { get; set; }
        [JsonProperty]
        public string currentCount { get; set; }
        [JsonProperty]
        public string currentOuts { get; set; }
        [JsonProperty]
        public string awayResult { get; set; }
        [JsonProperty]
        public string homeResult { get; set; }
        [JsonProperty]
        public string gameID { get; set; }
        [JsonProperty]
        public string gameStatus { get; set; }
        [JsonProperty]
        public string gameStatusCode { get; set; }
        public int? lineScore_Id { get; set; }
    }

    public class TeamScore
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty]
        public string H { get; set; }
        [JsonProperty]
        public string R { get; set; }
        [JsonProperty]
        public string team { get; set; }
        [JsonProperty]
        public Dictionary<string, string> scoresByInning { get; set; }
        [JsonProperty]
        public string E { get; set; }
        //public virtual ICollection<LineScore> LineScores { get; set; }
    }
}