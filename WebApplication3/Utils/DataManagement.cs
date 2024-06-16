using MLB_App.Models;
using MLB_App.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLB_App.Utils
{
    public interface IDataManagement<T> where T : class
    {
        void Save(List<T> entity);
    }
    public class DataManagement : IDataManagement<Schedule>
    {
        DataContext context = new DataContext();
        public void Save(List<Schedule> schedule)
        {
            if(schedule != null)
            {
                foreach(var s in schedule)
                {
                    context.Schedules.Add(s);
                    context.SaveChanges();
                }
            }
        }
    }

    public interface IPlayerManagement<T> where T : class
    {
        void Save(T obj);
    }

    public class PlayerManagement : IPlayerManagement<Player>
    {
        DataContext context = new DataContext();

        public void Save(Player obj)
        {
            if(obj != null)
            {
                context.Players.Add(obj);
                context.SaveChanges();
            }
        }
    }

    public interface IRealTimeBoxScore<T> where T : class
    {
        void Save(T obj);
        void Update(T obj);
    }

    public class RealTimeBoxScoreManagement : IRealTimeBoxScore<RealTimeBoxScore>
    {
        DataContext context = new DataContext();
        public void Save(RealTimeBoxScore obj)
        {
            if(obj != null)
            {
                context.RealTimeBoxScore.Add(obj);
                context.SaveChanges();
            }
        }

        public void Update(RealTimeBoxScore obj)
        {
            DataContext context = new DataContext();

            var game = context.RealTimeBoxScore.Find(obj.Id);

            if(game != null)
            {
                try
                {
                    context.Entry(game).CurrentValues.SetValues(obj);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                context.RealTimeBoxScore.Add(obj);
                context.SaveChanges();
            }
        }
    }


}