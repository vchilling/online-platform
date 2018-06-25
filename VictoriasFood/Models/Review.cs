using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace VictoriasFood.Models
{
    public class Review
    {
        [Key]
        public int reviewID { get; set; }

        public Recipe Recipes { get; set; }
        [Required(ErrorMessage = "Please enter recipeID")]
        [Display(Name = "recipeID")]
        public int recipeID { get; set; }

        public Author Authors { get; set; }
        [Required(ErrorMessage = "Please enter the name of the author")]
        [Display(Name = "Author name")]
        public int authorID { get; set; }

        [Required(ErrorMessage = "Please enter date and time.")]
        [Display(Name = "Date and time of review")]
        public DateTime reviewDateTime { get; set; }

        [StringLength(250)]
        [Display(Name = "Review text")]
        public string reviewText { get; set; }

        [Required(ErrorMessage = "Please enter rate of the recipe")]
        [Display(Name = "Rate of the recipe")]
        public int reviewRate { get; set; }
    }
}