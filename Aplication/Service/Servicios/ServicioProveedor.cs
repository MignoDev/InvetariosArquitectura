using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Models;
using ProyectoInventario.Domain.Ports;

namespace ProyectoInventario.Application.Service.Servicios
{
    /// <summary>
    /// Servicio de aplicación para la gestión de proveedores
    /// Implementa la lógica de negocio para proveedores
    /// </summary>
    public class ServicioProveedor
    {
        private readonly IRepositorioProveedor _repositorioProveedor;
        private readonly IRepositorioMovimiento _repositorioMovimiento;

        public ServicioProveedor(
            IRepositorioProveedor repositorioProveedor,
            IRepositorioMovimiento repositorioMovimiento)
        {
            _repositorioProveedor = repositorioProveedor;
            _repositorioMovimiento = repositorioMovimiento;
        }

        /// <summary>
        /// Crea un nuevo proveedor
        /// </summary>
        public async Task<Proveedor> CrearProveedorAsync(string codigo, string nombre, 
            string contacto = null, string email = null, string telefono = null, string direccion = null)
        {
            // Validar que el código no exista
            if (await _repositorioProveedor.ExisteCodigoAsync(codigo))
            {
                throw new InvalidOperationException($"Ya existe un proveedor con el código '{codigo}'");
            }

            // Validar email si se proporciona
            if (!string.IsNullOrWhiteSpace(email) && await _repositorioProveedor.ExisteEmailAsync(email))
            {
                throw new InvalidOperationException($"Ya existe un proveedor con el email '{email}'");
            }

            // Validar datos requeridos
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El código del proveedor no puede estar vacío");
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del proveedor no puede estar vacío");
            }

            // Crear proveedor
            var proveedor = new Proveedor
            {
                Id = Guid.NewGuid(),
                Codigo = codigo,
                Nombre = nombre,
                Contacto = contacto,
                Email = email,
                Telefono = telefono,
                Direccion = direccion,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };

            // Guardar en repositorio
            return await _repositorioProveedor.CrearAsync(proveedor);
        }

        /// <summary>
        /// Actualiza la información de un proveedor
        /// </summary>
        public async Task<Proveedor> ActualizarProveedorAsync(Guid proveedorId, string nombre, 
            string contacto, string email, string telefono, string direccion)
        {
            // Obtener proveedor existente
            var proveedor = await _repositorioProveedor.ObtenerPorIdAsync(proveedorId);
            if (proveedor == null)
            {
                throw new ArgumentException("El proveedor no existe");
            }

            // Validar email si cambió
            if (proveedor.Email != email && !string.IsNullOrWhiteSpace(email) && 
                await _repositorioProveedor.ExisteEmailAsync(email, proveedorId))
            {
                throw new InvalidOperationException($"Ya existe un proveedor con el email '{email}'");
            }

            // Validar datos requeridos
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del proveedor no puede estar vacío");
            }

            // Actualizar información
            proveedor.Nombre = nombre;
            proveedor.Contacto = contacto;
            proveedor.Email = email;
            proveedor.Telefono = telefono;
            proveedor.Direccion = direccion;

