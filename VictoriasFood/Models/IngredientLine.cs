using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class IngredientLine
    {
        [Key]
        public int ingredientLineID { get; set; }

        [Required(ErrorMessage = "Please enter the ingredient")]
        [StringLength(50)]
        [Display(Name = "Item")]
        public string itemTitle { get; set; }

        [StringLength(250)]
        [Display(Name = "description")]
        public string itemDescription { get; set; }

        [StringLength(50)]
        [Display(Name = "Measurement Metric System")]
        public string measurementMetricSystem { get; set; }

        [StringLength(50)]
        [Display(Name = "Measurement Imperial System")]
        public string measurementImperialSystem { get; set; }

        [Display(Name = "Unit converter")]
        public double unitConverter { get; set; }

        [Required(ErrorMessage = "Please enter the ingredient quantity")]
        [Display(Name = "Ingredient quantity Metric System")]
        public double baseQuantity { get; set; }

        [Display(Name = "Ingredient Imperial System")]
        public double calculatedQuantity { get; set; }
    }
}