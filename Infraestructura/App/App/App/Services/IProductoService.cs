using ProyectoInventario.Domain.Models;

namespace AppBlazor.Services
{
    public interface IProductoService
    {

        Task<List<Producto>> ObtenerTodosAsync();
        Task<Producto?> ObtenerPorIdAsync(int id);
        Task<ServiceResult<Producto>> CrearAsync(Producto producto);
        Task<ServiceResult<Producto>> ActualizarAsync(Producto producto);


    }
}
