using System;

namespace ProyectoInventario.Domain.Events
{
    /// <summary>
    /// Clase base para eventos de dominio
    /// </summary>
    public abstract class BaseDomainEvent : IDomainEvent
    {
        public int Id { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public int Version { get; } = 1;
    }
}
