using MLB_App.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLB_App.Models
{
    public class Schedule : StaticData
    {
        public string gameID;
        public string gameType;
        public string away;
        public string gameTime;
        public string gameDate;
        public string teamIDHome;
        public string gameTime_epoch;
        public string teamIDAway;
        public ProbableStartingPitchers probableStartingPitchers;
        public string home;

        public string GetPlayerNameById(string id)
        {
           if(String.IsNullOrEmpty(id)) return "";

            string url = $"https://tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com/getMLBPlayerInfo?playerID={id}&getStats=false";
            ApiCall call = new ApiCall();
            string response = call.PrepareApiCall(url, "get");

            var playerObject = JsonConvert.DeserializeObject<PlayerDetail>(response);

            string playerName = playerObject.body.longName;

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