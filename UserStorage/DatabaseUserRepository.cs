namespace UserStorage;

public class DatabaseUserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public DatabaseUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> FindByUserNameAsync(string userName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName, nameof(userName));

        CheckValidUserName(userName);

        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task SaveAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        CheckValidUserName(user.UserName);

        var userExisted = await _context.Users.AsNoTracking().AnyAsync(u => u.UserName == user.UserName);
        if (!userExisted)
        {
            await _context.Users.AddAsync(user);
        }
        else
        {
            foreach (var entry in _context.ChangeTracker.Entries<User>())
            {
                entry.State = EntityState.Detached;
            }
            _context.Users.Attach(user);
            _context.Users.Entry(user).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();
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