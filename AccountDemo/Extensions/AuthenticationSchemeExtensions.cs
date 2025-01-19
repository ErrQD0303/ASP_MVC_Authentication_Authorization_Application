namespace AccountDemo.Extensions;

public static class AuthenticationSchemeExtensions
{
    public static IHostApplicationBuilder AddAppAuthentication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/Account/LogOn";
                options.AccessDeniedPath = "/Account/LogOn";
                options.LogoutPath = "/Account/LogOff";
                options.Cookie.Name = "MyAccountDemo.Cookie";
            });

        return builder;
    }
}