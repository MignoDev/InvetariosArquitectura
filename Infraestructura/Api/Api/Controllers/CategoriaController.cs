using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoInventario.Domain.Models;
using System.Data;
using System.Text.Json.Serialization;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class CategoriaController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CategoriaController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Obtiene todas las categorías
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObtenerCategorias()
        {
            try
            {
                var categorias = new List<Categoria>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("sp_Categorias_GetAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                categorias.Add(new Categoria
                                {
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                                    Activo = reader.GetBoolean("Activo")
                                });
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = categorias, message = "Categorías obtenidas correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Obtiene todas las categorías
        /// </summary>
        [HttpGet("activo")]
        public async Task<IActionResult> ObtenerCategoriasActivas()
        {
            try
            {
                var categorias = new List<Categoria>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Categorias_GetActive", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                categorias.Add(new Categoria
                                {
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                                    Activo = reader.GetBoolean("Activo")
                                });
                            }
                        }
                    }
                }

                return Ok(new { success = true, data = categorias, message = "Categorías obtenidas correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Crea una nueva categoría
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearCategoria([FromBody] Categoria request)
        {
            try
            {

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Categorias_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);
                        command.Parameters.AddWithValue("@Descripcion", request.Descripcion ?? (object)DBNull.Value);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var categoria = new Categoria
                                {
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                                    Activo = reader.GetBoolean("Activo")
                                };

                                return CreatedAtAction(nameof(ObtenerCategorias),
                                    new { success = true, data = categoria, message = "Categoría creada correctamente" });
                            }
                        }
                    }
                }

                return StatusCode(500, new { success = false, message = "Error al crear la categoría" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }

        /// <summary>
        /// Crea una nueva categoría
        /// </summary>
        [HttpPut("{IdCategoria}")]
        public async Task<IActionResult> ActualizarCategoria(int IdCategoria, [FromBody] Categoria request)
        {
            try
            {

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_Categorias_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                        command.Parameters.AddWithValue("@Nombre", request.Nombre);
                        command.Parameters.AddWithValue("@Descripcion", request.Descripcion ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Activo", request.Activo);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var categoria = new Categoria
                                {
                                    IdCategoria = reader.GetInt32("IdCategoria"),
                                    Nombre = reader.GetString("Nombre"),
                                    Descripcion = reader.IsDBNull("Descripcion") ? null : reader.GetString("Descripcion"),
                                    Activo = reader.GetBoolean("Activo")
                                };

                                return CreatedAtAction(nameof(ObtenerCategorias),
                                    new { success = true, data = categoria, message = "Categoría actualizada correctamente" });
                            }
                        }
                    }
                }

                return StatusCode(500, new { success = false, message = "Error al actualizar la categoría" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, errors = new[] { ex.Message } });
            }
        }
        
    }
}
