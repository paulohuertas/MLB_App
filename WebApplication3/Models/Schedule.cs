using MLB_App.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLB_App.Models
{
    public class Schedule
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

        public string GetTeamLogoByNameAbbreviation(string abbrev)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("ARI", "https://www.mlbstatic.com/team-logos/team-cap-on-light/109.svg");
            dict.Add("ATL", "https://www.mlbstatic.com/team-logos/team-cap-on-light/144.svg");
            dict.Add("BAL", "https://www.mlbstatic.com/team-logos/team-cap-on-light/110.svg");
            dict.Add("BOS", "https://www.mlbstatic.com/team-logos/team-cap-on-light/111.svg");
            dict.Add("CHC", "https://www.mlbstatic.com/team-logos/team-cap-on-light/112.svg");
            dict.Add("CHW", "https://www.mlbstatic.com/team-logos/team-cap-on-light/145.svg");
            dict.Add("CIN", "https://www.mlbstatic.com/team-logos/team-cap-on-light/113.svg");
            dict.Add("CLE", "https://www.mlbstatic.com/team-logos/team-cap-on-light/114.svg");
            dict.Add("COL", "https://www.mlbstatic.com/team-logos/team-cap-on-light/115.svg");
            dict.Add("DET", "https://www.mlbstatic.com/team-logos/team-cap-on-light/116.svg");
            dict.Add("HOU", "https://www.mlbstatic.com/team-logos/team-cap-on-light/117.svg");
            dict.Add("KC", "https://www.mlbstatic.com/team-logos/team-cap-on-light/118.svg");
            dict.Add("LAA", "https://www.mlbstatic.com/team-logos/team-cap-on-light/108.svg");
            dict.Add("LAD", "https://www.mlbstatic.com/team-logos/team-cap-on-light/119.svg");
            dict.Add("MIA", "https://www.mlbstatic.com/team-logos/team-cap-on-light/146.svg");
            dict.Add("MIL", "https://www.mlbstatic.com/team-logos/team-cap-on-light/158.svg");
            dict.Add("MIN", "https://www.mlbstatic.com/team-logos/team-cap-on-light/142.svg");
            dict.Add("NYM", "https://www.mlbstatic.com/team-logos/team-cap-on-light/121.svg");
            dict.Add("NYY", "https://www.mlbstatic.com/team-logos/team-cap-on-light/147.svg");
            dict.Add("OAK", "https://www.mlbstatic.com/team-logos/team-cap-on-light/133.svg");
            dict.Add("PHI", "https://www.mlbstatic.com/team-logos/team-cap-on-light/143.svg");
            dict.Add("PIT", "https://www.mlbstatic.com/team-logos/team-cap-on-light/134.svg");
            dict.Add("SD", "https://www.mlbstatic.com/team-logos/team-cap-on-light/135.svg");
            dict.Add("SF", "https://www.mlbstatic.com/team-logos/team-cap-on-light/137.svg");
            dict.Add("SEA", "https://www.mlbstatic.com/team-logos/team-cap-on-light/136.svg");
            dict.Add("STL", "https://www.mlbstatic.com/team-logos/team-cap-on-light/138.svg");
            dict.Add("TB", "https://www.mlbstatic.com/team-logos/team-cap-on-light/139.svg");
            dict.Add("TEX", "https://www.mlbstatic.com/team-logos/team-cap-on-light/140.svg");
            dict.Add("TOR", "https://www.mlbstatic.com/team-logos/team-cap-on-light/141.svg");
            dict.Add("WAS", "https://www.mlbstatic.com/team-logos/team-cap-on-light/120.svg");

            if (dict.ContainsKey(abbrev))
            {
                return dict[abbrev];
            }
            return String.Empty;
        }
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