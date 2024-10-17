namespace Movie.WebUI.Controllers;

public class UserController : Controller
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly ITokenProvider _tokenProvider;

    public UserController(ISender sender,
        IMapper mapper,
        ITokenProvider tokenProvider)
    {
        _sender = sender;
        _mapper = mapper;
        _tokenProvider = tokenProvider;
    }
    [HttpGet]
    public IActionResult Login()
    {
        var model = new LoginRequestDto();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var command = _mapper.Map<LoginCommand>(request);

        var result = await _sender.Send(command);

        if (result.IsSuccess)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("LoginError", result.ErrorMessage);
        return View(request);
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

    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync();

        HttpContext.Session.Clear();

        var token = _tokenProvider.GetToken();

        if(token != null)
        {
            var command = new LogoutCommand(token);

            var result = await _sender.Send(command);

            _tokenProvider.ClearToken();
        }

        return RedirectToAction("Index", "Home");
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
