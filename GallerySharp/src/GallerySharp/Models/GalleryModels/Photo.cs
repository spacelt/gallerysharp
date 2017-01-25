using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySharp.Models.GalleryModels
{
    public class Photo
    {
        public Guid PhotoID { get; set; }
        [MinLength(4, ErrorMessage = "Minimum of 4 characters is expected.")]
        [MaxLength(15, ErrorMessage = "Maximum of 15 characters is expected.")]
        public string PhotoName { get; set; }
        public Guid UserID { get; set; }
        public Guid AlbumID { get; set; }
        public DateTime CreationDate { get; set; }
        public int LikeCount { get; set; }
        public bool PointedOut { get; set; }
        public string ContentType { get; set; }
        [Required(ErrorMessage = "You must upload a photo")]
        public byte[] Content { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }

        public Photo()
        {
            this.Profiles = new HashSet<Profile>();
        }

        public Photo(string photoName, string userId, string albumId, string contentType, byte[] content)
        {
            PhotoID = Guid.NewGuid();
            PhotoName = photoName;
            UserID = new Guid(userId);
            AlbumID = new Guid(albumId);
            ContentType = contentType;
            Content = content;
            CreationDate = DateTime.Now;
            LikeCount = 0;
            PointedOut = false;
        }

    }
}
