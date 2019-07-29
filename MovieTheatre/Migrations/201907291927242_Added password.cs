namespace MovieTheatre.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedpassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Password", c => c.String(nullable: false));
            AlterColumn("dbo.Movie", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.User", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "Name", c => c.String());
            AlterColumn("dbo.Movie", "Name", c => c.String());
            DropColumn("dbo.User", "Password");
        }
    }
}
