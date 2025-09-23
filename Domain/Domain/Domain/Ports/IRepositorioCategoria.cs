using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Models;

namespace ProyectoInventario.Domain.Ports
{
    /// <summary>
    /// Puerto (interfaz) para el repositorio de categorías
    /// Define el contrato para la persistencia de categorías
    /// </summary>
    public interface IRepositorioCategoria
    {
        // Operaciones CRUD básicas
        Task<Categoria> ObtenerPorIdAsync(Guid id);
        Task<Categoria> ObtenerPorNombreAsync(string nombre);
        Task<IEnumerable<Categoria>> ObtenerTodasAsync();
        Task<IEnumerable<Categoria>> ObtenerActivasAsync();
        Task<Categoria> CrearAsync(Categoria categoria);
        Task<Categoria> ActualizarAsync(Categoria categoria);
        Task EliminarAsync(Guid id);

        // Operaciones de búsqueda
        Task<IEnumerable<Categoria>> BuscarPorNombreAsync(string nombre);

        // Operaciones de validación
        Task<bool> ExisteNombreAsync(string nombre);
        Task<bool> ExisteNombreAsync(string nombre, Guid idExcluir);
        Task<bool> TieneProductosAsync(Guid categoriaId);

        // Operaciones de conteo
        Task<int> ContarTotalAsync();
        Task<int> ContarActivasAsync();
    }
}
