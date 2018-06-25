using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class HomeNavigationViewModel
    {
        public IEnumerable<Recipe> RecipeData { get; set; }
        public IEnumerable<Subcategory> SubcategoryData { get; set; }
        public IEnumerable<Category> CategoryData { get; set; }
        public IEnumerable<SuitableFor> SuitableForData { get; set; }
    }
}