namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updated_Order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "SureName", c => c.String());
            DropColumn("dbo.Orders", "SurName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "SurName", c => c.String());
            DropColumn("dbo.Orders", "SureName");
        }
    }
}
