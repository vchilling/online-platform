namespace VictoriasFood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IngedientsTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IngredientLines",
                c => new
                    {
                        ingredientLineID = c.Int(nullable: false, identity: true),
                        itemTitle = c.String(nullable: false, maxLength: 50),
                        itemDescription = c.String(maxLength: 250),
                        measurementMetricSystem = c.String(maxLength: 50),
                        measurementImperialSystem = c.String(maxLength: 50),
                        unitConverter = c.Double(nullable: false),
                        baseQuantity = c.Double(nullable: false),
                        calculatedQuantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ingredientLineID);
            
            CreateTable(
                "dbo.IngredientsIngredientLines",
                c => new
                    {
                        ingredientsIngredientLineID = c.Int(nullable: false, identity: true),
                        ingredientsID = c.Int(nullable: false),
                        ingredientLineID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ingredientsIngredientLineID)
                .ForeignKey("dbo.IngredientLines", t => t.ingredientLineID, cascadeDelete: true)
                .ForeignKey("dbo.Ingredients", t => t.ingredientsID, cascadeDelete: true)
                .Index(t => t.ingredientsID)
                .Index(t => t.ingredientLineID);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        ingredientsID = c.Int(nullable: false, identity: true),
                        ingredientsCategoryTitle = c.String(maxLength: 50),
                        ingredientsDescription = c.String(),
                    })
                .PrimaryKey(t => t.ingredientsID);
            
            CreateTable(
                "dbo.RecipeIngredients",
                c => new
                    {
                        recipeIngredientsID = c.Int(nullable: false, identity: true),
                        recipeID = c.Int(nullable: false),
                        ingredientsID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.recipeIngredientsID)
                .ForeignKey("dbo.Ingredients", t => t.ingredientsID, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.recipeID, cascadeDelete: true)
                .Index(t => t.recipeID)
                .Index(t => t.ingredientsID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecipeIngredients", "recipeID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeIngredients", "ingredientsID", "dbo.Ingredients");
            DropForeignKey("dbo.IngredientsIngredientLines", "ingredientsID", "dbo.Ingredients");
            DropForeignKey("dbo.IngredientsIngredientLines", "ingredientLineID", "dbo.IngredientLines");
            DropIndex("dbo.RecipeIngredients", new[] { "ingredientsID" });
            DropIndex("dbo.RecipeIngredients", new[] { "recipeID" });
            DropIndex("dbo.IngredientsIngredientLines", new[] { "ingredientLineID" });
            DropIndex("dbo.IngredientsIngredientLines", new[] { "ingredientsID" });
            DropTable("dbo.RecipeIngredients");
            DropTable("dbo.Ingredients");
            DropTable("dbo.IngredientsIngredientLines");
            DropTable("dbo.IngredientLines");
        }
    }
}
