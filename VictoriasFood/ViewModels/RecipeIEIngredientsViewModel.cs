using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class RecipeIEIngredientsViewModel
    {
        public IEnumerable<Ingredients> IngredientsData { get; set; }
        public int RecipeID { get; set; }
    }
}
