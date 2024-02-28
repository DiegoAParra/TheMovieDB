using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TheMovieDB.Models;
using TheMovieDB.Services;

namespace TheMovieDB.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieApiService _movieApiService;

        public MovieController()
        {
            _movieApiService = new MovieApiService();
        }

        [HttpGet]
        public async Task<ActionResult> GetMovieDetails(int movieId)
        {
            var movieDetails = await _movieApiService.GetMovieDetailsAsync(movieId);

            if (movieDetails != null)
            {
                return View(movieDetails);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var movieList = await _movieApiService.GetMovieListAsync();

                var movieViewModels = movieList.Results.Select(m => new MovieListItem
                {
                    Id = m.Id,
                    Adult = m.Adult
                }).ToList();

                return View("Index", movieViewModels);
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }
    }
}
