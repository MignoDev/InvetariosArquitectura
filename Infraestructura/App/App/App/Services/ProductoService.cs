using AppBlazor.Models;
using ProyectoInventario.Domain.Models;

namespace AppBlazor.Services
{
    /// <summary>
    /// Servicio para gestión de productos con transformación automática
    /// </summary>
    public class ProductoService : BaseService, IProductoService
    {
        public ProductoService(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// Obtiene todos los productos activos con stock
        /// </summary>
        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await GetListAsync<Producto>("api/producto");
        }

        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        public async Task<Producto?> ObtenerPorIdAsync(int id)
        {
            return await GetModelAsync<Producto>($"api/producto/{id}");
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        public async Task<ServiceResult<Producto>> CrearAsync(Producto producto)
        {
            return await ExecuteAsync(async () => await PostAsync<Producto>("api/producto", producto));
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        public async Task<ServiceResult<Producto>> ActualizarAsync(Producto producto)
        {
            return await ExecuteAsync(async () => await PutAsync<Producto>($"api/producto/{producto.IdProducto}", producto));
        }

        /// <summary>
        /// Obtiene el stock de un producto
        /// </summary>
        public async Task<Stock?> ObtenerStockAsync(int productoId)
        {
            return await GetModelAsync<Stock>($"api/producto/{productoId}/stock");
        }

        /// <summary>
        /// Ajusta el stock de un producto
        /// </summary>
        public async Task<ServiceResult<Stock>> AjustarStockAsync(int productoId, int cantidad)
        {
            var request = new { id = productoId, NuevaCantidad = cantidad };
            return await ExecuteAsync(async () => await PutAsync<Stock>($"api/inventario/productos/stock/ajustar", request));
        }
    }
}
