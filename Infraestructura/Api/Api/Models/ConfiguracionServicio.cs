using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para configuración de servicios
    /// </summary>
    public class ConfiguracionServicio
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string NombreServicio { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(500)]
        public string EndpointBase { get; set; } = string.Empty;
        
        public int TimeoutSegundos { get; set; } = 30;
        
        public bool Activo { get; set; } = true;
        
        public DateTime FechaCreacion { get; set; }
        
        public DateTime FechaModificacion { get; set; }
    }
}
