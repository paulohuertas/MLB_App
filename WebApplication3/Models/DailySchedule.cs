using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MLB_App.Models.Interfaces;

namespace MLB_App.Models
{
    public class DailySchedule : Root, IRoot<Schedule>
    {
        private List<Schedule> Body;
        public List<Schedule> body
        {
            get
            {
                return Body;
            }
            set
            {
                this.Body = value;
            }
        }
    }
}