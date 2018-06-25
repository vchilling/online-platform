namespace VictoriasFood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewAndFavourites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Favourites",
                c => new
                    {
                        favouriteID = c.Int(nullable: false, identity: true),
                        recipeID = c.Int(nullable: false),
                        authorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.favouriteID)
                .ForeignKey("dbo.Authors", t => t.authorID, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.recipeID, cascadeDelete: false)
                .Index(t => t.recipeID)
                .Index(t => t.authorID);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        reviewID = c.Int(nullable: false, identity: true),
                        recipeID = c.Int(nullable: false),
                        authorID = c.Int(nullable: false),
                        reviewDateTime = c.DateTime(nullable: false),
                        reviewText = c.String(maxLength: 250),
                        reviewRate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.reviewID)
                .ForeignKey("dbo.Authors", t => t.authorID, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.recipeID, cascadeDelete: false)
                .Index(t => t.recipeID)
                .Index(t => t.authorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "recipeID", "dbo.Recipes");
            DropForeignKey("dbo.Reviews", "authorID", "dbo.Authors");
            DropForeignKey("dbo.Favourites", "recipeID", "dbo.Recipes");
            DropForeignKey("dbo.Favourites", "authorID", "dbo.Authors");
            DropIndex("dbo.Reviews", new[] { "authorID" });
            DropIndex("dbo.Reviews", new[] { "recipeID" });
            DropIndex("dbo.Favourites", new[] { "authorID" });
            DropIndex("dbo.Favourites", new[] { "recipeID" });
            DropTable("dbo.Reviews");
            DropTable("dbo.Favourites");
        }
    }
}
