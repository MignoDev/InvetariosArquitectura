using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo extendido de EntradaProducto que incluye información detallada
    /// Útil para reportes de compras y seguimiento de inventario
    /// </summary>
    public class EntradaProductoConDetalles : EntradaProducto
    {
        // Información del producto
        public string? ProductoCodigo { get; set; }
        public string? ProductoNombre { get; set; }
        public string? ProductoDescripcion { get; set; }
        public decimal? ProductoPrecioActual { get; set; }
        
        // Información del proveedor
        public string? ProveedorCodigo { get; set; }
        public string? ProveedorNombre { get; set; }
        public string? ProveedorContacto { get; set; }
        
        // Información de la categoría
        public string? CategoriaNombre { get; set; }
        
        // Cálculos financieros
        public decimal ValorTotal { get; set; }
        public decimal? DiferenciaPrecio { get; set; }
        
        // Información temporal
        public int DiasDesdeEntrada { get; set; }
        public bool EsEntradaReciente { get; set; }
    }
}
