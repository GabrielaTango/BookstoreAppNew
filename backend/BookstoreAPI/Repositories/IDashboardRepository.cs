using BookstoreAPI.DTOs;

namespace BookstoreAPI.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardStatsDto> GetStatsAsync();
        Task<IEnumerable<ActividadRecienteDto>> GetActividadesRecientesAsync(int cantidad = 10);
    }
}
