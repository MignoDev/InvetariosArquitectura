using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Models;

namespace ProyectoInventario.Application.Service.Servicios
{
    /// <summary>
    /// Puerto de entrada para el servicio de categorías
    /// Define la interfaz para la gestión de categorías de productos
    /// </summary>
    public interface IServicioCategoria
    {
        /// <summary>
        /// Crea una nueva categoría
        /// </summary>
        Task<Categoria> CrearCategoriaAsync(string nombre, string descripcion = null);

        /// <summary>
        /// Actualiza la información de una categoría
        /// </summary>
        Task<Categoria> ActualizarCategoriaAsync(int categoriaId, string nombre, string descripcion);

        /// <summary>
        /// Obtiene una categoría por su ID
        /// </summary>
        Task<Categoria> ObtenerCategoriaAsync(int categoriaId);

        /// <summary>
        /// Obtiene una categoría por su nombre
        /// </summary>
        Task<Categoria> ObtenerCategoriaPorNombreAsync(string nombre);

        /// <summary>
        /// Obtiene todas las categorías activas
        /// </summary>
        Task<IEnumerable<Categoria>> ObtenerCategoriasActivasAsync();

        /// <summary>
        /// Obtiene todas las categorías
        /// </summary>
        Task<IEnumerable<Categoria>> ObtenerTodasLasCategoriasAsync();

        /// <summary>
        /// Busca categorías por nombre
        /// </summary>
        Task<IEnumerable<Categoria>> BuscarCategoriasAsync(string terminoBusqueda);

        /// <summary>
        /// Activa una categoría
        /// </summary>
        Task<Categoria> ActivarCategoriaAsync(int categoriaId);

        /// <summary>
        /// Desactiva una categoría
        /// </summary>
        Task<Categoria> DesactivarCategoriaAsync(int categoriaId);

        /// <summary>
        /// Elimina una categoría
        /// </summary>
        Task EliminarCategoriaAsync(int categoriaId);

        /// <summary>
        /// Obtiene estadísticas de una categoría
        /// </summary>
        Task<object> ObtenerEstadisticasCategoriaAsync(int categoriaId);

        /// <summary>
        /// Obtiene el conteo total de categorías
        /// </summary>
        Task<int> ContarCategoriasAsync();

        /// <summary>
        /// Obtiene el conteo de categorías activas
        /// </summary>
        Task<int> ContarCategoriasActivasAsync();
    }
}
