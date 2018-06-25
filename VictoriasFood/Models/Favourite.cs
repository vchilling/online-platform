using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class Favourite
    {
        [Key]
        public int favouriteID { get; set; }

        public Recipe Recipes { get; set; }
        [Required(ErrorMessage = "Please enter recipeID")]
        [Display(Name = "Recipe")]
        public int recipeID { get; set; }
        
        public Author Authors { get; set; }
        [Required(ErrorMessage = "Please enter the name of the author")]
        [Display(Name = "Author name")]
        public int authorID { get; set; }
    }
}