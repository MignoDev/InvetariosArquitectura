using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para Productos - Refleja la tabla Productos de la base de datos
    /// </summary>
    public class Producto
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Descripcion { get; set; }
        
        [Required]
        public decimal Precio { get; set; }
        
        public int StockMinimo { get; set; } = 0;
        
        public int StockMaximo { get; set; } = 1000;
        
        public bool Activo { get; set; } = true;
        
        public DateTime FechaCreacion { get; set; }
        
        public DateTime FechaModificacion { get; set; }
        
        public Guid? CategoriaId { get; set; }
    }
}
