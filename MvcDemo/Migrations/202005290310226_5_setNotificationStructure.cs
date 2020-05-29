namespace MvcDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5_setNotificationStructure : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "ApplicationUser_Id", c => c.Guid(nullable: false));
            AddColumn("dbo.Notifications", "IsBeyondDeadline", c => c.Boolean(nullable: false));
            AddColumn("dbo.Notifications", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            CreateIndex("dbo.Notifications", "ApplicationUser_Id1");
            AddForeignKey("dbo.Notifications", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Notifications", "StaffId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "StaffId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Notifications", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "ApplicationUser_Id1" });
            DropColumn("dbo.Notifications", "ApplicationUser_Id1");
            DropColumn("dbo.Notifications", "IsBeyondDeadline");
            DropColumn("dbo.Notifications", "ApplicationUser_Id");
        }
    }
}
