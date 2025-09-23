using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para Salidas de Productos - Refleja la tabla SalidasProductos de la base de datos
    /// </summary>
    public class SalidaProducto
    {
        public Guid Id { get; set; }
        
        [Required]
        public Guid ProductoId { get; set; }
        
        [Required]
        public int Cantidad { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Motivo { get; set; } = string.Empty;
        
        public DateTime FechaSalida { get; set; }
        
        [MaxLength(200)]
        public string? Responsable { get; set; }
        
        [MaxLength(500)]
        public string? Observaciones { get; set; }
    }
}
