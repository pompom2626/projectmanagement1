namespace MvcDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3_userToTaskHelper_PjojectToTaskHelper_Reslation : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TaskHelpers", name: "ProjectId", newName: "Project_Id");
            RenameIndex(table: "dbo.TaskHelpers", name: "IX_ProjectId", newName: "IX_Project_Id");
            CreateTable(
                "dbo.UserTasks",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        TaskHelper_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.TaskHelper_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskHelpers", t => t.TaskHelper_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.TaskHelper_Id);
            
            AddColumn("dbo.AspNetUsers", "TaskHelper_Id", c => c.Guid(nullable: false));
            AddColumn("dbo.TaskHelpers", "ApplicationUser_Id", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTasks", "TaskHelper_Id", "dbo.TaskHelpers");
            DropForeignKey("dbo.UserTasks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserTasks", new[] { "TaskHelper_Id" });
            DropIndex("dbo.UserTasks", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.TaskHelpers", "ApplicationUser_Id");
            DropColumn("dbo.AspNetUsers", "TaskHelper_Id");
            DropTable("dbo.UserTasks");
            RenameIndex(table: "dbo.TaskHelpers", name: "IX_Project_Id", newName: "IX_ProjectId");
            RenameColumn(table: "dbo.TaskHelpers", name: "Project_Id", newName: "ProjectId");
        }
    }
}
