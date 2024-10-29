using MLB_App.Models;
using MLB_App.Models.Data;
using MLB_App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MLB_App.Utils
{
    
    public class DataManagement : IDataService<Schedule>
    {
        DataContext context = new DataContext();

        public void Save(List<Schedule> schedule)
        {
            if (schedule != null)
            {
                foreach (var s in schedule)
                {
                    context.Schedules.Add(s);
                    context.SaveChanges();
                }
            }
        }
    }

    public class PlayerManagement : IPlayerManagement
    {
        DataContext context = new DataContext();

        public IEnumerable<Player> GetAllPlayers()
        {
            return context.Players.ToList();
        }

        public void Save(Player obj)
        {
            try
            {
                if (obj != null)
                {
                    obj.injury = new Injury();
                    context.Players.Add(obj);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                new Exception(ex.Message, new ArgumentException("Player not found", "Player"));
            }
        }

        public void Update(Player obj)
        {
            if(obj != null)
            {
                Player player = context.Players.Where(p => p.playerID == obj.playerID).FirstOrDefault();
                
                if(player != null)
                {

                    player.espnID = obj.espnID;
                    player.sleeperBotID = obj.sleeperBotID;
                    player.fantasyProsPlayerID = obj.fantasyProsPlayerID;
                    player.highSchool = obj.highSchool;
                    player._throw = obj._throw;
                    player.weight = obj.weight;
                    player.jerseyNum = obj.jerseyNum;
                    player.team = obj.team;
                    player.mlbHeadshot = obj.mlbHeadshot;
                    player.yahooPlayerID = obj.yahooPlayerID;
                    player.espnLink = obj.espnLink;
                    player.yahooLink = obj.yahooLink;
                    player.bDay = obj.bDay;
                    player.mlbLink = obj.mlbLink;
                    player.teamAbv = obj.teamAbv;
                    player.espnHeadshot = obj.espnHeadshot;
                    player.rotoWirePlayerIDFull = obj.rotoWirePlayerIDFull;
                    player.injury = obj.injury;
                    player.teamID = obj.teamID;
                    player.pos = obj.pos;
                    player.mlbIDFull = obj.mlbIDFull;
                    player.cbsPlayerID = obj.cbsPlayerID;
                    player.longName = obj.longName;
                    player.bat = obj.bat;
                    player.rotoWirePlayerID = obj.rotoWirePlayerID;
                    player.height = obj.height;
                    player.lastGamePlayed = obj.lastGamePlayed;
                    player.mlbID = obj.mlbID;
                    player.playerID = obj.playerID;
                    player.fantasyProsLink = obj.fantasyProsLink;
                    player.amendedDateTime = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }
    }

    public class RealTimeScore : ServiceBase<RealTimeBoxScore>
    {
        public RealTimeScore(DataContext context) : base(context) { }

        public void Update(RealTimeBoxScore game, RealTimeBoxScore obj)
        {
            if (game != null && obj != null)
            {
                try
                {
                    game.away = obj.away;
                    game.Attendance = obj.Attendance;
                    game.awayResult = obj.awayResult;
                    game.gameID = obj.gameID;
                    game.GameLength = obj.GameLength;
                    game.gameStatus = obj.gameStatus;
                    game.home = obj.home;
                    game.homeResult = obj.homeResult;
                    game.lineScore = obj.lineScore;
                    game.Venue = obj.Venue;

                    Update(game);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }

    public interface IGameDetails<T> where T : class
    {
        void Save(Dictionary<string, T> obj);
        void Update(Dictionary<string, T> obj);
    }

    public class GameDetailsManagement : IGameDetails<GameDetails>
    {
        DataContext context = new DataContext();

        public void Save(Dictionary<string, GameDetails> obj)
        {
            try
            {
                if (obj != null)
                {
                    foreach(var game in obj.Values)
                    {
                        context.GameDetails.Add(game);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, new ArgumentException("Saving Game Details", ex.InnerException));
            }

        }

        public void Update(Dictionary<string, GameDetails> game)
        {
            DataContext context = new DataContext();
            foreach (var obj in game.Values)
            {
                var gameData = context.GameDetails.Where(f => f.gameID == obj.gameID).FirstOrDefault();

                if (gameData != null)
                {
                    try
                    {
                        gameData.away = obj.away;
                        gameData.home = obj.home;
                        gameData.teamIDAway = obj.teamIDAway;
                        gameData.teamIDHome = obj.teamIDHome;
                        gameData.gameTime = obj.gameTime;
                        gameData.gameTime_epoch = obj.gameTime_epoch;
                        gameData.currentInning = obj.currentInning;
                        gameData.currentCount = obj.currentCount;
                        gameData.currentOuts = obj.currentOuts;
                        gameData.awayResult = obj.awayResult;
                        gameData.homeResult = obj.homeResult;
                        gameData.gameID = obj.gameID;
                        gameData.gameStatus = obj.gameStatus;
                        gameData.gameStatusCode = obj.gameStatusCode;
                        gameData.lineScore = obj.lineScore;
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    context.GameDetails.Add(obj);
                    context.SaveChanges();
                }
            }

        }
    }
}
