using Newtonsoft.Json;
using RestSharp;
using System;
using System.Globalization;
using System.Web.Mvc;
using MLB_App.Models;
using MLB_App.Utils;


namespace MLB_App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "MLB through API";
            return View();
        }
        
        [HttpPost]
        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult GetDailySchedule(string gameDate)
        {
            ApiCall call = new ApiCall();

            if (String.IsNullOrEmpty(gameDate)) gameDate = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            gameDate = gameDate.Replace("-", "");
           
            string url = $"https://tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com/getMLBGamesForDate?gameDate={gameDate}";

            string respJson = call.PrepareApiCall(url, "GET");

            var jsonObject = JsonConvert.DeserializeObject<DailySchedule>(respJson);

            ViewData["gameDate"] = DateParser.ParseDateToString(gameDate);

            return View("Schedule", jsonObject);
        }

        public ActionResult PlayerInformation(string id)
        {
            ApiCall call = new ApiCall();

            string url = $"https://tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com/getMLBPlayerInfo?playerID={id}&getStats=false";

            string respJson = call.PrepareApiCall(url, "GET");

            var jsonObject = JsonConvert.DeserializeObject<PlayerDetail>(respJson);

            return View("PlayerDetail", jsonObject);
        }

        [HttpGet]
        public ActionResult GameResult(string gameId)
        {
            ApiCall call = new ApiCall();

            string url = $"https://tank01-mlb-live-in-game-real-time-statistics.p.rapidapi.com/getMLBBoxScore?gameID={gameId}&startingLineups=false";

            string respJson = call.PrepareApiCall(url, "GET");

            var jsonObject = JsonConvert.DeserializeObject<LiveResult>(respJson);

            return View("LiveResult", jsonObject);
        }
    }
}