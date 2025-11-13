-- =============================================
-- Script: Creación de Base de Datos del Sistema de Inventario (Simplificado)
-- Descripción: Sistema de inventario simplificado con solo las funcionalidades esenciales
-- Fecha: 2024
-- =============================================

-- Crear la base de datos
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'InventarioDB')
    DROP DATABASE InventarioDB;
GO

CREATE DATABASE InventarioDB;
GO

USE InventarioDB;
GO

-- =============================================
-- TABLAS DEL DOMINIO (CORE BUSINESS)
-- =============================================

-- Tabla de Categorías
CREATE TABLE Categorias (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    Nombre NVARCHAR(100) NOT NULL UNIQUE,
    Descripcion NVARCHAR(300),
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Tabla de Proveedores
CREATE TABLE Proveedores (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    Codigo VARCHAR(50) NOT NULL UNIQUE,
    Nombre NVARCHAR(200) NOT NULL,
    Contacto NVARCHAR(200),
    Email VARCHAR(100),
    Telefono VARCHAR(20),
    Direccion NVARCHAR(300),
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Tabla de Productos
CREATE TABLE Productos (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    Codigo VARCHAR(50) NOT NULL UNIQUE,
    Nombre NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(500),
    Precio DECIMAL(18,2) NOT NULL,
    StockMinimo INT NOT NULL DEFAULT 0,
    StockMaximo INT NOT NULL DEFAULT 1000,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FechaModificacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CategoriaId UNIQUEIDENTIFIER NULL,
    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id) ON DELETE SET NULL
);

-- Tabla de Stock (Inventario)
CREATE TABLE Stock (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    ProductoId UNIQUEIDENTIFIER NOT NULL,
    Cantidad INT NOT NULL DEFAULT 0,
    Ubicacion VARCHAR(100),
    FechaUltimaActualizacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (ProductoId) REFERENCES Productos(Id) ON DELETE CASCADE
);

-- =============================================
-- TABLAS DE MOVIMIENTOS DE INVENTARIO
-- =============================================

-- Tabla de Entradas de Productos
CREATE TABLE EntradasProductos (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    ProductoId UNIQUEIDENTIFIER NOT NULL,
    ProveedorId UNIQUEIDENTIFIER NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    FechaEntrada DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    NumeroFactura VARCHAR(100),
    Observaciones NVARCHAR(500),
    FOREIGN KEY (ProductoId) REFERENCES Productos(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProveedorId) REFERENCES Proveedores(Id) ON DELETE CASCADE
);

-- Tabla de Salidas de Productos
CREATE TABLE SalidasProductos (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    ProductoId UNIQUEIDENTIFIER NOT NULL,
    Cantidad INT NOT NULL,
    Motivo NVARCHAR(200) NOT NULL,
    FechaSalida DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Responsable NVARCHAR(200),
    Observaciones NVARCHAR(500),
    FOREIGN KEY (ProductoId) REFERENCES Productos(Id) ON DELETE CASCADE
);

-- =============================================
-- ÍNDICES PARA OPTIMIZACIÓN
-- =============================================

-- Índices para Productos
CREATE INDEX IX_Productos_Codigo ON Productos(Codigo);
CREATE INDEX IX_Productos_Activo ON Productos(Activo);
CREATE INDEX IX_Productos_CategoriaId ON Productos(CategoriaId);

-- Índices para Stock
CREATE INDEX IX_Stock_ProductoId ON Stock(ProductoId);
CREATE INDEX IX_Stock_Cantidad ON Stock(Cantidad);

-- Índices para Entradas
CREATE INDEX IX_EntradasProductos_ProductoId ON EntradasProductos(ProductoId);
CREATE INDEX IX_EntradasProductos_ProveedorId ON EntradasProductos(ProveedorId);
CREATE INDEX IX_EntradasProductos_FechaEntrada ON EntradasProductos(FechaEntrada);

-- Índices para Salidas
CREATE INDEX IX_SalidasProductos_ProductoId ON SalidasProductos(ProductoId);
CREATE INDEX IX_SalidasProductos_FechaSalida ON SalidasProductos(FechaSalida);

-- =============================================
-- DATOS INICIALES
-- =============================================

-- Insertar categorías iniciales
INSERT INTO Categorias (Nombre, Descripcion) VALUES
('Electrónicos', 'Productos electrónicos y tecnológicos'),
('Ropa', 'Vestimenta y accesorios'),
('Hogar', 'Artículos para el hogar'),
('Deportes', 'Equipos y accesorios deportivos'),
('Libros', 'Libros y material educativo');

-- Insertar proveedores iniciales
INSERT INTO Proveedores (Codigo, Nombre, Contacto, Email, Telefono) VALUES
('PROV001', 'Tecnología Avanzada S.A.', 'Juan Pérez', 'juan.perez@tecnologia.com', '+1234567890'),
('PROV002', 'Moda y Estilo Ltda.', 'María García', 'maria.garcia@moda.com', '+1234567891'),
('PROV003', 'Hogar y Decoración', 'Carlos López', 'carlos.lopez@hogar.com', '+1234567892');

-- Insertar productos de ejemplo
INSERT INTO Productos (Codigo, Nombre, Descripcion, Precio, StockMinimo, StockMaximo, CategoriaId) VALUES
('PROD001', 'Laptop Dell Inspiron', 'Laptop de 15 pulgadas, 8GB RAM, 256GB SSD', 899.99, 5, 50, (SELECT Id FROM Categorias WHERE Nombre = 'Electrónicos')),
('PROD002', 'Smartphone Samsung Galaxy', 'Teléfono inteligente Android, 128GB', 699.99, 10, 100, (SELECT Id FROM Categorias WHERE Nombre = 'Electrónicos')),
('PROD003', 'Camiseta Básica', 'Camiseta de algodón 100%, varios colores', 19.99, 20, 200, (SELECT Id FROM Categorias WHERE Nombre = 'Ropa')),
('PROD004', 'Sofá 3 Plazas', 'Sofá moderno de tela gris', 599.99, 2, 10, (SELECT Id FROM Categorias WHERE Nombre = 'Hogar')),
('PROD005', 'Balón de Fútbol', 'Balón oficial de fútbol', 29.99, 15, 50, (SELECT Id FROM Categorias WHERE Nombre = 'Deportes'));

-- Insertar stock inicial
INSERT INTO Stock (ProductoId, Cantidad, Ubicacion) 
SELECT Id, 25, 'Almacén A' FROM Productos WHERE Codigo = 'PROD001'
UNION ALL
SELECT Id, 50, 'Almacén A' FROM Productos WHERE Codigo = 'PROD002'
UNION ALL
SELECT Id, 100, 'Almacén B' FROM Productos WHERE Codigo = 'PROD003'
UNION ALL
SELECT Id, 5, 'Almacén C' FROM Productos WHERE Codigo = 'PROD004'
UNION ALL
SELECT Id, 30, 'Almacén B' FROM Productos WHERE Codigo = 'PROD005';

-- =============================================
-- VISTAS ÚTILES
-- =============================================

-- Vista de productos con stock
GO
CREATE VIEW vw_ProductosConStock AS
SELECT 
    p.Id,
    p.Codigo,
    p.Nombre,
    p.Descripcion,
    p.Precio,
    p.StockMinimo,
    p.StockMaximo,
    s.Cantidad AS StockActual,
    s.Ubicacion,
    c.Nombre AS CategoriaNombre,
    c.Descripcion AS CategoriaDescripcion,
    CASE 
        WHEN s.Cantidad <= p.StockMinimo THEN 'Bajo'
        WHEN s.Cantidad >= p.StockMaximo THEN 'Exceso'
        WHEN s.Cantidad = 0 THEN 'Agotado'
        ELSE 'Normal'
    END AS EstadoStock,
    s.FechaUltimaActualizacion
FROM Productos p
LEFT JOIN Stock s ON p.Id = s.ProductoId
LEFT JOIN Categorias c ON p.CategoriaId = c.Id
WHERE p.Activo = 1;
GO

-- =============================================
-- MENSAJE FINAL
-- =============================================

PRINT '=============================================';
PRINT 'BASE DE DATOS INVENTARIO SIMPLIFICADA CREADA';
PRINT '=============================================';
PRINT 'Tablas creadas: 6';
PRINT '- Categorias';
PRINT '- Proveedores';
PRINT '- Productos';
PRINT '- Stock';
PRINT '- EntradasProductos';
PRINT '- SalidasProductos';
PRINT '=============================================';
PRINT 'Vista creada: 1';
PRINT '- vw_ProductosConStock';
PRINT '=============================================';
