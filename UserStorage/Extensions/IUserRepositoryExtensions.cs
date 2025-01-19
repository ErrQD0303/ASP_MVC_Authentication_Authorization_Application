namespace UserStorage.Extensions;

public static class IUserRepositoryExtensions
{
    public static IHostApplicationBuilder AddUserRepository(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<JsonFileUserRepositoryOptions>(
            builder.Configuration.GetSection("JsonDatabaseOptions"));
        var path = builder.Configuration.GetSection("JsonDatabaseOptions");
        builder.Services.AddScoped<IUserRepository, JsonFileUserRepository>();

        return builder;
    }
}