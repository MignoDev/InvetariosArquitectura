using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProyectoInventario.Domain.Events;
using ProyectoInventario.Domain.Ports;
using ProyectoInventario.Infraestructura.Api.Infrastructure.EventHandlers;

namespace ProyectoInventario.Infraestructura.Api.Infrastructure.Services
{
    /// <summary>
    /// Servicio para configurar las suscripciones de eventos al inicio de la aplicación
    /// </summary>
    public class EventSubscriptionService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventSubscriptionService> _logger;

        public EventSubscriptionService(IServiceProvider serviceProvider, ILogger<EventSubscriptionService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Configurando suscripciones de eventos...");

            using var scope = _serviceProvider.CreateScope();
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
            var stockEventHandler = scope.ServiceProvider.GetRequiredService<StockEventHandler>();

            // Suscribir manejadores de eventos
            await eventBus.SubscribeAsync<StockActualizadoEvent>(stockEventHandler.HandleAsync);
            await eventBus.SubscribeAsync<StockBajoEvent>(stockEventHandler.HandleAsync);
            await eventBus.SubscribeAsync<ProductoAgotadoEvent>(stockEventHandler.HandleAsync);

            _logger.LogInformation("Suscripciones de eventos configuradas correctamente");

            // Mantener el servicio ejecutándose
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
