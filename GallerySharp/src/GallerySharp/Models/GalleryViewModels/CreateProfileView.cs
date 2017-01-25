using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySharp.Models.GalleryViewModels
{
    public class CreateProfileView
    {
        [Required]
        [MinLength(4, ErrorMessage = "Minimun 4 characters expected.")]
        [StringLength(15, ErrorMessage = "Maximum 15 characters expected.")]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }
}
