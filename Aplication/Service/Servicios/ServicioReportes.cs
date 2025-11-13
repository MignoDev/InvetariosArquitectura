using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Ports;

namespace ProyectoInventario.Application.Service.Servicios
{
    /// <summary>
    /// Servicio de aplicación para la generación de reportes
    /// Implementa la lógica de negocio para reportes del inventario
    /// </summary>
    public class ServicioReportes : IServicioReportes
    {
        private readonly IRepositorioProducto _repositorioProducto;
        private readonly IRepositorioStock _repositorioStock;
        private readonly IRepositorioMovimiento _repositorioMovimiento;
        private readonly IRepositorioProveedor _repositorioProveedor;

        public ServicioReportes(
            IRepositorioProducto repositorioProducto,
            IRepositorioStock repositorioStock,
            IRepositorioMovimiento repositorioMovimiento,
            IRepositorioProveedor repositorioProveedor)
        {
            _repositorioProducto = repositorioProducto;
            _repositorioStock = repositorioStock;
            _repositorioMovimiento = repositorioMovimiento;
            _repositorioProveedor = repositorioProveedor;
        }

        #region Reportes de Stock

        /// <summary>
        /// Genera un reporte de stock actual
        /// </summary>
        public async Task<object> GenerarReporteStockActualAsync()
        {
            var productos = await _repositorioProducto.ObtenerActivosAsync();
            var productosConStock = new List<object>();

            foreach (var producto in productos)
            {
                var stock = await _repositorioStock.ObtenerPorProductoIdAsync(producto.Id);
                if (stock != null)
                {
                    productosConStock.Add(new
                    {
                        ProductoId = producto.Id,
                        Codigo = producto.Codigo,
                        Nombre = producto.Nombre,
                        Precio = producto.Precio,
                        StockActual = stock.Cantidad,
                        StockMinimo = producto.StockMinimo,
                        StockMaximo = producto.StockMaximo,
                        Ubicacion = stock.Ubicacion,
                        EstadoStock = ObtenerEstadoStock(stock.Cantidad, producto.StockMinimo, producto.StockMaximo),
                        ValorTotal = stock.Cantidad * producto.Precio,
                        FechaUltimaActualizacion = stock.FechaUltimaActualizacion
                    });
                }
            }

            var totalProductos = productosConStock.Count;
            var totalValorInventario = productosConStock.Sum(p => (decimal)p.GetType().GetProperty("ValorTotal").GetValue(p));
            var productosConStockBajo = productosConStock.Count(p => (string)p.GetType().GetProperty("EstadoStock").GetValue(p) == "Bajo");
            var productosAgotados = productosConStock.Count(p => (string)p.GetType().GetProperty("EstadoStock").GetValue(p) == "Agotado");

            return new
            {
                FechaGeneracion = DateTime.UtcNow,
                Resumen = new
                {
                    TotalProductos = totalProductos,
                    TotalValorInventario = totalValorInventario,
                    ProductosConStockBajo = productosConStockBajo,
                    ProductosAgotados = productosAgotados,
                    ProductosConStockNormal = totalProductos - productosConStockBajo - productosAgotados
                },
                Productos = productosConStock
            };
        }

        /// <summary>
        /// Genera un reporte de productos con stock bajo
        /// </summary>
        public async Task<object> GenerarReporteStockBajoAsync()
        {
            var productosConStockBajo = await _repositorioProducto.BuscarConStockBajoAsync();
            var reporte = new List<object>();

            foreach (var producto in productosConStockBajo)
            {
                var stock = await _repositorioStock.ObtenerPorProductoIdAsync(producto.Id);
                if (stock != null)
                {
                    reporte.Add(new
                    {
                        ProductoId = producto.Id,
                        Codigo = producto.Codigo,
                        Nombre = producto.Nombre,
                        StockActual = stock.Cantidad,
                        StockMinimo = producto.StockMinimo,
                        Diferencia = producto.StockMinimo - stock.Cantidad,
                        Precio = producto.Precio,
                        ValorTotal = stock.Cantidad * producto.Precio,
                        Ubicacion = stock.Ubicacion,
                        FechaUltimaActualizacion = stock.FechaUltimaActualizacion
                    });
                }
            }

            return new
            {
                FechaGeneracion = DateTime.UtcNow,
                TotalProductos = reporte.Count,
                Productos = reporte
            };
        }

        /// <summary>
        /// Genera un reporte de productos agotados
        /// </summary>
        public async Task<object> GenerarReporteProductosAgotadosAsync()
        {
            var productosAgotados = await _repositorioProducto.BuscarAgotadosAsync();
            var reporte = new List<object>();

            foreach (var producto in productosAgotados)
            {
                var stock = await _repositorioStock.ObtenerPorProductoIdAsync(producto.Id);
                reporte.Add(new
                {
                    ProductoId = producto.Id,
                    Codigo = producto.Codigo,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    StockMinimo = producto.StockMinimo,
                    StockMaximo = producto.StockMaximo,
                    Ubicacion = stock?.Ubicacion,
                    FechaUltimaActualizacion = stock?.FechaUltimaActualizacion
                });
            }

            return new
            {
                FechaGeneracion = DateTime.UtcNow,
                TotalProductos = reporte.Count,
                Productos = reporte
            };
        }

