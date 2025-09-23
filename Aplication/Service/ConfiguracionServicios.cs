using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Domain.Ports;
using ProyectoInventario.Application.Service.Servicios;

namespace ProyectoInventario.Application.Service
{
    /// <summary>
    /// Configuración de inyección de dependencias para el sistema de inventario
    /// Incluye manejo completo de eventos desde el servicio de aplicación
    /// </summary>
    public static class ConfiguracionServicios
    {
        /// <summary>
        /// Configura todos los servicios del sistema de inventario con eventos
        /// </summary>
        public static IServiceCollection ConfigurarServiciosInventarioCompleto(this IServiceCollection services, IConfiguration configuration)
        {
            // =============================================
            // CONFIGURACIÓN DE BASE DE DATOS
            // =============================================

            // Configurar Entity Framework
            services.AddDbContext<InventarioDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

            // =============================================
            // CONFIGURACIÓN DE REPOSITORIOS (ADAPTADORES DE PERSISTENCIA)
            // =============================================

            // Registrar repositorios (implementaciones concretas de los puertos)
            services.AddScoped<IRepositorioProducto, RepositorioProductoEF>();
            services.AddScoped<IRepositorioStock, RepositorioStockEF>();
            services.AddScoped<IRepositorioCategoria, RepositorioCategoriaEF>();
            services.AddScoped<IRepositorioProveedor, RepositorioProveedorEF>();
            services.AddScoped<IRepositorioMovimiento, RepositorioMovimientoEF>();

            // =============================================
            // CONFIGURACIÓN DE SERVICIOS EXTERNOS (ADAPTADORES DE INFRAESTRUCTURA)
            // =============================================

            // Servicio de notificaciones
            services.AddScoped<IServicioNotificacion, ServicioNotificacionEmail>();
            
            // Servicio de proveedores
            services.AddScoped<IServicioProveedores, ServicioProveedoresExterno>();
            
            // Servicio de promociones
            services.AddScoped<IServicioPromociones, ServicioPromocionesExterno>();

            // Publicador de eventos
            services.AddScoped<IPublicadorEventos, PublicadorEventosEF>();

            // =============================================
            // CONFIGURACIÓN DE SERVICIOS DE APLICACIÓN
            // =============================================

            // Servicios de aplicación (lógica de negocio)
            services.AddScoped<ServicioInventario>();
            services.AddScoped<ServicioCategoria>();
            services.AddScoped<ServicioProveedor>();
            services.AddScoped<ServicioReportes>();

            // =============================================
            // CONFIGURACIÓN DE SERVICIOS DE EVENTOS
            // =============================================

            // Servicio de procesamiento de eventos en segundo plano
            services.AddHostedService<EventProcessorService>();

            // =============================================
            // CONFIGURACIÓN DE SERVICIOS DE INFRAESTRUCTURA
            // =============================================

            // HttpClient para servicios externos
            services.AddHttpClient();

            // Configurar servicios de notificación
            services.Configure<EmailConfig>(configuration.GetSection("Email"));

            // =============================================
            // CONFIGURACIÓN DE CACHING
            // =============================================

            // Agregar memoria cache
            services.AddMemoryCache();

            // =============================================
            // CONFIGURACIÓN DE LOGGING
            // =============================================

            // Agregar logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                builder.AddEventSourceLogger();
            });

