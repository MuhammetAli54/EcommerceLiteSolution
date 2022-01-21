namespace EcommerceLiteDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoyTableUpdated : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Categories", new[] { "BaseCategoryId" });
            AlterColumn("dbo.Categories", "BaseCategoryId", c => c.Int());
            CreateIndex("dbo.Categories", "BaseCategoryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Categories", new[] { "BaseCategoryId" });
            AlterColumn("dbo.Categories", "BaseCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Categories", "BaseCategoryId");
        }
    }
}
