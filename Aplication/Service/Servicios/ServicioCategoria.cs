using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Models;
using ProyectoInventario.Domain.Ports;

namespace ProyectoInventario.Application.Service.Servicios
{
    /// <summary>
    /// Servicio de aplicación para la gestión de categorías
    /// Implementa la lógica de negocio para categorías de productos
    /// </summary>
    public class ServicioCategoria
    {
        private readonly IRepositorioCategoria _repositorioCategoria;
        private readonly IRepositorioProducto _repositorioProducto;

        public ServicioCategoria(
            IRepositorioCategoria repositorioCategoria,
            IRepositorioProducto repositorioProducto)
        {
            _repositorioCategoria = repositorioCategoria;
            _repositorioProducto = repositorioProducto;
        }

        /// <summary>
        /// Crea una nueva categoría
        /// </summary>
        public async Task<Categoria> CrearCategoriaAsync(string nombre, string descripcion = null)
        {
            // Validar que el nombre no exista
            if (await _repositorioCategoria.ExisteNombreAsync(nombre))
            {
                throw new InvalidOperationException($"Ya existe una categoría con el nombre '{nombre}'");
            }

            // Validar nombre
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la categoría no puede estar vacío");
            }

            // Crear categoría
            var categoria = new Categoria
            {
                Nombre = nombre,
                Descripcion = descripcion,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };

            // Guardar en repositorio
            return await _repositorioCategoria.CrearAsync(categoria);
        }

        /// <summary>
        /// Actualiza la información de una categoría
        /// </summary>
        public async Task<Categoria> ActualizarCategoriaAsync(int categoriaId, string nombre, string descripcion)
        {
            // Obtener categoría existente
            var categoria = await _repositorioCategoria.ObtenerPorIdAsync(categoriaId);
            if (categoria == null)
            {
                throw new ArgumentException("La categoría no existe");
            }

            // Validar nombre si cambió
            if (categoria.Nombre != nombre && await _repositorioCategoria.ExisteNombreAsync(nombre, categoriaId))
            {
                throw new InvalidOperationException($"Ya existe una categoría con el nombre '{nombre}'");
            }

            // Validar nombre
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre de la categoría no puede estar vacío");
            }

            // Actualizar información
            categoria.Nombre = nombre;
            categoria.Descripcion = descripcion;

            // Guardar cambios
            return await _repositorioCategoria.ActualizarAsync(categoria);
        }

        /// <summary>
        /// Obtiene una categoría por su ID
        /// </summary>
        public async Task<Categoria> ObtenerCategoriaAsync(int categoriaId)
        {
            return await _repositorioCategoria.ObtenerPorIdAsync(categoriaId);
        }

        /// <summary>
        /// Obtiene una categoría por su nombre
        /// </summary>
        public async Task<Categoria> ObtenerCategoriaPorNombreAsync(string nombre)
        {
            return await _repositorioCategoria.ObtenerPorNombreAsync(nombre);
        }

        /// <summary>
        /// Obtiene todas las categorías activas
        /// </summary>
        public async Task<IEnumerable<Categoria>> ObtenerCategoriasActivasAsync()
        {
            return await _repositorioCategoria.ObtenerActivasAsync();
        }

        /// <summary>
        /// Obtiene todas las categorías
        /// </summary>
        public async Task<IEnumerable<Categoria>> ObtenerTodasLasCategoriasAsync()
        {
            return await _repositorioCategoria.ObtenerTodasAsync();
        }

        /// <summary>
        /// Busca categorías por nombre
        /// </summary>
        public async Task<IEnumerable<Categoria>> BuscarCategoriasAsync(string terminoBusqueda)
        {
            return await _repositorioCategoria.BuscarPorNombreAsync(terminoBusqueda);
        }

        /// <summary>
        /// Activa una categoría
        /// </summary>
        public async Task<Categoria> ActivarCategoriaAsync(int categoriaId)
        {
            var categoria = await _repositorioCategoria.ObtenerPorIdAsync(categoriaId);
            if (categoria == null)
            {
                throw new ArgumentException("La categoría no existe");
            }

            categoria.Activo = true;
            return await _repositorioCategoria.ActualizarAsync(categoria);
        }

        /// <summary>
        /// Desactiva una categoría
        /// </summary>
        public async Task<Categoria> DesactivarCategoriaAsync(int categoriaId)
        {
            var categoria = await _repositorioCategoria.ObtenerPorIdAsync(categoriaId);
            if (categoria == null)
            {
                throw new ArgumentException("La categoría no existe");
            }

            // Verificar si tiene productos asociados
            var productos = await _repositorioProducto.BuscarPorCategoriaAsync(categoriaId);
            if (productos.Any())
            {
                throw new InvalidOperationException("No se puede desactivar una categoría que tiene productos asociados");
            }

            categoria.Activo = false;
            return await _repositorioCategoria.ActualizarAsync(categoria);
        }

        /// <summary>
        /// Elimina una categoría
        /// </summary>
        public async Task EliminarCategoriaAsync(int categoriaId)
        {
            var categoria = await _repositorioCategoria.ObtenerPorIdAsync(categoriaId);
            if (categoria == null)
            {
                throw new ArgumentException("La categoría no existe");
            }

            // Verificar si tiene productos asociados
            var productos = await _repositorioProducto.BuscarPorCategoriaAsync(categoriaId);
            if (productos.Any())
            {
                throw new InvalidOperationException("No se puede eliminar una categoría que tiene productos asociados");
            }

            await _repositorioCategoria.EliminarAsync(categoriaId);
        }

        /// <summary>
        /// Obtiene estadísticas de una categoría
        /// </summary>
        public async Task<object> ObtenerEstadisticasCategoriaAsync(int categoriaId)
        {
            var categoria = await _repositorioCategoria.ObtenerPorIdAsync(categoriaId);
            if (categoria == null)
            {
                throw new ArgumentException("La categoría no existe");
            }

            var productos = await _repositorioProducto.BuscarPorCategoriaAsync(categoriaId);
            var totalProductos = productos.Count();

            return new
            {
                CategoriaId = categoria.Id,
                Nombre = categoria.Nombre,
                TotalProductos = totalProductos,
                Activa = categoria.Activo,
                FechaCreacion = categoria.FechaCreacion
            };
        }

        /// <summary>
        /// Obtiene el conteo total de categorías
        /// </summary>
        public async Task<int> ContarCategoriasAsync()
        {
            return await _repositorioCategoria.ContarTotalAsync();
        }

        /// <summary>
        /// Obtiene el conteo de categorías activas
        /// </summary>
        public async Task<int> ContarCategoriasActivasAsync()
        {
            return await _repositorioCategoria.ContarActivasAsync();
        }
    }
}
