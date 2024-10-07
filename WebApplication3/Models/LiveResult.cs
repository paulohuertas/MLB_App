using MLB_App.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MLB_App.Models
{
    public class LiveResult : Root
    {
        public RealTimeBoxScore body;
    }

    public class RealTimeBoxScore
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty]
        public string GameLength { get; set; }
        [JsonProperty]
        public string gameStatus { get; set; }
        [JsonProperty]
        public string Attendance { get; set; }
        [JsonProperty]
        public string Venue { get; set; }
        [JsonProperty]
        [ForeignKey("lineScore_Id")]
        public lineScore lineScore { get; set; }
        [JsonProperty]
        public string gameID { get; set; }
        [JsonProperty]
        public string homeResult { get; set; }
        [JsonProperty]
        public string home { get; set; }
        [JsonProperty]
        public string awayResult { get; set; }
        [JsonProperty]
        public string away { get; set; }
        public int lineScore_Id { get; set; }

    }

    public class lineScore
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty]
        [ForeignKey("away_Id")]
        public away away { get; set; }
        [JsonProperty]
        [ForeignKey("home_Id")]
        public home home { get; set; }
        public int away_Id { get; set; }
        public int home_Id { get; set; }
        public virtual ICollection<RealTimeBoxScore> RealTimeBoxScores { get; set; }
        public virtual ICollection<GameDetails> GameDetails { get; set; }
    }

    public class away
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty]
        public string H { get; set; }
        [JsonProperty]
        public string R { get; set; }
        [JsonProperty]
        public string E { get; set; }

        public virtual ICollection<lineScore> lineScores { get; set; }
    }

    public class home
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty]
        public string H { get; set; }
        [JsonProperty]
        public string R { get; set; }
        [JsonProperty]
        public string E { get; set; }

        public virtual ICollection<lineScore> lineScores { get; set; }
    }
}