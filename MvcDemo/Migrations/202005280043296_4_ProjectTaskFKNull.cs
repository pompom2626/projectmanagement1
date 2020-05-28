namespace MvcDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4_ProjectTaskFKNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaskHelpers", "ProjectTask_Id", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaskHelpers", "ProjectTask_Id", c => c.Guid(nullable: false));
        }
    }
}
