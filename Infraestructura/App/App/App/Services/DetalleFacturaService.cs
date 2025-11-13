
using Domain.Models;

namespace AppBlazor.Services
{
    public class DetalleFacturaService : BaseService, IDetalleFactura
    {

        public DetalleFacturaService(HttpClient httpClient) : base(httpClient) { }

        public async Task<ServiceResult<DetalleFacturas>> CreateDetalleFactura(DetalleFacturas detalleFacturas)
        {
            return await ExecuteAsync(async () => await PostAsync<DetalleFacturas>("api/detalle-facturas", detalleFacturas));
        }

        public async Task<List<DetalleFacturas>> GetDetalleFacturas()
        {
            return await GetModelAsync<List<DetalleFacturas>>("api/detalle-facturas");
        }

        public async Task<List<DetalleFacturas>> GetDetalleFacturasByFacturaId(int id)
        {
            return await GetModelAsync<List<DetalleFacturas>>($"api/detalle-facturas/{id}");
        }
    }
}
