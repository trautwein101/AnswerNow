using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AnswerNow.Data;

public class AnswerNowDbContextFactory : IDesignTimeDbContextFactory<AnswerNowDbContext>
{
    public AnswerNowDbContext CreateDbContext(string[] args)
    {
        // Pull from env var so it works locally and in CI
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Default")
            ?? "Host=localhost;Port=5432;Database=answernow_dev;Username=answernow;Password=answernow_pw";

        var optionsBuilder = new DbContextOptionsBuilder<AnswerNowDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AnswerNowDbContext(optionsBuilder.Options);
    }
}