            return services;
        }
    }

    // =============================================
    // IMPLEMENTACIONES DE SERVICIOS EXTERNOS
    // =============================================

    /// <summary>
    /// Implementación del servicio de notificaciones por email
    /// </summary>
    public class ServicioNotificacionEmail : IServicioNotificacion
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ServicioNotificacionEmail> _logger;

        public ServicioNotificacionEmail(IEmailService emailService, ILogger<ServicioNotificacionEmail> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task EnviarNotificacionStockBajoAsync(string productoNombre, int cantidadActual, int stockMinimo)
        {
            var asunto = "Alerta: Stock Bajo";
            var mensaje = $"El producto {productoNombre} tiene stock bajo ({cantidadActual} unidades). Stock mínimo: {stockMinimo}";
            
            _logger.LogWarning("Stock bajo detectado: {Producto} - {Cantidad}/{Minimo}", productoNombre, cantidadActual, stockMinimo);
            await _emailService.EnviarEmailAsync("admin@empresa.com", asunto, mensaje);
        }

        public async Task EnviarNotificacionProductoAgotadoAsync(string productoNombre)
        {
            var asunto = "Alerta: Producto Agotado";
            var mensaje = $"El producto {productoNombre} se ha agotado completamente.";
            
            _logger.LogError("Producto agotado: {Producto}", productoNombre);
            await _emailService.EnviarEmailAsync("admin@empresa.com", asunto, mensaje);
        }

        public async Task EnviarNotificacionExcesoStockAsync(string productoNombre, int cantidadActual, int stockMaximo)
        {
            var asunto = "Oportunidad de Promoción";
            var mensaje = $"El producto {productoNombre} tiene exceso de stock ({cantidadActual} unidades). Considera crear una promoción.";
            
            _logger.LogInformation("Exceso de stock detectado: {Producto} - {Cantidad}/{Maximo}", productoNombre, cantidadActual, stockMaximo);
            await _emailService.EnviarEmailAsync("admin@empresa.com", asunto, mensaje);
        }

        public async Task EnviarNotificacionStockActualizadoAsync(string productoNombre, int cantidadAnterior, 
            int cantidadNueva, string tipoMovimiento)
        {
            var asunto = "Stock Actualizado";
            var mensaje = $"El stock del producto {productoNombre} ha sido actualizado de {cantidadAnterior} a {cantidadNueva} unidades ({tipoMovimiento}).";
            
            _logger.LogInformation("Stock actualizado: {Producto} - {Anterior} -> {Nueva} ({Tipo})", 
                productoNombre, cantidadAnterior, cantidadNueva, tipoMovimiento);
            await _emailService.EnviarEmailAsync("admin@empresa.com", asunto, mensaje);
        }

        public async Task EnviarNotificacionPersonalizadaAsync(string asunto, string mensaje, string[] destinatarios)
        {
            foreach (var destinatario in destinatarios)
            {
                await _emailService.EnviarEmailAsync(destinatario, asunto, mensaje);
            }
        }

        public async Task EnviarEmailAsync(string destinatario, string asunto, string cuerpo)
        {
            await _emailService.EnviarEmailAsync(destinatario, asunto, cuerpo);
        }

        public async Task EnviarSmsAsync(string numeroTelefono, string mensaje)
        {
            _logger.LogInformation("SMS enviado a {Telefono}: {Mensaje}", numeroTelefono, mensaje);
            await Task.CompletedTask;
        }

        public async Task EnviarNotificacionPushAsync(string usuarioId, string titulo, string mensaje)
        {
            _logger.LogInformation("Push notification enviado a {Usuario}: {Titulo} - {Mensaje}", usuarioId, titulo, mensaje);
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Implementación del servicio de proveedores
    /// </summary>
    public class ServicioProveedoresExterno : IServicioProveedores
    {
        private readonly ILogger<ServicioProveedoresExterno> _logger;

        public ServicioProveedoresExterno(ILogger<ServicioProveedoresExterno> logger)
        {
            _logger = logger;
        }

        public async Task GenerarOrdenCompraAsync(Guid productoId)
        {
            _logger.LogInformation("Generando orden de compra para producto {ProductoId}", productoId);
            // Implementar lógica de generación de orden de compra
            await Task.CompletedTask;
        }

        public async Task GenerarOrdenCompraUrgenteAsync(Guid productoId)
        {
            _logger.LogWarning("Generando orden de compra URGENTE para producto {ProductoId}", productoId);
            // Implementar lógica de generación de orden de compra urgente
            await Task.CompletedTask;
        }

        public async Task<Guid?> ObtenerProveedorRecomendadoAsync(Guid productoId)
        {
            // Implementar lógica para obtener proveedor recomendado
            await Task.CompletedTask;
            return Guid.NewGuid();
        }

        public async Task<int> CalcularCantidadRecomendadaAsync(Guid productoId)
        {
            // Implementar lógica para calcular cantidad recomendada
            await Task.CompletedTask;
            return 100;
        }

        public async Task<decimal> CalcularPrecioEstimadoAsync(Guid productoId, int cantidad)
        {
            // Implementar lógica para calcular precio estimado
            await Task.CompletedTask;
            return cantidad * 10.0m;
        }

        public async Task<bool> ValidarDisponibilidadProveedorAsync(Guid proveedorId, Guid productoId)
        {
            // Implementar lógica de validación
            await Task.CompletedTask;
            return true;
        }

        public async Task<int> ObtenerTiempoEntregaEstimadoAsync(Guid proveedorId, Guid productoId)
        {
            // Implementar lógica para obtener tiempo de entrega
            await Task.CompletedTask;
            return 7; // días
        }

        public async Task EnviarSolicitudCotizacionAsync(Guid proveedorId, Guid productoId, int cantidad)
        {
            _logger.LogInformation("Enviando solicitud de cotización a proveedor {ProveedorId} para producto {ProductoId}", proveedorId, productoId);
            await Task.CompletedTask;
        }

        public async Task ProcesarRespuestaCotizacionAsync(Guid proveedorId, Guid productoId, decimal precio, int tiempoEntrega)
        {
            _logger.LogInformation("Procesando respuesta de cotización del proveedor {ProveedorId}", proveedorId);
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Implementación del servicio de promociones
    /// </summary>
    public class ServicioPromocionesExterno : IServicioPromociones
    {
        private readonly ILogger<ServicioPromocionesExterno> _logger;

        public ServicioPromocionesExterno(ILogger<ServicioPromocionesExterno> logger)
        {
            _logger = logger;
        }

        public async Task CrearPromocionAutomaticaAsync(Guid productoId, int cantidadActual, int stockMaximo)
        {
            _logger.LogInformation("Creando promoción automática para producto {ProductoId} con exceso de stock", productoId);
            // Implementar lógica de creación de promoción automática
            await Task.CompletedTask;
        }

        public async Task CrearPromocionDescuentoAsync(Guid productoId, decimal porcentajeDescuento, 
            DateTime fechaInicio, DateTime fechaFin)
        {
            _logger.LogInformation("Creando promoción de descuento para producto {ProductoId}", productoId);
            await Task.CompletedTask;
        }

        public async Task CrearPromocionCantidadAsync(Guid productoId, int cantidadMinima, decimal porcentajeDescuento, 
            DateTime fechaInicio, DateTime fechaFin)
        {
            _logger.LogInformation("Creando promoción por cantidad para producto {ProductoId}", productoId);
            await Task.CompletedTask;
        }

        public async Task CrearPromocion2x1Async(Guid productoId, DateTime fechaInicio, DateTime fechaFin)
        {
            _logger.LogInformation("Creando promoción 2x1 para producto {ProductoId}", productoId);
            await Task.CompletedTask;
        }

        public async Task<decimal> CalcularDescuentoRecomendadoAsync(Guid productoId, int cantidadActual, int stockMaximo)
        {
            // Implementar lógica para calcular descuento recomendado
            await Task.CompletedTask;
            return 15.0m; // 15%
        }

        public async Task<bool> EsCandidatoParaPromocionAsync(Guid productoId)
        {
            // Implementar lógica de validación
            await Task.CompletedTask;
            return true;
        }

        public async Task<object[]> ObtenerPromocionesActivasAsync(Guid productoId)
        {
            // Implementar lógica para obtener promociones activas
            await Task.CompletedTask;
            return new object[0];
        }

        public async Task DesactivarPromocionAsync(Guid promocionId)
        {
            _logger.LogInformation("Desactivando promoción {PromocionId}", promocionId);
            await Task.CompletedTask;
        }

        public async Task<decimal> CalcularImpactoVentasAsync(Guid promocionId)
        {
            // Implementar lógica para calcular impacto en ventas
            await Task.CompletedTask;
            return 0.0m;
        }
    }

    // =============================================
    // INTERFACES Y CLASES DE CONFIGURACIÓN
    // =============================================

    public interface IEmailService
    {
        Task EnviarEmailAsync(string destinatario, string asunto, string cuerpo);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailConfig _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailConfig> config, ILogger<EmailService> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public async Task EnviarEmailAsync(string destinatario, string asunto, string cuerpo)
        {
            _logger.LogInformation("Enviando email a {Destinatario}: {Asunto}", destinatario, asunto);
            // Implementar envío de email usando SMTP
            await Task.CompletedTask;
        }
    }

    public class EmailConfig
    {
        public string SmtpServer { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587;
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public bool EnableSsl { get; set; } = true;
    }
}
