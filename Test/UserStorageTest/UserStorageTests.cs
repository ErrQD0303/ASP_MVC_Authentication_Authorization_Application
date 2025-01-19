using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using UserStorage.Models;

namespace UserStorageTest;

public class UserStorageTests
{
    [Fact]
    public async Task CreateNewUser_SaveItToFile_RetrieveItFromFile_AndCompareProperties_ResultsInEqualityAsync()
    {
        // Arrange
        var user = new User
        {
            UserName = "test-user",
            Email = "testUser@gmail.com",
            PhoneNumber = "0123456789",
            Password = "qwe@123",
            Roles = ["Admin", "User"]
        };

        var userRepositoryOptions = new JsonFileUserRepositoryOptions
        {
            DatabasePath = "TestUserJsonDb"
        };
        var optionsWrapper = new OptionsWrapper<JsonFileUserRepositoryOptions>(userRepositoryOptions);
        var userRepository = new JsonFileUserRepository(optionsWrapper);

        // Act
        await userRepository.SaveAsync(user);
        var retrievedUser = await userRepository.FindByUserNameAsync(user.UserName);

        // Assert
        Assert.NotNull(retrievedUser);
        Assert.Equal(user.UserName, retrievedUser.UserName);
        Assert.Equal(user.Email, retrievedUser.Email);
        Assert.Equal(user.PhoneNumber, retrievedUser.PhoneNumber);
        Assert.Equal(user.Password, retrievedUser.Password);
        Assert.Equal(user.Roles, retrievedUser.Roles);
    }

    [Fact]
    public async Task CreateNewUserWithInvalidUserName_ThrowsArgumentExceptionAsync()
    {
        // Arrange
        var user = new User
        {
            UserName = "test-user@",
            Email = "testUser@gmail.com",
            PhoneNumber = "0123456789",
            Password = "qwe@123",
            Roles = ["Admin", "User"]
        };

        // Act
        var userRepositoryOptions = new JsonFileUserRepositoryOptions
        {
            DatabasePath = "TestUserJsonDb"
        };
        var optionsWrapper = new OptionsWrapper<JsonFileUserRepositoryOptions>(userRepositoryOptions);
        var userRepository = new JsonFileUserRepository(optionsWrapper);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => userRepository.SaveAsync(user));
    }
}
