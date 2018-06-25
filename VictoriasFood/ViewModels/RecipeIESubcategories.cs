using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class RecipeIESubcategories
    {
        public Recipe RecipeData { get; set; }
        public IEnumerable<Subcategory> SubcategoryData { get; set; }
    }
}
