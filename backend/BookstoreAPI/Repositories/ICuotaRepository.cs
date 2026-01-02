using BookstoreAPI.DTOs;
using BookstoreAPI.Models;
using System.Data;

namespace BookstoreAPI.Repositories
{
    public interface ICuotaRepository
    {
        Task<IEnumerable<Cuota>> GetByComprobanteIdAsync(int comprobanteId);
        Task CreateCuotasAsync(int comprobanteId, List<Cuota> cuotas, IDbConnection connection, IDbTransaction transaction);
        Task DeleteByComprobanteIdAsync(int comprobanteId, IDbConnection connection, IDbTransaction transaction);
        Task<IEnumerable<CuotaListadoDto>> GetCuotasByZonaAsync(int? zonaId);
        Task<bool> UpdateImportePagadoAsync(int cuotaId, decimal importePagado);
        Task<bool> UpdateContraEntregaPagadoAsync(int comprobanteId, decimal importePagado);
    }
}
