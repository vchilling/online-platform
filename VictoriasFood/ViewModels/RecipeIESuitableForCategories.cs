using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class RecipeIESuitableForCategories
    {
        public int RecipeID { get; set; }
        public IEnumerable<SuitableFor> SuitableForData { get; set; }
        public List<int> OldRecipeSuitableForData { get; set; }
    }
}