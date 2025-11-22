using FinTrack.Data;
using FinTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Services
{
    public class ReportService : IReportService
    {
        private readonly FinTrackContext _context;

        public ReportService(FinTrackContext context)
        {
            _context = context;
        }

        // --------------------------------------------------------------------
        // 1) Totais mensais (array de 12 posições)
        // --------------------------------------------------------------------
        public async Task<decimal[]> GetTotals(int ano, string usuarioId)
        {
            var valores = new decimal[12];

            var query = _context.Transacoes
                .Include(t => t.Categoria)
                .Where(t => t.UsuarioId == usuarioId && t.Data.Year == ano);

            var grupos = await query
                .GroupBy(t => t.Data.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    Total = g.Sum(t =>
                        t.Categoria.Tipo == TipoCategoria.Despesa ? -t.Valor : t.Valor)
                })
                .ToListAsync();

            foreach (var item in grupos)
            {
                valores[item.Mes - 1] = item.Total;
            }

            return valores;
        }

        // --------------------------------------------------------------------
        // 2) Totais por categoria (labels + data)
        // --------------------------------------------------------------------
        public async Task<(string[] labels, decimal[] data)> GetByCategory(
            DateTime inicio, DateTime fim, string usuarioId)
        {
            fim = fim.Date.AddDays(1).AddTicks(-1);

            var query = _context.Transacoes
                .Include(t => t.Categoria)
                .Where(t => t.UsuarioId == usuarioId &&
                            t.Data >= inicio &&
                            t.Data <= fim);

            var grupos = await query
                .GroupBy(t => t.Categoria.Nome)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Total = g.Sum(t =>
                        t.Categoria.Tipo == TipoCategoria.Despesa ? -t.Valor : t.Valor)
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();

            return (
                labels: grupos.Select(g => g.Categoria).ToArray(),
                data: grupos.Select(g => g.Total).ToArray()
            );
        }

        // --------------------------------------------------------------------
        // 3) Pivot categoria x meses
        // --------------------------------------------------------------------
        public async Task<List<PivotDto>> GetPivotByYear(int ano, string usuarioId)
        {
            var query = _context.Transacoes
                .Include(t => t.Categoria)
                .Where(t => t.UsuarioId == usuarioId && t.Data.Year == ano);

            var grupos = await query
                .GroupBy(t => new { t.Categoria.Nome, Mes = t.Data.Month })
                .Select(g => new
                {
                    Categoria = g.Key.Nome,
                    g.Key.Mes,
                    Total = g.Sum(t =>
                        t.Categoria.Tipo == TipoCategoria.Despesa ? -t.Valor : t.Valor)
                })
                .ToListAsync();

            var dict = new Dictionary<string, PivotDto>();

            foreach (var item in grupos)
            {
                if (!dict.TryGetValue(item.Categoria, out var dto))
                {
                    dto = new PivotDto { Categoria = item.Categoria };
                    dict[item.Categoria] = dto;
                }

                dto.Valores[item.Mes - 1] = item.Total;
            }

            return dict.Values.OrderBy(d => d.Categoria).ToList();
        }
    }
}
