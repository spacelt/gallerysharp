using GallerySharp.Models.GalleryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GallerySharp.Models.HomeViewModels
{
    public class HomeViewModel
    {
        public List<Photo> PointedOutPhotos { get; set; }
        public List<Photo> SubscribedPhotos { get; set; }

        public HomeViewModel()
        {
            PointedOutPhotos = new List<Photo>();
            SubscribedPhotos = new List<Photo>();
        }
    }
}
