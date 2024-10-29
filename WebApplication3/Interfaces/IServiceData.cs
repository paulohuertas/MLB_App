using MLB_App.Models;
using MLB_App.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLB_App.Interfaces
{
    //initial class
    public interface IServiceData<T> where T : class
    {
        void Save(T obj);
        void Update(T obj);
    }

    //Paulo added for learning/testing purposes.
    public interface IBaseModel<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }

    public class ServiceBase<T> : IBaseModel<T> where T : class
    {
        private readonly DataContext dataContext;

        public ServiceBase(DataContext context)
        {
            dataContext = context;
        }

        public void Add(T obj)
        {
            if (obj != null)
            {
                dataContext.Set<T>().Add(obj);
                dataContext.SaveChanges();
            }
        }

        public void Delete(T obj)
        {
            if (obj != null)
            {
                dataContext.Set<T>().Remove(obj);
                dataContext.SaveChanges();
            }
        }

        public void Update(T obj)
        {
            if (obj != null)
            {
                dataContext.Entry<T>(obj).State = EntityState.Modified;
                dataContext.SaveChanges();
            }
        }
    }

    //Repositories

    public class ScheduleRepository : IScheduleBaseRepository<Schedule>
    {
        DataContext dataContext = new DataContext();

        public ScheduleRepository() { }

        public IList<Schedule> GetAllScheduledGames(string game)
        {
            IList<Schedule> schedules = dataContext.Set<Schedule>().Include("ProbableStartingPitchers").Where(s => s.gameDate == game).OrderBy(t => t.gameTime).ToList();
            return schedules;
        }
    }

    public interface IScheduleBaseRepository<T> where T : class
    {
        IList<T> GetAllScheduledGames(string gameDate);
    }

    public class RealTimeBoxScoreRepository : IRealTimeBoxScoreRepository<RealTimeBoxScore>
    {
        private readonly DataContext _context;
        public RealTimeBoxScoreRepository(DataContext context)
        {
            _context = context;
        }

        public RealTimeBoxScore GetGameRealTimeBoxScore(string gameId)
        {
            RealTimeBoxScore score =_context.RealTimeBoxScore.
                            Include("lineScore").
                            Include("lineScore.away").
                            Include("lineScore.home").
                            Where(g => g.gameID == gameId).FirstOrDefault();

            if(score != null)
            {
                return score;
            }

            return null;
        }
    }

    public interface IRealTimeBoxScoreRepository<T> where T : class
    {
        T GetGameRealTimeBoxScore(string gameId);
    }

    public class GameDetailDailyScoreBoardRepository : IGameDetailDailyScoreBoardRepository<GameDetails>
    {
        private readonly DataContext _context;

        public GameDetailDailyScoreBoardRepository(DataContext context)
        {
            _context = context;
        }

        public IList<GameDetails> GetGameDetailsScoreBoard(string gameDate)
        {
            IList<GameDetails> gameDetailsList = _context.GameDetails.
                            Include("lineScore").
                            Include("lineScore.away").
                            Include("lineScore.home").
                            Where(s => s.gameID.Contains(gameDate)).
                            OrderBy(t => t.gameTime).ToList();

            return gameDetailsList;
        }
    }

    public interface IGameDetailDailyScoreBoardRepository<T> where T : class
    {
        IList<T> GetGameDetailsScoreBoard(string gameDate);
    }
}


