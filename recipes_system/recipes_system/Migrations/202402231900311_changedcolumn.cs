namespace recipes_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedcolumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Instructions", "InstructionName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Instructions", "InstructionName", c => c.Int(nullable: false));
        }
    }
}
