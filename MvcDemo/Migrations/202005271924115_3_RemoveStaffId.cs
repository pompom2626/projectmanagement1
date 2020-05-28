namespace MvcDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3_RemoveStaffId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TaskHelpers", "StaffId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskHelpers", "StaffId", c => c.Guid(nullable: false));
        }
    }
}
