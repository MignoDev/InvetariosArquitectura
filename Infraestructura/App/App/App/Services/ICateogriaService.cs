using ProyectoInventario.Domain.Models;

namespace AppBlazor.Services
{
    public interface ICateogriaService
    {
        Task<List<Categoria>> ObtenerTodasAsync();
        Task<ServiceResult<Categoria>> CrearAsync(Categoria categoria);
        Task<ServiceResult<Categoria>> ActualizarAsync(Categoria categoria);

    }
}
