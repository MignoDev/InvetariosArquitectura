using System;
using System.Threading.Tasks;

namespace ProyectoInventario.Domain.Ports
{
    /// <summary>
    /// Puerto para el servicio de proveedores externos
    /// </summary>
    public interface IServicioProveedores
    {
        /// <summary>
        /// Genera una orden de compra para un producto
        /// </summary>
        Task GenerarOrdenCompraAsync(Guid productoId);

        /// <summary>
        /// Genera una orden de compra urgente para un producto
        /// </summary>
        Task GenerarOrdenCompraUrgenteAsync(Guid productoId);

        /// <summary>
        /// Obtiene el proveedor recomendado para un producto
        /// </summary>
        Task<Guid?> ObtenerProveedorRecomendadoAsync(Guid productoId);

        /// <summary>
        /// Calcula la cantidad recomendada para una orden de compra
        /// </summary>
        Task<int> CalcularCantidadRecomendadaAsync(Guid productoId);

        /// <summary>
        /// Calcula el precio estimado para una cantidad específica
        /// </summary>
        Task<decimal> CalcularPrecioEstimadoAsync(Guid productoId, int cantidad);

        /// <summary>
        /// Valida la disponibilidad de un proveedor para un producto
        /// </summary>
        Task<bool> ValidarDisponibilidadProveedorAsync(Guid proveedorId, Guid productoId);

        /// <summary>
        /// Obtiene el tiempo de entrega estimado
        /// </summary>
        Task<int> ObtenerTiempoEntregaEstimadoAsync(Guid proveedorId, Guid productoId);

        /// <summary>
        /// Envía una solicitud de cotización a un proveedor
        /// </summary>
        Task EnviarSolicitudCotizacionAsync(Guid proveedorId, Guid productoId, int cantidad);

        /// <summary>
        /// Procesa la respuesta de una cotización
        /// </summary>
        Task ProcesarRespuestaCotizacionAsync(Guid proveedorId, Guid productoId, decimal precio, int tiempoEntrega);
    }
}
