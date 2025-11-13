using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para Stock - Refleja la tabla Stock de la base de datos
    /// </summary>
    public class Stock
    {
        public int Id { get; set; }
        
        public int ProductoId { get; set; }
        
        public int Cantidad { get; set; } = 0;
        
        public string? Ubicacion { get; set; }
        
        public DateTime FechaUltimaActualizacion { get; set; }
    }
}
