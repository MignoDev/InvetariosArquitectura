using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Models;

namespace ProyectoInventario.Domain.Ports
{
    /// <summary>
    /// Puerto (interfaz) para el repositorio de productos
    /// Define el contrato para la persistencia de productos
    /// </summary>
    public interface IRepositorioProducto
    {
        // Operaciones CRUD básicas
        Task<Producto> ObtenerPorIdAsync(Guid id);
        Task<Producto> ObtenerPorCodigoAsync(string codigo);
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        Task<IEnumerable<Producto>> ObtenerActivosAsync();
        Task<Producto> CrearAsync(Producto producto);
        Task<Producto> ActualizarAsync(Producto producto);
        Task EliminarAsync(Guid id);

        // Operaciones de búsqueda
        Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre);
        Task<IEnumerable<Producto>> BuscarPorCategoriaAsync(Guid categoriaId);
        Task<IEnumerable<Producto>> BuscarConStockBajoAsync();
        Task<IEnumerable<Producto>> BuscarConExcesoStockAsync();
        Task<IEnumerable<Producto>> BuscarAgotadosAsync();

        // Operaciones de paginación
        Task<(IEnumerable<Producto> Productos, int Total)> ObtenerPaginadosAsync(int pagina, int tamañoPagina);
        Task<(IEnumerable<Producto> Productos, int Total)> BuscarPaginadosAsync(string terminoBusqueda, int pagina, int tamañoPagina);

        // Operaciones de validación
        Task<bool> ExisteCodigoAsync(string codigo);
        Task<bool> ExisteCodigoAsync(string codigo, Guid idExcluir);

        // Operaciones de conteo
        Task<int> ContarTotalAsync();
        Task<int> ContarActivosAsync();
        Task<int> ContarConStockBajoAsync();
        Task<int> ContarAgotadosAsync();
    }
}
