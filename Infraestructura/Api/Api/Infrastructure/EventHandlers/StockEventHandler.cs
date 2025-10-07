using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ProyectoInventario.Domain.Events;
using ProyectoInventario.Domain.Ports;

namespace ProyectoInventario.Infraestructura.Api.Infrastructure.EventHandlers
{
    /// <summary>
    /// Manejador de eventos de stock
    /// </summary>
    public class StockEventHandler : 
        IEventHandler<StockActualizadoEvent>,
        IEventHandler<StockBajoEvent>,
        IEventHandler<ProductoAgotadoEvent>
    {
        private readonly IServicioNotificacion _servicioNotificacion;
        private readonly IServicioPromociones _servicioPromociones;
        private readonly ILogger<StockEventHandler> _logger;

        public StockEventHandler(
            IServicioNotificacion servicioNotificacion,
            IServicioPromociones servicioPromociones,
            ILogger<StockEventHandler> logger)
        {
            _servicioNotificacion = servicioNotificacion;
            _servicioPromociones = servicioPromociones;
            _logger = logger;
        }

        public async Task HandleAsync(StockActualizadoEvent @event)
        {
            _logger.LogInformation("Procesando evento StockActualizado: {Producto} - {Anterior} -> {Nueva}", 
                @event.NombreProducto, @event.CantidadAnterior, @event.CantidadNueva);

            // Enviar notificación de stock actualizado
            await _servicioNotificacion.EnviarNotificacionStockActualizadoAsync(
                @event.NombreProducto, 
                @event.CantidadAnterior, 
                @event.CantidadNueva, 
                @event.TipoMovimiento);

            // Verificar si necesita promoción por exceso de stock
            if (@event.CantidadNueva > 100) // Ejemplo: stock máximo
            {
                await _servicioPromociones.CrearPromocionAutomaticaAsync(
                    @event.ProductoId, 
                    @event.CantidadNueva, 
                    100);
            }
        }

        public async Task HandleAsync(StockBajoEvent @event)
        {
            _logger.LogWarning("Procesando evento StockBajo: {Producto} - {Actual}/{Minimo}", 
                @event.NombreProducto, @event.CantidadActual, @event.StockMinimo);

            // Enviar notificación de stock bajo
            await _servicioNotificacion.EnviarNotificacionStockBajoAsync(
                @event.NombreProducto, 
                @event.CantidadActual, 
                @event.StockMinimo);

            // Generar orden de compra automática
            // await _servicioProveedores.GenerarOrdenCompraAsync(@event.ProductoId);
        }

        public async Task HandleAsync(ProductoAgotadoEvent @event)
        {
            _logger.LogError("Procesando evento ProductoAgotado: {Producto}", @event.NombreProducto);

            // Enviar notificación de producto agotado
            await _servicioNotificacion.EnviarNotificacionProductoAgotadoAsync(@event.NombreProducto);

            // Generar orden de compra urgente
            // await _servicioProveedores.GenerarOrdenCompraUrgenteAsync(@event.ProductoId);
        }
    }
}
