using BilleteraApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraApp.Services
{
    public class RecomendacionService : IRecomendacionService
    {

        private readonly BilleteraContext _context;

        public RecomendacionService(BilleteraContext context)
        {
            _context = context;
        }
        public void GenerarRecomendaciones(int usuarioId)
        {
            var saldo = _context.Saldos.FirstOrDefault(s => s.UsuarioId == usuarioId);
            var gastos = _context.Gastos.Include(g => g.Categoria)
                                        .Where(g => g.UsuarioId == usuarioId).ToList();

            if (saldo == null || !gastos.Any()) return;

            var totalGastado = gastos.Sum(g => g.Monto);
            if (totalGastado > saldo.MontoActual * 0.7m)
            {
                GuardarRecomendacion(usuarioId, "⚠️ Atención: estás gastando más del 70% de tu saldo este mes.");
            }

            var mesActual = DateTime.Now.Month;
            var anioActual = DateTime.Now.Year;

            var gastosMesActual = gastos.Where(g => g.Fecha.Month == mesActual && g.Fecha.Year == anioActual);

            var gastosMesAnterior = gastos.Where(g => g.Fecha.Month == (mesActual - 1) && g.Fecha.Year == anioActual);

            var categorias = gastosMesActual.GroupBy(g => g.CategoriaId);

            foreach (var categoria in categorias)
            {
                var totalActual = categoria.Sum(x => x.Monto);
                var totalAnterior = gastosMesAnterior.Where(x => x.CategoriaId == categoria.Key).Sum(x => x.Monto);

                if (totalAnterior > 0 && totalActual > totalAnterior * 1.3m)
                {
                    var nombreCategoria = _context.Categorias.Find(categoria.Key)?.Nombre ?? "Categoría desconocida";
                    GuardarRecomendacion(usuarioId, $"📈 Tus gastos en {nombreCategoria} aumentaron más del 30% respecto al mes pasado.");
                }
            }
        }

        public void MarcarComoLeida(int recomendacionId)
        {
            var rec = _context.Recomendaciones.FirstOrDefault(r => r.Id == recomendacionId);
            if (rec != null)
            {
                rec.Activa = false;
                _context.SaveChanges();
            }
        }

        public IEnumerable<Recomendacion> ObtenerRecomendacionesActivas(int usuarioId)
        {
            return _context.Recomendaciones
            .Where(r => r.UsuarioId == usuarioId && r.Activa)
            .OrderByDescending(r => r.Fecha)
            .ToList();
        }

        private void GuardarRecomendacion(int usuarioId, string mensaje)
        {
            // Evitar duplicados exactos en el mismo día
            var existe = _context.Recomendaciones.Any(r =>
                r.UsuarioId == usuarioId &&
                r.Mensaje == mensaje &&
                r.Fecha.Date == DateTime.Now.Date);

            if (!existe)
            {
                var recomendacion = new Recomendacion
                {
                    UsuarioId = usuarioId,
                    Mensaje = mensaje,
                    Fecha = DateTime.Now,
                    Activa = true
                };

                _context.Recomendaciones.Add(recomendacion);
                _context.SaveChanges();
            }
        }

    }
}
