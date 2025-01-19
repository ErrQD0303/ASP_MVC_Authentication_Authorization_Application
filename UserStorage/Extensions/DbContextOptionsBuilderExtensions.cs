namespace UserStorage.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseSeedData(this DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSeeding((context, _) =>
                {
                    context.SeedAsync(CancellationToken.None).Wait();
                })
                .UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    await context.SeedAsync(cancellationToken);
                });

            return optionsBuilder;
        }
    }
}