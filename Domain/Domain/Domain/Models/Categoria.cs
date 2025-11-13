using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para Categorías - Refleja la tabla Categorias de la base de datos
    /// </summary>
    public class Categoria
    {
        public int IdCategoria { get; set; }
        
        public string? Nombre { get; set; }
        
        public string? Descripcion { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public DateTime FechaCreacion { get; set; }
    }
}
