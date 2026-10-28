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

app.MapGet("/indicadores", ([FromServices] Proyeccion2050Context data) => {
    var anios = data.FilasPoblacion.Select(f => f.Anio).Distinct().OrderBy(anio => anio).ToList();
    var service = new ServicioIndicadores();
    var resultados = new List<Indicadores>();
    var filasPorAnio = data.FilasPoblacion
     .AsEnumerable()
     .GroupBy(f => f.Anio)
     .OrderBy(grupo => grupo.Key);
    foreach (var grupo in filasPorAnio)
    {
        
        resultados.Add(service.Calcular(grupo, grupo.Key));
    };

    return Results.Ok(resultados);

});

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

app.MapGet("/indicadores/serie", ([FromServices] Proyeccion2050Context db) =>
{
    var anios = new[] { 1950, 1955, 1960, 1965, 1970, 1975, 1980, 1985, 1990, 1995, 2000, 2005, 2010, 2015, 2020, 2025, 2030, 2035, 2040, 2045, 2050 };
    var servicio = new ServicioIndicadores();
    var lista = new List<Indicadores>();

    foreach (var a in anios)
    {
        var filas = db.FilasPoblacion.Where(f => f.Anio == a).ToList();
        if (filas.Count == 0) continue;
        lista.Add(servicio.Calcular(filas, a));
    }

    return lista;
});

app.Run();
