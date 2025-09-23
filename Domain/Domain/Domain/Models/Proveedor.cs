using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para Proveedores - Refleja la tabla Proveedores de la base de datos
    /// </summary>
    public class Proveedor
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string? Contacto { get; set; }
        
        [MaxLength(100)]
        public string? Email { get; set; }
        
        [MaxLength(20)]
        public string? Telefono { get; set; }
        
        [MaxLength(300)]
        public string? Direccion { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public DateTime FechaCreacion { get; set; }
    }
}
