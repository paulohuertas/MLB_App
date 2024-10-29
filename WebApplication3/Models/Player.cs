using MLB_App.Models.Interfaces;
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
        public Player body { get; set; }
        public List<Player> players { get; set; }
    }

    public class Injury
    {
        public string description { get; set; }
        public string injDate { get; set; }
        public string designation { get; set; }
    }

    public class Player : Root
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
        private DateTime? _amendedDateTime;

        public DateTime amendedDateTime
        {
            get
            {
                if(_amendedDateTime == null)
                {
                    _amendedDateTime = DateTime.Now;
                }

                return _amendedDateTime.Value;
            }
            set
            {
                _amendedDateTime = value;
            }
        }

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

        public string ConvertLastGameDate(string dateString)
        {
            if (String.IsNullOrEmpty(dateString)) return "";

            if (dateString.Contains("/")) dateString = dateString.Replace("/", "");
            string dateToReturn = dateString.Substring(0, 8);
            if (!String.IsNullOrEmpty(dateToReturn))
            {
                string year = dateToReturn.Substring(0, 4);
                string month = dateToReturn.Substring(4, 2);
                string day = dateToReturn.Substring(6, 2);

                dateToReturn = day + "/" + month + "/" + year;
            }

            return dateToReturn;
        }

        public string ConvertPlayerDOB(string dob)
        {
            if (String.IsNullOrEmpty(dob)) return "";

            if (dob.Contains("/")) dob = dob.Replace("/", "");

            int numberOfChars = dob.Length;

            string dateOfBirth = dob.Substring(0, numberOfChars);

            //6/4/1993	
            if(dateOfBirth.Length == 6)
            {
                string month = dateOfBirth.Substring(0, 1);
                string day = dateOfBirth.Substring(1, 1);
                string year = dateOfBirth.Substring(2, 4);

                dateOfBirth = day + "/" + month + "/" + year;

                return dateOfBirth;
            }
            //6/12/1995
            if (dateOfBirth.Length == 7)
            {
                string month = dateOfBirth.Substring(0, 1);
                string day = dateOfBirth.Substring(1, 2);
                string year = dateOfBirth.Substring(3, 4);

                dateOfBirth = day + "/" + month + "/" + year;

                return dateOfBirth;
            }

            //10/17/1994
            if (dateOfBirth.Length == 8)
            {
                string month = dateOfBirth.Substring(0, 2);
                string day = dateOfBirth.Substring(2, 2);
                string year = dateOfBirth.Substring(4, 4);

                dateOfBirth = day + "/" + month + "/" + year;

                return dateOfBirth;
            }

            return null;
        }
    }

}