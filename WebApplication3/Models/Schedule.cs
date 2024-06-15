using MLB_App.Models.Data;
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
    public class Schedule : StaticData
    {
        [Key]
        public int Id { get; set; }
        [JsonProperty]
        public string gameID { get; set; }
        [JsonProperty]
        public string gameType { get; set; }
        [JsonProperty]
        public string away { get; set; }
        [JsonProperty]
        public string gameTime { get; set; }
        [JsonProperty]
        public string gameDate { get; set; }
        [JsonProperty]
        public string teamIDHome { get; set; }
        [JsonProperty]
        public string gameTime_epoch { get; set; }
        [JsonProperty]
        public string teamIDAway { get; set; }
        [JsonProperty]
        [ForeignKey("ProbableStartingPitchers_Id")]
        public virtual ProbableStartingPitchers probableStartingPitchers { get; set; }
        [JsonProperty]
        public string home { get; set; }
        public int ProbableStartingPitchers_Id { get; set; }

        public string GetPlayerNameById(string id)
        {
           if(String.IsNullOrEmpty(id)) return "";

            DataContext dataContext = new DataContext();
            Player player = dataContext.Players.Where(p => p.playerID == id).FirstOrDefault();

            if(player != null)
            {
                return player.longName;
            }

            string url = $"https://tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com/getMLBPlayerInfo?playerID={id}&getStats=false";
            ApiCall call = new ApiCall();
            string response = call.PrepareApiCall(url, "get");

            var playerObject = JsonConvert.DeserializeObject<PlayerDetail>(response);

            PlayerManagement playerManagement = new PlayerManagement();
            playerManagement.Save(playerObject.player);

            string playerName = playerObject.player.longName;

            return playerName;
        }

        public DateTime ConvertStringToDateTime(string date)
        {
            int year = int.Parse(date.Substring(0, 4));
            int month = int.Parse(date.Substring(4, 2));
            int day = int.Parse(date.Substring(6, 2));

            DateTime dt = new DateTime(year, month, day);

            return dt;
        }
    }
}