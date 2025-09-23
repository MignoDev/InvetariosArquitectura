using System.Text;
using System.Text.Json;
using AppBlazor.Models;

namespace AppBlazor.Services
{
    /// <summary>
    /// Servicio base para comunicación con la API
    /// </summary>
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        /// <summary>
        /// Realiza una petición GET a la API y retorna directamente los datos
        /// </summary>
        protected async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
        {
            try
            {
                Console.WriteLine($"GET {endpoint}");
                var response = await _httpClient.GetAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {content}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // La API retorna ApiResponse con estructura {success, data, message}
                        var result = JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);
                        return result ?? new ApiResponse<T> { Success = false, Message = "Error al deserializar la respuesta" };
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error de deserialización: {ex.Message}");
                        return new ApiResponse<T>
                        {
                            Success = false,
                            Message = "Error al deserializar la respuesta",
                            Errors = new List<string> { ex.Message, content }
                        };
                    }
                }
                else
                {
                    return new ApiResponse<T>
                    {
                        Success = false,
                        Message = $"Error HTTP {response.StatusCode}",
                        Errors = new List<string> { content }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción en GetAsync: {ex.Message}");
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = "Error de conexión con la API",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Realiza una petición POST a la API
        /// </summary>
        protected async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"POST {endpoint}");
                Console.WriteLine($"Request Data: {json}");

                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // La API retorna ApiResponse con estructura {success, data, message}
                        var result = JsonSerializer.Deserialize<ApiResponse<T>>(responseContent, _jsonOptions);
                        return result ?? new ApiResponse<T> { Success = false, Message = "Error al deserializar la respuesta" };
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error de deserialización: {ex.Message}");
                        return new ApiResponse<T>
                        {
                            Success = false,
                            Message = "Error al deserializar la respuesta",
                            Errors = new List<string> { ex.Message, responseContent }
                        };
                    }
                }
                else
                {
                    return new ApiResponse<T>
                    {
                        Success = false,
                        Message = $"Error HTTP {response.StatusCode}",
                        Errors = new List<string> { responseContent }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción en PostAsync: {ex.Message}");
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = "Error de conexión con la API",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Realiza una petición PUT a la API
        /// </summary>
        protected async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine($"PUT {endpoint}");
                Console.WriteLine($"Request Data: {json}");

                var response = await _httpClient.PutAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // La API retorna ApiResponse con estructura {success, data, message}
                        var result = JsonSerializer.Deserialize<ApiResponse<T>>(responseContent, _jsonOptions);
                        return result ?? new ApiResponse<T> { Success = false, Message = "Error al deserializar la respuesta" };
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error de deserialización: {ex.Message}");
                        return new ApiResponse<T>
                        {
                            Success = false,
                            Message = "Error al deserializar la respuesta",
                            Errors = new List<string> { ex.Message, responseContent }
                        };
                    }
                }
                else
                {
                    return new ApiResponse<T>
                    {
                        Success = false,
                        Message = $"Error HTTP {response.StatusCode}",
                        Errors = new List<string> { responseContent }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción en PutAsync: {ex.Message}");
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = "Error de conexión con la API",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Realiza una petición DELETE a la API
        /// </summary>
        protected async Task<ApiResponse> DeleteAsync(string endpoint)
        {
            try
            {
                Console.WriteLine($"DELETE {endpoint}");
                var response = await _httpClient.DeleteAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {content}");

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse
                    {
                        Success = true,
                        Message = "Operación exitosa"
                    };
                }
                else
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = $"Error HTTP {response.StatusCode}",
                        Errors = new List<string> { content }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción en DeleteAsync: {ex.Message}");
                return new ApiResponse
                {
                    Success = false,
                    Message = "Error de conexión con la API",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
