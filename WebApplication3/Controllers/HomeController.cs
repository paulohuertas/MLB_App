using Newtonsoft.Json;
using RestSharp;
using System;
using System.Globalization;
using System.Web.Mvc;
using MLB_App.Models;
using MLB_App.Utils;
using MLB_App.Models.Data;
using System.Linq;
using System.Collections.Generic;

namespace MLB_App.Controllers
{
    public class HomeController : Controller
    {
        DataContext context = new DataContext();
        ApiCall call = new ApiCall();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "MLB through API";

            return View();
        }

        [HttpPost]
        public JsonResult GetGameList()
        {

            string today = DateTime.Today.ToString("yyyyMMdd");
            var result = context.Schedules.Take(15).OrderByDescending(t => t.gameDate).ToList();

            List<Schedule> data = result.ToList();

            var myJsonData = data.Select(S => new
            {
                GameType = S.gameType,
                Home = S.home,
                Away = S.away,
                GameDate = S.gameDate,
                GameID = S.gameID,
                GameTime = S.gameTime,
                TeamHome = S.teamIDAway,
                TeamAway = S.teamIDAway
            });

            if (myJsonData.Count() > 0 && myJsonData != null)
            {
                return Json(myJsonData);
            }

            return null;
        }

        [HttpPost]
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult GetDailySchedule(string gameDate)
        {
            string[] gameDateSplit = gameDate.Split('-');
            DateTime dateTime = new DateTime(int.Parse(gameDateSplit[0]), int.Parse(gameDateSplit[1]), int.Parse(gameDateSplit[2]));
            gameDate = gameDate.Replace("-", "");

            using (var context = new DataContext())
            {

                var result = context.Schedules.Include("ProbableStartingPitchers").Where(s => s.gameDate == gameDate).OrderBy(t => t.gameTime).ToList();

                var data = result.ToList();

                if (data.Count() > 0 && data != null)
                {
                    DailySchedule dailySchedule = new DailySchedule();
                    dailySchedule.body = data;
                    ViewData["gameDate"] = DateParser.ParseDateToString(gameDate);
                    return View("Schedule", dailySchedule);
                }
            }

            if (String.IsNullOrEmpty(gameDate)) gameDate = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
           
            string url = $"https://tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com/getMLBGamesForDate?gameDate={gameDate}";

            string respJson = call.PrepareApiCall(url, "GET");

            var jsonObject = JsonConvert.DeserializeObject<DailySchedule>(respJson);

            DataManagement dataManagement = new DataManagement();
            dataManagement.Save(jsonObject.body);

            ViewData["gameDate"] = DateParser.ParseDateToString(gameDate);

            return View("Schedule", jsonObject);
        }

        public ActionResult PlayerInformation(string id)
        {
            Player data = context.Players.Where(p => p.playerID == id).FirstOrDefault();

            if(data != null)
            {
                PlayerDetail playerDetail = new PlayerDetail();
                playerDetail.player = data;

                if(playerDetail.player != null)
                {
                    return View("PlayerDetail", playerDetail);
                }
            }

            string url = $"https://tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com/getMLBPlayerInfo?playerID={id}&getStats=false";

            string respJson = call.PrepareApiCall(url, "GET");

            var jsonObject = JsonConvert.DeserializeObject<PlayerDetail>(respJson);

            if(jsonObject.StatusCode == "200")
            {
                PlayerManagement playerManagement = new PlayerManagement();
                playerManagement.Save(jsonObject.player);
                return View("PlayerDetail", jsonObject);
            }

            return View("PlayerDetail");
        }

        [HttpGet]
        public ActionResult GameResult(string gameId)
        {
            //20240614_STL@CHC
            //RealTimeBoxScore score = context.RealTimeBoxScore.Include("lineScore").Where(g => g.gameID == gameId).FirstOrDefault();

            RealTimeBoxScore score = context.RealTimeBoxScore.
                            Include("lineScore").
                            Include("lineScore.away").
                            Include("lineScore.home").
                            Where(g => g.gameID == gameId).FirstOrDefault();

            if (score != null)
            {
                LiveResult liveResult = new LiveResult();
                liveResult.body = score;

                if(liveResult != null)
                {
                    return View("LiveResult", liveResult);
                }
            }

            string url = $"https://tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com/getMLBBoxScore?gameID={gameId}&startingLineups=false";

            string respJson = call.PrepareApiCall(url, "GET");

            var jsonObject = JsonConvert.DeserializeObject<LiveResult>(respJson);

            if (jsonObject.StatusCode == "200")
            {
                RealTimeBoxScoreManagement resultManagement = new RealTimeBoxScoreManagement();
                resultManagement.Save(jsonObject.body);
                return View("LiveResult", jsonObject);
            }

            return View("LiveResult");
        }
    }
}