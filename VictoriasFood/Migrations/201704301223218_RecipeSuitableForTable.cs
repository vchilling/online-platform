namespace VictoriasFood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecipeSuitableForTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RecipeSuitableFors",
                c => new
                    {
                        recipeSuitableForID = c.Int(nullable: false, identity: true),
                        recipeID = c.Int(nullable: false),
                        suitableForID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.recipeSuitableForID)
                .ForeignKey("dbo.Recipes", t => t.recipeID, cascadeDelete: true)
                .ForeignKey("dbo.SuitableFors", t => t.suitableForID, cascadeDelete: true)
                .Index(t => t.recipeID)
                .Index(t => t.suitableForID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecipeSuitableFors", "suitableForID", "dbo.SuitableFors");
            DropForeignKey("dbo.RecipeSuitableFors", "recipeID", "dbo.Recipes");
            DropIndex("dbo.RecipeSuitableFors", new[] { "suitableForID" });
            DropIndex("dbo.RecipeSuitableFors", new[] { "recipeID" });
            DropTable("dbo.RecipeSuitableFors");
        }
    }
}
