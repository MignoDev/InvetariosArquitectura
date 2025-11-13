using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo extendido de Producto que incluye información de stock
    /// Útil para la API y AppBlazor cuando necesitan datos completos
    /// </summary>
    public class ProductoConStock : Producto
    {
        // Información de la categoría
        public string? CategoriaNombre { get; set; }
        public string? CategoriaDescripcion { get; set; }
        
        // Información del stock
        public int StockId { get; set; }
        public int? StockActual { get; set; }
        public int? StockInicial { get; set; }
        public string? UbicacionStock { get; set; }
        public DateTime? FechaUltimaActualizacionStock { get; set; }
        
        // Estado del stock
        public string EstadoStock { get; set; } = "Normal"; // "Bajo", "Normal", "Exceso", "Agotado"
        
        // Propiedades adicionales
        public bool TieneStockBajo { get; set; }
        public bool TieneExcesoStock { get; set; }
        public bool EstaAgotado { get; set; }
        public decimal? ValorTotalInventario { get; set; }
    }
}
