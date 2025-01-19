namespace UserStorage.Seeds
{
    public static class SeedDataExtensions
    {
        public static async Task SeedAsync(this DbContext context, CancellationToken cancellationToken)
        {
            if (context is not ApplicationDbContext appDbContext)
            {
                return;
            }

            if (appDbContext.Users.Any())
            {
                return;
            }

            var seedUsers = new List<User>
            {
                new(){
                    UserName = "admin",
                    Email = "admin@localhost.com",
                    PhoneNumber = "1234567890",
                    Password = "shinichi",
                    Roles = new() {
                        "Admin",
                        "User"
                    }
                }
            };

            await context.AddRangeAsync(seedUsers, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}