        #endregion

        #region Reportes de Movimientos

        /// <summary>
        /// Genera un reporte de movimientos por período
        /// </summary>
        public async Task<object> GenerarReporteMovimientosAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var entradas = await _repositorioMovimiento.ObtenerEntradasPorFechaAsync(fechaInicio, fechaFin);
            var salidas = await _repositorioMovimiento.ObtenerSalidasPorFechaAsync(fechaInicio, fechaFin);

            // Crear tipos anónimos con propiedades comunes para poder concatenar
            var reporteEntradas = entradas.Select(e => new
            {
                Id = e.Id,
                Tipo = "Entrada",
                ProductoId = e.ProductoId,
                Cantidad = e.Cantidad,
                PrecioUnitario = (decimal?)e.PrecioUnitario,
                Total = (decimal?)(e.Cantidad * e.PrecioUnitario),
                Fecha = e.FechaEntrada,
                NumeroFactura = e.NumeroFactura,
                Motivo = (string)null,
                Responsable = (string)null,
                Observaciones = e.Observaciones
            });

            var reporteSalidas = salidas.Select(s => new
            {
                Id = s.Id,
                Tipo = "Salida",
                ProductoId = s.ProductoId,
                Cantidad = s.Cantidad,
                PrecioUnitario = (decimal?)null,
                Total = (decimal?)null,
                Fecha = s.FechaSalida,
                NumeroFactura = (string)null,
                Motivo = s.Motivo,
                Responsable = s.Responsable,
                Observaciones = s.Observaciones
            });

            var totalEntradas = entradas.Count();
            var totalSalidas = salidas.Count();
            var valorTotalEntradas = entradas.Sum(e => e.Cantidad * e.PrecioUnitario);
            var cantidadTotalEntradas = entradas.Sum(e => e.Cantidad);
            var cantidadTotalSalidas = salidas.Sum(s => s.Cantidad);

            return new
            {
                FechaGeneracion = DateTime.UtcNow,
                Periodo = new
                {
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin
                },
                Resumen = new
                {
                    TotalEntradas = totalEntradas,
                    TotalSalidas = totalSalidas,
                    ValorTotalEntradas = valorTotalEntradas,
                    CantidadTotalEntradas = cantidadTotalEntradas,
                    CantidadTotalSalidas = cantidadTotalSalidas,
                    BalanceCantidad = cantidadTotalEntradas - cantidadTotalSalidas
                },
                Movimientos = reporteEntradas.Concat(reporteSalidas).OrderBy(m => m.Fecha)
            };
        }

        /// <summary>
        /// Genera un reporte de movimientos por producto
        /// </summary>
        public async Task<object> GenerarReporteMovimientosPorProductoAsync(int productoId)
        {
            var producto = await _repositorioProducto.ObtenerPorIdAsync(productoId);

            if (producto == null)
            {
                throw new ArgumentException("El producto no existe");
            }

            // Obtener entradas y salidas por separado
            var entradas = await _repositorioMovimiento.ObtenerEntradasPorProductoAsync(productoId);
            var salidas = await _repositorioMovimiento.ObtenerSalidasPorProductoAsync(productoId);

            // Crear tipos anónimos con propiedades comunes
            var reporteEntradas = entradas.Select(e => new
            {
                Id = e.Id,
                Tipo = "Entrada",
                ProductoId = e.ProductoId,
                Cantidad = e.Cantidad,
                PrecioUnitario = (decimal?)e.PrecioUnitario,
                Total = (decimal?)(e.Cantidad * e.PrecioUnitario),
                Fecha = e.FechaEntrada,
                NumeroFactura = e.NumeroFactura,
                Motivo = (string)null,
                Responsable = (string)null,
                Observaciones = e.Observaciones
            });

            var reporteSalidas = salidas.Select(s => new
            {
                Id = s.Id,
                Tipo = "Salida",
                ProductoId = s.ProductoId,
                Cantidad = s.Cantidad,
                PrecioUnitario = (decimal?)null,
                Total = (decimal?)null,
                Fecha = s.FechaSalida,
                NumeroFactura = (string)null,
                Motivo = s.Motivo,
                Responsable = s.Responsable,
                Observaciones = s.Observaciones
            });

            return new
            {
                FechaGeneracion = DateTime.UtcNow,
                Producto = new
                {
                    Id = producto.Id,
                    Codigo = producto.Codigo,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio
                },
                Movimientos = reporteEntradas.Concat(reporteSalidas).OrderBy(m => m.Fecha)
            };
        }

