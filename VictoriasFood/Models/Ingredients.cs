using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class Ingredients
    {
        [Key]
        public int ingredientsID { get; set; }

        [StringLength(50)]
        [Display(Name = "Ingredients category title")]
        public string ingredientsCategoryTitle { get; set; }

        [Display(Name = "Ingredients description")]
        public string ingredientsDescription { get; set; }
    }
}