using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class Subcategory
    {
        [Key]
        public int subcategoryID { get; set; }

        [Required(ErrorMessage = "Please enter Title of Subcategory")]
        [StringLength(50)]
        [Display(Name = "Title of the Subcategory")]
        public string subcategoryTitle { get; set; }

        [StringLength(1024)]
        [Display(Name = "Description of the Subcategory")]
        public string subcategoryDescription { get; set; }

        public Category Categories { get; set; }
        public int categoryID { get; set; }
    }
}