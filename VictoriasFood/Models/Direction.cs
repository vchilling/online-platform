using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class Direction
    {
        [Key]
        public int directionID { get; set; }

        [StringLength(50)]
        [Display(Name = "Direction title")]
        public string directionTitle { get; set; }

        [StringLength(1024)]
        [Display(Name = "Direction description")]
        public string directionDescription { get; set; }

        public Recipe Recipes { get; set; }
        [Required(ErrorMessage = "Please enter recipeID")]
        [Display(Name = "recipeID")]
        public int recipeID { get; set; }
    }
}