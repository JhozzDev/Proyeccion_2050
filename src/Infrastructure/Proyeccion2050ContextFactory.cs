using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Proyeccion2050.Infrastructure;

public class Proyeccion2050ContextFactory : IDesignTimeDbContextFactory<Proyeccion2050Context>
{
    public Proyeccion2050Context CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Proyeccion2050Context>();
        optionsBuilder.UseSqlite("Data Source=data/proyeccion2050.db");

        return new Proyeccion2050Context(optionsBuilder.Options);
    }
}
