namespace AccountDemo.Helpers;

public static class AccountDemoHelpers
{
    public static bool IsValidUserName(string userName)
    {
        return !string.IsNullOrWhiteSpace(userName) && userName.All((c) => char.IsAsciiLetterOrDigit(c) || c == '-');
    }

    public static async Task CreateNewUserAsync(this IUserRepository userRepository, RegisterModel model, bool isAdmin = false)
    {
        var roles = new List<string> { "User" };
        if (isAdmin)
        {
            roles.Add("Admin");
        }

        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email ?? string.Empty,
            PhoneNumber = model.PhoneNumber ?? string.Empty,
            Password = model.Password ?? string.Empty,
            Roles = roles
        };

        await userRepository.SaveAsync(user);
    }

    public static async Task<List<string>> GetUserRolesAsync(this IUserRepository userRepository, string userName)
    {
        var user = await userRepository.FindByUserNameAsync(userName);
        return user?.Roles ?? [];
    }

    public static async Task<string> GetUserPasswordAsync(this IUserRepository userRepository, string userName)
    {
        var user = await userRepository.FindByUserNameAsync(userName);
        return user?.Password ?? string.Empty;
    }

    public static async Task ChangeUserInfoAsync(this IUserRepository userRepository, UserProfileModel model, bool isAdmin = false)
    {
        var roles = await userRepository.GetUserRolesAsync(model.UserName);

        if (isAdmin && !roles.Contains("Admin"))
        {
            roles.Add("Admin");
        }

        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email ?? string.Empty,
            PhoneNumber = model.PhoneNumber ?? string.Empty,
            Password = model.NewPassword ?? await userRepository.GetUserPasswordAsync(model.UserName),
            Roles = roles
        };

        await userRepository.SaveAsync(user);
    }
}