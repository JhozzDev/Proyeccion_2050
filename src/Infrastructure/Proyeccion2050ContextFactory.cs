using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Proyeccion2050.Infrastructure;

public class Proyeccion2050ContextFactory : IDesignTimeDbContextFactory<Proyeccion2050Context>
{
    public Proyeccion2050Context CreateDbContext(string[] args)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        // Subimos desde bin/Debug/net10.0 hasta encontrar la carpeta data/ de la raíz del repo
        while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "data")))
        {
            dir = dir.Parent;
        }

        if (dir == null)
            throw new InvalidOperationException("No se encontró la carpeta 'data' subiendo desde AppContext.BaseDirectory");

        var dbPath = Path.Combine(dir.FullName, "data", "proyeccion2050.db");
        var optionsBuilder = new DbContextOptionsBuilder<Proyeccion2050Context>();
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new Proyeccion2050Context(optionsBuilder.Options);
    }
}