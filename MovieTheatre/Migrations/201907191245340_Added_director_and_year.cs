namespace MovieTheatre.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_director_and_year : DbMigration
    {
        public override void Up()
        {
           /* AddColumn("dbo.Movie", "Year", c => c.String(nullable: false));
            AddColumn("dbo.Movie", "Director", c => c.String());*/
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movie", "Director");
            DropColumn("dbo.Movie", "Year");
        }
    }
}
