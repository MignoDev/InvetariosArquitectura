using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoInventario.Domain.Models;
using System.Data;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/cliente")]
    public class ClienteController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ClienteController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Obtiene todos los clientes
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerClientes()
        {
            try
            {
                var clientes = new List<Cliente>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Clientes_GetAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clientes.Add(new Cliente
                                {
                                    IdCliente = reader.GetInt32("IdCliente"),                                    
                                    Nombre = reader.GetString("Nombre"),                                    
                                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                                    Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
                                    Direccion = reader.IsDBNull("Direccion") ? null : reader.GetString("Direccion"),                                                                       
                                });
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = clientes, message = "Clientes obtenidos correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Obtiene Cliente por su id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerClienteById(int id)
        {
            try
            {
                var clientes = new List<Cliente>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Clientes_GetById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdCliente", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clientes.Add(new Cliente
                                {
                                    IdCliente = reader.GetInt32("IdCliente"),
                                    Nombre = reader.GetString("Nombre"),
                                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                                    Telefono = reader.IsDBNull("Telefono") ? null : reader.GetString("Telefono"),
                                    Direccion = reader.IsDBNull("Direccion") ? null : reader.GetString("Direccion")
                                });
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = clientes, message = "Clientes obtenidos correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Obtiene Cliente por su id
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearClientes([FromBody] Cliente cliente)
        {
            try
            {

                int? idCliente = null;

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Clientes_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                        command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                        command.Parameters.AddWithValue("@Email", cliente);
                        command.Parameters.AddWithValue("@Direccion", cliente.Direccion);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                               idCliente = reader.GetInt32("IdCliente");
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = idCliente, message = "Clientes obtenidos correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Obtiene Cliente por su id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarClientes(int id, [FromBody] Cliente cliente)
        {
            try
            {

                int? idCliente = null;

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Clientes_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                        command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                        command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                        command.Parameters.AddWithValue("@Email", cliente);
                        command.Parameters.AddWithValue("@Direccion", cliente.Direccion);

                        using (var reader = await command.ExecuteReaderAsync()) 
                        {
                            while (await reader.ReadAsync())
                            {
                               idCliente = reader.GetInt32("IdCliente");
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = idCliente, message = "Clientes obtenidos correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }
    }
}
