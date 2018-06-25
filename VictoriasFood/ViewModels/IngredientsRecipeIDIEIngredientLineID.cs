using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class IngredientsRecipeIDIEIngredientLineID
    {
        public Ingredients IngredientsData { get; set; }
        public int RecipeIDData { get; set; }
        public List<int> IngredientLineIDData { get; set; }
        public IngredientLine IngredientLineData { get; set; }
    }
}