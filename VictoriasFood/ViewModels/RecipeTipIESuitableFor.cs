using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VictoriasFood.Models;

namespace VictoriasFood.ViewModels
{
    public class RecipeTipIESuitableFor
    {
        public Recipe RecipeData { get; set; }

        public Tip TipData { get; set; }
        public IEnumerable<SuitableFor> SuitableForData { get; set; }

    }
}