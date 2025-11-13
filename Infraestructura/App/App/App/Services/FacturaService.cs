using Domain.Models;

namespace AppBlazor.Services
{
    public class FacturaService : BaseService, IFacturaService
    {
        public FacturaService(HttpClient httpClient) : base(httpClient) { }

        public Task<ServiceResult<Factura>> ActualizarAsync(Factura factura)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<Factura>> CrearAsync(Factura factura)
        {
            return await ExecuteAsync(async () => await PostAsync<Factura>("api/factura", factura));
        }

        public async Task<Factura> ObtenerPorIdAsync(int id)
        {
            return await GetModelAsync<Factura>($"api/factura/{id}");
        }

        public async Task<List<Factura>> ObtenerTodosAsync()
        {
            return await GetModelAsync<List<Factura>>($"api/factura");
        }
    }
}
