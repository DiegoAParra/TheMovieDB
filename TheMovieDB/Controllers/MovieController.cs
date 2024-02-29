using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
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

        [HttpPost]
        public ActionResult AddToCart(int movieId, string title)
        {
            var shoppingCartItem = new ShoppingCartItem
            {
                MovieId = movieId,
                Title = title,
                Price = 1000
            };

            List<ShoppingCartItem> shoppingCart = Session["Cart"] as List<ShoppingCartItem> ?? new List<ShoppingCartItem>();
            shoppingCart.Add(shoppingCartItem);
            Session["Cart"] = shoppingCart;

            return RedirectToAction("Index");
        }

        public ActionResult Cart()
        {
            List<ShoppingCartItem> shoppingCart = Session["Cart"] as List<ShoppingCartItem> ?? new List<ShoppingCartItem>();
            return View(shoppingCart);
        }

        public ActionResult RemoveFromCart(int movieId)
        {
            var shoppingCart = Session["ShoppingCart"] as List<ShoppingCartItem>;

            if (shoppingCart != null)
            {
                var itemToRemove = shoppingCart.FirstOrDefault(item => item.MovieId == movieId);

                if (itemToRemove != null)
                {
                    shoppingCart.Remove(itemToRemove);
                    Session["ShoppingCart"] = shoppingCart;
                }
            }

            return RedirectToAction("Cart");
        }

        public ActionResult Pay(string format = "PDF")
        {
            var list = Session["Cart"] as List<ShoppingCartItem>;
            List<ShoppingCartItem> recordsList = new List<ShoppingCartItem>();

            foreach (var cartItem in list)
            {
                recordsList.Add(
                    new ShoppingCartItem()
                    {
                        MovieId = cartItem.MovieId,
                        Title = cartItem.Title,
                        Price = cartItem.Price,
                    });
            }

            string reportPath = Server.MapPath("~/Reports/RdlcFiles/CartReport.rdlc");

            LocalReport lr = new LocalReport();

            lr.ReportPath = reportPath;
            lr.EnableHyperlinks = true;

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            string mimeType, encoding, fileNameExtension;

            ReportDataSource datasource = new ReportDataSource("CartDataSet", recordsList);
            lr.DataSources.Add(datasource);


            renderedBytes = lr.Render(
            format,
            string.Empty,
            out mimeType,
            out encoding,
            out fileNameExtension,
            out streams,
            out warnings
            );

            return File(renderedBytes, mimeType);
        }
    }
}
