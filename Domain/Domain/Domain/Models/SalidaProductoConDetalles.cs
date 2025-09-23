using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo extendido de SalidaProducto que incluye información detallada
    /// Útil para reportes de salidas y seguimiento de inventario
    /// </summary>
    public class SalidaProductoConDetalles : SalidaProducto
    {
        // Información del producto
        public string? ProductoCodigo { get; set; }
        public string? ProductoNombre { get; set; }
        public string? ProductoDescripcion { get; set; }
        public decimal? ProductoPrecio { get; set; }
        
        // Información de la categoría
        public string? CategoriaNombre { get; set; }
        
        // Cálculos financieros
        public decimal? ValorTotal { get; set; }
        
        // Información temporal
        public int DiasDesdeSalida { get; set; }
        public bool EsSalidaReciente { get; set; }
        
        // Información adicional
        public bool TieneResponsable { get; set; }
        public bool TieneObservaciones { get; set; }
    }
}
