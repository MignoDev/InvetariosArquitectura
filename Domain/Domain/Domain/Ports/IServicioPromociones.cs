using System;
using System.Threading.Tasks;

namespace ProyectoInventario.Domain.Ports
{
    /// <summary>
    /// Puerto para el servicio de promociones
    /// </summary>
    public interface IServicioPromociones
    {
        /// <summary>
        /// Crea una promoción automática para productos con exceso de stock
        /// </summary>
        Task CrearPromocionAutomaticaAsync(Guid productoId, int cantidadActual, int stockMaximo);

        /// <summary>
        /// Crea una promoción de descuento
        /// </summary>
        Task CrearPromocionDescuentoAsync(Guid productoId, decimal porcentajeDescuento, 
            DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Crea una promoción por cantidad
        /// </summary>
        Task CrearPromocionCantidadAsync(Guid productoId, int cantidadMinima, decimal porcentajeDescuento, 
            DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Crea una promoción 2x1
        /// </summary>
        Task CrearPromocion2x1Async(Guid productoId, DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Calcula el descuento recomendado para un producto
        /// </summary>
        Task<decimal> CalcularDescuentoRecomendadoAsync(Guid productoId, int cantidadActual, int stockMaximo);

        /// <summary>
        /// Verifica si un producto es candidato para promoción
        /// </summary>
        Task<bool> EsCandidatoParaPromocionAsync(Guid productoId);

        /// <summary>
        /// Obtiene las promociones activas de un producto
        /// </summary>
        Task<object[]> ObtenerPromocionesActivasAsync(Guid productoId);

        /// <summary>
        /// Desactiva una promoción
        /// </summary>
        Task DesactivarPromocionAsync(Guid promocionId);

        /// <summary>
        /// Calcula el impacto en ventas de una promoción
        /// </summary>
        Task<decimal> CalcularImpactoVentasAsync(Guid promocionId);
    }
}
