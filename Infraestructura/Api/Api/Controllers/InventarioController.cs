//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
//using System.Data;
//using ProyectoInventario.Domain.Models;

//namespace ProyectoInventario.Infraestructura.Api.Controllers
//{
//    /// <summary>
//    /// Controlador para la gestión del inventario usando procedimientos almacenados simplificados
//    /// </summary>
//    [ApiController]
//    [Route("api/inventario")]
//    public class InventarioController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;
//        private readonly string _connectionString;

//        public InventarioController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//            _connectionString = _configuration.GetConnectionString("DefaultConnection");
//        }
        
//        #region Gestión de Categorías      

//        /// <summary>
//        /// Obtiene todos los proveedores
//        /// </summary>
//        [HttpGet("proveedores")]
//        public async Task<IActionResult> ObtenerProveedores()
//        {
//            try
//            {
//                var proveedores = new List<Proveedor>();

//                using (var connection = new SqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
                    
//                    using (var command = new SqlCommand("sp_ObtenerProveedoresActivos", connection))
//                    {
//                        command.CommandType = CommandType.StoredProcedure;
                        
//                        using (var reader = await command.ExecuteReaderAsync())
//                        {
//                            while (await reader.ReadAsync())
//                            {
//                                proveedores.Add(new Proveedor
//                                {
//                                    Id = reader.GetInt32("Id"),
//                                    Codigo = reader.GetString("Codigo"),
//                                    Nombre = reader.GetString("Nombre"),
//                                    Contacto = reader.IsDBNull("Contacto") ? null : reader.GetString("Contacto"),
//                                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
//                                    Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
//                                    Direccion = reader.IsDBNull("Direccion") ? null : reader.GetString("Direccion"),
//                                    Activo = reader.GetBoolean("Activo"),
//                                    FechaCreacion = reader.GetDateTime("FechaCreacion")
//                                });
//                            }
//                        }
//                    }
//                }

//                return Ok(new { success = true, data = proveedores, message = "Proveedores obtenidos correctamente" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
//            }
//        }

//        /// <summary>
//        /// Crea un nuevo proveedor
//        /// </summary>
//        //[HttpPost("proveedores")]
//        //public async Task<IActionResult> CrearProveedor([FromBody] Proveedor request)
//        //{
//        //    try
//        //    {

//        //        using (var connection = new SqlConnection(_connectionString))
//        //        {
//        //            await connection.OpenAsync();
                    
//        //            using (var command = new SqlCommand("sp_CrearProveedor", connection))
//        //            {
//        //                command.CommandType = CommandType.StoredProcedure;
//        //                command.Parameters.AddWithValue("@Codigo", request.Codigo);
//        //                command.Parameters.AddWithValue("@Nombre", request.Nombre);
//        //                command.Parameters.AddWithValue("@Contacto", request.Contacto ?? (object)DBNull.Value);
//        //                command.Parameters.AddWithValue("@Email", request.Email ?? (object)DBNull.Value);
//        //                command.Parameters.AddWithValue("@Telefono", request.Telefono ?? (object)DBNull.Value);
//        //                command.Parameters.AddWithValue("@Direccion", request.Direccion ?? (object)DBNull.Value);
                        
//        //                using (var reader = await command.ExecuteReaderAsync())
//        //                {
//        //                    if (await reader.ReadAsync())
//        //                    {
//        //                        var proveedor = new Proveedor
//        //                        {
//        //                            Id = reader.GetInt32("Id"),
//        //                            Codigo = reader.GetString("Codigo"),
//        //                            Nombre = reader.GetString("Nombre"),
//        //                            Contacto = reader.IsDBNull("Contacto") ? null : reader.GetString("Contacto"),
//        //                            Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
//        //                            Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
//        //                            Direccion = reader.IsDBNull("Direccion") ? null : reader.GetString("Direccion"),
//        //                            Activo = reader.GetBoolean("Activo"),
//        //                            FechaCreacion = reader.GetDateTime("FechaCreacion")
//        //                        };

//        //                        return CreatedAtAction(nameof(ObtenerProveedores), 
//        //                            new { success = true, data = proveedor, message = "Proveedor creado correctamente" });
//        //                    }
//        //                }
//        //            }
//        //        }

//        //        return StatusCode(500, new { success = false, message = "Error al crear el proveedor" });
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
//        //    }
//        //}

