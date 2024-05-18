﻿using System;
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

        public Bitmap GetPlayerImage(string playerId)
        {
            string imgUrl = $"https://img.mlbstatic.com/mlb-photos/image/upload/d_people:generic:headshot:silo:current.png/r_max/w_180,q_auto:best/v1/people/{playerId}/headshot/silo/current";
            WebRequest request = HttpWebRequest.Create(imgUrl);

            WebResponse response1 = request.GetResponse();
            var stream = response1.GetResponseStream();
            //var reader = new StreamReader(response1.GetResponseStream());

            if (stream != null)
            {
                Bitmap bitmap = new Bitmap(stream);
                return bitmap;
            }
            return null;
        }
    }

    public class Injury
    {
        public string description { get; set; }
        public string injDate { get; set; }
        public string designation { get; set; }
    }
}