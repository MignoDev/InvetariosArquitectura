using AppBlazor.Components.Dialogs;
using AppBlazor.Services;
using Microsoft.AspNetCore.Components;
using ProyectoInventario.Domain.Models;
using Radzen;

namespace AppBlazor.Components.Pages
{
    public partial class GestionInventario : ComponentBase
    {

        [Inject] CategoriaService CategoriaService { get; set; }
        [Inject] ProductoService ProductoService { get; set; }
        [Inject] ProveedorService ProveedorService { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        private List<Producto>? productos;
        private List<Categoria>? categorias;
        private List<Proveedor>? proveedores;
        private bool cargando = false;
        protected override async Task OnInitializedAsync()
        {
            await CargarDatosIniciales();
        }

        private async Task AbrirDialogoProductoAsync()
        {

            var parameteres = new Dictionary<string, object>()
            {
                { "Categorias", categorias }
            };
            var dialogo = await DialogService.OpenAsync<CreateProducto>("Registrar producto nuevo", parameteres);
        }

        private async Task AbrirDialogCategoriaAsync()
        {

            var dialogo = await DialogService.OpenAsync<CreateProducto>("Registrar categoría");

        }

        private async Task AbrirDialogoStockAsync(ProductoConStock producto)
        {

            var parameteres = new Dictionary<string, object>()
            {
                { "Categorias", categorias },
                { "ProductoParam",  producto}
            };
            var dialogo = await DialogService.OpenAsync<CreateProducto>("Registrar producto nuevo", parameteres);
        }

        private async Task CargarDatosIniciales()
        {
            Console.WriteLine("Iniciando carga de datos iniciales...");
            cargando = true;
            try
            {
                // Cargar datos en paralelo pero sin bloquear la UI
                var tareas = new[]
                {
                CargarProductos(),
                CargarCategorias(),
                CargarProveedores()
            };

                await Task.WhenAll(tareas);
                Console.WriteLine("Carga de datos iniciales completada");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en carga inicial: {ex.Message}");
            }
            finally
            {
                cargando = false;
                StateHasChanged();
            }
        }

        private async Task CargarProductos()
        {
            try
            {
                Console.WriteLine("Iniciando carga de productos...");
                productos = await ProductoService.ObtenerTodosAsync();
                Console.WriteLine($"Productos cargados: {productos.Count}");

                StateHasChanged(); // Forzar actualización de la UI
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al cargar productos: {ex.Message}");
                productos = new List<Producto>();
            }
        }

        private async Task CargarCategorias()
        {
            try
            {
                Console.WriteLine("Iniciando carga de categorías...");
                categorias = await CategoriaService.ObtenerTodasAsync();
                Console.WriteLine($"Categorías cargadas: {categorias.Count}");

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al cargar categorías: {ex.Message}");
                categorias = new List<Categoria>();
            }
        }

        private async Task CargarProveedores()
        {
            try
            {
                Console.WriteLine("Iniciando carga de proveedores...");
                proveedores = await ProveedorService.ObtenerTodosAsync();
                Console.WriteLine($"Proveedores cargados: {proveedores.Count}");

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al cargar proveedores: {ex.Message}");
                proveedores = new List<Proveedor>();
            }
        }

        private async Task EliminarProducto(int id)
        {
            try
            {
                // Nota: El método EliminarAsync no existe en la API actual
                // Por ahora solo mostramos un mensaje
                Console.WriteLine($"Eliminar producto: {id} - Funcionalidad no implementada en la API");

                // TODO: Implementar eliminación cuando esté disponible en la API
                // await CargarProductos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al eliminar producto: {ex.Message}");
            }
        }

        private void EditarCategoria(int id)
        {
            Console.WriteLine($"Editar categoría: {id}");
        }



        private void EditarProveedor(int id)
        {
            Console.WriteLine($"Editar proveedor: {id}");
        }
    }
}
