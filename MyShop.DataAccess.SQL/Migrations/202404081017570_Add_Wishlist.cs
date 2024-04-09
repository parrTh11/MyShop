namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Wishlist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WishLists",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    ProductId = c.String(),
                    ProductName = c.String(),
                    CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.WishLists");
        }
    }
}
