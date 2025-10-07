using System;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Events;

namespace ProyectoInventario.Domain.Ports
{
    /// <summary>
    /// Puerto para el publicador de eventos
    /// </summary>
    public interface IPublicadorEventos
    {
        /// <summary>
        /// Publica un evento de dominio
        /// </summary>
        Task PublicarAsync<T>(T @event) where T : IDomainEvent;

        /// <summary>
        /// Publica múltiples eventos
        /// </summary>
        Task PublicarAsync(params IDomainEvent[] events);
    }
}
