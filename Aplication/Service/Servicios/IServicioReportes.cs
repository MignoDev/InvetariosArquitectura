using System;
using System.Threading.Tasks;

namespace ProyectoInventario.Application.Service.Servicios
{
    /// <summary>
    /// Puerto de entrada para el servicio de reportes
    /// Define la interfaz para la generación de reportes del inventario
    /// </summary>
    public interface IServicioReportes
    {
        #region Reportes de Stock

        /// <summary>
        /// Genera un reporte de stock actual
        /// </summary>
        Task<object> GenerarReporteStockActualAsync();

        /// <summary>
        /// Genera un reporte de productos con stock bajo
        /// </summary>
        Task<object> GenerarReporteStockBajoAsync();

        /// <summary>
        /// Genera un reporte de productos agotados
        /// </summary>
        Task<object> GenerarReporteProductosAgotadosAsync();

        #endregion

        #region Reportes de Movimientos

        /// <summary>
        /// Genera un reporte de movimientos por período
        /// </summary>
        Task<object> GenerarReporteMovimientosAsync(DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Genera un reporte de movimientos por producto
        /// </summary>
        Task<object> GenerarReporteMovimientosPorProductoAsync(Guid productoId);

        #endregion

        #region Reportes de Proveedores

        /// <summary>
        /// Genera un reporte de proveedores
        /// </summary>
        Task<object> GenerarReporteProveedoresAsync();

        #endregion

        #region Reportes de Estadísticas Generales

        /// <summary>
        /// Genera un reporte de estadísticas generales del inventario
        /// </summary>
        Task<object> GenerarReporteEstadisticasGeneralesAsync();

        #endregion
    }
}
