using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Models;

namespace ProyectoInventario.Application.Service.Servicios
{
    /// <summary>
    /// Puerto de entrada para el servicio de proveedores
    /// Define la interfaz para la gestión de proveedores
    /// </summary>
    public interface IServicioProveedor
    {
        /// <summary>
        /// Crea un nuevo proveedor
        /// </summary>
        Task<Proveedor> CrearProveedorAsync(string codigo, string nombre, 
            string contacto = null, string email = null, string telefono = null, string direccion = null);

        /// <summary>
        /// Actualiza la información de un proveedor
        /// </summary>
        Task<Proveedor> ActualizarProveedorAsync(int proveedorId, string nombre, 
            string contacto, string email, string telefono, string direccion);

        /// <summary>
        /// Actualiza solo la información de contacto de un proveedor
        /// </summary>
        Task<Proveedor> ActualizarContactoAsync(int proveedorId, string contacto, 
            string email, string telefono);

        /// <summary>
        /// Actualiza solo la dirección de un proveedor
        /// </summary>
        Task<Proveedor> ActualizarDireccionAsync(int proveedorId, string direccion);

        /// <summary>
        /// Obtiene un proveedor por su ID
        /// </summary>
        Task<Proveedor> ObtenerProveedorAsync(int proveedorId);

        /// <summary>
        /// Obtiene un proveedor por su código
        /// </summary>
        Task<Proveedor> ObtenerProveedorPorCodigoAsync(string codigo);

        /// <summary>
        /// Obtiene todos los proveedores activos
        /// </summary>
        Task<IEnumerable<Proveedor>> ObtenerProveedoresActivosAsync();

        /// <summary>
        /// Obtiene todos los proveedores
        /// </summary>
        Task<IEnumerable<Proveedor>> ObtenerTodosLosProveedoresAsync();

        /// <summary>
        /// Busca proveedores por nombre
        /// </summary>
        Task<IEnumerable<Proveedor>> BuscarProveedoresAsync(string terminoBusqueda);

        /// <summary>
        /// Busca proveedores por email
        /// </summary>
        Task<IEnumerable<Proveedor>> BuscarProveedoresPorEmailAsync(string email);

        /// <summary>
        /// Obtiene proveedores con información completa
        /// </summary>
        Task<IEnumerable<Proveedor>> ObtenerProveedoresConInformacionCompletaAsync();

        /// <summary>
        /// Activa un proveedor
        /// </summary>
        Task<Proveedor> ActivarProveedorAsync(int proveedorId);

        /// <summary>
        /// Desactiva un proveedor
        /// </summary>
        Task<Proveedor> DesactivarProveedorAsync(int proveedorId);

        /// <summary>
        /// Elimina un proveedor
        /// </summary>
        Task EliminarProveedorAsync(int proveedorId);

        /// <summary>
        /// Obtiene estadísticas de un proveedor
        /// </summary>
        Task<object> ObtenerEstadisticasProveedorAsync(int proveedorId);

        /// <summary>
        /// Valida la información de contacto de un proveedor
        /// </summary>
        Task<object> ValidarInformacionContactoAsync(int proveedorId);

        /// <summary>
        /// Obtiene el conteo total de proveedores
        /// </summary>
        Task<int> ContarProveedoresAsync();

        /// <summary>
        /// Obtiene el conteo de proveedores activos
        /// </summary>
        Task<int> ContarProveedoresActivosAsync();

        /// <summary>
        /// Obtiene el conteo de proveedores con información completa
        /// </summary>
        Task<int> ContarProveedoresConInformacionCompletaAsync();
    }
}
