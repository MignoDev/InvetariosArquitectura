using Domain.Models;

namespace AppBlazor.Services
{
    public interface IDetalleFactura
    {

        Task<List<DetalleFacturas>> GetDetalleFacturas();
        Task<List<DetalleFacturas>> GetDetalleFacturasByFacturaId(int id);
        Task<ServiceResult<DetalleFacturas>> CreateDetalleFactura(DetalleFacturas detalleFacturas);

    }
}
