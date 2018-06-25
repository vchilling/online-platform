using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class SuitableForIERecipe
    {
        public SuitableFor SuitableForData { get; set; }
        public IEnumerable<RecipeSuitableFor> RecipeSuitableForData { get; set; }
        public IEnumerable<Recipe> RecipeData { get; set; }
    }
}