using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para Stock - Refleja la tabla Stock de la base de datos
    /// </summary>
    public class Stock
    {
        public Guid Id { get; set; }
        
        [Required]
        public Guid ProductoId { get; set; }
        
        public int Cantidad { get; set; } = 0;
        
        [MaxLength(100)]
        public string? Ubicacion { get; set; }
        
        public DateTime FechaUltimaActualizacion { get; set; }
    }
}
