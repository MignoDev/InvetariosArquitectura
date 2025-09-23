using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ProyectoInventario.Application.Service;

namespace ProyectoInventario.Application.Service
{
    /// <summary>
    /// Ejemplos de uso de los servicios de inventario con eventos
    /// </summary>
    public class EjemploUsoServicios
    {
        /// <summary>
        /// Ejemplo de configuración en Program.cs para la API
        /// </summary>
        public static void ConfigurarApi(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configurar servicios del inventario con eventos
            builder.Services.ConfigurarServiciosInventarioCompleto(builder.Configuration);
        }

        /// <summary>
        /// Ejemplo de configuración en Program.cs para la App Blazor
        /// </summary>
        public static void ConfigurarBlazor(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            // Configurar servicios del inventario con eventos
            builder.Services.ConfigurarServiciosInventarioCompleto(builder.Configuration);
        }

        /// <summary>
        /// Ejemplo de configuración de appsettings.json
        /// </summary>
        public static string GetAppSettingsJson()
        {
            return """
            {
              "ConnectionStrings": {
                "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventarioDB;Trusted_Connection=true;MultipleActiveResultSets=true"
              },
              "Email": {
                "SmtpServer": "smtp.gmail.com",
                "SmtpPort": 587,
                "UserName": "tu-email@gmail.com",
                "Password": "tu-password",
                "EnableSsl": true
              },
              "Logging": {
                "LogLevel": {
                  "Default": "Information",
                  "Microsoft.AspNetCore": "Warning"
                }
              },
              "AllowedHosts": "*"
            }
            """;
        }
    }
}

/// <summary>
/// Ejemplo de uso en un controlador de API
/// </summary>
namespace ProyectoInventario.Infraestructura.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioControllerEjemplo : ControllerBase
    {
        private readonly ServicioInventario _servicioInventario;

        public InventarioControllerEjemplo(ServicioInventario servicioInventario)
        {
            _servicioInventario = servicioInventario;
        }

        /// <summary>
        /// Crea un nuevo producto con eventos automáticos
        /// </summary>
        [HttpPost("productos")]
        public async Task<IActionResult> CrearProducto([FromBody] CrearProductoRequest request)
        {
            try
            {
                var producto = await _servicioInventario.CrearProductoAsync(
                    request.Codigo,
                    request.Nombre,
                    request.Descripcion,
                    request.Precio,
                    request.StockMinimo,
                    request.StockMaximo,
                    request.StockInicial,
                    request.Ubicacion,
                    request.CategoriaId);

                return CreatedAtAction(nameof(ObtenerProducto), new { id = producto.Id }, producto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Ajusta el stock de un producto (dispara eventos automáticamente)
        /// </summary>
        [HttpPut("productos/{id}/stock/ajustar")]
        public async Task<IActionResult> AjustarStock(Guid id, [FromBody] AjustarStockRequest request)
        {
            try
            {
                var stock = await _servicioInventario.AjustarStockAsync(id, request.Cantidad);
                return Ok(stock);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Registra una entrada de productos (dispara eventos automáticamente)
        /// </summary>
        [HttpPost("movimientos/entradas")]
        public async Task<IActionResult> RegistrarEntrada([FromBody] RegistrarEntradaRequest request)
        {
            try
            {
                var entrada = await _servicioInventario.RegistrarEntradaAsync(
                    request.ProductoId,
                    request.ProveedorId,
                    request.Cantidad,
                    request.PrecioUnitario,
                    request.NumeroFactura,
                    request.Observaciones);

                return CreatedAtAction(nameof(ObtenerEntrada), new { id = entrada.Id }, entrada);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("productos/{id}")]
        public async Task<IActionResult> ObtenerProducto(Guid id)
        {
            var producto = await _servicioInventario.ObtenerProductoAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        private IActionResult ObtenerEntrada(Guid id)
        {
            return Ok();
        }
    }

    public class CrearProductoRequest
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        public int StockInicial { get; set; }
        public string Ubicacion { get; set; }
        public Guid? CategoriaId { get; set; }
    }

    public class AjustarStockRequest
    {
        public int Cantidad { get; set; }
    }

    public class RegistrarEntradaRequest
    {
        public Guid ProductoId { get; set; }
        public Guid ProveedorId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string NumeroFactura { get; set; }
        public string Observaciones { get; set; }
    }
}

/// <summary>
/// Ejemplo de uso en un componente Blazor
/// </summary>
namespace ProyectoInventario.Infraestructura.App.Components.Pages
{
    public partial class InventarioEjemplo
    {
        [Inject] private ServicioInventario ServicioInventario { get; set; }

        private List<Producto> productos = new();
        private bool cargando = false;
        private string mensaje = "";

        protected override async Task OnInitializedAsync()
        {
            await CargarProductos();
        }

        private async Task CargarProductos()
        {
            cargando = true;
            try
            {
                productos = (await ServicioInventario.ObtenerProductosActivosAsync()).ToList();
            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
            }
            finally
            {
                cargando = false;
            }
        }

        private async Task CrearProducto()
        {
            try
            {
                var producto = await ServicioInventario.CrearProductoAsync(
                    "PROD001",
                    "Producto de Prueba",
                    "Descripción del producto",
                    99.99m,
                    10,
                    100,
                    50);

                mensaje = "Producto creado exitosamente";
                await CargarProductos();
            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
            }
        }

        private async Task AjustarStock(Guid productoId, int nuevaCantidad)
        {
            try
            {
                await ServicioInventario.AjustarStockAsync(productoId, nuevaCantidad);
                mensaje = "Stock ajustado exitosamente";
                await CargarProductos();
            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
            }
        }
    }
}
