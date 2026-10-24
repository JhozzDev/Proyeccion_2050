using Proyeccion2050.Domain;
namespace Proyeccion2050.Infrastructure
{
    public class ImportarDatos
    {
        public static int ImportarData(Proyeccion2050Context contexto)
        {
            if (contexto.FilasPoblacion.Any())
                contexto.FilasPoblacion.RemoveRange(contexto.FilasPoblacion);

            contexto.SaveChanges();

           
            var filas = LectorCsv.Leer();
            contexto.FilasPoblacion.AddRange(filas);
            contexto.SaveChanges();

            return filas.Count;

        }
    }
}
