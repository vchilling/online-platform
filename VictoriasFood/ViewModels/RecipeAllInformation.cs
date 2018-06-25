using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class RecipeAllInformation
    {
        public Recipe RecipeData { get; set; }
        public Author AuthorData { get; set; }
        public Subcategory SubcategoryData { get; set; }
        public IEnumerable<Subcategory> SubcategoriesData { get; set; }
        public Category CategoryData { get; set; }
        public IEnumerable<SuitableFor> SuitableForData { get; set; }
        public IEnumerable<Ingredients> IngredientsData { get; set; }
        public IEnumerable<IngredientLine> IngredientLineData { get; set; }
        public IEnumerable<IngredientsIngredientLine> IngredientsIngredientLineData { get; set; }
        public Direction DirectionData { get; set; }
        public IEnumerable<DirectionLine> DirectionLineData { get; set; }
        public Tip TipData { get; set; }
        public IEnumerable<Review> ReviewData { get; set; }
        public IEnumerable<Author> ReviewAuthorData { get; set; }
        public Favourite FavouriteData { get; set; }

    }
}