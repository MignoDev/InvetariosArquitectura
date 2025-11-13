using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoInventario.Domain.Models;
using System.Data;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/facturas")]
    public class FacturasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public FacturasController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Obtiene todas las facturas
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerFacturas()
        {
            try
            {
                var facturas = new List<Factura>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Facturas_GetAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                facturas.Add(new Factura
                                {
                                    IdFactura = reader.GetInt32("IdFactura"),
                                    FechaFactura = reader.GetDateTime("FechaFactura"),
                                    IdCliente = reader.GetInt32("IdCliente"),
                                    Cliente = reader.GetString("Cliente"),
                                    IdEmpleado = reader.GetInt32("IdEmpleado"),
                                    Empleado = reader.GetString("Empleado"),
                                    Total = reader.GetDecimal("Total")
                                });
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = facturas, message = "Facturas obtenidas correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Obtiene factura por su id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerFacturasById(int id)
        {
            try
            {
                var facturas = new List<Factura>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Facturas_GetById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdFactura", id);


                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            await reader.ReadAsync();
                            facturas.Add(new Factura
                            {
                                IdFactura = reader.GetInt32("IdFactura"),
                                FechaFactura = reader.GetDateTime("FechaFactura"),
                                IdCliente = reader.GetInt32("IdCliente"),
                                Cliente = reader.GetString("Cliente"),
                                IdEmpleado = reader.GetInt32("IdEmpleado"),
                                Empleado = reader.GetString("Empleado"),
                                Total = reader.GetDecimal("Total")
                            });
                        }
                    }
                }

                return Ok(new { success = true, data = facturas, message = "Facturas obtenidas correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Obtiene factura por su id
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearFactura([FromBody] Factura factura)
        {
            try
            {
                var facturas = new Factura();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Facturas_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdCliente", factura.IdCliente);
                        command.Parameters.AddWithValue("@IdEmpleado", factura.IdEmpleado);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            facturas = new Factura
                            {
                                IdFactura = reader.GetInt32("IdFactura"),
                            };
                        }
                    }
                }

                return Ok(new { success = true, data = facturas.IdFactura, message = "Factura obtenidas correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        
    }
}
