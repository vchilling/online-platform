using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class Author
    {
        [Key]
        public int authorID { get; set; }

        [Required(ErrorMessage = "Please enter your First Name")]
        [StringLength(50)]
        [Display(Name = "Your First Name")]
        public string authorFirstName { get; set; }

        [Required(ErrorMessage = "Please enter the Second name")]
        [StringLength(50)]
        [Display(Name = "Your Second name")]
        public string authorSecondName { get; set; }

        [Required(ErrorMessage = "Please enter the Last name")]
        [StringLength(50)]
        [Display(Name = "Your Last name")]
        public string authorLastName { get; set; }

        [Required(ErrorMessage = "Please enter your addres")]
        [StringLength(50)]
        [Display(Name = "Your address")]
        public string addressLine1 { get; set; }

        [Required(ErrorMessage = "Please enter your zip code")]
        [StringLength(50)]
        [Display(Name = "Your zip code")]
        public string addressLine2 { get; set; }

        [Required(ErrorMessage = "Please enter your telephone number")]
        [StringLength(50)]
        [Display(Name = "Your telephone number")]
        public string telephoneNumber { get; set; }

        [StringLength(1024)]
        [Display(Name = "Describe yourself in a few words")]
        public string authorCv { get; set; }

        [StringLength(50)]
        [Display(Name = "Your facebook")]
        public string facebook { get; set; }

        [StringLength(50)]
        [Display(Name = "Your twitter")]
        public string twitter { get; set; }

        [StringLength(50)]
        [Display(Name = "Your instagram")]
        public string instagram { get; set; }

        [StringLength(50)]
        [Display(Name = "Your website")]
        public string website { get; set; }

        [StringLength(50)]
        [Display(Name = "Your email")]
        public string email { get; set; }
    }
}