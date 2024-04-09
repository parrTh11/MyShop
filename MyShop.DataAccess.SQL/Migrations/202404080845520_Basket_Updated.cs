namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Basket_Updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Baskets", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Baskets", "UserId");
        }
    }
}
