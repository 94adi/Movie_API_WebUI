namespace Movie.WebUI.Areas.Customer.Controllers
{
    [Area("Customer")]
    [AllowAnonymous]
    public class MovieController : Controller
    {
        public IActionResult Index(int id)
        {
            return View();
        }
    }
}
