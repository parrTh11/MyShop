namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Description", c => c.String());
            DropColumn("dbo.Products", "Discription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Discription", c => c.String());
            DropColumn("dbo.Products", "Description");
        }
    }
}
