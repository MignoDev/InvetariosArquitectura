using AppBlazor.Models;
using ProyectoInventario.Domain.Models;

namespace AppBlazor.Services
{
    /// <summary>
    /// Servicio para gestión de categorías con transformación automática
    /// </summary>
    public class CategoriaService : BaseService, ICateogriaService
    {
        public CategoriaService(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// Obtiene todas las categorías activas
        /// </summary>
        public async Task<List<Categoria>> ObtenerTodasAsync()
        {
            return await GetListAsync<Categoria>("api/categorias");
        }

        /// <summary>
        /// Crea una nueva categoría
        /// </summary>
        public async Task<ServiceResult<Categoria>> CrearAsync(Categoria categoria)
        {
            return await ExecuteAsync(async () => await PostAsync<Categoria>("api/categorias", categoria));
        }

        /// <summary>
        /// Crea una nueva categoría
        /// </summary>
        public async Task<ServiceResult<Categoria>> ActualizarAsync(Categoria categoria)
        {
            return await ExecuteAsync(async () => await PutAsync<Categoria>("api/categorias", categoria));
        }
    }
}
