using MLB_App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLB_App.Models
{
    public class LiveResult : Root
    {
        public RealTimeBoxScore body;
    }

    public class RealTimeBoxScore : StaticData
    {
        public string GameLength;
        public string gameStatus;
        public string Attendance;
        public string Venue;
        public lineScore lineScore;
        public string gameID;
        public string homeResult;
        public string home;
        public string awayResult;
        public string away;

    }

    public class lineScore
    {
        public away away;
        public home home;
    }

    public class away
    {
        public string H;
        public string R;
        public string E;
    }

    public class home
    {
        public string H;
        public string R;
        public string E;
    }
}