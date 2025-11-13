using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoInventario.Domain.Models;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.Controllers
{
    /// <summary>
    /// Controlador de proveedores
    /// </summary>
    [ApiController]
    [Route("api/proveedor")]
    public class ProveedorController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ProveedorController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Obtiene todos los proveedores
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerProveedores()
        {
            try
            {
                var proveedores = new List<Proveedor>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Proveedores_GetAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                proveedores.Add(new Proveedor
                                {
                                    IdProveedor = reader.GetInt32("IdProveedor"),                                    
                                    Nombre = reader.GetString("Nombre"),
                                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                                    Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
                                    Direccion = reader.IsDBNull("Direccion") ? null : reader.GetString("Direccion")
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
        /// Obtiene todos los proveedores
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerProveedores(int id)
        {
            try
            {
                var proveedores = new List<Proveedor>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Proveedores_GetAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdProveedor", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                proveedores.Add(new Proveedor
                                {
                                    IdProveedor = reader.GetInt32("IdProveedor"),
                                    Nombre = reader.GetString("Nombre"),
                                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                                    Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
                                    Direccion = reader.IsDBNull("Direccion") ? null : reader.GetString("Direccion")
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
        /// Obtiene todos los proveedores
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearProveedor([FromBody] Proveedor proveedor)
        {
            try
            {
                var proveedores = new List<Proveedor>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Proveedores_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                        command.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                        command.Parameters.AddWithValue("@Email", proveedor.Email);
                        command.Parameters.AddWithValue("@Direccion", proveedor.Direccion);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            
                            while (await reader.ReadAsync())
                            {
                                proveedor.IdProveedor = reader.GetInt32("IdProveedor");
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = proveedor.IdProveedor, message = "Proveedor creado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Actualiza un proveedor
        /// </summary>
        [HttpPost("{id}")]
        public async Task<IActionResult> ActualizarProveedor(int id, [FromBody] Proveedor proveedor)
        {
            try
            {
                var proveedores = new List<Proveedor>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Proveedores_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@IdProveedor", id);
                        command.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                        command.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                        command.Parameters.AddWithValue("@Email", proveedor.Email);
                        command.Parameters.AddWithValue("@Direccion", proveedor.Direccion);

                        using (var reader = await command.ExecuteReaderAsync())
                        {

                            while (await reader.ReadAsync())
                            {
                                proveedor.IdProveedor = reader.GetInt32("IdProveedor");
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = proveedor.IdProveedor, message = "Proveedor creado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }
    }
}
