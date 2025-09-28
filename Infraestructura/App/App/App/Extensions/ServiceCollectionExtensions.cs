using AppBlazor.Services;
using Radzen;

namespace AppBlazor.Extensions
{
    /// <summary>
    /// Extensiones para el registro de servicios de la aplicación Blazor
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra todos los servicios de la aplicación Blazor
        /// </summary>
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            // Registrar servicios de API con HttpClient específico
            services.AddScoped<ProductoService>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("ApiClient");
                return new ProductoService(httpClient);
            });
            
            services.AddScoped<CategoriaService>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("ApiClient");
                return new CategoriaService(httpClient);
            });
            
            services.AddScoped<ProveedorService>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("ApiClient");
                return new ProveedorService(httpClient);
            });
            
            services.AddScoped<MovimientoService>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("ApiClient");
                return new MovimientoService(httpClient);
            });
            
            services.AddScoped<ReporteService>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("ApiClient");
                return new ReporteService(httpClient);
            });

            services.AddScoped<DialogService>();

            return services;
        }
    }
}