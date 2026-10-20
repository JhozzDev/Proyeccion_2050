namespace Proyeccion2050.Domain;

public class ServicioIndicadores
{
    public Indicadores Calcular(IEnumerable<FilaPoblacion> filas, int anio)
    {
        var filasDelAnio = filas.Where(f => f.Anio == anio).ToList();
        var total = filasDelAnio.Where(f => f.Sexo == Sexo.AmbosSexos).Sum(f => f.Poblacion);
        var sumaMenores15 = filasDelAnio.Where(f => f.EdadMin < 15 && f.Sexo == Sexo.AmbosSexos).Sum(f => f.Poblacion);
        var sumaMayores65 = filasDelAnio.Where(f => f.EdadMin >= 65 && f.Sexo == Sexo.AmbosSexos).Sum(f => f.Poblacion);
        var razonFeminidad = filasDelAnio.Where(f => f.Sexo == Sexo.Mujer).Sum(f => f.Poblacion) / filasDelAnio.Where(f => f.Sexo == Sexo.Hombre).Sum(f => f.Poblacion);
        var suma15a64 = filasDelAnio.Where(f => f.EdadMin >= 15 && f.EdadMin < 65 && f.Sexo == Sexo.AmbosSexos).Sum(f => f.Poblacion);
        var razonDependencia = (sumaMenores15 + sumaMayores65) / suma15a64 * 100;

        return new Indicadores(
            anio,
            Math.Round(sumaMenores15 / total * 100, 1),
            Math.Round(sumaMayores65 / total * 100, 1),
            Math.Round(razonFeminidad * 100, 1),
            Math.Round(razonDependencia, 1)
        );
    }
}
