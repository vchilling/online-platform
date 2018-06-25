using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class IngredientsIngredientLine
    {
        [Key]
        public int ingredientsIngredientLineID { get; set; }

        public Ingredients IngredientsList { get; set; }
        [Required(ErrorMessage = "Please enter ingredientsID")]
        [Display(Name = "ingredientsID")]
        public int ingredientsID { get; set; }

        public IngredientLine IngredientLines { get; set; }
        [Required(ErrorMessage = "Please enter ingredientLineID")]
        [Display(Name = "ingredientLineID")]
        public int ingredientLineID { get; set; }
    }
}