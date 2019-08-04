namespace MovieTheatre.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbkeys : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Username", c => c.String(nullable: false));
            AlterColumn("dbo.Rating", "Stars", c => c.Int(nullable: false));
            AlterColumn("dbo.User", "Email", c => c.String(nullable: false));
            CreateIndex("dbo.Rating", "UserID");
            CreateIndex("dbo.Rating", "MovieID");
            AddForeignKey("dbo.Rating", "MovieID", "dbo.Movie", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Rating", "UserID", "dbo.User", "ID", cascadeDelete: true);
            DropColumn("dbo.User", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Name", c => c.String(nullable: false));
            DropForeignKey("dbo.Rating", "UserID", "dbo.User");
            DropForeignKey("dbo.Rating", "MovieID", "dbo.Movie");
            DropIndex("dbo.Rating", new[] { "MovieID" });
            DropIndex("dbo.Rating", new[] { "UserID" });
            AlterColumn("dbo.User", "Email", c => c.String());
            AlterColumn("dbo.Rating", "Stars", c => c.Double(nullable: false));
            DropColumn("dbo.User", "Username");
        }
    }
}
