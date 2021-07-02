using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MayMoviesASP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MayMoviesASP.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public AdminController(IHostingEnvironment environment)
        {
            hostingEnvironment = environment;
        }

        // GET: AdminController
        public ActionResult Index()
        {
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;

            return View(context.GetAllMovies());
        }
        public ActionResult AddMovie()
        {
            return View();
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMovie(Movie movie)
        {
                DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
            if (movie.ImageFile != null)
            {
                var uniqueFileName = GetUniqueFileName(movie.ImageFile.FileName);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "images");
                var filePath = Path.Combine(uploads, uniqueFileName);
                movie.ImageFile.CopyTo(new FileStream(filePath, FileMode.Create));
                movie.Image = uniqueFileName;
                //to do : Save uniqueFileName  to your db table   
            }
            // to do  : Return something
                context.AddMovie(movie);
                return RedirectToAction(nameof(Index));
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
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

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
