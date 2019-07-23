namespace MovieTheatre.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPosterAndTrailer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movie", "Poster", c => c.String());
            AddColumn("dbo.Movie", "Trailer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movie", "Trailer");
            DropColumn("dbo.Movie", "Poster");
        }
    }
}
