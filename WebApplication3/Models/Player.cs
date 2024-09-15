using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;

namespace MLB_App.Models
{

    public class PlayerDetail : Root
    {
        [JsonProperty("body")]
        public Player player { get; set; }
    }

    public class Player
    {
        public string espnID { get; set; }
        public string sleeperBotID { get; set; }
        public string fantasyProsPlayerID { get; set; }
        public string highSchool { get; set; }
        public string _throw { get; set; }
        public string weight { get; set; }
        public string jerseyNum { get; set; }
        public string team { get; set; }
        public string mlbHeadshot { get; set; }
        public string yahooPlayerID { get; set; }
        public string espnLink { get; set; }
        public string yahooLink { get; set; }
        public string bDay { get; set; }
        public string mlbLink { get; set; }
        public string teamAbv { get; set; }
        public string espnHeadshot { get; set; }
        public string rotoWirePlayerIDFull { get; set; }
        public Injury injury { get; set; }
        public string teamID { get; set; }
        public string pos { get; set; }
        public string mlbIDFull { get; set; }
        public string cbsPlayerID { get; set; }
        public string longName { get; set; }
        public string bat { get; set; }
        public string rotoWirePlayerID { get; set; }
        public string height { get; set; }
        public string lastGamePlayed { get; set; }
        public string mlbID { get; set; }
        public string playerID { get; set; }
        public string fantasyProsLink { get; set; }

        public string ConvertHeightFromFootToCm(string height)
        {
            string altura = String.Empty;
            double alturaConv = 0;
            if (!String.IsNullOrEmpty(height))
            {
                string currentItem = height.Replace("-", ".");
                double.TryParse(currentItem, out alturaConv);
                double conversion = alturaConv * 30.48;
                altura = conversion.ToString().Substring(0, conversion.ToString().IndexOf(".")) + "cm";
            }

            return altura;
        }

        public string ConvertPoundsToKgs(string pounds)
        {
            string kgs = String.Empty;
            double weightKg = 0;
            if (!String.IsNullOrEmpty(pounds))
            {
                double.TryParse(pounds, out weightKg);
                weightKg = Math.Round((weightKg / 2.205), 0);
                kgs = weightKg.ToString() + "kg";
            }

            return kgs;
        }
    }

    public class Injury
    {
        public string description { get; set; }
        public string injDate { get; set; }
        public string designation { get; set; }
    }
}