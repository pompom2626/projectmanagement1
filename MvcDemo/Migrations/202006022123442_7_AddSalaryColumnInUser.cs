namespace MvcDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7_AddSalaryColumnInUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Salary", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Salary");
        }
    }
}
