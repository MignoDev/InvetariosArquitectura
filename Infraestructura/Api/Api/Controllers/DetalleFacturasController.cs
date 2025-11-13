using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoInventario.Domain.Models;
using System.Data;

namespace Api.Controllers;

[ApiController]
[Route("api/detalle-facturas")]
public class DetalleFacturasController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DetalleFacturasController(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    /// Obtiene todas los detalle de factura
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerDetalleFactura()
    {
        try
        {
            var DetalleFacturas = new List<DetalleFacturas>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_DetalleFacturas_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            DetalleFacturas.Add(new DetalleFacturas
                            {
                               IdDetalleFactura = reader.GetInt32("IdDetalleFactura"),
                               IdFactura = reader.GetInt32("IdFactura"),
                               IdProducto = reader.GetInt32("IdProducto"),
                               Cantidad = reader.GetInt32("Cantidad"),
                               NombreProducto = reader.GetString("NombreProducto"),
                               PrecioUnitario = reader.GetDecimal("PrecioUnitario"),
                               Subtotal = reader.GetDecimal("Subtotal")
                            });
                        }
                    }
                }
            }

            return Ok(new { success = true, data = DetalleFacturas, message = "Categorías obtenidas correctamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Obtiene todas los detalle de factura por id de la factura
    /// </summary>
    [HttpGet("{idFactura}")]
    public async Task<IActionResult> ObtenerDetalleFacturaByIdFactura(int idFactura)
    {
        try
        {
            var DetalleFacturas = new List<DetalleFacturas>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_DetalleFacturas_GetByIdFactura", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdFactura", idFactura);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            DetalleFacturas.Add(new DetalleFacturas
                            {
                                IdDetalleFactura = reader.GetInt32("IdDetalleFactura"),
                                IdFactura = reader.GetInt32("IdFactura"),
                                IdProducto = reader.GetInt32("IdProducto"),
                                Cantidad = reader.GetInt32("Cantidad"),
                                NombreProducto = reader.GetString("NombreProducto"),
                                PrecioUnitario = reader.GetDecimal("PrecioUnitario"),
                                Subtotal = reader.GetDecimal("Subtotal")
                            });
                        }
                    }
                }
            }

            return Ok(new { success = true, data = DetalleFacturas, message = "Categorías obtenidas correctamente" });
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
    public async Task<IActionResult> CrearDetalleFactura([FromBody] DetalleFacturas detalleFactura)
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
                    command.Parameters.AddWithValue("@Cantidad", detalleFactura.Cantidad);
                    command.Parameters.AddWithValue("@IdFactura", detalleFactura.IdFactura);
                    command.Parameters.AddWithValue("@IdProducto", detalleFactura.IdProducto);
  

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
