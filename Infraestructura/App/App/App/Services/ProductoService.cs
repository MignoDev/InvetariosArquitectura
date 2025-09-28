using AppBlazor.Models;
using ProyectoInventario.Domain.Models;

namespace AppBlazor.Services
{
    /// <summary>
    /// Servicio para gestión de productos con transformación automática
    /// </summary>
    public class ProductoService : BaseService
    {
        public ProductoService(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// Obtiene todos los productos activos con stock
        /// </summary>
        public async Task<List<ProductoConStock>> ObtenerTodosAsync()
        {
            return await GetListAsync<ProductoConStock>("api/inventario/productos");
        }

        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        public async Task<ProductoConStock?> ObtenerPorIdAsync(Guid id)
        {
            return await GetModelAsync<ProductoConStock>($"api/inventario/productos/{id}");
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        public async Task<ServiceResult<ProductoConStock>> CrearAsync(Producto producto)
        {
            return await ExecuteAsync(async () => await PostAsync<ProductoConStock>("api/inventario/productos", producto));
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        public async Task<ServiceResult<ProductoConStock>> ActualizarAsync(Producto producto)
        {
            return await ExecuteAsync(async () => await PutAsync<ProductoConStock>($"api/inventario/productos/{producto.Id}", producto));
        }

        /// <summary>
        /// Obtiene el stock de un producto
        /// </summary>
        public async Task<Stock?> ObtenerStockAsync(Guid productoId)
        {
            return await GetModelAsync<Stock>($"api/inventario/productos/{productoId}/stock");
        }

        /// <summary>
        /// Ajusta el stock de un producto
        /// </summary>
        public async Task<ServiceResult<Stock>> AjustarStockAsync(Guid productoId, int cantidad)
        {
            var request = new { id = productoId, NuevaCantidad = cantidad };
            return await ExecuteAsync(async () => await PutAsync<Stock>($"api/inventario/productos/stock/ajustar", request));
        }
    }
}
