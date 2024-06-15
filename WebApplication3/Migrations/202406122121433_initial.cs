namespace MLB_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        playerID = c.String(nullable: false, maxLength: 128),
                        espnID = c.String(),
                        sleeperBotID = c.String(),
                        fantasyProsPlayerID = c.String(),
                        highSchool = c.String(),
                        _throw = c.String(),
                        weight = c.String(),
                        jerseyNum = c.String(),
                        team = c.String(),
                        mlbHeadshot = c.String(),
                        yahooPlayerID = c.String(),
                        espnLink = c.String(),
                        yahooLink = c.String(),
                        bDay = c.String(),
                        mlbLink = c.String(),
                        teamAbv = c.String(),
                        espnHeadshot = c.String(),
                        rotoWirePlayerIDFull = c.String(),
                        injury_description = c.String(),
                        injury_injDate = c.String(),
                        injury_designation = c.String(),
                        teamID = c.String(),
                        pos = c.String(),
                        mlbIDFull = c.String(),
                        cbsPlayerID = c.String(),
                        longName = c.String(),
                        bat = c.String(),
                        rotoWirePlayerID = c.String(),
                        height = c.String(),
                        lastGamePlayed = c.String(),
                        mlbID = c.String(),
                        fantasyProsLink = c.String(),
                    })
                .PrimaryKey(t => t.playerID);
            
            CreateTable(
                "dbo.ProbableStartingPitchers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Away = c.String(),
                        Home = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Schedule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        gameID = c.String(),
                        gameType = c.String(),
                        away = c.String(),
                        gameTime = c.String(),
                        gameDate = c.String(),
                        teamIDHome = c.String(),
                        gameTime_epoch = c.String(),
                        teamIDAway = c.String(),
                        home = c.String(),
                        ProbableStartingPitchers_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProbableStartingPitchers", t => t.ProbableStartingPitchers_Id, cascadeDelete: true)
                .Index(t => t.ProbableStartingPitchers_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedule", "ProbableStartingPitchers_Id", "dbo.ProbableStartingPitchers");
            DropIndex("dbo.Schedule", new[] { "ProbableStartingPitchers_Id" });
            DropTable("dbo.Schedule");
            DropTable("dbo.ProbableStartingPitchers");
            DropTable("dbo.Player");
        }
    }
}
