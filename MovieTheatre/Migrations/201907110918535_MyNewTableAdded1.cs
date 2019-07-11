namespace MovieTheatre.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyNewTableAdded1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
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
            
            //AddColumn("dbo.User", "Email", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.User", "Email");
            DropTable("dbo.Rating");
        }
    }
}
