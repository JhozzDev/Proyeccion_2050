using Proyeccion2050.Domain;

namespace Proyeccion2050.Infrastructure;

public static class LectorCsv
{
    public static List<FilaPoblacion> Leer()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "data")))
        {
            dir = dir.Parent;
        }

        if (dir == null)
            throw new InvalidOperationException("No se encontró la carpeta 'data' subiendo desde AppContext.BaseDirectory");

        var ruta = Path.Combine(dir.FullName, "data", "limpio.csv");
        var lineas = File.ReadAllLines(ruta);

        var lista = new List<FilaPoblacion>(1100);

        foreach (var linea in lineas.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(linea))
                continue;

            var p = linea.Split(',');

            Sexo sexo = p[1] switch
            {
                "Hombres" => Sexo.Hombre,
                "Mujeres" => Sexo.Mujer,
                "Ambos sexos" => Sexo.AmbosSexos,
                _ => throw new FormatException($"Sexo desconocido: '{p[1]}'")
            };

            // 0=edad 1=sexo 2=poblacion 3=anio 4=edad_min
            lista.Add(new FilaPoblacion(
                sexo,
                int.Parse(p[4]),
                decimal.Parse(p[2]),
                int.Parse(p[3])
            ));
        }

        return lista;
    }
}
