namespace MvcDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8_2BudgetDBcreation : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Budgets");
            AlterColumn("dbo.Budgets", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Budgets", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Budgets");
            AlterColumn("dbo.Budgets", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Budgets", "Id");
        }
    }
}
