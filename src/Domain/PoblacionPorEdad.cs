namespace Proyeccion2050.Domain;

public record PoblacionPorEdad(int EdadMin, decimal Poblacion)
{
    public string Etiqueta => EdadMin >= 80 ? "80+" : $"{EdadMin}-{EdadMin + 4}";
}