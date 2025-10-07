using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ProyectoInventario.Domain.Events;
using ProyectoInventario.Domain.Ports;

namespace ProyectoInventario.Infraestructura.Api.Infrastructure.EventBus
{
    /// <summary>
    /// Implementación en memoria del bus de eventos
    /// </summary>
    public class InMemoryEventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, List<Func<IDomainEvent, Task>>> _handlers;
        private readonly ILogger<InMemoryEventBus> _logger;

        public InMemoryEventBus(ILogger<InMemoryEventBus> logger)
        {
            _handlers = new ConcurrentDictionary<Type, List<Func<IDomainEvent, Task>>>();
            _logger = logger;
        }

        public async Task PublishAsync<T>(T @event) where T : IDomainEvent
        {
            var eventType = typeof(T);
            
            _logger.LogInformation("Publicando evento {EventType} con ID {EventId}", 
                eventType.Name, @event.Id);

            if (_handlers.TryGetValue(eventType, out var handlers))
            {
                var tasks = handlers.Select(handler => 
                {
                    try
                    {
                        return handler(@event);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al procesar evento {EventType} con ID {EventId}", 
                            eventType.Name, @event.Id);
                        return Task.CompletedTask;
                    }
                });

                await Task.WhenAll(tasks);
            }
            else
            {
                _logger.LogWarning("No hay manejadores registrados para el evento {EventType}", 
                    eventType.Name);
            }
        }

        public Task SubscribeAsync<T>(Func<T, Task> handler) where T : IDomainEvent
        {
            var eventType = typeof(T);
            
            _handlers.AddOrUpdate(
                eventType,
                new List<Func<IDomainEvent, Task>> { e => handler((T)e) },
                (key, existingHandlers) =>
                {
                    existingHandlers.Add(e => handler((T)e));
                    return existingHandlers;
                });

            _logger.LogInformation("Manejador registrado para evento {EventType}", eventType.Name);
            return Task.CompletedTask;
        }

        public Task UnsubscribeAsync<T>(Func<T, Task> handler) where T : IDomainEvent
        {
            var eventType = typeof(T);
            
            if (_handlers.TryGetValue(eventType, out var handlers))
            {
                handlers.RemoveAll(h => h == (Func<IDomainEvent, Task>)(e => handler((T)e)));
                
                if (!handlers.Any())
                {
                    _handlers.TryRemove(eventType, out _);
                }
            }

            _logger.LogInformation("Manejador desregistrado para evento {EventType}", eventType.Name);
            return Task.CompletedTask;
        }
    }
}
