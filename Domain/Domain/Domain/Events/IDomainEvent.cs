using System;

namespace ProyectoInventario.Domain.Events
{
    /// <summary>
    /// Interfaz base para todos los eventos de dominio
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Identificador único del evento
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Fecha y hora cuando ocurrió el evento
        /// </summary>
        DateTime OccurredOn { get; }

        /// <summary>
        /// Versión del evento para control de versiones
        /// </summary>
        int Version { get; }
    }
}
