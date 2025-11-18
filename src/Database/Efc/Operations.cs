using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace HowProgrammingWorksOnDotNet.Database.Efc;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}

public class Efc : IDesignTimeDbContextFactory<AppDbContext>
{
    [Fact]
    public void Usage()
    {
        var context = CreateDbContext([]);

        // Пример явного создания транзакции. Но это не IDbTransaction
        using var transaction = context.Database.BeginTransaction();
    }

    public AppDbContext CreateDbContext(string[] args)
    {
        var dbOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Username = "postgres",
            Password = "password",
            Host = "localhost",
            Port = 5445,
            Database = "estore",
        };
        dbOptionsBuilder.UseNpgsql(connectionStringBuilder.ConnectionString, options => { });
        var options = dbOptionsBuilder.Options;

        var context = new AppDbContext(options);
        return context;
    }
}
