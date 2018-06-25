namespace VictoriasFood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DirectionDirectionLineAndTip : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DirectionLines",
                c => new
                    {
                        directionLineID = c.Int(nullable: false, identity: true),
                        directionLineText = c.String(nullable: false, maxLength: 50),
                        directionLineDescription = c.String(maxLength: 1024),
                        directionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.directionLineID)
                .ForeignKey("dbo.Directions", t => t.directionID, cascadeDelete: true)
                .Index(t => t.directionID);
            
            CreateTable(
                "dbo.Directions",
                c => new
                    {
                        directionID = c.Int(nullable: false, identity: true),
                        directionTitle = c.String(maxLength: 50),
                        directionDescription = c.String(maxLength: 1024),
                        recipeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.directionID)
                .ForeignKey("dbo.Recipes", t => t.recipeID, cascadeDelete: true)
                .Index(t => t.recipeID);
            
            CreateTable(
                "dbo.Tips",
                c => new
                    {
                        tipID = c.Int(nullable: false, identity: true),
                        tipTitle = c.String(maxLength: 50),
                        tipDescription = c.String(maxLength: 1024),
                        recipeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.tipID)
                .ForeignKey("dbo.Recipes", t => t.recipeID, cascadeDelete: true)
                .Index(t => t.recipeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tips", "recipeID", "dbo.Recipes");
            DropForeignKey("dbo.DirectionLines", "directionID", "dbo.Directions");
            DropForeignKey("dbo.Directions", "recipeID", "dbo.Recipes");
            DropIndex("dbo.Tips", new[] { "recipeID" });
            DropIndex("dbo.Directions", new[] { "recipeID" });
            DropIndex("dbo.DirectionLines", new[] { "directionID" });
            DropTable("dbo.Tips");
            DropTable("dbo.Directions");
            DropTable("dbo.DirectionLines");
        }
    }
}
