namespace VictoriasFood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAuthor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "email", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "email");
        }
    }
}
