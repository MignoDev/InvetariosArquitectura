using System.Threading.Tasks;
using ProyectoInventario.Domain.Events;

namespace ProyectoInventario.Domain.Ports
{
    /// <summary>
    /// Interfaz base para manejadores de eventos
    /// </summary>
    public interface IEventHandler<in T> where T : IDomainEvent
    {
        /// <summary>
        /// Maneja un evento de dominio
        /// </summary>
        Task HandleAsync(T @event);
    }
}
