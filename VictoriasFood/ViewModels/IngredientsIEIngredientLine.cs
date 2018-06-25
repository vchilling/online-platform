using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class IngredientsIEIngredientLine
    {
        public RecipeIngredients RecipeIngredientsData { get; set; }
        public Ingredients IngredientsData { get; set; }
        public IEnumerable<IngredientsIngredientLine> IngredientsIngredientLineData { get; set; }
        public IEnumerable<IngredientLine> IngredientLineData { get; set; }
    }
}