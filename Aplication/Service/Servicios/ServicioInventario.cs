using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoInventario.Domain.Models;
using ProyectoInventario.Domain.Ports;

namespace ProyectoInventario.Application.Service.Servicios
{
    /// <summary>
    /// Servicio de aplicación para la gestión del inventario
    /// Implementa la lógica de negocio para productos, stock y movimientos
    /// </summary>
    public class ServicioInventario
    {
        private readonly IRepositorioProducto _repositorioProducto;
        private readonly IRepositorioStock _repositorioStock;
        private readonly IRepositorioCategoria _repositorioCategoria;
        private readonly IRepositorioProveedor _repositorioProveedor;
        private readonly IRepositorioMovimiento _repositorioMovimiento;

        public ServicioInventario(
            IRepositorioProducto repositorioProducto,
            IRepositorioStock repositorioStock,
            IRepositorioCategoria repositorioCategoria,
            IRepositorioProveedor repositorioProveedor,
            IRepositorioMovimiento repositorioMovimiento)
        {
            _repositorioProducto = repositorioProducto;
            _repositorioStock = repositorioStock;
            _repositorioCategoria = repositorioCategoria;
            _repositorioProveedor = repositorioProveedor;
            _repositorioMovimiento = repositorioMovimiento;
        }

        #region Gestión de Productos

        /// <summary>
        /// Crea un nuevo producto en el inventario
        /// </summary>
        public async Task<Producto> CrearProductoAsync(Producto producto)
        {
            // Validar que el código no exista
            if (await _repositorioProducto.ExisteCodigoAsync(producto.Codigo))
            {
                throw new InvalidOperationException($"Ya existe un producto con el código '{producto.Codigo}'");
            }

            // Validar categoría si se proporciona
            if (producto.CategoriaId.HasValue)
            {
                var categoria = await _repositorioCategoria.ObtenerPorIdAsync(producto.CategoriaId.Value);
                if (categoria == null)
                {
                    throw new ArgumentException("La categoría especificada no existe");
                }
            }

            // Guardar producto
            var productoGuardado = await _repositorioProducto.CrearAsync(producto);

            return productoGuardado;
        }

        /// <summary>
        /// Actualiza la información de un producto
        /// </summary>
        public async Task<Producto> ActualizarProductoAsync(Guid productoId, string nombre, 
            string descripcion, decimal precio)
        {
            var producto = await _repositorioProducto.ObtenerPorIdAsync(productoId);
            if (producto == null)
            {
                throw new ArgumentException("El producto no existe");
            }

            // Actualizar información
            producto.Nombre = nombre;
            producto.Descripcion = descripcion;
            producto.Precio = precio;
            producto.FechaModificacion = DateTime.UtcNow;

            // Guardar cambios
            return await _repositorioProducto.ActualizarAsync(producto);
        }

        /// <summary>
        /// Actualiza los límites de stock de un producto
        /// </summary>
        public async Task<Producto> ActualizarLimitesStockAsync(Guid productoId, int stockMinimo, int stockMaximo)
        {
            var producto = await _repositorioProducto.ObtenerPorIdAsync(productoId);
            if (producto == null)
            {
                throw new ArgumentException("El producto no existe");
            }

            producto.StockMinimo = stockMinimo;
            producto.StockMaximo = stockMaximo;
            producto.FechaModificacion = DateTime.UtcNow;

            return await _repositorioProducto.ActualizarAsync(producto);
        }

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        public async Task<Producto> ObtenerProductoAsync(Guid productoId)
        {
            return await _repositorioProducto.ObtenerPorIdAsync(productoId);
        }

        /// <summary>
        /// Obtiene un producto por su código
        /// </summary>
        public async Task<Producto> ObtenerProductoPorCodigoAsync(string codigo)
        {
            return await _repositorioProducto.ObtenerPorCodigoAsync(codigo);
        }

        /// <summary>
        /// Obtiene todos los productos activos
        /// </summary>
        public async Task<IEnumerable<Producto>> ObtenerProductosActivosAsync()
        {
            return await _repositorioProducto.ObtenerActivosAsync();
        }

        /// <summary>
        /// Busca productos por nombre
        /// </summary>
        public async Task<IEnumerable<Producto>> BuscarProductosAsync(string terminoBusqueda)
        {
            return await _repositorioProducto.BuscarPorNombreAsync(terminoBusqueda);
        }

        #endregion

        #region Gestión de Stock

        /// <summary>
        /// Ajusta el stock de un producto
        /// </summary>
        public async Task<Stock> AjustarStockAsync(Guid productoId, int nuevaCantidad)
        {
            var producto = await _repositorioProducto.ObtenerPorIdAsync(productoId);
            if (producto == null)
            {
                throw new ArgumentException("El producto no existe");
            }

            var stock = await _repositorioStock.ObtenerPorProductoIdAsync(productoId);
            if (stock == null)
            {
                throw new InvalidOperationException("El producto no tiene stock registrado");
            }

            stock.Cantidad = nuevaCantidad;
            stock.FechaUltimaActualizacion = DateTime.UtcNow;

            return await _repositorioStock.ActualizarAsync(stock);
        }

