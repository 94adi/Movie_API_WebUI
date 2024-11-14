using Microsoft.AspNetCore.Mvc;

namespace Movie.WebUI.Controllers
{
    public class MovieController(ISender sender) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var query = new GetMovieByIdQuery(id);

            var result = await sender.Send(query);

            if(result == null)
            {
                return NotFound();
            }

            return View(result.Movie);
        }
    }
}
