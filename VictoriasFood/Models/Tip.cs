using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class Tip
    {
        [Key]
        public int tipID { get; set; }

        [StringLength(50)]
        [Display(Name = "The title for tip of new recipe")]
        public string tipTitle { get; set; }

        [StringLength(1024)]
        [Display(Name = "The description for tip of new recipe")]
        public string tipDescription { get; set; }

        public Recipe Recipes { get; set; }
        [Required(ErrorMessage = "Please enter recipeID")]
        [Display(Name = "recipeID")]
        public int recipeID { get; set; }
    }
}