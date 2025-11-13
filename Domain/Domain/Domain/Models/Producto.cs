
namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para Productos - Refleja la tabla Productos de la base de datos
    /// </summary>
    public class Producto
    {
        public int IdProducto { get; set; }         
        public string? Nombre { get; set; }                
        public string? Descripcion { get; set; }        
        public decimal? PrecioCompra { get; set; }
        public decimal? PrecioVenta { get; set; }
        public int StockActual { get; set; } = 0;        
        public int StockMinimo { get; set; } = 10;        
        public int? IdCategoria { get; set; }
        public bool Activo { get; set; } = true;
        public string? Categoria { get; set; }
        
    }
}
