using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PhotoGallery20230717.Context;
using PhotoGallery20230717.Models;
using System.Diagnostics.Metrics;
using System.Drawing;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace PhotoGallery20230717.Controllers
{
    public class PhotoController : Controller
    {
        private readonly AppDbContext _context;

        public PhotoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PhotoController
        public ActionResult Index()
        {
            var photos = _context.Photos.Where(x => x.IsDeleted == false).OrderByDescending(x => x.UploadedOn).ToList();
            return View(photos);
        }

        // GET: Photo/Details/5
        [HttpGet]
        public IActionResult Details(int? id)
        {
            try
            {
                var photo = _context.Photos.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
                return View(photo);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: PhotoController/Create
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PhotoModel model)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var file = model.ImageFile;
                if (file != null && file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        model.ImageData = ms.ToArray();
                    }
                }

                model.IsDeleted = false;
                var currDate = DateTime.Now;
                model.UploadedOn = currDate;
                model.CreatedDate = currDate;
                model.UpdatedDate = currDate;
                _context.Photos.Add(model);
                var count = await _context.SaveChangesAsync();
                responseModel.respCode = count == 1 ? "000" : "999";
                responseModel.respMsg = count == 1 ? "Saving successful!" : "Saving failed!";
                //return View();
                //return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                responseModel.respCode = "999";
                responseModel.respMsg = "Error: " + ex.Message;
            }
            return Json(responseModel);
        }

        #region Not finish edit

        // GET: PhotoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PhotoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        #endregion

        // POST: PhotoController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            ResponseModel model = new ResponseModel();

            if (!User.Identity.IsAuthenticated)
            {
                model.respCode = "777";
                model.respMsg = "You do not have access to delete photo, Please login to delete photo";
                return Json(model);
            }

            try
            {
                var item = await _context.Photos.Where(x => x.Id == id).FirstOrDefaultAsync();
                item.IsDeleted = true;
                _context.Photos.Update(item);
                var count = await _context.SaveChangesAsync();
                model.respCode = count == 1 ? "000" : "999";
                model.respMsg = count == 1 ? "Photo delete successful!" : "Delete failed!";

            }
            catch (Exception ex)
            {
                model.respCode = "999";
                model.respMsg = ex.Message + ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
            return Json(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }
    }
}
