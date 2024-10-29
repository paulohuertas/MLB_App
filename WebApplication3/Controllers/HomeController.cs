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
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using MLB_App.Interfaces;

namespace MLB_App.Controllers
{
    public class HomeController : Controller
    {
        DataContext context = new DataContext();
        ApiCall apiCall = new ApiCall();
        ScheduleRepository scheduleModel = new ScheduleRepository();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Major League Baseball API";
            return View();
        }

        [HttpPost]
        public JsonResult GetGameList()
        {
            var query = from sc in context.Schedules
                        join rt in context.RealTimeBoxScore on sc.gameID equals rt.gameID
                        //join gd in context.GameDetails on sc.gameID equals gd.gameID
                        where rt.gameStatus == "Completed"
                        select new
                        {
                            GameId = sc.gameID,
                            GameDate = sc.gameDate,
                            GameType = sc.gameType,
                            GameStatus = rt.gameStatus,
                            Attendance = rt.Attendance,
                            Away = rt.away,
                            Venue = rt.Venue,
                            AwayRun = rt.lineScore.away.R,
                            Home = rt.home,
                            HomeRun = rt.lineScore.home.R,
                            AwayResult = rt.awayResult,
                            HomeResult = rt.homeResult
                        };

            var myJsonData = query.Take(20).OrderByDescending(t => t.GameDate).ToList();

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

            ViewData["gameDate"] = DateParser.ParseDateToString(gameDate);

            var data = scheduleModel.GetAllScheduledGames(gameDate);

            if(data.Count() > 0 && data != null)
            {
                DailySchedule schedule = new DailySchedule();
                schedule.body = data.ToList();

                return View("Schedule", schedule);
            }

            if (String.IsNullOrEmpty(gameDate)) gameDate = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            string url = apiCall.GetApiUrl("getMLBGamesForDate") + gameDate;

            string respJson = apiCall.PrepareApiCall(url, "GET");

            try
            {
                if (!respJson.Contains("Bad request") && respJson.Contains("No games on this date"))
                {
                    return View("Schedule");
                }
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
            }

            var jsonObject = JsonConvert.DeserializeObject<DailySchedule>(respJson);

            DataManagement dataManagement = new DataManagement();
            dataManagement.Save(jsonObject.body);

            return View("Schedule", jsonObject);
        }

        [HttpPost]
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


            DateTime dateTime = DateTime.ParseExact(gameDate, "yyyyMMdd", CultureInfo.InvariantCulture);
            ViewData["gameDate"] = dateTime.ToString("yyyy-MM-dd");

            var data = scheduleModel.GetAllScheduledGames(gameDate);

            if (data.Count() > 0 && data != null)
            {
                DailySchedule dailySchedule = new DailySchedule();
                dailySchedule.body = data.ToList();

                return View("Schedule", dailySchedule);
            }
            
            string apiUrl = "getMLBGamesForDate";

            if (String.IsNullOrEmpty(gameDate)) gameDate = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            string url = apiCall.GetApiUrl(apiUrl) + gameDate;

            string respJson = apiCall.PrepareApiCall(url, "GET");

            if (respJson.Contains("No games on this date"))
            {
                return View("Schedule");
            }

            var jsonObject = JsonConvert.DeserializeObject<DailySchedule>(respJson);

            DataManagement dataManagement = new DataManagement();
            dataManagement.Save(jsonObject.body);

