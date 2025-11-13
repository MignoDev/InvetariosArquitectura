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
        Task<Stock> ObtenerPorIdAsync(int id);
        Task<Stock> ObtenerPorProductoIdAsync(int productoId);
        Task<IEnumerable<Stock>> ObtenerTodosAsync();
        Task<Stock> CrearAsync(Stock stock);
        Task<Stock> ActualizarAsync(Stock stock);
        Task EliminarAsync(int id);

        // Operaciones de búsqueda
        Task<IEnumerable<Stock>> BuscarPorUbicacionAsync(string ubicacion);
        Task<IEnumerable<Stock>> BuscarConStockBajoAsync();
        Task<IEnumerable<Stock>> BuscarConExcesoStockAsync();
        Task<IEnumerable<Stock>> BuscarAgotadosAsync();

        // Operaciones de stock
        Task<Stock> AjustarStockAsync(int productoId, int nuevaCantidad);
        Task<Stock> AgregarStockAsync(int productoId, int cantidad);
        Task<Stock> ReducirStockAsync(int productoId, int cantidad);

        // Operaciones de validación
        Task<bool> TieneStockDisponibleAsync(int productoId, int cantidadRequerida);
        Task<int> ObtenerCantidadDisponibleAsync(int productoId);

        // Operaciones de conteo
        Task<int> ContarConStockBajoAsync();
        Task<int> ContarAgotadosAsync();
        Task<int> ContarConExcesoStockAsync();
    }
}
