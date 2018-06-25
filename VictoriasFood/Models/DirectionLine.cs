using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VictoriasFood.Models
{
    public class DirectionLine
    {
        [Key]
        public int directionLineID { get; set; }

        [Required(ErrorMessage = "Please enter the direction text")]
        [StringLength(1024)]
        [Display(Name = "Direction")]
        public string directionLineText { get; set; }

        [StringLength(250)]
        [Display(Name = "Direction headline")]
        public string directionLineDescription { get; set; }

        public Direction Directions { get; set; }
        [Required(ErrorMessage = "Please enter directionID")]
        [Display(Name = "directionID")]
        public int directionID { get; set; }
    }
}