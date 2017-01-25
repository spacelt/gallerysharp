using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GallerySharp.Data.GalleryRepository;
using GallerySharp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using GallerySharp.Models.HomeViewModels;

namespace GallerySharp.Controllers
{
    public class HomeController : Controller
    {

        private readonly IGalleryRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IGalleryRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            HomeViewModel homeModel = new HomeViewModel {
                PointedOutPhotos = _repository.getNPointedOutPhotos(10)
            };
            if (currentUser == null)
            {
                HttpContext.Session.Set("userType", new byte[] { 0 });
            }
            else if(currentUser.Email == "administrator@administrator.admin")
            {
                HttpContext.Session.Set("userType", new byte[] { 1 });
            } else
            {
                HttpContext.Session.Set("userType", new byte[] { 2 });
                bool doesHaveProfile = _repository.doesUserHaveProfile(currentUser.Id);
                if (!doesHaveProfile)
                {
                    return RedirectToAction("CreateProfile", "Album");
                }
                homeModel.SubscribedPhotos = _repository.getUserSubscriptionPhotos(currentUser.Id);
            }
            return View(homeModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
