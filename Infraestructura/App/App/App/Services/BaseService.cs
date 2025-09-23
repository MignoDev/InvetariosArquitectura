using AppBlazor.Models;

namespace AppBlazor.Services
{
    /// <summary>
    /// Servicio base mejorado que maneja la transformación de ApiResponse automáticamente
    /// </summary>
    public abstract class BaseService : ApiService
    {
        public BaseService(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// Ejecuta una operación GET y retorna directamente el modelo o null si hay error
        /// </summary>
        protected async Task<T?> GetModelAsync<T>(string endpoint)
        {
            var response = await GetAsync<T>(endpoint);
            return response.Success ? response.Data : default(T);
        }

        /// <summary>
        /// Ejecuta una operación GET y retorna directamente la lista o lista vacía si hay error
        /// </summary>
        protected async Task<List<T>> GetListAsync<T>(string endpoint)
        {
            var response = await GetAsync<List<T>>(endpoint);
            return response.Success ? response.Data ?? new List<T>() : new List<T>();
        }

        /// <summary>
        /// Ejecuta una operación POST y retorna directamente el modelo o null si hay error
        /// </summary>
        protected async Task<T?> PostModelAsync<T>(string endpoint, object data)
        {
            var response = await PostAsync<T>(endpoint, data);
            return response.Success ? response.Data : default(T);
        }

        /// <summary>
        /// Ejecuta una operación PUT y retorna directamente el modelo o null si hay error
        /// </summary>
        protected async Task<T?> PutModelAsync<T>(string endpoint, object data)
        {
            var response = await PutAsync<T>(endpoint, data);
            return response.Success ? response.Data : default(T);
        }

        /// <summary>
        /// Ejecuta una operación DELETE y retorna true si fue exitosa
        /// </summary>
        protected async Task<bool> DeleteModelAsync(string endpoint)
        {
            var response = await DeleteAsync(endpoint);
            return response.Success;
        }

        /// <summary>
        /// Ejecuta una operación y retorna un resultado con información de éxito/error
        /// </summary>
        protected async Task<ServiceResult<T>> ExecuteAsync<T>(Func<Task<ApiResponse<T>>> operation)
        {
            try
            {
                var response = await operation();
                return new ServiceResult<T>
                {
                    Success = response.Success,
                    Data = response.Data,
                    Message = response.Message,
                    Errors = response.Errors
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<T>
                {
                    Success = false,
                    Data = default(T),
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }

    /// <summary>
    /// Resultado de una operación de servicio
    /// </summary>
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
    }
}
