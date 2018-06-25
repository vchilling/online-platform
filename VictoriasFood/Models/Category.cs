using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace VictoriasFood.Models
{
    public class Category
    {
        [Key]
        public int categoryID { get; set; }

        [Required(ErrorMessage = "Please enter Title of Category")]
        [StringLength(50)]
        [Display(Name = "Title of the Category")]
        public string categoryTitle { get; set; }

        [StringLength(1024)]
        [Display(Name = "Description of the Category")]
        public string categoryDescription { get; set; }
    }
}