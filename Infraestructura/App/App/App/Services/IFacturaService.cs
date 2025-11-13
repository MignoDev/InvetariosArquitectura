using Domain.Models;
using ProyectoInventario.Domain.Models;

namespace AppBlazor.Services
{
    public interface IFacturaService
    {
        Task<List<Factura>> ObtenerTodosAsync();
        Task<Factura?> ObtenerPorIdAsync(int id);
        Task<ServiceResult<Factura>> CrearAsync(Factura producto);
        Task<ServiceResult<Factura>> ActualizarAsync(Factura producto);
    }
}
