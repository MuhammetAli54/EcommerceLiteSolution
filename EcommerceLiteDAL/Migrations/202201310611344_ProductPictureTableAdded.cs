namespace EcommerceLiteDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductPictureTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductPictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ProductPicture1 = c.String(maxLength: 400),
                        ProductPicture2 = c.String(maxLength: 400),
                        ProductPicture3 = c.String(maxLength: 400),
                        ProductPicture4 = c.String(maxLength: 400),
                        ProductPicture5 = c.String(maxLength: 400),
                        RegisterDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductPictures", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductPictures", new[] { "ProductId" });
            DropTable("dbo.ProductPictures");
        }
    }
}
