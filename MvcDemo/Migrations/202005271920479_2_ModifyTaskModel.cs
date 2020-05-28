namespace MvcDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2_ModifyTaskModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaskHelpers", "Content", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaskHelpers", "Content", c => c.String(nullable: false, maxLength: 500));
        }
    }
}
