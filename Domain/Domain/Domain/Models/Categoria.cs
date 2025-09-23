using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para Categorías - Refleja la tabla Categorias de la base de datos
    /// </summary>
    public class Categoria
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        
        [MaxLength(300)]
        public string? Descripcion { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public DateTime FechaCreacion { get; set; }
    }
}
