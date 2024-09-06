using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Movie.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/Movie")]
    [ApiVersion("1.0")]
    //[Authorize]
    public class MovieController : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<APIResponse> GetAllMovies()
        {

            return Ok(new APIResponse());
        }
        
    }
}