        #endregion

        #region Reportes de Proveedores

        /// <summary>
        /// Genera un reporte de proveedores
        /// </summary>
        public async Task<object> GenerarReporteProveedoresAsync()
        {
            var proveedores = await _repositorioProveedor.ObtenerTodosAsync();
            var reporte = new List<object>();

            foreach (var proveedor in proveedores)
            {
                var entradas = await _repositorioMovimiento.ObtenerEntradasPorProveedorAsync(proveedor.Id);
                var totalEntradas = entradas.Count();
                var valorTotalEntradas = entradas.Sum(e => e.Cantidad * e.PrecioUnitario);

                reporte.Add(new
                {
                    ProveedorId = proveedor.Id,
                    Codigo = proveedor.Codigo,
                    Nombre = proveedor.Nombre,
                    Contacto = proveedor.Contacto,
                    Email = proveedor.Email,
                    Telefono = proveedor.Telefono,
                    Activo = proveedor.Activo,
                    TotalEntradas = totalEntradas,
                    ValorTotalEntradas = valorTotalEntradas,
                    TieneInformacionCompleta = !string.IsNullOrWhiteSpace(proveedor.Contacto) && 
                                             !string.IsNullOrWhiteSpace(proveedor.Email) && 
                                             !string.IsNullOrWhiteSpace(proveedor.Telefono) && 
                                             !string.IsNullOrWhiteSpace(proveedor.Direccion),
                    FechaCreacion = proveedor.FechaCreacion
                });
            }

            var totalProveedores = reporte.Count;
            var proveedoresActivos = reporte.Count(p => (bool)p.GetType().GetProperty("Activo").GetValue(p));
            var proveedoresConInformacionCompleta = reporte.Count(p => (bool)p.GetType().GetProperty("TieneInformacionCompleta").GetValue(p));

            return new
            {
                FechaGeneracion = DateTime.UtcNow,
                Resumen = new
                {
                    TotalProveedores = totalProveedores,
                    ProveedoresActivos = proveedoresActivos,
                    ProveedoresConInformacionCompleta = proveedoresConInformacionCompleta,
                    ProveedoresIncompletos = totalProveedores - proveedoresConInformacionCompleta
                },
                Proveedores = reporte
            };
        }

        #endregion

        #region Reportes de Estadísticas Generales

        /// <summary>
        /// Genera un reporte de estadísticas generales del inventario
        /// </summary>
        public async Task<object> GenerarReporteEstadisticasGeneralesAsync()
        {
            var totalProductos = await _repositorioProducto.ContarTotalAsync();
            var productosActivos = await _repositorioProducto.ContarActivosAsync();
            var productosConStockBajo = await _repositorioProducto.ContarConStockBajoAsync();
            var productosAgotados = await _repositorioProducto.ContarAgotadosAsync();
            var totalProveedores = await _repositorioProveedor.ContarTotalAsync();
            var proveedoresActivos = await _repositorioProveedor.ContarActivosAsync();

            // Obtener estadísticas de movimientos del último mes
            var fechaInicio = DateTime.UtcNow.AddMonths(-1);
            var fechaFin = DateTime.UtcNow;
            var entradasUltimoMes = await _repositorioMovimiento.ObtenerEntradasPorFechaAsync(fechaInicio, fechaFin);
            var salidasUltimoMes = await _repositorioMovimiento.ObtenerSalidasPorFechaAsync(fechaInicio, fechaFin);

            return new
            {
                FechaGeneracion = DateTime.UtcNow,
                Productos = new
                {
                    Total = totalProductos,
                    Activos = productosActivos,
                    ConStockBajo = productosConStockBajo,
                    Agotados = productosAgotados,
                    ConStockNormal = productosActivos - productosConStockBajo - productosAgotados
                },
                Proveedores = new
                {
                    Total = totalProveedores,
                    Activos = proveedoresActivos,
                    Inactivos = totalProveedores - proveedoresActivos
                },
                MovimientosUltimoMes = new
                {
                    Entradas = entradasUltimoMes.Count(),
                    Salidas = salidasUltimoMes.Count(),
                    ValorTotalEntradas = entradasUltimoMes.Sum(e => e.PrecioUnitario * e.Cantidad),
                    CantidadTotalEntradas = entradasUltimoMes.Sum(e => e.Cantidad),
                    CantidadTotalSalidas = salidasUltimoMes.Sum(s => s.Cantidad)
                }
            };
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Determina el estado del stock basado en la cantidad actual y los límites
        /// </summary>
        private string ObtenerEstadoStock(int cantidadActual, int stockMinimo, int stockMaximo)
        {
            if (cantidadActual == 0)
                return "Agotado";
            if (cantidadActual <= stockMinimo)
                return "Bajo";
            if (cantidadActual >= stockMaximo)
                return "Exceso";
            return "Normal";
        }

        #endregion
    }
}
