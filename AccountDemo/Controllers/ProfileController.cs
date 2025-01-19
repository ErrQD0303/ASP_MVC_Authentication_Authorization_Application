namespace AccountDemo.Controllers;

public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IUserRepository _userRepository;

    public ProfileController(ILogger<ProfileController> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var user = await _userRepository.FindByUserNameAsync(User.Identity!.Name!);
        if (user == null)
        {
            return NotFound();
        }

        var model = new UserProfileModel
        {
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Roles = user.Roles,
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SaveAsync(UserProfileModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), model);
        }

        try
        {
            EnsureValidUserName(model.UserName);

            await EnsureValidPassword(model.Password, model.NewPassword, model.ConfirmPassword);

            var user = _userRepository.ChangeUserInfoAsync(model);

            TempData["SuccessMessage"] = "User information updated successfully";

            return RedirectToAction(nameof(Index));
        }
        catch (ModelException e)
        {
            ModelState.AddModelError(e.PropertyName, e.Message);

            return View(nameof(Index), model);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }

    private static void EnsureValidUserName(string userName)
    {
        if (AccountDemoHelpers.IsValidUserName(userName))
        {
            return;
        }

        throw new ModelException("User name can contain only letters, digits and hyphen", nameof(UserProfileModel.UserName));
    }

    private async Task EnsureValidPassword(string? password, string? newPassword, string? confirmPassword)
    {
        var user = (await _userRepository.FindByUserNameAsync(User.Identity!.Name!)) ?? throw new ModelException("User not found", nameof(UserProfileModel.UserName));

        var userCurrentPassword = user.Password;

        if (string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        if (!password.Equals(userCurrentPassword))
        {
            throw new ModelException("Password not valid", nameof(UserProfileModel.Password));
        }

        if (string.IsNullOrWhiteSpace(newPassword) && String.IsNullOrWhiteSpace(confirmPassword))
        {
            return;
        }

        if (!newPassword!.Equals(confirmPassword))
        {
            throw new ModelException("New Password and Confirm Password must match", nameof(UserProfileModel.ConfirmPassword));
        }

        if (string.Equals(userCurrentPassword, newPassword, StringComparison.Ordinal))
        {
            throw new ModelException("New Password must be different from the current password", nameof(UserProfileModel.NewPassword));
        }
    }
}