using System.Threading.Tasks;
using System.Web.Mvc;
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

        public async Task<ActionResult> Index()
        {
            var movies = await _movieApiService.GetMoviesFromApiAsync();

            foreach (var movie in movies)
            {
                var movieDetails = await _movieApiService.GetMovieDetailsAsync(movie.Id);

                movie.Title = movieDetails.Title;
                movie.Poster_Path = movieDetails.Poster_Path;
                movie.Release_Date = movieDetails.Release_Date;
            }
            return View(movies);
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var movie = await _movieApiService.GetMovieDetailsAsync(id);

            if (movie != null)
            {
                return View(movie);
            }
            else
            {
                return HttpNotFound();
            }
        }        
    }
}
