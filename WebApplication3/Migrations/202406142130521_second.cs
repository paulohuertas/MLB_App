namespace MLB_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RealTimeBoxScore",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameLength = c.String(),
                        gameStatus = c.String(),
                        Attendance = c.String(),
                        Venue = c.String(),
                        gameID = c.String(),
                        homeResult = c.String(),
                        home = c.String(),
                        awayResult = c.String(),
                        away = c.String(),
                        lineScore_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.lineScore", t => t.lineScore_Id, cascadeDelete: true)
                .Index(t => t.lineScore_Id);
            
            CreateTable(
                "dbo.lineScore",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        away_Id = c.Int(nullable: false),
                        home_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.away", t => t.away_Id, cascadeDelete: true)
                .ForeignKey("dbo.home", t => t.home_Id, cascadeDelete: true)
                .Index(t => t.away_Id)
                .Index(t => t.home_Id);
            
            CreateTable(
                "dbo.away",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        H = c.String(),
                        R = c.String(),
                        E = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.home",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        H = c.String(),
                        R = c.String(),
                        E = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RealTimeBoxScore", "lineScore_Id", "dbo.lineScore");
            DropForeignKey("dbo.lineScore", "home_Id", "dbo.home");
            DropForeignKey("dbo.lineScore", "away_Id", "dbo.away");
            DropIndex("dbo.lineScore", new[] { "home_Id" });
            DropIndex("dbo.lineScore", new[] { "away_Id" });
            DropIndex("dbo.RealTimeBoxScore", new[] { "lineScore_Id" });
            DropTable("dbo.home");
            DropTable("dbo.away");
            DropTable("dbo.lineScore");
            DropTable("dbo.RealTimeBoxScore");
        }
    }
}
