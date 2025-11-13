USE FerreteriaDB;
GO

-- ========================================
-- 🔹 1. CATEGORÍAS
-- ========================================
INSERT INTO Categorias (Nombre, Descripcion)
VALUES 
('Herramientas Manuales', 'Herramientas como martillos, destornilladores, llaves.'),
('Pinturas', 'Pinturas y accesorios de pintura.'),
('Materiales de Construcción', 'Cemento, arena, ladrillos, etc.'),
('Fontanería', 'Tuberías, válvulas y accesorios para plomería.'),
('Eléctricos', 'Cables, interruptores y enchufes.');

-- ========================================
-- 🔹 2. PRODUCTOS
-- ========================================
INSERT INTO Productos (Nombre, Descripcion, PrecioCompra, PrecioVenta, StockActual, StockMinimo, IdCategoria)
VALUES
('Martillo Stanley 16oz', 'Martillo de acero con mango de fibra', 8.50, 12.90, 50, 10, 1),
('Destornillador Philips', 'Destornillador punta cruz, mango ergonómico', 3.00, 5.00, 80, 20, 1),
('Pintura Blanca 1 Galón', 'Pintura látex interior', 15.00, 25.00, 40, 5, 2),
('Cemento Gris 50kg', 'Cemento de alta resistencia', 10.00, 16.00, 100, 20, 3),
('Tubo PVC 1”', 'Tubo de PVC para agua fría', 4.50, 7.50, 60, 10, 4),
('Interruptor Simple', 'Interruptor eléctrico blanco 10A', 1.20, 2.50, 200, 30, 5);

-- ========================================
-- 🔹 3. PROVEEDORES
-- ========================================
INSERT INTO Proveedores (Nombre, Telefono, Email, Direccion)
VALUES
('Suministros Industriales S.A.', '555-1001', 'contacto@suministros.com', 'Av. Central 123'),
('Pinturas ColorMix', '555-2002', 'ventas@colormix.com', 'Calle Amarilla 45'),
('Construmax Ltda.', '555-3003', 'pedidos@construmax.com', 'Carretera Norte Km 5');

-- ========================================
-- 🔹 4. CLIENTES
-- ========================================
INSERT INTO Clientes (Nombre, Telefono, Email, Direccion)
VALUES
('Juan Pérez', '555-4004', 'juanperez@mail.com', 'Calle Real 12'),
('Ferretería La Esquina', '555-5005', 'compras@laesquina.com', 'Av. Principal 88'),
('Constructora Alfa', '555-6006', 'contacto@alfa.com', 'Zona Industrial 5');

-- ========================================
-- 🔹 5. EMPLEADOS
-- ========================================
INSERT INTO Empleados (Nombre, Cargo, Email, Telefono)
VALUES
('Carlos Gómez', 'Vendedor', 'carlos@ferreteria.com', '555-7007'),
('Laura Martínez', 'Cajera', 'laura@ferreteria.com', '555-8008'),
('Miguel Torres', 'Administrador', 'miguel@ferreteria.com', '555-9009');

-- ========================================
-- 🔹 6. PEDIDOS A PROVEEDORES
-- ========================================
INSERT INTO Pedidos (IdProveedor, Estado, Total)
VALUES
(1, 'Recibido', 425.00),
(2, 'Pendiente', 200.00);

-- Detalle de pedidos
INSERT INTO DetallePedidos (IdPedido, IdProducto, Cantidad, PrecioUnitario)
VALUES
(1, 1, 20, 8.50),   -- Martillos
(1, 2, 50, 3.00),   -- Destornilladores
(2, 3, 10, 15.00);  -- Pintura Blanca

-- ========================================
-- 🔹 7. FACTURAS (VENTAS)
-- ========================================
INSERT INTO Facturas (IdCliente, IdEmpleado, Estado, Total)
VALUES
(1, 1, 'Pagada', 40.00),
(2, 2, 'Emitida', 160.00);

-- Detalle de facturas
INSERT INTO DetalleFacturas (IdFactura, IdProducto, Cantidad, PrecioUnitario)
VALUES
(1, 1, 2, 12.90),   -- Martillo
(1, 2, 2, 5.00),    -- Destornillador
(2, 3, 4, 25.00),   -- Pintura Blanca
(2, 6, 10, 2.50);   -- Interruptor

-- ========================================
-- 🔹 8. PAGOS
-- ========================================
INSERT INTO Pagos (IdFactura, Monto, MetodoPago)
VALUES
(1, 40.00, 'Efectivo'),
(2, 160.00, 'Transferencia');

-- ========================================
-- 🔹 9. AJUSTE DE STOCK AUTOMÁTICO (se actualiza por triggers)
-- ========================================
-- Si ya tienes los triggers TR_ActualizarStockVenta / TR_ActualizarStockCompra,
-- el stock se ajustará automáticamente por las inserciones anteriores.
-- Pero si no los tienes activos, puedes recalcular stock manualmente:

-- Ejemplo de actualización manual (opcional)
-- UPDATE Productos
-- SET StockActual = StockActual - (
--     SELECT SUM(Cantidad) FROM DetalleFacturas WHERE DetalleFacturas.IdProducto = Productos.IdProducto
-- )
-- WHERE IdProducto IN (SELECT IdProducto FROM DetalleFacturas);

-- UPDATE Productos
-- SET StockActual = StockActual + (
--     SELECT SUM(Cantidad) FROM DetallePedidos WHERE DetallePedidos.IdProducto = Productos.IdProducto
-- )
-- WHERE IdProducto IN (SELECT IdProducto FROM DetallePedidos);
