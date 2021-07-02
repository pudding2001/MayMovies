using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MayMoviesASP.Models;

namespace MayMoviesASP.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;

            return View(context.GetAllMovies());
        }

        public IActionResult Result(int id)
        {
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;

            return View(context.GetMovieById(id));
        }

        public IActionResult BrowseMovies()
        {
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;

            return View(context.GetAllMovies());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // POST: HomeControler/SearchMovies
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchMovies(string searchText)
        {
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
            Console.WriteLine(searchText);
            List<Movie> movies = context.GetMoviesByNameOrActor(searchText);
            return View("BrowseMovies", movies);
        }
    }
}
