namespace MLB_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class third : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        away = c.String(),
                        home = c.String(),
                        teamIDAway = c.String(),
                        teamIDHome = c.String(),
                        gameTime = c.String(),
                        gameTime_epoch = c.String(),
                        currentInning = c.String(),
                        currentCount = c.String(),
                        currentOuts = c.String(),
                        awayResult = c.String(),
                        homeResult = c.String(),
                        gameID = c.String(),
                        gameStatus = c.String(),
                        gameStatusCode = c.String(),
                        lineScore_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.lineScore", t => t.lineScore_Id, cascadeDelete: true)
                .Index(t => t.lineScore_Id);
            
            CreateTable(
                "dbo.TeamScore",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        H = c.String(),
                        R = c.String(),
                        team = c.String(),
                        E = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameDetails", "lineScore_Id", "dbo.lineScore");
            DropIndex("dbo.GameDetails", new[] { "lineScore_Id" });
            DropTable("dbo.TeamScore");
            DropTable("dbo.GameDetails");
        }
    }
}
