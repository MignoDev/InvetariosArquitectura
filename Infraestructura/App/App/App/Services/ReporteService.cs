using AppBlazor.Models;

namespace AppBlazor.Services
{
    /// <summary>
    /// Servicio para generación de reportes con transformación automática
    /// </summary>
    public class ReporteService : BaseService
    {
        public ReporteService(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// Genera un reporte de stock actual
        /// </summary>
        public async Task<List<object>> GenerarReporteStockActualAsync()
        {
            return await GetListAsync<object>("api/inventario/reportes/stock-actual");
        }

        /// <summary>
        /// Genera un reporte de estadísticas generales
        /// </summary>
        public async Task<object?> GenerarReporteEstadisticasAsync()
        {
            return await GetModelAsync<object>("api/inventario/reportes/estadisticas");
        }
    }
}
