namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wishListUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WishLists", "ProductId", c => c.String());
            AddColumn("dbo.WishLists", "ProductName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WishLists", "ProductName");
            DropColumn("dbo.WishLists", "ProductId");
        }
    }
}
