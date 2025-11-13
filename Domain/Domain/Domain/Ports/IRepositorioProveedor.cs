using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Models;

namespace ProyectoInventario.Domain.Ports
{
    /// <summary>
    /// Puerto (interfaz) para el repositorio de proveedores
    /// Define el contrato para la persistencia de proveedores
    /// </summary>
    public interface IRepositorioProveedor
    {
        // Operaciones CRUD básicas
        Task<Proveedor> ObtenerPorIdAsync(int id);
        Task<Proveedor> ObtenerPorCodigoAsync(string codigo);
        Task<IEnumerable<Proveedor>> ObtenerTodosAsync();
        Task<IEnumerable<Proveedor>> ObtenerActivosAsync();
        Task<Proveedor> CrearAsync(Proveedor proveedor);
        Task<Proveedor> ActualizarAsync(Proveedor proveedor);
        Task EliminarAsync(int id);

        // Operaciones de búsqueda
        Task<IEnumerable<Proveedor>> BuscarPorNombreAsync(string nombre);
        Task<IEnumerable<Proveedor>> BuscarPorEmailAsync(string email);
        Task<IEnumerable<Proveedor>> BuscarConInformacionCompletaAsync();

        // Operaciones de validación
        Task<bool> ExisteCodigoAsync(string codigo);
        Task<bool> ExisteCodigoAsync(string codigo, int idExcluir);
        Task<bool> ExisteEmailAsync(string email);
        Task<bool> ExisteEmailAsync(string email, int idExcluir);

        // Operaciones de conteo
        Task<int> ContarTotalAsync();
        Task<int> ContarActivosAsync();
        Task<int> ContarConInformacionCompletaAsync();
    }
}
