using MLB_App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLB_App.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        public JsonResult GetTeamLogo(string logo)
        {
            string teamLogo = StaticData.GetTeamLogoByNameAbbreviation(logo);
            return Json(teamLogo);
        }
    }
}