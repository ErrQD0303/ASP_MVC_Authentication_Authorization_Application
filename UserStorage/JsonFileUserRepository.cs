namespace UserStorage;

public class JsonFileUserRepository : IUserRepository
{
    private readonly string _dbPath;

    public JsonFileUserRepository(IOptions<JsonFileUserRepositoryOptions> options)
    {
        _dbPath = options.Value.DatabasePath;

        if (!Directory.Exists(_dbPath))
        {
            Directory.CreateDirectory(_dbPath);
        }
    }

    public async Task<User?> FindByUserNameAsync(string userName)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(userName, nameof(userName));

        CheckValidUserName(userName);

        var fileInfo = new FileInfo(Path.Combine(_dbPath, userName));
        if (!fileInfo.Exists)
        {
            return null;
        }

        var json = await File.ReadAllTextAsync(fileInfo.FullName);
        return JsonSerializer.Deserialize<User>(json);
    }

    public Task SaveAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        CheckValidUserName(user.UserName);

        var fileInfo = new FileInfo(Path.Combine(_dbPath, user.UserName));
        var json = JsonSerializer.Serialize(user);
        return File.WriteAllTextAsync(fileInfo.FullName, json);
    }

    private static void CheckValidUserName(string userName)
    {
        if (userName.All(c => char.IsLetterOrDigit(c) || c == '-'))
        {
            return;
        }

        throw new ArgumentException("Invalid user name", nameof(userName));
    }
}