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
        Task GenerarOrdenCompraAsync(int productoId);

        /// <summary>
        /// Genera una orden de compra urgente para un producto
        /// </summary>
        Task GenerarOrdenCompraUrgenteAsync(int productoId);

        /// <summary>
        /// Obtiene el proveedor recomendado para un producto
        /// </summary>
        Task<int?> ObtenerProveedorRecomendadoAsync(int productoId);

        /// <summary>
        /// Calcula la cantidad recomendada para una orden de compra
        /// </summary>
        Task<int> CalcularCantidadRecomendadaAsync(int productoId);

        /// <summary>
        /// Calcula el precio estimado para una cantidad específica
        /// </summary>
        Task<decimal> CalcularPrecioEstimadoAsync(int productoId, int cantidad);

        /// <summary>
        /// Valida la disponibilidad de un proveedor para un producto
        /// </summary>
        Task<bool> ValidarDisponibilidadProveedorAsync(int proveedorId, int productoId);

        /// <summary>
        /// Obtiene el tiempo de entrega estimado
        /// </summary>
        Task<int> ObtenerTiempoEntregaEstimadoAsync(int proveedorId, int productoId);

        /// <summary>
        /// Envía una solicitud de cotización a un proveedor
        /// </summary>
        Task EnviarSolicitudCotizacionAsync(int proveedorId, int productoId, int cantidad);

        /// <summary>
        /// Procesa la respuesta de una cotización
        /// </summary>
        Task ProcesarRespuestaCotizacionAsync(int proveedorId, int productoId, decimal precio, int tiempoEntrega);
    }
}
