using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para Entradas de Productos - Refleja la tabla EntradasProductos de la base de datos
    /// </summary>
    public class EntradaProducto
    {
        public Guid Id { get; set; }
        
        [Required]
        public Guid ProductoId { get; set; }
        
        [Required]
        public Guid ProveedorId { get; set; }
        
        [Required]
        public int Cantidad { get; set; }
        
        [Required]
        public decimal PrecioUnitario { get; set; }
        
        public DateTime FechaEntrada { get; set; }
        
        [MaxLength(100)]
        public string? NumeroFactura { get; set; }
        
        [MaxLength(500)]
        public string? Observaciones { get; set; }
    }
}
