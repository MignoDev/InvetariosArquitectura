using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Models;

namespace ProyectoInventario.Domain.Ports
{
    /// <summary>
    /// Puerto (interfaz) para el repositorio de stock
    /// Define el contrato para la persistencia de stock
    /// </summary>
    public interface IRepositorioStock
    {
        // Operaciones CRUD básicas
        Task<Stock> ObtenerPorIdAsync(Guid id);
        Task<Stock> ObtenerPorProductoIdAsync(Guid productoId);
        Task<IEnumerable<Stock>> ObtenerTodosAsync();
        Task<Stock> CrearAsync(Stock stock);
        Task<Stock> ActualizarAsync(Stock stock);
        Task EliminarAsync(Guid id);

        // Operaciones de búsqueda
        Task<IEnumerable<Stock>> BuscarPorUbicacionAsync(string ubicacion);
        Task<IEnumerable<Stock>> BuscarConStockBajoAsync();
        Task<IEnumerable<Stock>> BuscarConExcesoStockAsync();
        Task<IEnumerable<Stock>> BuscarAgotadosAsync();

        // Operaciones de stock
        Task<Stock> AjustarStockAsync(Guid productoId, int nuevaCantidad);
        Task<Stock> AgregarStockAsync(Guid productoId, int cantidad);
        Task<Stock> ReducirStockAsync(Guid productoId, int cantidad);

        // Operaciones de validación
        Task<bool> TieneStockDisponibleAsync(Guid productoId, int cantidadRequerida);
        Task<int> ObtenerCantidadDisponibleAsync(Guid productoId);

        // Operaciones de conteo
        Task<int> ContarConStockBajoAsync();
        Task<int> ContarAgotadosAsync();
        Task<int> ContarConExcesoStockAsync();
    }
}
