using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySharp.Models.GalleryModels
{
    public class Album
    {
        public Guid AlbumID { get; set; }
        public string AlbumName { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public Album()
        {

        }

        public Album(string userId, string albumName, string userName)
        {
            AlbumID = Guid.NewGuid();
            AlbumName = albumName;
            UserID = new Guid(userId);
            UserName = userName;
            CreationDate = DateTime.Now;
        }

    }
}
