using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.BuildingBlocks;
using Movie.WebUI.Services.Handlers.Movies.Commands;
using Movie.WebUI.Services.Handlers.Movies.Queries;

namespace Movie.WebUI.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISender _sender;

        public MovieController(IMapper mapper,
            ISender sender)
        {
            _mapper = mapper;
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<MovieDto> moviesList = new List<MovieDto>();

            var result = await _sender.Send(new GetAllMoviesQuery());

            return View(result.Movies);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateMovieDto model)
        {
            if (ModelState.IsValid)
            {
                var command = _mapper.Map<CreateMovieCommand>(model);

                var result = await _sender.Send(command);

                if (result.IsSuccess)
                {
                    return RedirectToAction("Index", "Movie");
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Update(UpdateMovieDto model)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Delete(int movieId) 
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Delete(DeleteMovieDto model) 
        {
            return View();
        }
    }
}
