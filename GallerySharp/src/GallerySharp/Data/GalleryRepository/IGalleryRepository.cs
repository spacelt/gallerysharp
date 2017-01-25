using GallerySharp.Models.GalleryModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GallerySharp.Data.GalleryRepository
{
    public interface IGalleryRepository
    {
        List<Photo> getNPointedOutPhotos(int n);
        List<Photo> getUserSubscriptionPhotos(string userId);
        List<Album> getAllAlbums();
        void pointOutPhoto(Guid photoId);
        void pointDownPhoto(Guid photoId);
        Photo getPhoto(Guid photoId);
        void addCommentToPhoto(Guid photoId,string userId, string commentText);
        void likePhoto(Guid photoId);
        void createNewProfile(string userId, string userName);
        bool doesUserHaveProfile(string userId);
        string getProfileUserName(string userId);
        void createNewAlbum(string userId, string albumName, string userName);
        bool albumExists(string userId, Guid albumId);
        void deleteAlbum(Guid albumId);
        Album getAlbum(Guid albumId);
        void addPhoto(Guid albumId, string userId, string photoName, IFormFile stream);
        void dislikePhoto(Guid photoId);
        List<Photo> getAllPhotos();
        Guid getAlbumIdOfPhoto(Guid photoId);
        string getPhotoName(Guid photoId);
        void changePhotoName(Guid photoId, string newPhotoName);
        string getAlbumName(Guid albumId);
        void changeAlbumName(Guid albumId, string newAlbumName);
        void addPhotoToFavorites(Guid userId, Guid photoId);
        List<Photo> getAllUserFavorites(Guid userId);
        void removePhotoFromFavorites(Guid userId, Guid photoId);
        void subscribeToUser(Guid userId, Guid userIdToSub);
        void unsubscribeToUser(Guid userId, Guid userIdToSub);
        List<Guid> getUserSubscriptionsIds(string userId);
        void deletePhoto(string userId, Guid photoId);
        List<Profile> getAllUsersExcept(string userId);
        bool doesUserNameExists(string userName);
    }
}
