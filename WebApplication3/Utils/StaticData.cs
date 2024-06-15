using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLB_App.Utils
{
    public class StaticData
    {
        public static Dictionary<string, string> TeamLogos = new Dictionary<string, string>();

        public string GetTeamLogoByNameAbbreviation(string abbrev)
        {
            if(TeamLogos.Count == 0)
            {
                TeamLogos.Add("ARI", "https://www.mlbstatic.com/team-logos/team-cap-on-light/109.svg");
                TeamLogos.Add("ATL", "https://www.mlbstatic.com/team-logos/team-cap-on-light/144.svg");
                TeamLogos.Add("BAL", "https://www.mlbstatic.com/team-logos/team-cap-on-light/110.svg");
                TeamLogos.Add("BOS", "https://www.mlbstatic.com/team-logos/team-cap-on-light/111.svg");
                TeamLogos.Add("CHC", "https://www.mlbstatic.com/team-logos/team-cap-on-light/112.svg");
                TeamLogos.Add("CHW", "https://www.mlbstatic.com/team-logos/team-cap-on-light/145.svg");
                TeamLogos.Add("CIN", "https://www.mlbstatic.com/team-logos/team-cap-on-light/113.svg");
                TeamLogos.Add("CLE", "https://www.mlbstatic.com/team-logos/team-cap-on-light/114.svg");
                TeamLogos.Add("COL", "https://www.mlbstatic.com/team-logos/team-cap-on-light/115.svg");
                TeamLogos.Add("DET", "https://www.mlbstatic.com/team-logos/team-cap-on-light/116.svg");
                TeamLogos.Add("HOU", "https://www.mlbstatic.com/team-logos/team-cap-on-light/117.svg");
                TeamLogos.Add("KC",  "https://www.mlbstatic.com/team-logos/team-cap-on-light/118.svg");
                TeamLogos.Add("LAA", "https://www.mlbstatic.com/team-logos/team-cap-on-light/108.svg");
                TeamLogos.Add("LAD", "https://www.mlbstatic.com/team-logos/team-cap-on-light/119.svg");
                TeamLogos.Add("MIA", "https://www.mlbstatic.com/team-logos/team-cap-on-light/146.svg");
                TeamLogos.Add("MIL", "https://www.mlbstatic.com/team-logos/team-cap-on-light/158.svg");
                TeamLogos.Add("MIN", "https://www.mlbstatic.com/team-logos/team-cap-on-light/142.svg");
                TeamLogos.Add("NYM", "https://www.mlbstatic.com/team-logos/team-cap-on-light/121.svg");
                TeamLogos.Add("NYY", "https://www.mlbstatic.com/team-logos/team-cap-on-light/147.svg");
                TeamLogos.Add("OAK", "https://www.mlbstatic.com/team-logos/team-cap-on-light/133.svg");
                TeamLogos.Add("PHI", "https://www.mlbstatic.com/team-logos/team-cap-on-light/143.svg");
                TeamLogos.Add("PIT", "https://www.mlbstatic.com/team-logos/team-cap-on-light/134.svg");
                TeamLogos.Add("SD",  "https://www.mlbstatic.com/team-logos/team-cap-on-light/135.svg");
                TeamLogos.Add("SF",  "https://www.mlbstatic.com/team-logos/team-cap-on-light/137.svg");
                TeamLogos.Add("SEA", "https://www.mlbstatic.com/team-logos/team-cap-on-light/136.svg");
                TeamLogos.Add("STL", "https://www.mlbstatic.com/team-logos/team-cap-on-light/138.svg");
                TeamLogos.Add("TB",  "https://www.mlbstatic.com/team-logos/team-cap-on-light/139.svg");
                TeamLogos.Add("TEX", "https://www.mlbstatic.com/team-logos/team-cap-on-light/140.svg");
                TeamLogos.Add("TOR", "https://www.mlbstatic.com/team-logos/team-cap-on-light/141.svg");
                TeamLogos.Add("WAS", "https://www.mlbstatic.com/team-logos/team-cap-on-light/120.svg");
            }
            
            if (TeamLogos.ContainsKey(abbrev))
            {
                return TeamLogos[abbrev];
            }
            return String.Empty;
        }
    }
}