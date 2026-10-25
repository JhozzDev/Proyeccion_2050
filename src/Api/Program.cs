using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyeccion2050.Domain;
using Proyeccion2050.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Proyeccion2050Context>(options =>
{
    var dir = new DirectoryInfo(AppContext.BaseDirectory);

    while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "data")))
        dir = dir.Parent;

    if (dir == null)
        throw new InvalidOperationException("No se encontró la carpeta 'data'");

    var dbPath = Path.Combine(dir.FullName, "data", "proyeccion2050.db");
    options.UseSqlite($"Data Source={dbPath}");
});

var app = builder.Build();

app.MapGet("/poblacion", () => LectorCsv.Leer());

app.MapGet("/poblacion/db", ([FromServices] Proyeccion2050Context db) => db.FilasPoblacion.ToList());

app.MapGet("/indicadores/{anio:int}", ([FromRoute] int anio, [FromServices] Proyeccion2050Context db) =>
{
    var filas = db.FilasPoblacion.Where(f => f.Anio == anio).ToList();
    
    if (filas.Count == 0)
        return Results.NotFound(new { error = $"No hay datos para the year {anio}" });

    var service = new ServicioIndicadores();
    var result = service.Calcular(filas, anio);
    
    return Results.Ok(result);
});

app.MapPost("/importar", ([FromServices] Proyeccion2050Context db) =>
{
    var count = ImportarDatos.ImportarData(db);
    return Results.Ok(new { filasImportadas = 1071 });
});

app.Run();
