using System.ComponentModel.DataAnnotations;

namespace ProyectoInventario.Domain.Models
{
    /// <summary>
    /// Modelo para Proveedores - Refleja la tabla Proveedores de la base de datos
    /// </summary>
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string? Nombre { get; set; } 
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }        
    }
}
