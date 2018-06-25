using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class SubcategoryIECategories
    {
        public Subcategory SubcategoryData { get; set; }
        public IEnumerable<Category> CategoriesData { get; set; }
    }
}