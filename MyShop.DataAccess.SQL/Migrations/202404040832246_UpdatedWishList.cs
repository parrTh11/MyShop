namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedWishList : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WishLists", t => t.WishList_Id)
                .Index(t => t.WishList_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductViewModels", "WishList_Id", "dbo.WishLists");
            DropIndex("dbo.ProductViewModels", new[] { "WishList_Id" });
            DropTable("dbo.ProductViewModels");
        }
    }
}
