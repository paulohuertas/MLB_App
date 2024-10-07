namespace MLB_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fourth : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameDetails", "lineScore_Id", "dbo.lineScore");
            DropIndex("dbo.GameDetails", new[] { "lineScore_Id" });
            AlterColumn("dbo.GameDetails", "lineScore_Id", c => c.Int());
            CreateIndex("dbo.GameDetails", "lineScore_Id");
            AddForeignKey("dbo.GameDetails", "lineScore_Id", "dbo.lineScore", "Id");
            DropTable("dbo.TeamScore");
        }
        
        public override void Down()
        {
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
            
            DropForeignKey("dbo.GameDetails", "lineScore_Id", "dbo.lineScore");
            DropIndex("dbo.GameDetails", new[] { "lineScore_Id" });
            AlterColumn("dbo.GameDetails", "lineScore_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.GameDetails", "lineScore_Id");
            AddForeignKey("dbo.GameDetails", "lineScore_Id", "dbo.lineScore", "Id", cascadeDelete: true);
        }
    }
}
