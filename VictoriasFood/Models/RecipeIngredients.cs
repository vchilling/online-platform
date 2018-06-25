using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class RecipeIngredients
    {
        [Key]
        public int recipeIngredientsID { get; set; }

        public Recipe Recipes { get; set; }
        [Required(ErrorMessage = "Please enter recipeID")]
        [Display(Name = "recipeID")]
        public int recipeID { get; set; }

        public Ingredients IngredientsList { get; set; }
        [Required(ErrorMessage = "Please enter ingredientsID")]
        [Display(Name = "ingredientsID")]
        public int ingredientsID { get; set; }
    }
}
