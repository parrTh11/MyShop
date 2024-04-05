namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpadatedWishList : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ProductViewModels", new[] { "WishList_Id" });
            AddColumn("dbo.Products", "WishList_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Products", "WishList_Id");
            DropTable("dbo.ProductViewModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductViewModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 20),
                        Description = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Category = c.String(),
                        Image = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        WishList_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.Products", new[] { "WishList_Id" });
            DropColumn("dbo.Products", "WishList_Id");
            CreateIndex("dbo.ProductViewModels", "WishList_Id");
        }
    }
}
