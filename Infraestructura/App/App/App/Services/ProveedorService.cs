using AppBlazor.Models;
using ProyectoInventario.Domain.Models;

namespace AppBlazor.Services
{
    /// <summary>
    /// Servicio para gestión de proveedores con transformación automática
    /// </summary>
    public class ProveedorService : BaseService
    {
        public ProveedorService(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// Obtiene todos los proveedores activos
        /// </summary>
        public async Task<List<Proveedor>> ObtenerTodosAsync()
        {
            return await GetListAsync<Proveedor>("api/inventario/proveedores");
        }

        /// <summary>
        /// Crea un nuevo proveedor
        /// </summary>
        public async Task<ServiceResult<Proveedor>> CrearAsync(Proveedor proveedor)
        {
            return await ExecuteAsync(async () => await PostAsync<Proveedor>("api/inventario/proveedores", proveedor));
        }
    }
}
