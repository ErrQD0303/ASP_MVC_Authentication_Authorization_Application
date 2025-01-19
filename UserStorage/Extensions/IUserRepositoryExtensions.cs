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

    public static IHostApplicationBuilder AddUserRepository(this IHostApplicationBuilder builder, UserRepository type = UserRepository.JsonFile)
    {
        switch (type)
        {
            case UserRepository.JsonFile:
                builder.Services.Configure<JsonFileUserRepositoryOptions>(
                    builder.Configuration.GetSection("JsonDatabaseOptions"));

                builder.Services.AddScoped<IUserRepository, JsonFileUserRepository>();

                break;

            case UserRepository.Database:
                builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                builder.Services.AddScoped<IUserRepository, DatabaseUserRepository>();

                break;
            default:
                throw new ArgumentException("Invalid user repository type");
        }

        return builder;
    }
}