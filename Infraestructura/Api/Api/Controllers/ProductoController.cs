using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoInventario.Domain.Models;
using System.Data;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/producto")]
    public class ProductoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ProductoController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerProductos()
        {
            try
            {
                var productos = new List<Producto>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_Productos_GetAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                productos.Add(new Producto
                                {
                                    IdProducto = reader.GetInt32("IdProducto"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                                    PrecioCompra = reader.GetDecimal("PrecioCompra"),
                                    PrecioVenta = reader.GetDecimal("PrecioVenta"),
                                    StockMinimo = reader.GetInt32("StockMinimo"),
                                    StockActual = reader.GetInt32("StockActual"),
                                    IdCategoria = reader.IsDBNull("IdCategoria") ? (int?)null : reader.GetInt32("IdCategoria"),
                                    Categoria = reader.IsDBNull("Categoria") ? null : reader.GetString("Categoria"),
                                    Activo = reader.GetBoolean("Activo"),
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
        /// Obtiene todos los productos activos con stock
        /// </summary>
        [HttpGet("activo")]
        public async Task<IActionResult> ObtenerProductosActivos()
        {
            try
            {
                var productos = new List<Producto>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Productos_GetActive", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                productos.Add(new ProductoConStock
                                {
                                    IdProducto = reader.GetInt32("IdProducto"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                                    PrecioCompra = reader.GetDecimal("PrecioCompra"),
                                    PrecioVenta = reader.GetDecimal("PrecioVenta"),
                                    StockMinimo = reader.GetInt32("StockMinimo"),
                                    StockActual = reader.GetInt32("StockActual"),
                                    IdCategoria = reader.IsDBNull("IdCategoria") ? (int?)null : reader.GetInt32("IdCategoria"),
                                    CategoriaNombre = reader.IsDBNull("CategoriaNombre") ? null : reader.GetString("CategoriaNombre"),
                                    Activo = reader.GetBoolean("Activo"),                               
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
        /// Obtiene un producto por IdProducto
        /// </summary>
        [HttpGet("{IdProducto}")]
        public async Task<IActionResult> ObtenerProducto(int IdProducto)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Productos_GetById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdProducto", IdProducto);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var producto = new Producto()
                                {
                                    IdProducto = reader.GetInt32("IdProducto"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                                    PrecioCompra = reader.GetDecimal("PrecioCompra"),
                                    PrecioVenta = reader.GetDecimal("PrecioVenta"),
                                    StockMinimo = reader.GetInt32("StockMinimo"),
                                    StockActual = reader.GetInt32("StockActual"),
                                    IdCategoria = reader.IsDBNull("IdCategoria") ? (int?)null : reader.GetInt32("IdCategoria"),                                    
                                    Activo = reader.GetBoolean("Activo"),                                    
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
        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] Producto request)
        {
            try
            {

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Productos_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);
                        command.Parameters.AddWithValue("@Descripcion", request.Descripcion);
                        command.Parameters.AddWithValue("@PrecioCompra", request.PrecioCompra);
                        command.Parameters.AddWithValue("@PrecioVenta", request.PrecioVenta);
                        command.Parameters.AddWithValue("@StockActual", request.StockActual);
                        command.Parameters.AddWithValue("@StockMinimo", request.StockMinimo);
                        command.Parameters.AddWithValue("@IdCategoria", request.IdCategoria ?? (object)DBNull.Value);                        

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                

                                return CreatedAtAction(nameof(ObtenerProducto),
                                    new { success = true, data = true, message = "Producto creado correctamente" });
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
        [HttpPut("{IdProducto}")]
        public async Task<IActionResult> ActualizarProducto(int IdProducto, [FromBody] Producto request)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Productos_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@IdProducto", IdProducto);
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);
                        command.Parameters.AddWithValue("@Descripcion", request.Descripcion);
                        command.Parameters.AddWithValue("@PrecioCompra", request.PrecioCompra);
                        command.Parameters.AddWithValue("@PrecioVenta", request.PrecioVenta);
                        command.Parameters.AddWithValue("@StockActual", request.StockActual);
                        command.Parameters.AddWithValue("@StockMinimo", request.StockMinimo);
                        command.Parameters.AddWithValue("@IdCategoria", request.IdCategoria ?? (object)DBNull.Value);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                

                                return Ok(new { success = true, data = IdProducto, message = "Producto actualizado correctamente" });
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
    }
}
