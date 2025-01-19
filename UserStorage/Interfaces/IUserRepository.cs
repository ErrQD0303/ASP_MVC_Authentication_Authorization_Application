namespace UserStorage.Interfaces;

public interface IUserRepository
{
    Task<User?> FindByUserNameAsync(string userName);
    Task SaveAsync(User user);
}
