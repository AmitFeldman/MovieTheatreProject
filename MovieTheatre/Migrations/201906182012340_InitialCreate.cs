namespace MovieTheatre.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movie",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Genre = c.String(),
                    Description = c.String(),
                    Poster = c.String(),
                    Trailer = c.String(),
                    Year = c.String(nullable: false),
                    Director = c.String()
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.User",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Username = c.String(nullable: false),
                    Email = c.String(nullable: false),
                    Password = c.String(nullable: false),
                    isManager = c.Boolean(nullable: false)
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Rating",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    UserID = c.Int(nullable: false),
                    MovieID = c.Int(nullable: false),
                    Review = c.String(),
                    Stars = c.Int(nullable: false),
                    ReviewDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateIndex("dbo.Rating", "UserID");
            CreateIndex("dbo.Rating", "MovieID");
            AddForeignKey("dbo.Rating", "MovieID", "dbo.Movie", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Rating", "UserID", "dbo.User", "ID", cascadeDelete: true);

        }

        public override void Down()
        {
            DropTable("dbo.Movie");
            DropTable("dbo.User");
            DropTable("dbo.Rating");
            DropForeignKey("dbo.Rating", "UserID", "dbo.User");
            DropForeignKey("dbo.Rating", "MovieID", "dbo.Movie");
            DropIndex("dbo.Rating", new[] { "MovieID" });
            DropIndex("dbo.Rating", new[] { "UserID" });
        }
    }
}