//        #endregion

//        #region Gestión de Stock

//        /// <summary>
//        /// Obtiene el stock de un producto
//        /// </summary>
//        [HttpGet("productos/{id}/stock")]
//        public async Task<IActionResult> ObtenerStock(int id)
//        {
//            try
//            {
//                using (var connection = new SqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
                    
//                    using (var command = new SqlCommand("sp_ObtenerStock", connection))
//                    {
//                        command.CommandType = CommandType.StoredProcedure;
//                        command.Parameters.AddWithValue("@ProductoId", id);
                        
//                        using (var reader = await command.ExecuteReaderAsync())
//                        {
//                            if (await reader.ReadAsync())
//                            {
//                                var stock = new Stock
//                                {
//                                    Id = reader.GetInt32("Id"),
//                                    ProductoId = reader.GetInt32("ProductoId"),
//                                    Cantidad = reader.GetInt32("Cantidad"),
//                                    Ubicacion = reader.IsDBNull("Ubicacion") ? null : reader.GetString("Ubicacion"),
//                                    FechaUltimaActualizacion = reader.GetDateTime("FechaUltimaActualizacion")
//                                };

//                                return Ok(new { success = true, data = stock, message = "Stock obtenido correctamente" });
//                            }
//                            else
//                            {
//                                return NotFound(new { success = false, message = "Stock no encontrado" });
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
//            }
//        }

//        /// <summary>
//        /// Ajusta el stock de un producto
//        /// </summary>
//        [HttpPut("productos/stock/ajustar")]
//        public async Task<IActionResult> AjustarStock([FromBody] AjusteStockRequest request)
//        {
//            try
//            {
//                using (var connection = new SqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
                    
//                    using (var command = new SqlCommand("sp_AjustarStock", connection))
//                    {
//                        command.CommandType = CommandType.StoredProcedure;
//                        command.Parameters.Add("@ProductoId", SqlDbType.Int).Value = request.id;
//                        command.Parameters.AddWithValue("@CantidadAjuste", request.NuevaCantidad);
                        
//                        using (var reader = await command.ExecuteReaderAsync())
//                        {
//                            if (await reader.ReadAsync())
//                            {
//                                var stock = new Stock
//                                {
//                                    Id = reader.GetInt32("Id"),
//                                    ProductoId = reader.GetInt32("ProductoId"),
//                                    Cantidad = reader.GetInt32("Cantidad"),
//                                    Ubicacion = reader.IsDBNull("Ubicacion") ? null : reader.GetString("Ubicacion"),
//                                    FechaUltimaActualizacion = reader.GetDateTime("FechaUltimaActualizacion")
//                                };

//                                return Ok(new { success = true, data = stock, message = "Stock ajustado correctamente" });
//                            }
//                            else
//                            {
//                                return NotFound(new { success = false, message = "Producto no encontrado" });
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
//            }
//        }

//        #endregion

//        #region Gestión de Movimientos

//        /// <summary>
//        /// Registra una entrada de productos
//        /// </summary>
//        [HttpPost("entradas")]
//        public async Task<IActionResult> RegistrarEntrada([FromBody] EntradaProducto request)
//        {
//            try
//            {
//                using (var connection = new SqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
                    
//                    using (var command = new SqlCommand("sp_RegistrarEntrada", connection))
//                    {
//                        command.CommandType = CommandType.StoredProcedure;
//                        command.Parameters.AddWithValue("@ProductoId", request.ProductoId);
//                        command.Parameters.AddWithValue("@ProveedorId", request.ProveedorId);
//                        command.Parameters.AddWithValue("@Cantidad", request.Cantidad);
//                        command.Parameters.AddWithValue("@PrecioUnitario", request.PrecioUnitario);
//                        command.Parameters.AddWithValue("@NumeroFactura", request.NumeroFactura ?? (object)DBNull.Value);
//                        command.Parameters.AddWithValue("@Observaciones", request.Observaciones ?? (object)DBNull.Value);
                        
