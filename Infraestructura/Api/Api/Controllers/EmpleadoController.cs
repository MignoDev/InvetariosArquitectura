using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoInventario.Domain.Models;
using System.Data;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/empleado")]
    public class EmpleadoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public EmpleadoController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }


        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerEmpleados()
        {
            try
            {
                var empleado = new List<Empleado>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Empleados_GetAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                empleado.Add(new Empleado
                                {
                                    IdEmpleado = reader.GetInt32("IdEmpleado"),
                                    Nombre = reader.GetString("Nombre"),
                                    Cargo = reader.GetString("Cargo"),
                                    Email = reader.GetString("Email"),
                                    Telefono = reader.GetString("Telefono"),
                                    Activo = reader.GetBoolean("Activo"),
                                });
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = empleado, message = "Productos obtenidos correctamente" });
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
        public async Task<IActionResult> ObtenerEmpleadosActivos()
        {
            try
            {
                var empleado = new List<Empleado>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Empleados_GetActive", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                empleado.Add(new Empleado
                                {
                                    IdEmpleado = reader.GetInt32("IdEmpleado"),
                                    Nombre = reader.GetString("Nombre"),
                                    Cargo = reader.GetString("Cargo"),
                                    Email = reader.GetString("Email"),
                                    Telefono = reader.GetString("Telefono"),
                                    Activo = reader.GetBoolean("Activo"),
                                });
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = empleado, message = "Empleados obtenidos correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerEmpleado(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Productos_GetById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdProducto", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var empleado = new Empleado()
                                {
                                    IdEmpleado = reader.GetInt32("IdEmpleado"),
                                    Nombre = reader.GetString("Nombre"),
                                    Cargo = reader.GetString("Cargo"),
                                    Email = reader.GetString("Email"),
                                    Telefono = reader.GetString("Telefono"),
                                    Activo = reader.GetBoolean("Activo")
                                };

                                return Ok(new { success = true, data = empleado, message = "Empleado obtenido correctamente" });
                            }
                            else
                            {
                                return NotFound(new { success = false, message = "Empleado no encontrado" });
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
        public async Task<IActionResult> CrearEmpleado([FromBody] Empleado request)
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
                        command.Parameters.AddWithValue("@Cargo", request.Cargo);
                        command.Parameters.AddWithValue("@Email", request.Email);
                        command.Parameters.AddWithValue("@Telefono", request.Telefono);
                        command.Parameters.AddWithValue("@Activo", request.Activo);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var producto = new Producto()
                                {
                                    IdProducto = reader.GetInt32("IdProducto")
                                };

                                return CreatedAtAction(nameof(CrearEmpleado),
                                    new { success = true, data = producto, message = "Empleado creado correctamente" });
                            }
                        }
                    }
                }

                return StatusCode(500, new { success = false, message = "Error al crear el empleado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Actualiza un producto
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEmpleado(int id, [FromBody] Empleado request)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_ActualizarProducto", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdEmpleado", id);
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);
                        command.Parameters.AddWithValue("@Cargo", request.Cargo);
                        command.Parameters.AddWithValue("@Email", request.Email);
                        command.Parameters.AddWithValue("@Telefono", request.Telefono);
                        command.Parameters.AddWithValue("@Activo", request.Activo);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {


                                return Ok(new { success = true, data = id, message = "Empleado actualizado correctamente" });
                            }
                            else
                            {
                                return NotFound(new { success = false, message = "Empleado no encontrado" });
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
