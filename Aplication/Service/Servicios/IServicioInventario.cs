using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Models;

namespace ProyectoInventario.Application.Service.Servicios
{
    /// <summary>
    /// Puerto de entrada para el servicio de inventario
    /// Define la interfaz para la gestión de productos, stock y movimientos
    /// </summary>
    public interface IServicioInventario
    {
        #region Gestión de Productos

        /// <summary>
        /// Crea un nuevo producto en el inventario
        /// </summary>
        Task<Producto> CrearProductoAsync(Producto producto);

        /// <summary>
        /// Actualiza la información de un producto
        /// </summary>
        Task<Producto> ActualizarProductoAsync(int productoId, string nombre, 
            string descripcion, decimal precio);

        /// <summary>
        /// Actualiza los límites de stock de un producto
        /// </summary>
        Task<Producto> ActualizarLimitesStockAsync(int productoId, int stockMinimo, int stockMaximo);

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        Task<Producto> ObtenerProductoAsync(int productoId);

        /// <summary>
        /// Obtiene un producto por su código
        /// </summary>
        Task<Producto> ObtenerProductoPorCodigoAsync(string codigo);

        /// <summary>
        /// Obtiene todos los productos activos
        /// </summary>
        Task<IEnumerable<Producto>> ObtenerProductosActivosAsync();

        /// <summary>
        /// Busca productos por nombre
        /// </summary>
        Task<IEnumerable<Producto>> BuscarProductosAsync(string terminoBusqueda);

        #endregion

        #region Gestión de Stock

        /// <summary>
        /// Ajusta el stock de un producto
        /// </summary>
        Task<Stock> AjustarStockAsync(int productoId, int nuevaCantidad);

        /// <summary>
        /// Agrega stock a un producto
        /// </summary>
        Task<Stock> AgregarStockAsync(int productoId, int cantidad);

        /// <summary>
        /// Reduce el stock de un producto
        /// </summary>
        Task<Stock> ReducirStockAsync(int productoId, int cantidad);

        /// <summary>
        /// Obtiene el stock de un producto
        /// </summary>
        Task<Stock> ObtenerStockAsync(int productoId);

        /// <summary>
        /// Obtiene productos con stock bajo
        /// </summary>
        Task<IEnumerable<Producto>> ObtenerProductosConStockBajoAsync();

        /// <summary>
        /// Obtiene productos agotados
        /// </summary>
        Task<IEnumerable<Producto>> ObtenerProductosAgotadosAsync();

        #endregion

        #region Gestión de Movimientos

        /// <summary>
        /// Registra una entrada de productos
        /// </summary>
        Task<EntradaProducto> RegistrarEntradaAsync(int productoId, int proveedorId, 
            int cantidad, decimal precioUnitario, string numeroFactura = null, string observaciones = null);

        /// <summary>
        /// Registra una salida de productos
        /// </summary>
        Task<SalidaProducto> RegistrarSalidaAsync(int productoId, int cantidad, 
            string motivo, string responsable = null, string observaciones = null);

        /// <summary>
        /// Obtiene el historial de movimientos de un producto
        /// </summary>
        Task<IEnumerable<object>> ObtenerHistorialMovimientosAsync(int productoId);

        #endregion
    }
}