        /// <summary>
        /// Agrega stock a un producto
        /// </summary>
        public async Task<Stock> AgregarStockAsync(Guid productoId, int cantidad)
        {
            var producto = await _repositorioProducto.ObtenerPorIdAsync(productoId);
            if (producto == null)
            {
                throw new ArgumentException("El producto no existe");
            }

            var stock = await _repositorioStock.ObtenerPorProductoIdAsync(productoId);
            if (stock == null)
            {
                throw new InvalidOperationException("El producto no tiene stock registrado");
            }

            stock.Cantidad += cantidad;
            stock.FechaUltimaActualizacion = DateTime.UtcNow;

            return await _repositorioStock.ActualizarAsync(stock);
        }

        /// <summary>
        /// Reduce el stock de un producto
        /// </summary>
        public async Task<Stock> ReducirStockAsync(Guid productoId, int cantidad)
        {
            var producto = await _repositorioProducto.ObtenerPorIdAsync(productoId);
            if (producto == null)
            {
                throw new ArgumentException("El producto no existe");
            }

            var stock = await _repositorioStock.ObtenerPorProductoIdAsync(productoId);
            if (stock == null)
            {
                throw new InvalidOperationException("El producto no tiene stock registrado");
            }

            if (stock.Cantidad < cantidad)
            {
                throw new InvalidOperationException("No hay suficiente stock disponible");
            }

            stock.Cantidad -= cantidad;
            stock.FechaUltimaActualizacion = DateTime.UtcNow;

            return await _repositorioStock.ActualizarAsync(stock);
        }

        /// <summary>
        /// Obtiene el stock de un producto
        /// </summary>
        public async Task<Stock> ObtenerStockAsync(Guid productoId)
        {
            return await _repositorioStock.ObtenerPorProductoIdAsync(productoId);
        }

        /// <summary>
        /// Obtiene productos con stock bajo
        /// </summary>
        public async Task<IEnumerable<Producto>> ObtenerProductosConStockBajoAsync()
        {
            return await _repositorioProducto.BuscarConStockBajoAsync();
        }

        /// <summary>
        /// Obtiene productos agotados
        /// </summary>
        public async Task<IEnumerable<Producto>> ObtenerProductosAgotadosAsync()
        {
            return await _repositorioProducto.BuscarAgotadosAsync();
        }

        #endregion

        #region Gestión de Movimientos

        /// <summary>
        /// Registra una entrada de productos
        /// </summary>
        public async Task<EntradaProducto> RegistrarEntradaAsync(Guid productoId, Guid proveedorId, 
            int cantidad, decimal precioUnitario, string numeroFactura = null, string observaciones = null)
        {
            // Validar producto
            var producto = await _repositorioProducto.ObtenerPorIdAsync(productoId);
            if (producto == null)
            {
                throw new ArgumentException("El producto no existe");
            }

            // Validar proveedor
            var proveedor = await _repositorioProveedor.ObtenerPorIdAsync(proveedorId);
            if (proveedor == null)
            {
                throw new ArgumentException("El proveedor no existe");
            }

            // Validar número de factura si se proporciona
            if (!string.IsNullOrWhiteSpace(numeroFactura) && 
                await _repositorioMovimiento.ExisteEntradaConFacturaAsync(numeroFactura))
            {
                throw new InvalidOperationException($"Ya existe una entrada con el número de factura '{numeroFactura}'");
            }

            // Crear entrada
            var entrada = new EntradaProducto
            {
                Id = Guid.NewGuid(),
                ProductoId = productoId,
                ProveedorId = proveedorId,
                Cantidad = cantidad,
                PrecioUnitario = precioUnitario,
                FechaEntrada = DateTime.UtcNow,
                NumeroFactura = numeroFactura,
                Observaciones = observaciones
            };

            // Guardar entrada
            var entradaGuardada = await _repositorioMovimiento.CrearEntradaAsync(entrada);

            // Actualizar stock
            await AgregarStockAsync(productoId, cantidad);

            return entradaGuardada;
        }

        /// <summary>
        /// Registra una salida de productos
        /// </summary>
        public async Task<SalidaProducto> RegistrarSalidaAsync(Guid productoId, int cantidad, 
            string motivo, string responsable = null, string observaciones = null)
        {
            // Validar producto
            var producto = await _repositorioProducto.ObtenerPorIdAsync(productoId);
            if (producto == null)
            {
                throw new ArgumentException("El producto no existe");
            }

            // Validar stock disponible
            var stock = await _repositorioStock.ObtenerPorProductoIdAsync(productoId);
            if (stock == null || stock.Cantidad < cantidad)
            {
                throw new InvalidOperationException("No hay suficiente stock disponible");
            }

            // Crear salida
            var salida = new SalidaProducto
            {
                Id = Guid.NewGuid(),
                ProductoId = productoId,
                Cantidad = cantidad,
                Motivo = motivo,
                FechaSalida = DateTime.UtcNow,
                Responsable = responsable,
                Observaciones = observaciones
            };

            // Guardar salida
            var salidaGuardada = await _repositorioMovimiento.CrearSalidaAsync(salida);

            // Actualizar stock
            await ReducirStockAsync(productoId, cantidad);

            return salidaGuardada;
        }

        /// <summary>
        /// Obtiene el historial de movimientos de un producto
        /// </summary>
        public async Task<IEnumerable<object>> ObtenerHistorialMovimientosAsync(Guid productoId)
        {
            return await _repositorioMovimiento.ObtenerMovimientosPorProductoAsync(productoId);
        }

        #endregion

    }
}
