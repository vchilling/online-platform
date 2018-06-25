using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class Recipe
    {
        [Key]
        public int recipeID { get; set; }

        [Required(ErrorMessage = "Please enter the Title of Recipe")]
        [StringLength(50)]
        [Display(Name = "Recipe title")]
        public string recipeTitle { get; set; }

        [Required(ErrorMessage = "Please enter the Description of Recipe")]
        [StringLength(250)]
        [Display(Name = "Recipe description")]
        public string recipeDescription { get; set; }

        [Required(ErrorMessage = "Please enter the number of serving of recipe")]
        [Display(Name = "Number of serving")]
        public int recipeNumberOfServings { get; set; }

        [Required(ErrorMessage = "Please enter required time to prepare the recipe.")]
        [Display(Name = "Required time to prepare the recipe")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm tt}")]
        public DateTime recipeReadyIn { get; set; }

        [Required(ErrorMessage = "Please upload image of Recipe")]
        [StringLength(250)]
        [Display(Name = "Recipe image")]
        public string recipeImage { get; set; }

        public Author Authors { get; set; }
        [Required(ErrorMessage = "Please enter the name of the author")]
        [Display(Name = "Author name")]
        public int authorID { get; set; }

        public Subcategory Subcategories { get; set; }
        [Required(ErrorMessage = "Please enter the subcategory")]
        [Display(Name = "Subcategory")]
        public int subcategoryID { get; set; }
    }
}