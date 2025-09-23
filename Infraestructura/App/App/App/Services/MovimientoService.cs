using AppBlazor.Models;
using ProyectoInventario.Domain.Models;

namespace AppBlazor.Services
{
    /// <summary>
    /// Servicio para gestión de movimientos de inventario con transformación automática
    /// </summary>
    public class MovimientoService : BaseService
    {
        public MovimientoService(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// Registra una entrada de producto
        /// </summary>
        public async Task<ServiceResult<EntradaProductoConDetalles>> RegistrarEntradaAsync(EntradaProducto entrada)
        {
            return await ExecuteAsync(async () => await PostAsync<EntradaProductoConDetalles>("api/inventario/entradas", entrada));
        }

        /// <summary>
        /// Registra una salida de producto
        /// </summary>
        public async Task<ServiceResult<SalidaProductoConDetalles>> RegistrarSalidaAsync(SalidaProducto salida)
        {
            return await ExecuteAsync(async () => await PostAsync<SalidaProductoConDetalles>("api/inventario/salidas", salida));
        }

        /// <summary>
        /// Obtiene el historial de movimientos de un producto
        /// </summary>
        public async Task<List<object>> ObtenerHistorialMovimientosAsync(Guid productoId)
        {
            return await GetListAsync<object>($"api/inventario/productos/{productoId}/movimientos");
        }
    }
}
