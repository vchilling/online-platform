using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class SuitableFor
    {
        [Key]
        public int suitableForID { get; set; }

        [Required(ErrorMessage = "Please enter the Title of Suitable for")]
        [StringLength(50)]
        [Display(Name = "Suitable for category")]
        public string suitableForTitle { get; set; }

        [StringLength(1024)]
        [Display(Name = "Description for Suitable for category")]
        public string suitableForDescription { get; set; }
    }
}