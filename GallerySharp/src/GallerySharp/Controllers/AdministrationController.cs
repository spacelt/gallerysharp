using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GallerySharp.Data.GalleryRepository;
using Microsoft.AspNetCore.Authorization;

namespace GallerySharp.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly IGalleryRepository _repository;

        public AdministrationController(IGalleryRepository repository)
        {
            _repository = repository;
        }
        public ActionResult Index()
        {
            return View(_repository.getAllPhotos());
        }

        public ActionResult PointDownPhoto(string id)
        {
            if (ModelState.IsValid)
            {
                _repository.pointDownPhoto(new Guid(id));
            }
            return RedirectToAction("Index");
        }

        public ActionResult PointOutPhoto(string id)
        {
            if (ModelState.IsValid)
            {
                _repository.pointOutPhoto(new Guid(id));
            }
            return RedirectToAction("Index");
        }
    }
}