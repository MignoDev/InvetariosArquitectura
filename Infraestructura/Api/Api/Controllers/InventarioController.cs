using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using ProyectoInventario.Domain.Models;

namespace ProyectoInventario.Infraestructura.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión del inventario usando procedimientos almacenados simplificados
    /// </summary>
    [ApiController]
    [Route("api/inventario")]
    public class InventarioController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public InventarioController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        #region Gestión de Productos

        /// <summary>
        /// Obtiene todos los productos activos con stock
        /// </summary>
        [HttpGet("productos")]
        public async Task<IActionResult> ObtenerProductosActivos()
        {
            try
            {
                var productos = new List<ProductoConStock>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_ObtenerProductosActivos", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                productos.Add(new ProductoConStock
                                {
                                    Id = reader.GetGuid("Id"),
                                    Codigo = reader.GetString("Codigo"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                                    Precio = reader.GetDecimal("Precio"),
                                    StockMinimo = reader.GetInt32("StockMinimo"),
                                    StockMaximo = reader.GetInt32("StockMaximo"),
                                    Activo = reader.GetBoolean("Activo"),
                                    FechaCreacion = reader.GetDateTime("FechaCreacion"),
                                    FechaModificacion = reader.GetDateTime("FechaModificacion"),
                                    CategoriaId = reader.IsDBNull("CategoriaId") ? (Guid?)null : reader.GetGuid("CategoriaId"),
                                    CategoriaNombre = reader.IsDBNull("CategoriaNombre") ? null : reader.GetString("CategoriaNombre"),
                                    StockActual = reader.IsDBNull("StockActual") ? 0 : reader.GetInt32("StockActual"),                                  
                                    //TieneStockBajo = reader.IsDBNull("TieneStockBajo") ? false : reader.GetBoolean("TieneStockBajo"),
                                    //TieneExcesoStock = reader.IsDBNull("TieneExcesoStock") ? false : reader.GetBoolean("TieneExcesoStock"),
                                    //EstaAgotado = reader.IsDBNull("EstaAgotado") ? false : reader.GetBoolean("EstaAgotado"),
                                    //ValorTotalInventario = reader.IsDBNull("ValorTotalInventario") ? 0 : reader.GetDecimal("ValorTotalInventario")
                                });
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = productos, message = "Productos obtenidos correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        [HttpGet("productos/{id}")]
        public async Task<IActionResult> ObtenerProducto(Guid id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_ObtenerProductoPorId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var producto = new ProductoConStock
                                {
                                    Id = reader.GetGuid("Id"),
                                    Codigo = reader.GetString("Codigo"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                                    Precio = reader.GetDecimal("Precio"),
                                    StockMinimo = reader.GetInt32("StockMinimo"),
                                    StockMaximo = reader.GetInt32("StockMaximo"),
                                    Activo = reader.GetBoolean("Activo"),
                                    FechaCreacion = reader.GetDateTime("FechaCreacion"),
                                    FechaModificacion = reader.GetDateTime("FechaModificacion"),
                                    CategoriaId = reader.IsDBNull("CategoriaId") ? (Guid?)null : reader.GetGuid("CategoriaId"),
                                    CategoriaNombre = reader.IsDBNull("CategoriaNombre") ? null : reader.GetString("CategoriaNombre"),
                                    StockActual = reader.IsDBNull("StockActual") ? 0 : reader.GetInt32("StockActual"),
                                    
                                    TieneStockBajo = reader.IsDBNull("TieneStockBajo") ? false : reader.GetBoolean("TieneStockBajo"),
                                    TieneExcesoStock = reader.IsDBNull("TieneExcesoStock") ? false : reader.GetBoolean("TieneExcesoStock"),
                                    EstaAgotado = reader.IsDBNull("EstaAgotado") ? false : reader.GetBoolean("EstaAgotado"),
                                    ValorTotalInventario = reader.IsDBNull("ValorTotalInventario") ? 0 : reader.GetDecimal("ValorTotalInventario")
                                };

                                return Ok(new { success = true, data = producto, message = "Producto obtenido correctamente" });
                            }
                            else
                            {
                                return NotFound(new { success = false, message = "Producto no encontrado" });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        [HttpPost("productos")]
        public async Task<IActionResult> CrearProducto([FromBody] ProductoConStock request)
        {
            try
            {
                var productoId = Guid.NewGuid();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_CrearProducto", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Codigo", request.Codigo);
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);
                        command.Parameters.AddWithValue("@Precio", request.Precio);
                        command.Parameters.AddWithValue("@CategoriaId", request.CategoriaId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@StockInicial", request.StockInicial); // Stock inicial por defecto
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var producto = new ProductoConStock
                                {
                                    Id = reader.GetGuid("Id"),
                                    Codigo = reader.GetString("Codigo"),
                                    Nombre = reader.GetString("Nombre"),
                                    CategoriaId = reader.IsDBNull("CategoriaId") ? (Guid?)null : reader.GetGuid("CategoriaId")
                                };

                                return CreatedAtAction(nameof(ObtenerProducto), new { id = productoId }, 
                                    new { success = true, data = producto, message = "Producto creado correctamente" });
                            }
                        }
                    }
                }

                return StatusCode(500, new { success = false, message = "Error al crear el producto" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Actualiza un producto
        /// </summary>
        [HttpPut("productos/{id}")]
        public async Task<IActionResult> ActualizarProducto(Guid id, [FromBody] Producto request)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_ActualizarProducto", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);
                        command.Parameters.AddWithValue("@Descripcion", request.Descripcion ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Precio", request.Precio);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var producto = new ProductoConStock
                                {
                                    Id = reader.GetGuid("Id"),
                                    Codigo = reader.GetString("Codigo"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                                    Precio = reader.GetDecimal("Precio"),
                                    StockMinimo = reader.GetInt32("StockMinimo"),
                                    StockMaximo = reader.GetInt32("StockMaximo"),
                                    Activo = reader.GetBoolean("Activo"),
                                    FechaCreacion = reader.GetDateTime("FechaCreacion"),
                                    FechaModificacion = reader.GetDateTime("FechaModificacion"),
                                    CategoriaId = reader.IsDBNull("CategoriaId") ? (Guid?)null : reader.GetGuid("CategoriaId"),
                                    CategoriaNombre = reader.IsDBNull("CategoriaNombre") ? null : reader.GetString("CategoriaNombre"),
                                    StockActual = reader.IsDBNull("StockActual") ? 0 : reader.GetInt32("StockActual"),
                                    TieneStockBajo = reader.IsDBNull("TieneStockBajo") ? false : reader.GetBoolean("TieneStockBajo"),
                                    TieneExcesoStock = reader.IsDBNull("TieneExcesoStock") ? false : reader.GetBoolean("TieneExcesoStock"),
                                    EstaAgotado = reader.IsDBNull("EstaAgotado") ? false : reader.GetBoolean("EstaAgotado"),
                                    ValorTotalInventario = reader.IsDBNull("ValorTotalInventario") ? 0 : reader.GetDecimal("ValorTotalInventario")
                                };

                                return Ok(new { success = true, data = producto, message = "Producto actualizado correctamente" });
                            }
                            else
                            {
                                return NotFound(new { success = false, message = "Producto no encontrado" });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        #endregion

        #region Gestión de Categorías

        /// <summary>
        /// Obtiene todas las categorías
        /// </summary>
        [HttpGet("categorias")]
        public async Task<IActionResult> ObtenerCategorias()
        {
            try
            {
                var categorias = new List<Categoria>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_ObtenerCategoriasActivas", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                categorias.Add(new Categoria
                                {
                                    Id = reader.GetGuid("Id"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                                    Activo = reader.GetBoolean("Activo"),
                                    FechaCreacion = reader.GetDateTime("FechaCreacion")
                                });
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = categorias, message = "Categorías obtenidas correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Crea una nueva categoría
        /// </summary>
        [HttpPost("categorias")]
        public async Task<IActionResult> CrearCategoria([FromBody] Categoria request)
        {
            try
            {
                var categoriaId = Guid.NewGuid();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_CrearCategoria", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", categoriaId);
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);
                        command.Parameters.AddWithValue("@Descripcion", request.Descripcion ?? (object)DBNull.Value);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var categoria = new Categoria
                                {
                                    Id = reader.GetGuid("Id"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                                    Activo = reader.GetBoolean("Activo"),
                                    FechaCreacion = reader.GetDateTime("FechaCreacion")
                                };

                                return CreatedAtAction(nameof(ObtenerCategorias), new { id = categoriaId }, 
                                    new { success = true, data = categoria, message = "Categoría creada correctamente" });
                            }
                        }
                    }
                }

                return StatusCode(500, new { success = false, message = "Error al crear la categoría" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        #endregion

        #region Gestión de Proveedores

        /// <summary>
        /// Obtiene todos los proveedores
        /// </summary>
        [HttpGet("proveedores")]
        public async Task<IActionResult> ObtenerProveedores()
        {
            try
            {
                var proveedores = new List<Proveedor>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_ObtenerProveedoresActivos", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                proveedores.Add(new Proveedor
                                {
                                    Id = reader.GetGuid("Id"),
                                    Codigo = reader.GetString("Codigo"),
                                    Nombre = reader.GetString("Nombre"),
                                    Contacto = reader.IsDBNull("Contacto") ? null : reader.GetString("Contacto"),
                                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                                    Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
                                    Direccion = reader.IsDBNull("Direccion") ? null : reader.GetString("Direccion"),
                                    Activo = reader.GetBoolean("Activo"),
                                    FechaCreacion = reader.GetDateTime("FechaCreacion")
                                });
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = proveedores, message = "Proveedores obtenidos correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Crea un nuevo proveedor
        /// </summary>
        [HttpPost("proveedores")]
        public async Task<IActionResult> CrearProveedor([FromBody] Proveedor request)
        {
            try
            {
                var proveedorId = Guid.NewGuid();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_CrearProveedor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", proveedorId);
                        command.Parameters.AddWithValue("@Codigo", request.Codigo);
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);
                        command.Parameters.AddWithValue("@Contacto", request.Contacto ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Email", request.Email ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Telefono", request.Telefono ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Direccion", request.Direccion ?? (object)DBNull.Value);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var proveedor = new Proveedor
                                {
                                    Id = reader.GetGuid("Id"),
                                    Codigo = reader.GetString("Codigo"),
                                    Nombre = reader.GetString("Nombre"),
                                    Contacto = reader.IsDBNull("Contacto") ? null : reader.GetString("Contacto"),
                                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                                    Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
                                    Direccion = reader.IsDBNull("Direccion") ? null : reader.GetString("Direccion"),
                                    Activo = reader.GetBoolean("Activo"),
                                    FechaCreacion = reader.GetDateTime("FechaCreacion")
                                };

                                return CreatedAtAction(nameof(ObtenerProveedores), new { id = proveedorId }, 
                                    new { success = true, data = proveedor, message = "Proveedor creado correctamente" });
                            }
                        }
                    }
                }

                return StatusCode(500, new { success = false, message = "Error al crear el proveedor" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        #endregion

        #region Gestión de Stock

        /// <summary>
        /// Obtiene el stock de un producto
        /// </summary>
        [HttpGet("productos/{id}/stock")]
        public async Task<IActionResult> ObtenerStock(Guid id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_ObtenerStock", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductoId", id);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var stock = new Stock
                                {
                                    Id = reader.GetGuid("Id"),
                                    ProductoId = reader.GetGuid("ProductoId"),
                                    Cantidad = reader.GetInt32("Cantidad"),
                                    Ubicacion = reader.IsDBNull("Ubicacion") ? null : reader.GetString("Ubicacion"),
                                    FechaUltimaActualizacion = reader.GetDateTime("FechaUltimaActualizacion")
                                };

                                return Ok(new { success = true, data = stock, message = "Stock obtenido correctamente" });
                            }
                            else
                            {
                                return NotFound(new { success = false, message = "Stock no encontrado" });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Ajusta el stock de un producto
        /// </summary>
        [HttpPut("productos/stock/ajustar")]
        public async Task<IActionResult> AjustarStock([FromBody] AjusteStockRequest request)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_AjustarStock", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ProductoId", SqlDbType.UniqueIdentifier).Value = request.id;
                        command.Parameters.AddWithValue("@CantidadAjuste", request.NuevaCantidad);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var stock = new Stock
                                {
                                    Id = reader.GetGuid("Id"),
                                    ProductoId = reader.GetGuid("ProductoId"),
                                    Cantidad = reader.GetInt32("Cantidad"),
                                    Ubicacion = reader.IsDBNull("Ubicacion") ? null : reader.GetString("Ubicacion"),
                                    FechaUltimaActualizacion = reader.GetDateTime("FechaUltimaActualizacion")
                                };

                                return Ok(new { success = true, data = stock, message = "Stock ajustado correctamente" });
                            }
                            else
                            {
                                return NotFound(new { success = false, message = "Producto no encontrado" });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        #endregion

        #region Gestión de Movimientos

        /// <summary>
        /// Registra una entrada de productos
        /// </summary>
        [HttpPost("entradas")]
        public async Task<IActionResult> RegistrarEntrada([FromBody] EntradaProducto request)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_RegistrarEntrada", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductoId", request.ProductoId);
                        command.Parameters.AddWithValue("@ProveedorId", request.ProveedorId);
                        command.Parameters.AddWithValue("@Cantidad", request.Cantidad);
                        command.Parameters.AddWithValue("@PrecioUnitario", request.PrecioUnitario);
                        command.Parameters.AddWithValue("@NumeroFactura", request.NumeroFactura ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Observaciones", request.Observaciones ?? (object)DBNull.Value);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var entrada = new EntradaProductoConDetalles
                                {
                                    Id = reader.GetGuid("Id"),
                                    ProductoId = reader.GetGuid("ProductoId"),
                                    ProveedorId = reader.GetGuid("ProveedorId"),
                                    Cantidad = reader.GetInt32("Cantidad"),
                                    PrecioUnitario = reader.GetDecimal("PrecioUnitario"),
                                    FechaEntrada = reader.GetDateTime("FechaEntrada"),
                                    NumeroFactura = reader.IsDBNull("NumeroFactura") ? null : reader.GetString("NumeroFactura"),
                                    Observaciones = reader.IsDBNull("Observaciones") ? null : reader.GetString("Observaciones"),
                                    ProductoNombre = reader.GetString("ProductoNombre"),
                                    ProveedorNombre = reader.GetString("ProveedorNombre"),
                                    ValorTotal = reader.GetDecimal("ValorTotal"),
                                    DiferenciaPrecio = reader.IsDBNull("DiferenciaPrecio") ? 0 : reader.GetDecimal("DiferenciaPrecio"),
                                    DiasDesdeEntrada = reader.GetInt32("DiasDesdeEntrada"),
                                    EsEntradaReciente = reader.GetBoolean("EsEntradaReciente")
                                };

                                return Ok(new { success = true, data = entrada, message = "Entrada registrada correctamente" });
                            }
                        }
                    }
                }

                return StatusCode(500, new { success = false, message = "Error al registrar la entrada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Registra una salida de productos
        /// </summary>
        [HttpPost("salidas")]
        public async Task<IActionResult> RegistrarSalida([FromBody] SalidaProducto request)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_RegistrarSalida", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductoId", request.ProductoId);
                        command.Parameters.AddWithValue("@Cantidad", request.Cantidad);
                        command.Parameters.AddWithValue("@Motivo", request.Motivo);
                        command.Parameters.AddWithValue("@Responsable", request.Responsable ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Observaciones", request.Observaciones ?? (object)DBNull.Value);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var salida = new SalidaProductoConDetalles
                                {
                                    Id = reader.GetGuid("Id"),
                                    ProductoId = reader.GetGuid("ProductoId"),
                                    Cantidad = reader.GetInt32("Cantidad"),
                                    Motivo = reader.GetString("Motivo"),
                                    FechaSalida = reader.GetDateTime("FechaSalida"),
                                    Responsable = reader.IsDBNull("Responsable") ? null : reader.GetString("Responsable"),
                                    Observaciones = reader.IsDBNull("Observaciones") ? null : reader.GetString("Observaciones"),
                                    ProductoNombre = reader.GetString("ProductoNombre"),
                                    ValorTotal = reader.GetDecimal("ValorTotal"),
                                    DiasDesdeSalida = reader.GetInt32("DiasDesdeSalida"),
                                    EsSalidaReciente = reader.GetBoolean("EsSalidaReciente"),
                                    TieneResponsable = reader.GetBoolean("TieneResponsable"),
                                    TieneObservaciones = reader.GetBoolean("TieneObservaciones")
                                };

                                return Ok(new { success = true, data = salida, message = "Salida registrada correctamente" });
                            }
                        }
                    }
                }

                return StatusCode(500, new { success = false, message = "Error al registrar la salida" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Obtiene el historial de movimientos de un producto
        /// </summary>
        [HttpGet("productos/{id}/movimientos")]
        public async Task<IActionResult> ObtenerHistorialMovimientos(Guid id)
        {
            try
            {
                var movimientos = new List<object>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_ObtenerHistorialMovimientos", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductoId", id);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                movimientos.Add(new
                                {
                                    Id = reader.GetGuid("Id"),
                                    Tipo = reader.GetString("Tipo"),
                                    Cantidad = reader.GetInt32("Cantidad"),
                                    Fecha = reader.GetDateTime("Fecha"),
                                    Descripcion = reader.GetString("Descripcion"),
                                    Responsable = reader.IsDBNull("Responsable") ? null : reader.GetString("Responsable"),
                                    Observaciones = reader.IsDBNull("Observaciones") ? null : reader.GetString("Observaciones")
                                });
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = movimientos, message = "Historial de movimientos obtenido" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        #endregion

        #region Reportes

        /// <summary>
        /// Genera un reporte de stock actual
        /// </summary>
        [HttpGet("reportes/stock-actual")]
        public async Task<IActionResult> GenerarReporteStockActual()
        {
            try
            {
                var reporte = new List<object>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_ReporteStockActual", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                reporte.Add(new
                                {
                                    Id = reader.GetGuid("Id"),
                                    Codigo = reader.GetString("Codigo"),
                                    Nombre = reader.GetString("Nombre"),
                                    Precio = reader.GetDecimal("Precio"),
                                    StockActual = reader.GetInt32("StockActual"),
                                    StockMinimo = reader.GetInt32("StockMinimo"),
                                    StockMaximo = reader.GetInt32("StockMaximo"),
                                    Categoria = reader.IsDBNull("Categoria") ? null : reader.GetString("Categoria"),
                                    Ubicacion = reader.IsDBNull("Ubicacion") ? null : reader.GetString("Ubicacion"),
                                    EstadoStock = reader.GetString("EstadoStock")
                                });
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = reporte, message = "Reporte de stock actual generado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Genera un reporte de estadísticas generales
        /// </summary>
        [HttpGet("reportes/estadisticas")]
        public async Task<IActionResult> GenerarReporteEstadisticas()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_ReporteEstadisticas", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var estadisticas = new
                                {
                                    TotalProductos = reader.GetInt32("TotalProductos"),
                                    TotalCategorias = reader.GetInt32("TotalCategorias"),
                                    TotalProveedores = reader.GetInt32("TotalProveedores"),
                                    StockTotal = reader.IsDBNull("StockTotal") ? 0 : reader.GetInt32("StockTotal"),
                                    ProductosStockBajo = reader.GetInt32("ProductosStockBajo"),
                                    ProductosAgotados = reader.GetInt32("ProductosAgotados"),
                                    ProductosExcesoStock = reader.GetInt32("ProductosExcesoStock")
                                };

                                return Ok(new { success = true, data = estadisticas, message = "Estadísticas generadas correctamente" });
                            }
                        }
                    }
                }

                return StatusCode(500, new { success = false, message = "Error al generar estadísticas" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        #endregion
    }

    /// <summary>
    /// DTO para ajuste de stock
    /// </summary>
    public class AjusteStockRequest
    {
        public Guid id { get; set; }
        public int NuevaCantidad { get; set; }
    }
}
