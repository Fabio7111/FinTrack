using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinTrack.Models;

namespace FinTrack.Services
{
    public interface IReportService
    {
        Task<decimal[]> GetTotals(int ano, string usuarioId);
        Task<(string[] labels, decimal[] data)> GetByCategory(DateTime inicio, DateTime fim, string usuarioId);
        Task<List<PivotDto>> GetPivotByYear(int ano, string usuarioId);
    }

    public class PivotDto
    {
        public string Categoria { get; set; } = string.Empty;
        public decimal[] Valores { get; set; } = new decimal[12];
    }
}
