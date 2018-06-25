namespace VictoriasFood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthorAndRecipeTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        authorID = c.Int(nullable: false, identity: true),
                        authorFirstName = c.String(nullable: false, maxLength: 50),
                        authorSecondName = c.String(nullable: false, maxLength: 50),
                        authorLastName = c.String(nullable: false, maxLength: 50),
                        addressLine1 = c.String(nullable: false, maxLength: 50),
                        addressLine2 = c.String(nullable: false, maxLength: 50),
                        telephoneNumber = c.String(nullable: false, maxLength: 50),
                        authorCv = c.String(maxLength: 1024),
                        facebook = c.String(maxLength: 50),
                        twitter = c.String(maxLength: 50),
                        instagram = c.String(maxLength: 50),
                        website = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.authorID);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        recipeID = c.Int(nullable: false, identity: true),
                        recipeTitle = c.String(nullable: false, maxLength: 50),
                        recipeDescription = c.String(nullable: false, maxLength: 250),
                        recipeNumberOfServings = c.Int(nullable: false),
                        recipeReadyIn = c.DateTime(nullable: false),
                        recipeImage = c.String(nullable: false, maxLength: 250),
                        authorID = c.Int(nullable: false),
                        subcategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.recipeID)
                .ForeignKey("dbo.Authors", t => t.authorID, cascadeDelete: true)
                .ForeignKey("dbo.Subcategories", t => t.subcategoryID, cascadeDelete: true)
                .Index(t => t.authorID)
                .Index(t => t.subcategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recipes", "subcategoryID", "dbo.Subcategories");
            DropForeignKey("dbo.Recipes", "authorID", "dbo.Authors");
            DropIndex("dbo.Recipes", new[] { "subcategoryID" });
            DropIndex("dbo.Recipes", new[] { "authorID" });
            DropTable("dbo.Recipes");
            DropTable("dbo.Authors");
        }
    }
}
