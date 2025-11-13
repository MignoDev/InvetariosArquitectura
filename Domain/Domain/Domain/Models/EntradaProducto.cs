using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para Entradas de Productos - Refleja la tabla EntradasProductos de la base de datos
    /// </summary>
    public class EntradaProducto
    {
        public int Id { get; set; }
        
        public int ProductoId { get; set; }
        
        public int ProveedorId { get; set; }
        
        public int Cantidad { get; set; }
        
        public decimal PrecioUnitario { get; set; }
        
        public DateTime FechaEntrada { get; set; }
        
        public string? NumeroFactura { get; set; }
        
        public string? Observaciones { get; set; }
    }
}
