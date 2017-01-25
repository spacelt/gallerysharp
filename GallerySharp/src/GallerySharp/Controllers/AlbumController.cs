using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GallerySharp.Data.GalleryRepository;
using GallerySharp.Models;
using Microsoft.AspNetCore.Identity;
using GallerySharp.Models.GalleryViewModels;
using GallerySharp.Models.GalleryModels;
using Microsoft.AspNetCore.Authorization;

namespace GallerySharp.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly IGalleryRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AlbumController(IGalleryRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            bool doesHaveProfile = _repository.doesUserHaveProfile(currentUser.Id);
            if (!doesHaveProfile)
            {
                return RedirectToAction("CreateProfile");
            }
            ViewBag.usId = new Guid(currentUser.Id);
            ViewBag.userName = _repository.getProfileUserName(currentUser.Id);
            ViewBag.subs = _repository.getUserSubscriptionsIds(currentUser.Id);
            ViewBag.users = _repository.getAllUsersExcept(currentUser.Id);
            return View(_repository.getAllAlbums());
        }

        public ActionResult CreateProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile(CreateProfileView m)
        {
            if (ModelState.IsValid)
            {
                if (!_repository.doesUserNameExists(m.UserName))
                {
                    ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

                    _repository.createNewProfile(currentUser.Id, m.UserName);
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.errorMessage = "User name already taken!";
            return View(m);
        }

        public ActionResult CreateAlbum()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAlbum(CreateAlbumView m)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
                _repository.createNewAlbum(currentUser.Id, m.AlbumName, _repository.getProfileUserName(currentUser.Id));
                return RedirectToAction("Index");
            }
            return View(m);
        }

        public async Task<IActionResult> DeleteAlbum(Guid id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (_repository.albumExists(currentUser.Id, id))
            {
                _repository.deleteAlbum(id);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeletePhoto(Guid id)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
                _repository.deletePhoto(currentUser.Id, id);
            }
            return RedirectToAction("ViewAlbum");
        }

        public async Task<IActionResult> ViewAlbum(Guid id)
        {

            if (id != null && !id.ToString().StartsWith("000"))
            {
                HttpContext.Session.Set("albumId", id.ToByteArray());
            }
            else
            {
                id = new Guid(HttpContext.Session.Get("albumId"));
            }
            Album album = _repository.getAlbum(id);
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.userId = new Guid(currentUser.Id);
            return View(album);
        }

        public ActionResult AddPhoto()
        {

            ViewData["albumId"] = new Guid(HttpContext.Session.Get("albumId"));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoto(Guid id, string PhotoName, IFormFile upload)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid)
            {
                if (PhotoName != null && upload != null && upload.Length > 0)
                {
                    _repository.addPhoto(id, currentUser.Id, PhotoName, upload);
                    return RedirectToAction("ViewAlbum");
                }
            }
            return View();
        }


        public ActionResult ViewPhoto(Guid id)
        {
            if (id != null && !id.ToString().StartsWith("000"))
            {
                Guid albumId = _repository.getAlbumIdOfPhoto(id);
                HttpContext.Session.Set("photoId", id.ToByteArray());
                HttpContext.Session.Set("albumId", albumId.ToByteArray());
            }
            else
            {
                id = new Guid(HttpContext.Session.Get("photoId"));
            }
            Photo photo = _repository.getPhoto(id);
            return View(photo);
        }

        public ActionResult LikePhoto(Guid id)
        {
            _repository.likePhoto(id);
            return RedirectToAction("ViewPhoto", id);
        }

        public ActionResult DislikePhoto(Guid id)
        {
            _repository.dislikePhoto(id);
            return RedirectToAction("ViewPhoto", id);
        }

        public ActionResult CreateComment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(CreateCommentView m)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
                Guid photoId = new Guid(HttpContext.Session.Get("photoId"));
                ViewBag.photoId = photoId;
                _repository.addCommentToPhoto(photoId, currentUser.Id, m.CommentText);
                return RedirectToAction("ViewPhoto", photoId);
            }
            return View(m);
        }

        public ActionResult ChangePhotoName(string id)
        {
            if (ModelState.IsValid)
            {
                Guid photoId = new Guid(id);
                if (photoId != null && !photoId.ToString().StartsWith("000"))
                {
                    Guid albumId = _repository.getAlbumIdOfPhoto(photoId);
                    HttpContext.Session.Set("photoId", photoId.ToByteArray());
                }
                else
                {
                    photoId = new Guid(HttpContext.Session.Get("photoId"));
                }
                ViewBag.oldPhotoName = _repository.getPhotoName(photoId);
                return View();
            }
            return RedirectToAction("ViewAlbum");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePhotoName(ChangePhotoNameView m)
        {
            if (ModelState.IsValid)
            {
                Guid photoId = new Guid(HttpContext.Session.Get("photoId"));
                _repository.changePhotoName(photoId, m.NewPhotoName);
                return RedirectToAction("ViewAlbum");
            }
            return View(m);

        }

        public ActionResult ChangeAlbumName(string id)
        {
            if (ModelState.IsValid)
            {
                Guid albumId = new Guid(id);
                if (albumId != null && !albumId.ToString().StartsWith("000"))
                {
                    HttpContext.Session.Set("albumId", albumId.ToByteArray());
                }
                else
                {
                    albumId = new Guid(HttpContext.Session.Get("albumId"));
                }
                ViewBag.oldAlbumName = _repository.getAlbumName(albumId);
                return View();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAlbumName(ChangeAlbumNameView m)
        {
            if (ModelState.IsValid)
            {
                Guid albumId = new Guid(HttpContext.Session.Get("albumId"));
                _repository.changeAlbumName(albumId, m.NewAlbumName);
                return RedirectToAction("Index");
            }
            return View(m);
        }

        public async Task<IActionResult> AddPhotoToFavorites(string id)
        {
            Guid photoG = new Guid(id);
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.addPhotoToFavorites(new Guid(currentUser.Id), photoG);
            return RedirectToAction("ViewPhoto");
        }


        public async Task<IActionResult> SubscribeToUser(string id)
        {
            Guid userGSub = new Guid(id);
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.subscribeToUser(new Guid(currentUser.Id), userGSub);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UnsubscribeToUser(string id)
        {
            Guid userGSub = new Guid(id);
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.unsubscribeToUser(new Guid(currentUser.Id), userGSub);
            return RedirectToAction("Index");
        }
    }
}