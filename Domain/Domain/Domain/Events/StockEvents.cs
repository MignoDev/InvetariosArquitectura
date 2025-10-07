using System;

namespace ProyectoInventario.Domain.Events
{
    /// <summary>
    /// Evento publicado cuando se actualiza el stock de un producto
    /// </summary>
    public class StockActualizadoEvent : BaseDomainEvent
    {
        public Guid ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadAnterior { get; set; }
        public int CantidadNueva { get; set; }
        public string TipoMovimiento { get; set; }
        public string Ubicacion { get; set; }

        public StockActualizadoEvent(Guid productoId, string nombreProducto, 
            int cantidadAnterior, int cantidadNueva, string tipoMovimiento, string ubicacion)
        {
            ProductoId = productoId;
            NombreProducto = nombreProducto;
            CantidadAnterior = cantidadAnterior;
            CantidadNueva = cantidadNueva;
            TipoMovimiento = tipoMovimiento;
            Ubicacion = ubicacion;
        }
    }

    /// <summary>
    /// Evento publicado cuando un producto tiene stock bajo
    /// </summary>
    public class StockBajoEvent : BaseDomainEvent
    {
        public Guid ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadActual { get; set; }
        public int StockMinimo { get; set; }
        public string Ubicacion { get; set; }

        public StockBajoEvent(Guid productoId, string nombreProducto, 
            int cantidadActual, int stockMinimo, string ubicacion)
        {
            ProductoId = productoId;
            NombreProducto = nombreProducto;
            CantidadActual = cantidadActual;
            StockMinimo = stockMinimo;
            Ubicacion = ubicacion;
        }
    }

    /// <summary>
    /// Evento publicado cuando un producto se agota
    /// </summary>
    public class ProductoAgotadoEvent : BaseDomainEvent
    {
        public Guid ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string Codigo { get; set; }
        public string Ubicacion { get; set; }

        public ProductoAgotadoEvent(Guid productoId, string nombreProducto, 
            string codigo, string ubicacion)
        {
            ProductoId = productoId;
            NombreProducto = nombreProducto;
            Codigo = codigo;
            Ubicacion = ubicacion;
        }
    }

    /// <summary>
    /// Evento publicado cuando se registra una entrada de producto
    /// </summary>
    public class EntradaProductoRegistradaEvent : BaseDomainEvent
    {
        public Guid EntradaId { get; set; }
        public Guid ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public Guid ProveedorId { get; set; }
        public string NombreProveedor { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string NumeroFactura { get; set; }

        public EntradaProductoRegistradaEvent(Guid entradaId, Guid productoId, 
            string nombreProducto, Guid proveedorId, string nombreProveedor, 
            int cantidad, decimal precioUnitario, string numeroFactura)
        {
            EntradaId = entradaId;
            ProductoId = productoId;
            NombreProducto = nombreProducto;
            ProveedorId = proveedorId;
            NombreProveedor = nombreProveedor;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            NumeroFactura = numeroFactura;
        }
    }

    /// <summary>
    /// Evento publicado cuando se registra una salida de producto
    /// </summary>
    public class SalidaProductoRegistradaEvent : BaseDomainEvent
    {
        public Guid SalidaId { get; set; }
        public Guid ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; }
        public string Responsable { get; set; }

        public SalidaProductoRegistradaEvent(Guid salidaId, Guid productoId, 
            string nombreProducto, int cantidad, string motivo, string responsable)
        {
            SalidaId = salidaId;
            ProductoId = productoId;
            NombreProducto = nombreProducto;
            Cantidad = cantidad;
            Motivo = motivo;
            Responsable = responsable;
        }
    }
}
