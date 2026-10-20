using Proyeccion2050.Domain;

namespace Proyeccion2050.Test;

public class ServicioIndicadoresTests
{
  
    private static List<FilaPoblacion> CargarFilas()
    {
      
        var ruta = Path.Combine(AppContext.BaseDirectory, "data", "limpio.csv");
        var lineas = File.ReadAllLines(ruta);

        var filas = new List<FilaPoblacion>();

        foreach (var linea in lineas.Skip(1)) 
        {
            var p = linea.Split(','); 

           
            Sexo sexo = p[1] switch
            {
                "Hombres" => Sexo.Hombre,
                "Mujeres" => Sexo.Mujer,
                "Ambos sexos" => Sexo.AmbosSexos,
                _ => throw new FormatException($"Sexo desconocido: '{p[1]}'")
            };

            filas.Add(new FilaPoblacion(
                sexo,
                int.Parse(p[4]),      
                decimal.Parse(p[2]),  
                int.Parse(p[3])));    
        }

        return filas;
    }

    [Theory]


    [InlineData(1950, 45.4, 2.7, 92.7, 97.5)]
    [InlineData(2035, 22.9, 11.4, 52.4, 101.3)]
    [InlineData(2050, 19.4, 16.0, 54.9, 102.2)]

    public void Calcular_Esperados(int anio, decimal MenoresDe15, decimal MayoresDe65, decimal RazonDeDependencia, decimal Feminidad) {
        var filas = CargarFilas();
        var servicio = new ServicioIndicadores();
        var resultado = servicio.Calcular(filas, anio);
        Assert.Equal(MenoresDe15, resultado.PcMenores15);
        Assert.Equal(MayoresDe65, resultado.PcMayores65);
        Assert.Equal(RazonDeDependencia, resultado.RazonDependencia);
        Assert.Equal(Feminidad, resultado.RazonFeminidad);
    }
}
