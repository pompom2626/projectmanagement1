namespace MvcDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8_BudgetDBcreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Budgets",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProjectId = c.Guid(nullable: false),
                        CreatorId = c.String(),
                        days = c.Double(nullable: false),
                        FinishDays = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Budgets");
        }
    }
}
