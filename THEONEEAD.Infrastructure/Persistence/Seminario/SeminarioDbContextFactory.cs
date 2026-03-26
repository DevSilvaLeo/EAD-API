using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql;

namespace THEONEEAD.Infrastructure.Persistence.Seminario;

public class SeminarioDbContextFactory : IDesignTimeDbContextFactory<SeminarioDbContext>
{
    public SeminarioDbContext CreateDbContext(string[] args)
    {
        var conn =
            "Server=localhost;Port=3306;Database=seminario;User Id=root;Password=root;";
        var serverVersion = ServerVersion.Parse("8.0.36-mysql");
        var optionsBuilder = new DbContextOptionsBuilder<SeminarioDbContext>();
        optionsBuilder.UseMySql(conn, serverVersion);
        return new SeminarioDbContext(optionsBuilder.Options);
    }
}
