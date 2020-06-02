namespace MvcDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6_Notification_PrjectTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "DeadlineProjectId", c => c.Guid(nullable: false));
            AddColumn("dbo.Notifications", "DeadlineTaskHelperId", c => c.Guid(nullable: false));
            AddColumn("dbo.Notifications", "Project_Id", c => c.Guid());
            AddColumn("dbo.Notifications", "TaskHelper_Id", c => c.Guid());
            CreateIndex("dbo.Notifications", "Project_Id");
            CreateIndex("dbo.Notifications", "TaskHelper_Id");
            AddForeignKey("dbo.Notifications", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.Notifications", "TaskHelper_Id", "dbo.TaskHelpers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "TaskHelper_Id", "dbo.TaskHelpers");
            DropForeignKey("dbo.Notifications", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Notifications", new[] { "TaskHelper_Id" });
            DropIndex("dbo.Notifications", new[] { "Project_Id" });
            DropColumn("dbo.Notifications", "TaskHelper_Id");
            DropColumn("dbo.Notifications", "Project_Id");
            DropColumn("dbo.Notifications", "DeadlineTaskHelperId");
            DropColumn("dbo.Notifications", "DeadlineProjectId");
        }
    }
}
