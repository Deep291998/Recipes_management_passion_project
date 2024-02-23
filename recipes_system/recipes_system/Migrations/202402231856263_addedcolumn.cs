namespace recipes_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedcolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Instructions", "InstructionName", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Instructions", "InstructionName");
        }
    }
}
