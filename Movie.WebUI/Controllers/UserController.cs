using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movie.BuildingBlocks;
using Movie.WebUI.Services.Handlers.Users.Commands;

namespace Movie.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public UserController(ISender sender,
            IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.RoleList = LoadUserRoles();
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDto registerationRequest)
        {
            var command = _mapper.Map<RegisterCommand>(registerationRequest);

            var result = await _sender.Send(command);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }

            if(result.ErrorMessages.Count() > 0)
            {
                foreach(var error in result.ErrorMessages)
                {
                    ModelState.AddModelError(String.Empty, error);
                }  
            }

            ViewBag.RoleList = LoadUserRoles();
            return View();
        }

        public IActionResult LogOut()
        {
            return Ok();
        }

        private List<SelectListItem> LoadUserRoles()
        {
            var roleList = new List<SelectListItem>();

            foreach (var userRole in StaticDetails.userRolesDict)
            {
                var role = userRole.Value.ToLower();
                roleList.Add(new SelectListItem { Text = role, Value = role });
            }

            return roleList;
        }
    }
}