            // Guardar cambios
            return await _repositorioProveedor.ActualizarAsync(proveedor);
        }

        /// <summary>
        /// Actualiza solo la información de contacto de un proveedor
        /// </summary>
        public async Task<Proveedor> ActualizarContactoAsync(Guid proveedorId, string contacto, 
            string email, string telefono)
        {
            var proveedor = await _repositorioProveedor.ObtenerPorIdAsync(proveedorId);
            if (proveedor == null)
            {
                throw new ArgumentException("El proveedor no existe");
            }

            // Validar email si cambió
            if (proveedor.Email != email && !string.IsNullOrWhiteSpace(email) && 
                await _repositorioProveedor.ExisteEmailAsync(email, proveedorId))
            {
                throw new InvalidOperationException($"Ya existe un proveedor con el email '{email}'");
            }

            proveedor.Contacto = contacto;
            proveedor.Email = email;
            proveedor.Telefono = telefono;
            return await _repositorioProveedor.ActualizarAsync(proveedor);
        }

        /// <summary>
        /// Actualiza solo la dirección de un proveedor
        /// </summary>
        public async Task<Proveedor> ActualizarDireccionAsync(Guid proveedorId, string direccion)
        {
            var proveedor = await _repositorioProveedor.ObtenerPorIdAsync(proveedorId);
            if (proveedor == null)
            {
                throw new ArgumentException("El proveedor no existe");
            }

            proveedor.Direccion = direccion;
            return await _repositorioProveedor.ActualizarAsync(proveedor);
        }

        /// <summary>
        /// Obtiene un proveedor por su ID
        /// </summary>
        public async Task<Proveedor> ObtenerProveedorAsync(Guid proveedorId)
        {
            return await _repositorioProveedor.ObtenerPorIdAsync(proveedorId);
        }

        /// <summary>
        /// Obtiene un proveedor por su código
        /// </summary>
        public async Task<Proveedor> ObtenerProveedorPorCodigoAsync(string codigo)
        {
            return await _repositorioProveedor.ObtenerPorCodigoAsync(codigo);
        }

        /// <summary>
        /// Obtiene todos los proveedores activos
        /// </summary>
        public async Task<IEnumerable<Proveedor>> ObtenerProveedoresActivosAsync()
        {
            return await _repositorioProveedor.ObtenerActivosAsync();
        }

        /// <summary>
        /// Obtiene todos los proveedores
        /// </summary>
        public async Task<IEnumerable<Proveedor>> ObtenerTodosLosProveedoresAsync()
        {
            return await _repositorioProveedor.ObtenerTodosAsync();
        }

        /// <summary>
        /// Busca proveedores por nombre
        /// </summary>
        public async Task<IEnumerable<Proveedor>> BuscarProveedoresAsync(string terminoBusqueda)
        {
            return await _repositorioProveedor.BuscarPorNombreAsync(terminoBusqueda);
        }

        /// <summary>
        /// Busca proveedores por email
        /// </summary>
        public async Task<IEnumerable<Proveedor>> BuscarProveedoresPorEmailAsync(string email)
        {
            return await _repositorioProveedor.BuscarPorEmailAsync(email);
        }

        /// <summary>
        /// Obtiene proveedores con información completa
        /// </summary>
        public async Task<IEnumerable<Proveedor>> ObtenerProveedoresConInformacionCompletaAsync()
        {
            return await _repositorioProveedor.BuscarConInformacionCompletaAsync();
        }

        /// <summary>
        /// Activa un proveedor
        /// </summary>
        public async Task<Proveedor> ActivarProveedorAsync(Guid proveedorId)
        {
            var proveedor = await _repositorioProveedor.ObtenerPorIdAsync(proveedorId);
            if (proveedor == null)
            {
                throw new ArgumentException("El proveedor no existe");
            }

            proveedor.Activo = true;
            return await _repositorioProveedor.ActualizarAsync(proveedor);
        }

        /// <summary>
        /// Desactiva un proveedor
        /// </summary>
        public async Task<Proveedor> DesactivarProveedorAsync(Guid proveedorId)
        {
            var proveedor = await _repositorioProveedor.ObtenerPorIdAsync(proveedorId);
            if (proveedor == null)
            {
                throw new ArgumentException("El proveedor no existe");
            }

            proveedor.Activo = false;
            return await _repositorioProveedor.ActualizarAsync(proveedor);
        }

        /// <summary>
        /// Elimina un proveedor
        /// </summary>
        public async Task EliminarProveedorAsync(Guid proveedorId)
        {
            var proveedor = await _repositorioProveedor.ObtenerPorIdAsync(proveedorId);
            if (proveedor == null)
            {
                throw new ArgumentException("El proveedor no existe");
            }

            // Verificar si tiene entradas asociadas
            var entradas = await _repositorioMovimiento.ObtenerEntradasPorProveedorAsync(proveedorId);
            if (entradas.Any())
            {
                throw new InvalidOperationException("No se puede eliminar un proveedor que tiene entradas asociadas");
            }

            await _repositorioProveedor.EliminarAsync(proveedorId);
        }

        /// <summary>
        /// Obtiene estadísticas de un proveedor
        /// </summary>
        public async Task<object> ObtenerEstadisticasProveedorAsync(Guid proveedorId)
        {
            var proveedor = await _repositorioProveedor.ObtenerPorIdAsync(proveedorId);
            if (proveedor == null)
            {
                throw new ArgumentException("El proveedor no existe");
            }

            var entradas = await _repositorioMovimiento.ObtenerEntradasPorProveedorAsync(proveedorId);
            var totalEntradas = entradas.Count();
            var valorTotalEntradas = entradas.Sum(e => e.Cantidad * e.PrecioUnitario);

            return new
            {
                ProveedorId = proveedor.Id,
                Nombre = proveedor.Nombre,
                Codigo = proveedor.Codigo,
                TotalEntradas = totalEntradas,
                ValorTotalEntradas = valorTotalEntradas,
                TieneInformacionCompleta = !string.IsNullOrWhiteSpace(proveedor.Contacto) && 
                                         !string.IsNullOrWhiteSpace(proveedor.Email) && 
                                         !string.IsNullOrWhiteSpace(proveedor.Telefono) && 
                                         !string.IsNullOrWhiteSpace(proveedor.Direccion),
                Activo = proveedor.Activo,
                FechaCreacion = proveedor.FechaCreacion
            };
        }

        /// <summary>
        /// Valida la información de contacto de un proveedor
        /// </summary>
        public async Task<object> ValidarInformacionContactoAsync(Guid proveedorId)
        {
            var proveedor = await _repositorioProveedor.ObtenerPorIdAsync(proveedorId);
            if (proveedor == null)
            {
                throw new ArgumentException("El proveedor no existe");
            }

            return new
            {
                ProveedorId = proveedor.Id,
                Nombre = proveedor.Nombre,
                EmailValido = !string.IsNullOrWhiteSpace(proveedor.Email) && proveedor.Email.Contains("@"),
                TelefonoValido = !string.IsNullOrWhiteSpace(proveedor.Telefono),
                InformacionCompleta = !string.IsNullOrWhiteSpace(proveedor.Contacto) && 
                                    !string.IsNullOrWhiteSpace(proveedor.Email) && 
                                    !string.IsNullOrWhiteSpace(proveedor.Telefono) && 
                                    !string.IsNullOrWhiteSpace(proveedor.Direccion),
                Recomendaciones = ObtenerRecomendacionesValidacion(proveedor)
            };
        }

        /// <summary>
        /// Obtiene el conteo total de proveedores
        /// </summary>
        public async Task<int> ContarProveedoresAsync()
        {
            return await _repositorioProveedor.ContarTotalAsync();
        }

        /// <summary>
        /// Obtiene el conteo de proveedores activos
        /// </summary>
        public async Task<int> ContarProveedoresActivosAsync()
        {
            return await _repositorioProveedor.ContarActivosAsync();
        }

        /// <summary>
        /// Obtiene el conteo de proveedores con información completa
        /// </summary>
        public async Task<int> ContarProveedoresConInformacionCompletaAsync()
        {
            return await _repositorioProveedor.ContarConInformacionCompletaAsync();
        }

        #region Métodos Privados

        /// <summary>
        /// Obtiene recomendaciones para mejorar la información del proveedor
        /// </summary>
        private List<string> ObtenerRecomendacionesValidacion(Proveedor proveedor)
        {
            var recomendaciones = new List<string>();

            if (string.IsNullOrWhiteSpace(proveedor.Contacto))
            {
                recomendaciones.Add("Agregar información de contacto");
            }

            if (string.IsNullOrWhiteSpace(proveedor.Email) || !proveedor.Email.Contains("@"))
            {
                recomendaciones.Add("Agregar o corregir el email del proveedor");
            }

            if (string.IsNullOrWhiteSpace(proveedor.Telefono))
            {
                recomendaciones.Add("Agregar o corregir el teléfono del proveedor");
            }

            if (string.IsNullOrWhiteSpace(proveedor.Direccion))
            {
                recomendaciones.Add("Agregar la dirección del proveedor");
            }

            return recomendaciones;
        }

        #endregion
    }
}
