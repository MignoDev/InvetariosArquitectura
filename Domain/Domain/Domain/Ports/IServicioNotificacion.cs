using System;
using System.Threading.Tasks;

namespace ProyectoInventario.Domain.Ports
{
    /// <summary>
    /// Puerto para el servicio de notificaciones
    /// </summary>
    public interface IServicioNotificacion
    {
        /// <summary>
        /// Envía notificación cuando el stock está bajo
        /// </summary>
        Task EnviarNotificacionStockBajoAsync(string productoNombre, int cantidadActual, int stockMinimo);

        /// <summary>
        /// Envía notificación cuando un producto se agota
        /// </summary>
        Task EnviarNotificacionProductoAgotadoAsync(string productoNombre);

        /// <summary>
        /// Envía notificación cuando hay exceso de stock
        /// </summary>
        Task EnviarNotificacionExcesoStockAsync(string productoNombre, int cantidadActual, int stockMaximo);

        /// <summary>
        /// Envía notificación cuando se actualiza el stock
        /// </summary>
        Task EnviarNotificacionStockActualizadoAsync(string productoNombre, int cantidadAnterior, 
            int cantidadNueva, string tipoMovimiento);

        /// <summary>
        /// Envía notificación personalizada
        /// </summary>
        Task EnviarNotificacionPersonalizadaAsync(string asunto, string mensaje, string[] destinatarios);

        /// <summary>
        /// Envía email
        /// </summary>
        Task EnviarEmailAsync(string destinatario, string asunto, string cuerpo);

        /// <summary>
        /// Envía SMS
        /// </summary>
        Task EnviarSmsAsync(string numeroTelefono, string mensaje);

        /// <summary>
        /// Envía notificación push
        /// </summary>
        Task EnviarNotificacionPushAsync(string usuarioId, string titulo, string mensaje);
    }
}
