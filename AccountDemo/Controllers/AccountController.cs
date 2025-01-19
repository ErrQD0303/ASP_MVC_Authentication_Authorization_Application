namespace AccountDemo.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IUserRepository _userRepository;

    public AccountController(ILogger<AccountController> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    [HttpGet]
    public IActionResult LogOn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LogOnAsync(LogOnModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            EnsureValidUserName(model.UserName);

            EnsureValidUserCredentials(model, out var user);

            var claims = CreateClaims(user);

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction(nameof(ProfileController.IndexAsync).Replace("Async", ""), nameof(ProfileController).Replace("Controller", ""));
        }
        catch (ModelException e)
        {
            ModelState.AddModelError(e.PropertyName, e.Message);

            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> LogOffAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", ""));
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            EnsureValidUserName(model.UserName, "User name can contain only letters, digits and hypen");

            await _userRepository.CreateNewUserAsync(model);

            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", ""));
        }
        catch (ModelException e)
        {
            ModelState.AddModelError(e.PropertyName, e.Message);

            return View(model);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }

    private static void EnsureValidUserName(string userName, string description = "")
    {
        if (AccountDemoHelpers.IsValidUserName(userName))
        {
            return;
        }

        throw new ModelException($"Invalid user name. {description}", nameof(RegisterModel.UserName));
    }

    private void EnsureNotExistedUserName(string userName)
    {
        if (_userRepository.FindByUserNameAsync(userName) == null)
        {
            return;
        }

        throw new ModelException("User name already existed", nameof(RegisterModel.UserName));
    }

    private void EnsureValidUserCredentials(LogOnModel model, out User user)
    {
        user = _userRepository.FindByUserNameAsync(model.UserName).Result!;

        if (user != null && string.Equals(user.Password, model.Password, StringComparison.Ordinal))
        {
            return;
        }

        throw new ModelException("Invalid user name or password", string.Empty);
    }

    private static List<Claim> CreateClaims(User user)
    {
        var claims = new List<Claim> {
                new(ClaimTypes.Sid, user.UserName),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}