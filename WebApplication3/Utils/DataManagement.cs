using MLB_App.Models;
using MLB_App.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public interface IPlayerManagement<T> where T : class
    {
        void Save(T obj);
    }

    public class PlayerManagement : IPlayerManagement<Player>
    {
        DataContext context = new DataContext();

        public void Save(Player obj)
        {
            try
            {
                if (obj != null)
                {
                    Player player = context.Players.Where(p => p.playerID == obj.playerID).FirstOrDefault();

                    if(player == null)
                    {
                        obj.injury = new Injury();
                        context.Players.Add(obj);
                        context.SaveChanges();

                    }
                    else if(!player.Equals(obj))
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
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                new Exception(ex.Message, new ArgumentException("Player not found", "Player"));
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
            try
            {
                if (obj != null)
                {
                    context.RealTimeBoxScore.Add(obj);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, new ArgumentException("Real time box score not found", "RealTimeBoxScore"));
            }

        }

        public void Update(RealTimeBoxScore obj)
        {
            DataContext context = new DataContext();

            var game = context.RealTimeBoxScore.Where(f => f.gameID == obj.gameID).FirstOrDefault();

            if (game != null)
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
                    foreach(var k in obj.Values)
                    {
                        context.GameDetails.Add(k);
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
