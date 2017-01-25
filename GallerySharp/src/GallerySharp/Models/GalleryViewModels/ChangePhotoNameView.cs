using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySharp.Models.GalleryViewModels
{
    public class ChangePhotoNameView
    {
        [Required]
        [MinLength(4, ErrorMessage = "Minimun 4 characters expected.")]
        [StringLength(15, ErrorMessage = "Maximum 15 characters expected.")]
        [Display(Name = "New Photo Name")]
        public string NewPhotoName { get; set; }
    }
}
