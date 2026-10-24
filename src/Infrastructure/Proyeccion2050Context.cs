using Microsoft.EntityFrameworkCore;
using Proyeccion2050.Domain;

namespace Proyeccion2050.Infrastructure;

public class Proyeccion2050Context : DbContext
{
  
    public Proyeccion2050Context(DbContextOptions<Proyeccion2050Context> options)
        : base(options)
    {
    }

    public DbSet<FilaPoblacion> FilasPoblacion => Set<FilaPoblacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      
        modelBuilder.Entity<FilaPoblacion>()
            .HasKey(x => new { x.Sexo, x.EdadMin, x.Anio });

        base.OnModelCreating(modelBuilder);
    }
}
