using Microsoft.EntityFrameworkCore;

namespace AplicacionE.Helpers
{
    public static class HttpContextExtensions
    {
        public static async Task InsertarParametrosPaginacionEnRespuesta<T>(this HttpContext context,
            IQueryable<T> queryable, int cantidadRegistroAMostar)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            double conteo = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling(conteo / cantidadRegistroAMostar);

            context.Request.Headers.Add("totalPaginas", totalPaginas.ToString());
        }
    }
}