//                        using (var reader = await command.ExecuteReaderAsync())
//                        {
//                            if (await reader.ReadAsync())
//                            {
//                                var entrada = new EntradaProductoConDetalles
//                                {
//                                    Id = reader.GetInt32("Id"),
//                                    ProductoId = reader.GetInt32("ProductoId"),
//                                    ProveedorId = reader.GetInt32("ProveedorId"),
//                                    Cantidad = reader.GetInt32("Cantidad"),
//                                    PrecioUnitario = reader.GetDecimal("PrecioUnitario"),
//                                    FechaEntrada = reader.GetDateTime("FechaEntrada"),
//                                    NumeroFactura = reader.IsDBNull("NumeroFactura") ? null : reader.GetString("NumeroFactura"),
//                                    Observaciones = reader.IsDBNull("Observaciones") ? null : reader.GetString("Observaciones"),
//                                    ProductoNombre = reader.GetString("ProductoNombre"),
//                                    ProveedorNombre = reader.GetString("ProveedorNombre"),
//                                    ValorTotal = reader.GetDecimal("ValorTotal"),
//                                    DiferenciaPrecio = reader.IsDBNull("DiferenciaPrecio") ? 0 : reader.GetDecimal("DiferenciaPrecio"),
//                                    DiasDesdeEntrada = reader.GetInt32("DiasDesdeEntrada"),
//                                    EsEntradaReciente = reader.GetBoolean("EsEntradaReciente")
//                                };

//                                return Ok(new { success = true, data = entrada, message = "Entrada registrada correctamente" });
//                            }
//                        }
//                    }
//                }

//                return StatusCode(500, new { success = false, message = "Error al registrar la entrada" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
//            }
//        }

//        /// <summary>
//        /// Registra una salida de productos
//        /// </summary>
//        [HttpPost("salidas")]
//        public async Task<IActionResult> RegistrarSalida([FromBody] SalidaProducto request)
//        {
//            try
//            {
//                using (var connection = new SqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
                    
//                    using (var command = new SqlCommand("sp_RegistrarSalida", connection))
//                    {
//                        command.CommandType = CommandType.StoredProcedure;
//                        command.Parameters.AddWithValue("@ProductoId", request.ProductoId);
//                        command.Parameters.AddWithValue("@Cantidad", request.Cantidad);
//                        command.Parameters.AddWithValue("@Motivo", request.Motivo);
//                        command.Parameters.AddWithValue("@Responsable", request.Responsable ?? (object)DBNull.Value);
//                        command.Parameters.AddWithValue("@Observaciones", request.Observaciones ?? (object)DBNull.Value);
                        
//                        using (var reader = await command.ExecuteReaderAsync())
//                        {
//                            if (await reader.ReadAsync())
//                            {
//                                var salida = new SalidaProductoConDetalles
//                                {
//                                    Id = reader.GetInt32("Id"),
//                                    ProductoId = reader.GetInt32("ProductoId"),
//                                    Cantidad = reader.GetInt32("Cantidad"),
//                                    Motivo = reader.GetString("Motivo"),
//                                    FechaSalida = reader.GetDateTime("FechaSalida"),
//                                    Responsable = reader.IsDBNull("Responsable") ? null : reader.GetString("Responsable"),
//                                    Observaciones = reader.IsDBNull("Observaciones") ? null : reader.GetString("Observaciones"),
//                                    ProductoNombre = reader.GetString("ProductoNombre"),
//                                    ValorTotal = reader.GetDecimal("ValorTotal"),
//                                    DiasDesdeSalida = reader.GetInt32("DiasDesdeSalida"),
//                                    EsSalidaReciente = reader.GetBoolean("EsSalidaReciente"),
//                                    TieneResponsable = reader.GetBoolean("TieneResponsable"),
//                                    TieneObservaciones = reader.GetBoolean("TieneObservaciones")
//                                };

//                                return Ok(new { success = true, data = salida, message = "Salida registrada correctamente" });
//                            }
//                        }
//                    }
//                }

//                return StatusCode(500, new { success = false, message = "Error al registrar la salida" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
//            }
//        }

//        /// <summary>
//        /// Obtiene el historial de movimientos de un producto
//        /// </summary>
//        [HttpGet("productos/{id}/movimientos")]
//        public async Task<IActionResult> ObtenerHistorialMovimientos(int id)
//        {
//            try
//            {
//                var movimientos = new List<object>();

//                using (var connection = new SqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
                    
//                    using (var command = new SqlCommand("sp_ObtenerHistorialMovimientos", connection))
//                    {
//                        command.CommandType = CommandType.StoredProcedure;
//                        command.Parameters.AddWithValue("@ProductoId", id);
                        
