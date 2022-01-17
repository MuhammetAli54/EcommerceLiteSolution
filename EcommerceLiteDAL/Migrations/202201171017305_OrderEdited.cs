namespace EcommerceLiteDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderEdited : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Orders", new[] { "Order_Id" });
            DropColumn("dbo.Orders", "Order_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Order_Id", c => c.Int());
            CreateIndex("dbo.Orders", "Order_Id");
            AddForeignKey("dbo.Orders", "Order_Id", "dbo.Orders", "Id");
        }
    }
}
