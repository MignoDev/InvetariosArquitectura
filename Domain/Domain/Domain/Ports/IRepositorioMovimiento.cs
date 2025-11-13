using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Models;

namespace ProyectoInventario.Domain.Ports
{
    /// <summary>
    /// Puerto (interfaz) para el repositorio de movimientos de inventario
    /// Define el contrato para la persistencia de entradas y salidas
    /// </summary>
    public interface IRepositorioMovimiento
    {
        // Operaciones de Entradas
        Task<EntradaProducto> ObtenerEntradaPorIdAsync(int id);
        Task<IEnumerable<EntradaProducto>> ObtenerEntradasAsync();
        Task<IEnumerable<EntradaProducto>> ObtenerEntradasPorProductoAsync(int productoId);
        Task<IEnumerable<EntradaProducto>> ObtenerEntradasPorProveedorAsync(int proveedorId);
        Task<IEnumerable<EntradaProducto>> ObtenerEntradasPorFechaAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<EntradaProducto> CrearEntradaAsync(EntradaProducto entrada);
        Task<EntradaProducto> ActualizarEntradaAsync(EntradaProducto entrada);
        Task EliminarEntradaAsync(int id);

        // Operaciones de Salidas
        Task<SalidaProducto> ObtenerSalidaPorIdAsync(int id);
        Task<IEnumerable<SalidaProducto>> ObtenerSalidasAsync();
        Task<IEnumerable<SalidaProducto>> ObtenerSalidasPorProductoAsync(int productoId);
        Task<IEnumerable<SalidaProducto>> ObtenerSalidasPorFechaAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<SalidaProducto>> ObtenerSalidasPorMotivoAsync(string motivo);
        Task<SalidaProducto> CrearSalidaAsync(SalidaProducto salida);
        Task<SalidaProducto> ActualizarSalidaAsync(SalidaProducto salida);
        Task EliminarSalidaAsync(int id);

        // Operaciones de búsqueda combinadas
        Task<IEnumerable<object>> ObtenerMovimientosPorProductoAsync(int productoId);
        Task<IEnumerable<object>> ObtenerMovimientosPorFechaAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<object>> ObtenerMovimientosRecientesAsync(int dias = 7);

        // Operaciones de estadísticas
        Task<int> ContarEntradasAsync();
        Task<int> ContarSalidasAsync();
        Task<decimal> CalcularValorTotalEntradasAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<int> CalcularCantidadTotalEntradasAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<int> CalcularCantidadTotalSalidasAsync(DateTime fechaInicio, DateTime fechaFin);

        // Operaciones de validación
        Task<bool> ExisteEntradaConFacturaAsync(string numeroFactura);
        Task<bool> ExisteEntradaConFacturaAsync(string numeroFactura, int idExcluir);
    }
}