//                        using (var reader = await command.ExecuteReaderAsync())
//                        {
//                            while (await reader.ReadAsync())
//                            {
//                                movimientos.Add(new
//                                {
//                                    Id = reader.GetInt32("Id"),
//                                    Tipo = reader.GetString("Tipo"),
//                                    Cantidad = reader.GetInt32("Cantidad"),
//                                    Fecha = reader.GetDateTime("Fecha"),
//                                    Descripcion = reader.GetString("Descripcion"),
//                                    Responsable = reader.IsDBNull("Responsable") ? null : reader.GetString("Responsable"),
//                                    Observaciones = reader.IsDBNull("Observaciones") ? null : reader.GetString("Observaciones")
//                                });
//                            }
//                        }
//                    }
//                }

//                return Ok(new { success = true, data = movimientos, message = "Historial de movimientos obtenido" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
//            }
//        }

//        #endregion

//        #region Reportes

//        /// <summary>
//        /// Genera un reporte de stock actual
//        /// </summary>
//        [HttpGet("reportes/stock-actual")]
//        public async Task<IActionResult> GenerarReporteStockActual()
//        {
//            try
//            {
//                var reporte = new List<object>();

//                using (var connection = new SqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
                    
//                    using (var command = new SqlCommand("sp_ReporteStockActual", connection))
//                    {
//                        command.CommandType = CommandType.StoredProcedure;
                        
//                        using (var reader = await command.ExecuteReaderAsync())
//                        {
//                            while (await reader.ReadAsync())
//                            {
//                                reporte.Add(new
//                                {
//                                    Id = reader.GetInt32("Id"),
//                                    Codigo = reader.GetString("Codigo"),
//                                    Nombre = reader.GetString("Nombre"),
//                                    Precio = reader.GetDecimal("Precio"),
//                                    StockActual = reader.GetInt32("StockActual"),
//                                    StockMinimo = reader.GetInt32("StockMinimo"),
//                                    StockMaximo = reader.GetInt32("StockMaximo"),
//                                    Categoria = reader.IsDBNull("Categoria") ? null : reader.GetString("Categoria"),
//                                    Ubicacion = reader.IsDBNull("Ubicacion") ? null : reader.GetString("Ubicacion"),
//                                    EstadoStock = reader.GetString("EstadoStock")
//                                });
//                            }
//                        }
//                    }
//                }

//                return Ok(new { success = true, data = reporte, message = "Reporte de stock actual generado" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
//            }
//        }

//        /// <summary>
//        /// Genera un reporte de estadísticas generales
//        /// </summary>
//        [HttpGet("reportes/estadisticas")]
//        public async Task<IActionResult> GenerarReporteEstadisticas()
//        {
//            try
//            {
//                using (var connection = new SqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
                    
//                    using (var command = new SqlCommand("sp_ReporteEstadisticas", connection))
//                    {
//                        command.CommandType = CommandType.StoredProcedure;
                        
//                        using (var reader = await command.ExecuteReaderAsync())
//                        {
//                            if (await reader.ReadAsync())
//                            {
//                                var estadisticas = new
//                                {
//                                    TotalProductos = reader.GetInt32("TotalProductos"),
//                                    TotalCategorias = reader.GetInt32("TotalCategorias"),
//                                    TotalProveedores = reader.GetInt32("TotalProveedores"),
//                                    StockTotal = reader.IsDBNull("StockTotal") ? 0 : reader.GetInt32("StockTotal"),
//                                    ProductosStockBajo = reader.GetInt32("ProductosStockBajo"),
//                                    ProductosAgotados = reader.GetInt32("ProductosAgotados"),
//                                    ProductosExcesoStock = reader.GetInt32("ProductosExcesoStock")
//                                };

//                                return Ok(new { success = true, data = estadisticas, message = "Estadísticas generadas correctamente" });
//                            }
//                        }
//                    }
//                }

//                return StatusCode(500, new { success = false, message = "Error al generar estadísticas" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
//            }
//        }

//        #endregion
//    }

//    /// <summary>
//    /// DTO para ajuste de stock
//    /// </summary>
//    public class AjusteStockRequest
//    {
//        public int id { get; set; }
//        public int NuevaCantidad { get; set; }
//    }
//}
