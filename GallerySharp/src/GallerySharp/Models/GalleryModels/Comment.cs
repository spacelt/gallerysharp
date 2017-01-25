using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySharp.Models.GalleryModels
{
    public class Comment
    {
        public Guid CommentID { get; set; }
        public Guid PhotoID { get; set; }
        public Guid UserID { get; set; }
        public string CommentText { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserName { get; set; }

        public Comment()
        {

        }

        public Comment(Guid userId, string userName, Guid photoId, string commentText)
        {
            CommentID = Guid.NewGuid();
            PhotoID = photoId;
            UserID = userId;
            UserName = userName;
            CommentText = commentText;
            CreationDate = DateTime.Now;
        }
    }
}
