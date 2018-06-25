namespace VictoriasFood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuitableForTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SuitableFors",
                c => new
                    {
                        suitableForID = c.Int(nullable: false, identity: true),
                        suitableForTitle = c.String(nullable: false, maxLength: 50),
                        suitableForDescription = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.suitableForID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SuitableFors");
        }
    }
}
