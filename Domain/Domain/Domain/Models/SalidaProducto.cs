using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para Salidas de Productos - Refleja la tabla SalidasProductos de la base de datos
    /// </summary>
    public class SalidaProducto
    {
        public int Id { get; set; }
        
        public int ProductoId { get; set; }
        
        public int Cantidad { get; set; }
        
        public string Motivo { get; set; } = string.Empty;
        
        public DateTime FechaSalida { get; set; }
        
        public string? Responsable { get; set; }
        
        public string? Observaciones { get; set; }
    }
}
