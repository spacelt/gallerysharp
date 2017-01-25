using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GallerySharp.Models.GalleryModels;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace GallerySharp.Data.GalleryRepository
{
    public class GallerySqlRepository : IGalleryRepository
    {
        private readonly GalleryDbContext _context;

        public GallerySqlRepository(GalleryDbContext context)
        {
            _context = context;
        }

        public void addCommentToPhoto(Guid photoId, string userId, string commentText)
        {
            Guid userG = new Guid(userId);
            string userName = _context.Profiles.Where(p => p.UserID == userG).Select(u => u.UserName).FirstOrDefault();
            Comment comment = new Comment(userG, userName, photoId, commentText);
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public List<Album> getAllAlbums()
        {
            return _context.Albums.AsEnumerable().ToList();
        }

        public List<Photo> getNPointedOutPhotos(int n)
        {
            List<Photo> ans = _context.Photos.Where(p => p.PointedOut)?.OrderByDescending(s => s.LikeCount).
                ThenByDescending(s => s.CreationDate).Take(n).ToList();
            return ans != null ? ans : new List<Photo>();
        }

        public Photo getPhoto(Guid photoId)
        {
            return _context.Photos.Include(c => c.Comments).Where(p => p.PhotoID == photoId).FirstOrDefault();
        }

        public List<Photo> getUserSubscriptionPhotos(string userId)
        {
            List<Guid> subsUserIds = getUserSubscriptionsIds(userId);
            if (subsUserIds.Count == 0)
            {
                return new List<Photo>();
            }
            List<Photo> ans = new List<Photo>();
            foreach (var sub in subsUserIds)
            {
                List<Photo> temp = _context.Photos.Where(p => p.UserID == sub).Take(4).ToList();
                ans = ans.Union(temp).ToList();
            }
            return ans.OrderByDescending(p => p.CreationDate).ThenByDescending(p => p.LikeCount).Take(20).ToList();
        }
        public void likePhoto(Guid photoId)
        {
            Photo photo = _context.Photos.Where(p => p.PhotoID == photoId).FirstOrDefault();
            photo.LikeCount++;
            _context.Entry(photo).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void dislikePhoto(Guid photoId)
        {
            Photo photo = _context.Photos.Where(p => p.PhotoID == photoId).FirstOrDefault();
            photo.LikeCount--;
            _context.Entry(photo).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void pointDownPhoto(Guid photoId)
        {
            Photo photo = _context.Photos.Where(p => p.PhotoID == photoId).FirstOrDefault();
            photo.PointedOut = false;
            _context.Entry(photo).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void pointOutPhoto(Guid photoId)
        {
            Photo photo = _context.Photos.Where(p => p.PhotoID == photoId).FirstOrDefault();
            photo.PointedOut = true;
            _context.Entry(photo).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void createNewProfile(string userId, string userName)
        {
            _context.Profiles.Add(new Profile(userId, userName));
            _context.SaveChanges();
        }

        public bool doesUserHaveProfile(string userId)
        {
            Guid userG = new Guid(userId);
            return _context.Profiles.Any(p => p.UserID == userG);
        }

        public string getProfileUserName(string userId)
        {
            Guid userG = new Guid(userId);
            return _context.Profiles.Where(p => p.UserID == userG).First().UserName;
        }

        public void createNewAlbum(string userId, string albumName, string userName)
        {
            _context.Albums.Add(new Album(userId, albumName, userName));
            _context.SaveChanges();
        }

        public bool albumExists(string userId, Guid albumId)
        {
            Guid userG = new Guid(userId);
            return _context.Albums.Any(p => p.UserID == userG && p.AlbumID == albumId);
        }

        public void deleteAlbum(Guid albumId)
        {
            foreach (var photo in _context.Photos.Where(p => p.AlbumID == albumId).ToList())
            {
                _context.Photos.Remove(photo);
            }
            _context.Entry(new Album() { AlbumID = albumId }).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public Album getAlbum(Guid albumId)
        {
            return _context.Albums.Include(p => p.Photos).Where(a => a.AlbumID == albumId).FirstOrDefault();
        }

        public void addPhoto(Guid albumId, string userId, string photoName, IFormFile stream)
        {
            Photo photo = new Photo
            {
                AlbumID = albumId,
                PhotoID = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                ContentType = stream.ContentType,
                LikeCount = 0,
                PointedOut = false,
                PhotoName = photoName,
                UserID = new Guid(userId)
            };
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                photo.Content = memoryStream.ToArray();
            }

            _context.Photos.Add(photo);
            _context.SaveChanges();
        }

        public List<Photo> getAllPhotos()
        {
            return _context.Photos.ToList();
        }

        public Guid getAlbumIdOfPhoto(Guid photoId)
        {
            return _context.Photos.Where(s => s.PhotoID == photoId).Select(p => p.AlbumID).FirstOrDefault();
        }

        public string getPhotoName(Guid photoId)
        {
            return _context.Photos.Where(p => p.PhotoID == photoId).Select(p => p.PhotoName).FirstOrDefault();
        }

        public void changePhotoName(Guid photoId, string newPhotoName)
        {
            Photo photo = _context.Photos.Where(p => p.PhotoID == photoId).FirstOrDefault();
            photo.PhotoName = newPhotoName;
            _context.Entry(photo).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public string getAlbumName(Guid albumId)
        {
            return _context.Albums.Where(a => a.AlbumID == albumId).Select(p => p.AlbumName).FirstOrDefault();
        }

        public void changeAlbumName(Guid albumId, string newAlbumName)
        {
            Album album = _context.Albums.Where(a => a.AlbumID == albumId).FirstOrDefault();
            album.AlbumName = newAlbumName;
            _context.Entry(album).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void addPhotoToFavorites(Guid userId, Guid photoId)
        {
            Photo photo = _context.Photos.Where(p => p.PhotoID == photoId).FirstOrDefault();
            Profile profile = _context.Profiles.Where(p => p.UserID == userId).FirstOrDefault();
            photo.Profiles.Add(profile);
            _context.Entry(profile).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public List<Photo> getAllUserFavorites(Guid userId)
        {
            return _context.Profiles.Include(p => p.Photos).Where(u => u.UserID == userId).FirstOrDefault().Photos.ToList();
        }

        public void removePhotoFromFavorites(Guid userId, Guid photoId)
        {
            Photo photo = _context.Photos.Where(p => p.PhotoID == photoId).FirstOrDefault();
            Profile profile = _context.Profiles.Where(p => p.UserID == userId).FirstOrDefault();

            profile.Photos.Remove(photo);

            _context.SaveChanges();
        }

        public void subscribeToUser(Guid userId, Guid userIdToSub)
        {
            Profile profile = _context.Profiles.Where(p => p.UserID == userId).FirstOrDefault();
            Profile profileToSub = _context.Profiles.Where(p => p.UserID == userIdToSub).FirstOrDefault();

            profile.Profiles.Add(profileToSub);
            _context.Entry(profile).State = EntityState.Modified;
            _context.Entry(profileToSub).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void unsubscribeToUser(Guid userId, Guid userIdToSub)
        {
            Profile profile = _context.Profiles.Where(p => p.UserID == userId).FirstOrDefault();
            Profile profileToSub = _context.Profiles.Where(p => p.UserID == userIdToSub).FirstOrDefault();

            profile.Profiles.Remove(profileToSub);

            _context.SaveChanges();
        }

        public List<Guid> getUserSubscriptionsIds(string userId)
        {
            Guid userG = new Guid(userId);
            List<Guid> ans = _context?.Profiles?.Include(p => p.Profiles)?.Where(p => p.UserID == userG)?
                .FirstOrDefault()?.Profiles?.Select(p => p.UserID)?.ToList();
            return ans != null ? ans : new List<Guid>();
        }

        public void deletePhoto(string userId, Guid photoId)
        {
            Guid userG = new Guid(userId);
            Photo photo = _context.Photos.Where(p => p.UserID == userG && p.PhotoID == photoId).FirstOrDefault();
            _context.Entry(photo).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public List<Profile> getAllUsersExcept(string userId)
        {
            Guid userG = new Guid(userId);
            return _context.Profiles.Where(p => p.UserID != userG).ToList();
        }

        public bool doesUserNameExists(string userName)
        {
            return _context.Profiles.Any(p => p.UserName == userName);
        }
    }
}
