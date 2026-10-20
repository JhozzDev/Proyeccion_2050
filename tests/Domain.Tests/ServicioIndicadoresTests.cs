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

    [Fact]
    public void Indicadores_1950_CoincidenConLaOne()
    {
        var filas = CargarFilas();

        var ind = new ServicioIndicadores().Calcular(filas, 1950);

        Assert.Equal(45.4m, ind.PcMenores15);
        Assert.Equal(2.7m, ind.PcMayores65);
        Assert.Equal(92.7m, ind.RazonDependencia);
        Assert.Equal(97.5m, ind.RazonFeminidad);
        Assert.Equal(1950, ind.Anio);
    }
}
