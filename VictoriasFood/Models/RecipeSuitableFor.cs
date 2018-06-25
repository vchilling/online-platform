using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class RecipeSuitableFor
    {
        [Key]
        public int recipeSuitableForID { get; set; }

        public Recipe Recipes { get; set; }
        [Required(ErrorMessage = "Please enter recipe")]
        [Display(Name = "Recipe")]
        public int recipeID { get; set; }

        public SuitableFor SuitableForCategories { get; set; }
        [Required(ErrorMessage = "Please enter suitable for category")]
        [Display(Name = "Suitable for category")]
        public int suitableForID { get; set; }
    }
}