-- ========================================
-- BASE DE DATOS: FerreteriaDB
-- ========================================
--CREATE DATABASE FerreteriaDB;

GO

USE FerreteriaDB;
GO

IF OBJECT_ID('dbo.DetalleFacturas', 'U') IS NOT NULL DROP TABLE dbo.DetalleFacturas;
IF OBJECT_ID('dbo.DetallePedidos', 'U') IS NOT NULL DROP TABLE dbo.DetallePedidos;
IF OBJECT_ID('dbo.Pedidos', 'U') IS NOT NULL DROP TABLE dbo.Pedidos;
IF OBJECT_ID('dbo.Pagos', 'U') IS NOT NULL DROP TABLE dbo.Pagos;
IF OBJECT_ID('dbo.Facturas', 'U') IS NOT NULL DROP TABLE dbo.Facturas;
IF OBJECT_ID('dbo.Categorias', 'U') IS NOT NULL DROP TABLE dbo.Productos;
IF OBJECT_ID('dbo.Categorias', 'U') IS NOT NULL DROP TABLE dbo.Categorias;
IF OBJECT_ID('dbo.Proveedores', 'U') IS NOT NULL DROP TABLE dbo.Proveedores;
IF OBJECT_ID('dbo.Clientes', 'U') IS NOT NULL DROP TABLE dbo.Clientes;
IF OBJECT_ID('dbo.Empleados', 'U') IS NOT NULL DROP TABLE dbo.Empleados;





-- ========================================
-- TABLAS PRINCIPALES
-- ========================================

-- 1️⃣ CATEGORÍAS DE PRODUCTOS
CREATE TABLE Categorias (
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(250),
    Activo BIT NOT NULL DEFAULT 1
);

-- 2️⃣ PRODUCTOS
CREATE TABLE Productos (
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(300),
    PrecioCompra DECIMAL(10,2) NOT NULL,
    PrecioVenta DECIMAL(10,2) NOT NULL,
    StockActual INT NOT NULL DEFAULT 0,
    StockMinimo INT NOT NULL DEFAULT 0,
    IdCategoria INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (IdCategoria)
        REFERENCES Categorias(IdCategoria)
);

-- 3️⃣ PROVEEDORES
CREATE TABLE Proveedores (
    IdProveedor INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Telefono NVARCHAR(20),
    Email NVARCHAR(100),
    Direccion NVARCHAR(250)
);

-- 4️⃣ CLIENTES
CREATE TABLE Clientes (
    IdCliente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Telefono NVARCHAR(20),
    Email NVARCHAR(100),
    Direccion NVARCHAR(250)
);

-- 5️⃣ EMPLEADOS (VENDEDORES / ADMIN)
CREATE TABLE Empleados (
    IdEmpleado INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Cargo NVARCHAR(50),
    Email NVARCHAR(100),
    Telefono NVARCHAR(20),
    Activo BIT NOT NULL DEFAULT 1
);

-- ========================================
-- TABLAS DE MOVIMIENTOS / TRANSACCIONES
-- ========================================

-- 6️⃣ PEDIDOS A PROVEEDORES
CREATE TABLE Pedidos (
    IdPedido INT IDENTITY(1,1) PRIMARY KEY,
    FechaPedido DATETIME NOT NULL DEFAULT GETDATE(),
    IdProveedor INT NOT NULL,
    Estado NVARCHAR(50) NOT NULL DEFAULT 'Pendiente', -- Pendiente, Recibido, Cancelado
    Total DECIMAL(12,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_Pedidos_Proveedores FOREIGN KEY (IdProveedor)
        REFERENCES Proveedores(IdProveedor)
);

-- 7️⃣ DETALLE DE PEDIDOS
CREATE TABLE DetallePedidos (
    IdDetallePedido INT IDENTITY(1,1) PRIMARY KEY,
    IdPedido INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    Subtotal AS (Cantidad * PrecioUnitario) PERSISTED,
    CONSTRAINT FK_DetallePedidos_Pedidos FOREIGN KEY (IdPedido)
        REFERENCES Pedidos(IdPedido),
    CONSTRAINT FK_DetallePedidos_Productos FOREIGN KEY (IdProducto)
        REFERENCES Productos(IdProducto)
);

-- 8️⃣ FACTURAS DE CLIENTES (VENTAS)
CREATE TABLE Facturas (
    IdFactura INT IDENTITY(1,1) PRIMARY KEY,
    FechaFactura DATETIME NOT NULL DEFAULT GETDATE(),
    IdCliente INT ,
    IdEmpleado INT NOT NULL,
    Total DECIMAL(12,2) NOT NULL DEFAULT 0,
    Estado NVARCHAR(50) NOT NULL DEFAULT 'Emitida', -- Emitida, Pagada, Anulada
    CONSTRAINT FK_Facturas_Clientes FOREIGN KEY (IdCliente)
        REFERENCES Clientes(IdCliente),
    CONSTRAINT FK_Facturas_Empleados FOREIGN KEY (IdEmpleado)
        REFERENCES Empleados(IdEmpleado)
);

-- 9️⃣ DETALLE DE FACTURAS
CREATE TABLE DetalleFacturas (
    IdDetalleFactura INT IDENTITY(1,1) PRIMARY KEY,
    IdFactura INT NOT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    Subtotal AS (Cantidad * PrecioUnitario) PERSISTED,
    CONSTRAINT FK_DetalleFacturas_Facturas FOREIGN KEY (IdFactura)
        REFERENCES Facturas(IdFactura),
    CONSTRAINT FK_DetalleFacturas_Productos FOREIGN KEY (IdProducto)
        REFERENCES Productos(IdProducto)
);

-- 🔟 PAGOS
CREATE TABLE Pagos (
    IdPago INT IDENTITY(1,1) PRIMARY KEY,
    IdFactura INT NOT NULL,
    FechaPago DATETIME NOT NULL DEFAULT GETDATE(),
    Monto DECIMAL(12,2) NOT NULL,
    MetodoPago NVARCHAR(50) NOT NULL, -- Efectivo, Tarjeta, Transferencia
    CONSTRAINT FK_Pagos_Facturas FOREIGN KEY (IdFactura)
        REFERENCES Facturas(IdFactura)
);

-- ========================================
-- TRIGGER OPCIONAL: ACTUALIZAR STOCK AUTOMÁTICAMENTE
-- ========================================
GO
CREATE TRIGGER TR_ActualizarStockVenta
ON DetalleFacturas
AFTER INSERT
AS
BEGIN
    UPDATE p
    SET p.StockActual = p.StockActual - i.Cantidad
    FROM Productos p
    INNER JOIN inserted i ON p.IdProducto = i.IdProducto;
END;
GO

CREATE TRIGGER TR_ActualizarStockCompra
ON DetallePedidos
AFTER INSERT
AS
BEGIN
    UPDATE p
    SET p.StockActual = p.StockActual + i.Cantidad
    FROM Productos p
    INNER JOIN inserted i ON p.IdProducto = i.IdProducto;
END;
GO