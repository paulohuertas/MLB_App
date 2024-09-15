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
using PagedList;

namespace MLB_App.Controllers
{
    public class HomeController : Controller
    {
        DataContext context = new DataContext();
        ApiCall call = new ApiCall();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Major League Baseball API";
            //string url = @"https://tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com/getMLBTeams?teamStats=true&topPerformers=true";
            //string respJson = call.PrepareApiCall(url, "GET");
            return View();
        }


        public ActionResult GetPlayers(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IEnumerable<Player> players = new List<Player>();

            if (String.IsNullOrEmpty(searchString))
            {
                players = context.Players.OrderBy(p => p.longName).ToList();
            }
            else
            {
                players = context.Players.Where(p => p.longName.Contains(searchString)).OrderBy(p => p.longName);
                //players = context.Players.Where(p => p.team.Contains(searchString)).OrderBy(p => p.team);
            }

            if (players.Count() > 0 && players != null)
            {

                return View(players.ToPagedList(pageNumber, pageSize));
            }

            return View("GetPlayers", players.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public JsonResult GetGameList()
        {
            var query = from sc in context.Schedules
                        join rt in context.RealTimeBoxScore on sc.gameID equals rt.gameID
                        where rt.gameStatus == "Completed"
                        select new
                        {
                            GameDate = sc.gameDate,
                            GameType = sc.gameType,
                            GameStatus = rt.gameStatus,
                            Attendance = rt.Attendance,
                            Away = rt.away,
                            Venue = rt.Venue,
                            AwayRun = rt.lineScore.away.R,
                            Home = rt.home,
                            HomeRun = rt.lineScore.home.R
                        };

            var myJsonData = query.Take(15).OrderByDescending(t => t.GameDate).ToList();

            if (myJsonData.Count() > 0 && myJsonData != null)
            {
                return Json(myJsonData);
            }

            return null;
        }

        [HttpGet]
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult GetDailySchedule()
        {
            DateTime today = DateTime.Today;
            string gameDate = today.ToString("yyyy-MM-dd").Replace("-", "");

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

            if(respJson.Contains("No games on this date"))
            {
                return View("Schedule");
            }

            var jsonObject = JsonConvert.DeserializeObject<DailySchedule>(respJson);

            DataManagement dataManagement = new DataManagement();
            dataManagement.Save(jsonObject.body);

            DateTime dateTime = DateTime.ParseExact(gameDate, "yyyyMMdd", CultureInfo.InvariantCulture);
            string dt = dateTime.ToString("yyyy-MM-dd");
            ViewData["gameDate"] = dt;

            return View("Schedule", jsonObject);
        }

        [HttpPost]
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult GetDailySchedule(string gameDate)
        {
            if (String.IsNullOrEmpty(gameDate))
            {
                gameDate = DateTime.Today.ToString("yyyyMMdd");
            }
            else
            {
                gameDate = gameDate.Replace("-", "");
            }

            using (var context = new DataContext())
            {

                var result = context.Schedules.Include("ProbableStartingPitchers").Where(s => s.gameDate == gameDate).OrderBy(t => t.gameTime).ToList();

                var data = result.ToList();

                if (data.Count() > 0 && data != null)
                {
                    DailySchedule dailySchedule = new DailySchedule();
                    dailySchedule.body = data;

                    DateTime dateTime = DateTime.ParseExact(gameDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                    string dt = dateTime.ToString("yyyy-MM-dd");
                    ViewData["gameDate"] = dt;

                    return View("Schedule", dailySchedule);
                }
            }

            if (String.IsNullOrEmpty(gameDate)) gameDate = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            string url = $"https://tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com/getMLBGamesForDate?gameDate={gameDate}";

            string respJson = call.PrepareApiCall(url, "GET");

            if (respJson.Contains("No games on this date"))
            {
                return View("Schedule");
            }

            var jsonObject = JsonConvert.DeserializeObject<DailySchedule>(respJson);

            DataManagement dataManagement = new DataManagement();
            dataManagement.Save(jsonObject.body);

            ViewData["gameDate"] = DateParser.ParseDateToString(gameDate);

            return View("Schedule", jsonObject);
        }

        public ActionResult PlayerInformation(string id)
        {
            Player data = context.Players.Where(p => p.playerID == id).FirstOrDefault();

            if (data != null)
            {
                PlayerDetail playerDetail = new PlayerDetail();
                playerDetail.player = data;

                if (playerDetail.player != null)
                {
                    return View("PlayerDetail", playerDetail);
                }
            }

            string url = $"https://tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com/getMLBPlayerInfo?playerID={id}&getStats=false";

            string respJson = call.PrepareApiCall(url, "GET");

            var jsonObject = JsonConvert.DeserializeObject<PlayerDetail>(respJson);

            if (jsonObject.StatusCode == "200")
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
            RealTimeBoxScore score = context.RealTimeBoxScore.
                            Include("lineScore").
                            Include("lineScore.away").
                            Include("lineScore.home").
                            Where(g => g.gameID == gameId).FirstOrDefault();


            if (score != null)
            {
                if (score.gameStatus == "Completed")
                {
                    LiveResult liveResult = new LiveResult();
                    liveResult.body = score;

                    if (liveResult != null)
                    {
                        return View("LiveResult", liveResult);
                    }
                }
            }

            string url = $"https://tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com/getMLBBoxScore?gameID={gameId}&startingLineups=false";

            string respJson = call.PrepareApiCall(url, "GET");

            var jsonObject = JsonConvert.DeserializeObject<LiveResult>(respJson);

            if (jsonObject.StatusCode == "200")
            {
                //api call is good however no data is returned;
                if (jsonObject.body.gameID == null)
                {
                    return View("LiveResult");

                }

                RealTimeBoxScoreManagement resultManagement = new RealTimeBoxScoreManagement();
                resultManagement.Update(jsonObject.body);

                return View("LiveResult", jsonObject);
            }

            return View("LiveResult");
        }

        [HttpPost]
        public JsonResult GetPlayerById(string id)
        {
            Player data = context.Players.Where(p => p.playerID == id).FirstOrDefault();

            if (data != null)
            {
                PlayerDetail playerDetail = new PlayerDetail();
                playerDetail.player = data;

                if (playerDetail.player != null)
                {
                    return Json(playerDetail.player);
                }
            }

            return null;
        }
    }
}