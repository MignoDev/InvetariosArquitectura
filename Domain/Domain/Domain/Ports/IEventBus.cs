using System;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Events;

namespace ProyectoInventario.Domain.Ports
{
    /// <summary>
    /// Puerto para el bus de eventos (Publish-Subscribe)
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publica un evento de dominio
        /// </summary>
        Task PublishAsync<T>(T @event) where T : IDomainEvent;

        /// <summary>
        /// Suscribe un manejador de eventos
        /// </summary>
        Task SubscribeAsync<T>(Func<T, Task> handler) where T : IDomainEvent;

        /// <summary>
        /// Desuscribe un manejador de eventos
        /// </summary>
        Task UnsubscribeAsync<T>(Func<T, Task> handler) where T : IDomainEvent;
    }
}