            return View("Schedule", jsonObject);
        }

        public ActionResult PlayerInformation(string id)
        {
            Player data = context.Players.Where(p => p.playerID == id).FirstOrDefault();

            //check if it's been two months since last upated player info
            if (data != null && ((DateTime.Now.Month - data.amendedDateTime.Month) < 2))
            {
                return View("PlayerDetail", data);   
            }

            string url = apiCall.GetApiUrl("getMLBPlayerInfo") + id + "&getStats=true";

            string respJson = apiCall.PrepareApiCall(url, "GET");

            var jsonObject = JsonConvert.DeserializeObject<PlayerDetail>(respJson);

            if (jsonObject.StatusCode == "200" && jsonObject.body != null)
            {
                PlayerManagement playerManagement = new PlayerManagement();

                if((DateTime.Now.Month - data.amendedDateTime.Month) < 2)
                {
                    playerManagement.Save(jsonObject.body);
                }
                else
                {
                    playerManagement.Update(jsonObject.body);
                    
                }
                return View("PlayerDetail", jsonObject.body);
            }

            return View("PlayerDetail");
        }

        [HttpPost]
        public JsonResult GetPlayerById(string id)
        {
            Player data = context.Players.Where(p => p.playerID == id).FirstOrDefault();
            
            if (data != null)
            {
                return Json(data);
            }

            return null;
        }

        [HttpGet]
        public ActionResult GameResult(string gameId)
        {
            RealTimeBoxScoreRepository realTimeRepository = new RealTimeBoxScoreRepository(context);

            RealTimeBoxScore score = realTimeRepository.GetGameRealTimeBoxScore(gameId);

            if (score != null && score.gameStatus == "Completed")
            {
                
                LiveResult liveResult = new LiveResult();
                liveResult.body = score;

                if (liveResult != null)
                {
                    return View("LiveResult", liveResult);
                }
            }

            string url = apiCall.GetApiUrl("getMLBBoxScore") + gameId + "&startingLineups=true";

            string respJson = apiCall.PrepareApiCall(url, "GET");
            var jsonObject = JsonConvert.DeserializeObject<LiveResult>(respJson);

            if (jsonObject.StatusCode == "200")
            {
                //api call is good however no data is returned;
                if (jsonObject.body.gameID == null)
                {
                    return View("LiveResult");

                }

                RealTimeScore realTimeScore = new RealTimeScore(context);

                if(score == null)
                {
                    realTimeScore.Add(jsonObject.body);
                }
                else
                {
                    realTimeScore.Update(score, jsonObject.body);
                }

                return View("LiveResult", jsonObject);
            }

            return View("LiveResult");
        }

        [HttpPost]
        public ActionResult GetDailyScoreboard(string gameDate)
        {
            if (String.IsNullOrEmpty(gameDate)) gameDate = DateTime.Now.ToString("yyyy-MM-dd");

            DateTime gameDt = DateTime.Now;
            gameDt = DateTime.ParseExact(gameDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            ViewData["gameDate"] = gameDt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (gameDate.Contains("-")) gameDate = gameDate.Replace("-", "");

            GameDetailDailyScoreBoardRepository gameDetailDailyScoreBoard = new GameDetailDailyScoreBoardRepository(context);

            var result = gameDetailDailyScoreBoard.GetGameDetailsScoreBoard(gameDate);

            string apiUrl = apiCall.GetApiUrl("getMLBScoresOnly") + gameDate + "&topPerformers=false";

            if (result != null && result.Count > 0)
            {
                DailyScoreboard dailyScoreboard = new DailyScoreboard();
                dailyScoreboard.body = new Dictionary<string, GameDetails>();
                    
                foreach(var res in result)
                {
                    if(res.gameStatusCode != "2")
                    {
                        string respJson = apiCall.PrepareApiCall(apiUrl, "GET");
                        var jsonObject = JsonConvert.DeserializeObject<DailyScoreboard>(respJson);

                        try
                        {
                            GameDetailsManagement gameDetails = new GameDetailsManagement();
                            gameDetails.Update(jsonObject.body);

                            return View("DailyScoreboard", jsonObject.body);
                        }
                        catch (Exception ex)
                        {
                            new Exception(ex.Message);
                        }
                    }

                    dailyScoreboard.body.Add(res.gameID, res);
                }

                return View("DailyScoreboard", dailyScoreboard.body);
                
            }

            string respJson2 = apiCall.PrepareApiCall(apiUrl, "GET");
            var jsonObject2 = JsonConvert.DeserializeObject<DailyScoreboard>(respJson2);

            if (jsonObject2.StatusCode == "200")
            {
                if (jsonObject2.body.Count == 0)
                {
                    return View("DailyScoreboard");
                }

                try
                {
                    GameDetailsManagement gameDetails = new GameDetailsManagement();
                    gameDetails.Update(jsonObject2.body);

                    return View("DailyScoreboard", jsonObject2.body);
                }
                catch (Exception ex)
                {
                    new Exception(ex.Message);
                }
            }

            return View("DailyScoreboard");
        }

        //Get
        [HttpGet]
        public ActionResult GetDailyScoreboard()
        {
            
            DateTime today = DateTime.Now;
            string gameDate = today.ToString("yyyy-MM-dd").Replace("-", "");

            ViewData["gameDate"] = today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            GameDetailDailyScoreBoardRepository gameDetailDailyScoreBoard = new GameDetailDailyScoreBoardRepository(context);

            var results = gameDetailDailyScoreBoard.GetGameDetailsScoreBoard(gameDate);
            
            if (results != null && results.Count > 0)
            {
                DailyScoreboard dailyScoreboard = new DailyScoreboard();
                dailyScoreboard.body = new Dictionary<string, GameDetails>();

                foreach(var result in results)
                {
                    dailyScoreboard.body.Add(result.gameID, result);
                }

                return View("DailyScoreboard", dailyScoreboard.body);
                
            }

            string apiUrl = apiCall.GetApiUrl("getMLBScoresOnly") + gameDate + "&topPerformers=false";
            string respJson = apiCall.PrepareApiCall(apiUrl, "GET");

            var jsonObject = JsonConvert.DeserializeObject<DailyScoreboard>(respJson);

            if(jsonObject.StatusCode == "200")
            {
                if(jsonObject.body.Count > 0)
                {
                    DailyScoreboard dailyScoreboard = new DailyScoreboard();
                    dailyScoreboard.body = new Dictionary<string, GameDetails>();

                    try
                    {
                        GameDetailsManagement gameDetails = new GameDetailsManagement();
                        gameDetails.Update(jsonObject.body);

                        return View("DailyScoreboard", jsonObject.body);
                    }
                    catch (Exception ex)
                    {
                        new Exception(ex.Message);
                    }

                    return View("DailyScoreboard");
                }
            }

            return View("DailyScoreboard");
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
            }

            if (players.Count() > 0 && players != null)
            {

                return View(players.ToPagedList(pageNumber, pageSize));
            }

            return View("GetPlayers", players.ToPagedList(pageNumber, pageSize));
        }

        public string TestJsonResult()
        {
            string myJson = @"{'codigoDeStatus': 200, 'corpoMensagem': {'20241002_ATL@SD': {'away': 'ATL', 'home': 'SD', 'teamIDAway': '2', 'teamIDHome': '23', 'gameTime': '8:38p', 'gameTime_epoch': '1727915880.0', 'lineScore': {'away': {'H': '6', 'R': '4', 'team': 'ATL', 'scoresByInning': {'1': '1', '2': '0', '3': '0', '4': '0', '5': '1', '6': '0', '7': '0', '8': '2', '9': '0'}, 'E': '0'}, 'home': {'H': '10', 'R': '5', 'team': 'SD', 'scoresByInning': {'1': '0', '2': '5', '3': '0', '4': '0', '5': '0', '6': '0', '7': '0', '8': '0', '9': 'x'}, 'E': '0'}}, 'currentInning': 'Final', 'currentCount': '', 'currentOuts': '', 'awayResult': 'L', 'homeResult': 'W', 'gameID': '20241002_ATL@SD', 'gameStatus': 'Completed', 'gameStatusCode': '2'}, '20241002_KC@BAL': {'away': 'KC', 'home': 'BAL', 'teamIDAway': '12', 'teamIDHome': '3', 'gameTime': '4:38p', 'gameTime_epoch': '1727901480.0', 'lineScore': {'away': {'H': '9', 'R': '2', 'team': 'KC', 'scoresByInning': {'1': '1', '2': '0', '3': '0', '4': '0', '5': '0', '6': '1', '7': '0', '8': '0', '9': '0'}, 'E': '1'}, 'home': {'H': '6', 'R': '1', 'team': 'BAL', 'scoresByInning': {'1': '0', '2': '0', '3': '0', '4': '0', '5': '1', '6': '0', '7': '0', '8': '0', '9': '0'}, 'E': '0'}}, 'currentInning': 'Final', 'currentCount': '', 'currentOuts': '', 'awayResult': 'W', 'homeResult': 'L', 'gameID': '20241002_KC@BAL', 'gameStatus': 'Completed', 'gameStatusCode': '2'}, '20241002_NYM@MIL': {'away': 'NYM', 'home': 'MIL', 'teamIDAway': '18', 'teamIDHome': '16', 'gameTime': '7:38p', 'gameTime_epoch': '1727912280.0', 'lineScore': {'away': {'H': '8', 'R': '3', 'team': 'NYM', 'scoresByInning': {'1': '1', '2': '2', '3': '0', '4': '0', '5': '0', '6': '0', '7': '0', '8': '0', '9': '0'}, 'E': '0'}, 'home': {'H': '11', 'R': '5', 'team': 'MIL', 'scoresByInning': {'1': '1', '2': '0', '3': '0', '4': '0', '5': '1', '6': '0', '7': '0', '8': '3', '9': 'x'}, 'E': '1'}}, 'currentInning': 'Final', 'currentCount': '', 'currentOuts': '', 'awayResult': 'L', 'homeResult': 'W', 'gameID': '20241002_NYM@MIL', 'gameStatus': 'Completed', 'gameStatusCode': '2'}, '20241002_DET@HOU': {'away': 'DET', 'home': 'HOU', 'teamIDAway': '10', 'teamIDHome': '11', 'gameTime': '2:32p', 'gameTime_epoch': '1727893920.0', 'lineScore': {'away': {'H': '7', 'R': '5', 'team': 'DET', 'scoresByInning': {'1': '0', '2': '0', '3': '0', '4': '0', '5': '0', '6': '1', '7': '0', '8': '4', '9': '0'}, 'E': '0'}, 'home': {'H': '5', 'R': '2', 'team': 'HOU', 'scoresByInning': {'1': '0', '2': '0', '3': '0', '4': '0', '5': '0', '6': '0', '7': '2', '8': '0', '9': '0'}, 'E': '1'}}, 'currentInning': 'Final', 'currentCount': '', 'currentOuts': '', 'awayResult': 'W', 'homeResult': 'L', 'gameID': '20241002_DET@HOU', 'gameStatus': 'Completed', 'gameStatusCode': '2'}}}";

            return myJson;
        }
    }
}