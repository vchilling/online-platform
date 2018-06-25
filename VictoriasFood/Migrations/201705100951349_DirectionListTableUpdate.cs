namespace VictoriasFood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DirectionListTableUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DirectionLines", "directionLineText", c => c.String(nullable: false, maxLength: 1024));
            AlterColumn("dbo.DirectionLines", "directionLineDescription", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DirectionLines", "directionLineDescription", c => c.String(maxLength: 250));
            AlterColumn("dbo.DirectionLines", "directionLineText", c => c.String(nullable: false, maxLength: 1024));
        }
    }
}
