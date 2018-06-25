using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class CategoryIESubcategory
    {
        public Category CategoryData { get; set; }
        public IEnumerable<Subcategory> SubcategoryData { get; set; }
    }
}