namespace MovieTheatre.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SamuelPullMigration : DbMigration
    {
        public override void Up()
        {
            /*CreateTable(
                "dbo.Rating",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        MovieID = c.Int(nullable: false),
                        Review = c.String(),
                        Stars = c.Double(nullable: false),
                        ReviewDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            /*AddColumn("dbo.Movie", "Year", c => c.String());
            AddColumn("dbo.Movie", "Director", c => c.String());
            AddColumn("dbo.Movie", "Poster", c => c.String());
            AddColumn("dbo.Movie", "Trailer", c => c.String());*/
        }
        
        public override void Down()
        {
            /*DropColumn("dbo.Movie", "Trailer");
            DropColumn("dbo.Movie", "Poster");
            DropColumn("dbo.Movie", "Director");
            DropColumn("dbo.Movie", "Year");
            DropTable("dbo.User");
            DropTable("dbo.Rating");*/
        }
    }
}
