namespace MvcDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5_NotificationDB_changeIndexName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "ApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Notifications", "IsBeyondDeadline", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Notifications", "ApplicationUserId");
            AddForeignKey("dbo.Notifications", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Notifications", "StaffId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "StaffId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Notifications", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "ApplicationUserId" });
            DropColumn("dbo.Notifications", "IsBeyondDeadline");
            DropColumn("dbo.Notifications", "ApplicationUserId");
        }
    }
}
