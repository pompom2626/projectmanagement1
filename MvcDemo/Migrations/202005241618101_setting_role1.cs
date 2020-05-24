namespace MvcDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setting_role1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Tasks", newName: "TaskHelpers");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TaskHelpers", newName: "Tasks");
        }
    }
}
