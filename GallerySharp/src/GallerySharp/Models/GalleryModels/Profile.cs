using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySharp.Models.GalleryModels
{
    public class Profile
    {
        public Guid ProfileID { get; set; }
        public string UserName { get; set; }
        public Guid UserID { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }

        public Profile()
        {
            Profiles = new HashSet<Profile>();
        }

        public Profile(string userId, string userName)
        {
            ProfileID = Guid.NewGuid();
            UserName = userName;
            UserID = Guid.Parse(userId);
        }
    }
}
