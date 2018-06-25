using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class IngredientLineIngredientsIDRecipeID
    {
        public IngredientLine IngredientLineData { get; set; }
        public int IngredientsIDData { get; set; }
        public int RecipeIDData { get; set; }

    }
}