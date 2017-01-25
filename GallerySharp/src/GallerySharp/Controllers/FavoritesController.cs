using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GallerySharp.Data.GalleryRepository;
using Microsoft.AspNetCore.Identity;
using GallerySharp.Models;
using Microsoft.AspNetCore.Authorization;

namespace GallerySharp.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {

        private readonly IGalleryRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(IGalleryRepository repository, UserManager<ApplicationUser> userManager)
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
                return RedirectToAction("CreateProfile", "Album");
            }
            Guid userG = new Guid(currentUser.Id);
            return View(_repository.getAllUserFavorites(userG));
        }

        public async Task<IActionResult> RemovePhotoFromFavorites(string id)
        {
            Guid photoG = new Guid(id);
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.removePhotoFromFavorites(new Guid(currentUser.Id), photoG);
            return RedirectToAction("Index");
        }
    }
}