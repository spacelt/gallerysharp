using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySharp.Models.GalleryViewModels
{
    public class CreateCommentView
    {
        [Required]
        [MinLength(1, ErrorMessage = "Minimun 1 characters expected.")]
        [StringLength(300, ErrorMessage = "Maximum 300 characters expected.")]
        [Display(Name = "Enter Comment")]
        public string CommentText { get; set; }
    }
}
