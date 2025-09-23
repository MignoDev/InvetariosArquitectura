-- =============================================
-- PROCEDIMIENTOS ALMACENADOS PARA EL SISTEMA DE INVENTARIO (SIMPLIFICADO)
-- =============================================

USE InventarioDB;
GO

-- =============================================
-- PROCEDIMIENTOS PARA PRODUCTOS
-- =============================================

-- Obtener todos los productos activos con stock
CREATE OR ALTER PROCEDURE sp_ObtenerProductosActivos
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.Id,
        p.Codigo,
        p.Nombre,
        p.Descripcion,
        p.Precio,
        p.StockMinimo,
        p.StockMaximo,
        p.Activo,
        p.FechaCreacion,
        p.FechaModificacion,
        p.CategoriaId,
        c.Nombre AS CategoriaNombre,
        c.Descripcion AS CategoriaDescripcion,
        s.Cantidad AS StockActual,
        s.Ubicacion,
        CASE 
            WHEN s.Cantidad <= p.StockMinimo THEN 'Bajo'
            WHEN s.Cantidad >= p.StockMaximo THEN 'Exceso'
            WHEN s.Cantidad = 0 THEN 'Agotado'
            ELSE 'Normal'
        END AS EstadoStock
    FROM Productos p
    LEFT JOIN Categorias c ON p.CategoriaId = c.Id
    LEFT JOIN Stock s ON p.Id = s.ProductoId
    WHERE p.Activo = 1
    ORDER BY p.Nombre;
END
GO

-- Obtener producto por ID
CREATE OR ALTER PROCEDURE sp_ObtenerProductoPorId
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.Id,
        p.Codigo,
        p.Nombre,
        p.Descripcion,
        p.Precio,
        p.StockMinimo,
        p.StockMaximo,
        p.Activo,
        p.FechaCreacion,
        p.FechaModificacion,
        p.CategoriaId,
        c.Nombre AS CategoriaNombre,
        c.Descripcion AS CategoriaDescripcion,
        s.Cantidad AS StockActual,
        s.Ubicacion,
        CASE 
            WHEN s.Cantidad <= p.StockMinimo THEN 'Bajo'
            WHEN s.Cantidad >= p.StockMaximo THEN 'Exceso'
            WHEN s.Cantidad = 0 THEN 'Agotado'
            ELSE 'Normal'
        END AS EstadoStock
    FROM Productos p
    LEFT JOIN Categorias c ON p.CategoriaId = c.Id
    LEFT JOIN Stock s ON p.Id = s.ProductoId
    WHERE p.Id = @Id;
END
GO

-- Obtener producto por código
CREATE OR ALTER PROCEDURE sp_ObtenerProductoPorCodigo
    @Codigo VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.Id,
        p.Codigo,
        p.Nombre,
        p.Descripcion,
        p.Precio,
        p.StockMinimo,
        p.StockMaximo,
        p.Activo,
        p.FechaCreacion,
        p.FechaModificacion,
        p.CategoriaId,
        c.Nombre AS CategoriaNombre,
        c.Descripcion AS CategoriaDescripcion,
        s.Cantidad AS StockActual,
        s.Ubicacion,
        CASE 
            WHEN s.Cantidad <= p.StockMinimo THEN 'Bajo'
            WHEN s.Cantidad >= p.StockMaximo THEN 'Exceso'
            WHEN s.Cantidad = 0 THEN 'Agotado'
            ELSE 'Normal'
        END AS EstadoStock
    FROM Productos p
    LEFT JOIN Categorias c ON p.CategoriaId = c.Id
    LEFT JOIN Stock s ON p.Id = s.ProductoId
    WHERE p.Codigo = @Codigo;
END
GO

-- Crear nuevo producto
CREATE OR ALTER PROCEDURE sp_CrearProducto
    @Codigo VARCHAR(50),
    @Nombre NVARCHAR(200),
    @Descripcion NVARCHAR(500) = NULL,
    @Precio DECIMAL(18,2),
    @StockMinimo INT = 0,
    @StockMaximo INT = 1000,
    @CategoriaId UNIQUEIDENTIFIER = NULL,
    @StockInicial INT = 0,
    @Ubicacion VARCHAR(100) = 'Almacén Principal'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    DECLARE @ProductoId UNIQUEIDENTIFIER = NEWID();
    
    BEGIN TRY
        -- Insertar producto
        INSERT INTO Productos (Id, Codigo, Nombre, Descripcion, Precio, StockMinimo, StockMaximo, CategoriaId)
        VALUES (@ProductoId, @Codigo, @Nombre, @Descripcion, @Precio, @StockMinimo, @StockMaximo, @CategoriaId);
        
        -- Insertar stock inicial
        INSERT INTO Stock (ProductoId, Cantidad, Ubicacion)
        VALUES (@ProductoId, @StockInicial, @Ubicacion);
        
        COMMIT TRANSACTION;
        
        -- Devolver el producto creado
        EXEC sp_ObtenerProductoPorId @ProductoId;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Actualizar producto
CREATE OR ALTER PROCEDURE sp_ActualizarProducto
    @Id UNIQUEIDENTIFIER,
    @Nombre NVARCHAR(200),
    @Descripcion NVARCHAR(500) = NULL,
    @Precio DECIMAL(18,2),
    @StockMinimo INT,
    @StockMaximo INT,
    @CategoriaId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Productos 
    SET 
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        Precio = @Precio,
        StockMinimo = @StockMinimo,
        StockMaximo = @StockMaximo,
        CategoriaId = @CategoriaId,
        FechaModificacion = GETUTCDATE()
    WHERE Id = @Id;
    
    -- Devolver el producto actualizado
    EXEC sp_ObtenerProductoPorId @Id;
END
GO

-- =============================================
-- PROCEDIMIENTOS PARA CATEGORÍAS
-- =============================================

-- Obtener todas las categorías activas
CREATE OR ALTER PROCEDURE sp_ObtenerCategoriasActivas
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        c.Id,
        c.Nombre,
        c.Descripcion,
        c.Activo,
        c.FechaCreacion,
        COUNT(p.Id) AS TotalProductos
    FROM Categorias c
    LEFT JOIN Productos p ON c.Id = p.CategoriaId AND p.Activo = 1
    WHERE c.Activo = 1
    GROUP BY c.Id, c.Nombre, c.Descripcion, c.Activo, c.FechaCreacion
    ORDER BY c.Nombre;
END
GO

-- Crear nueva categoría
CREATE OR ALTER PROCEDURE sp_CrearCategoria
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Categorias (Nombre, Descripcion)
    VALUES (@Nombre, @Descripcion);
    
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

-- =============================================
-- PROCEDIMIENTOS PARA PROVEEDORES
-- =============================================

-- Obtener todos los proveedores activos
CREATE OR ALTER PROCEDURE sp_ObtenerProveedoresActivos
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.Id,
        p.Codigo,
        p.Nombre,
        p.Contacto,
        p.Email,
        p.Telefono,
        p.Direccion,
        p.Activo,
        p.FechaCreacion,
        COUNT(e.Id) AS TotalEntradas
    FROM Proveedores p
    LEFT JOIN EntradasProductos e ON p.Id = e.ProveedorId
    WHERE p.Activo = 1
    GROUP BY p.Id, p.Codigo, p.Nombre, p.Contacto, p.Email, p.Telefono, p.Direccion, p.Activo, p.FechaCreacion
    ORDER BY p.Nombre;
END
GO

-- Crear nuevo proveedor
CREATE OR ALTER PROCEDURE sp_CrearProveedor
    @Codigo VARCHAR(50),
    @Nombre NVARCHAR(200),
    @Contacto NVARCHAR(200) = NULL,
    @Email VARCHAR(100) = NULL,
    @Telefono VARCHAR(20) = NULL,
    @Direccion NVARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Proveedores (Codigo, Nombre, Contacto, Email, Telefono, Direccion)
    VALUES (@Codigo, @Nombre, @Contacto, @Email, @Telefono, @Direccion);
    
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

-- =============================================
-- PROCEDIMIENTOS PARA STOCK
-- =============================================

-- Actualizar stock de producto
CREATE OR ALTER PROCEDURE sp_ActualizarStock
    @ProductoId UNIQUEIDENTIFIER,
    @Cantidad INT,
    @Ubicacion VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM Stock WHERE ProductoId = @ProductoId)
    BEGIN
        UPDATE Stock 
        SET 
            Cantidad = @Cantidad,
            Ubicacion = ISNULL(@Ubicacion, Ubicacion),
            FechaUltimaActualizacion = GETUTCDATE()
        WHERE ProductoId = @ProductoId;
    END
    ELSE
    BEGIN
        INSERT INTO Stock (ProductoId, Cantidad, Ubicacion)
        VALUES (@ProductoId, @Cantidad, ISNULL(@Ubicacion, 'Almacén Principal'));
    END
END
GO

-- Ajustar stock (sumar o restar)
CREATE OR ALTER PROCEDURE sp_AjustarStock
    @ProductoId UNIQUEIDENTIFIER,
    @CantidadAjuste INT,
    @Ubicacion VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @StockActual INT;
    
    SELECT @StockActual = ISNULL(Cantidad, 0) 
    FROM Stock 
    WHERE ProductoId = @ProductoId;
    
    IF @StockActual IS NULL
    BEGIN
        -- Crear registro de stock si no existe
        INSERT INTO Stock (ProductoId, Cantidad, Ubicacion)
        VALUES (@ProductoId, @CantidadAjuste, ISNULL(@Ubicacion, 'Almacén Principal'));
    END
    ELSE
    BEGIN
        -- Ajustar stock existente
        UPDATE Stock 
        SET 
            Cantidad = @StockActual + @CantidadAjuste,
            Ubicacion = ISNULL(@Ubicacion, Ubicacion),
            FechaUltimaActualizacion = GETUTCDATE()
        WHERE ProductoId = @ProductoId;
    END
END
GO

-- =============================================
-- PROCEDIMIENTOS PARA ENTRADAS
-- =============================================

-- Registrar entrada de producto
CREATE OR ALTER PROCEDURE sp_RegistrarEntrada
    @ProductoId UNIQUEIDENTIFIER,
    @ProveedorId UNIQUEIDENTIFIER,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(18,2),
    @NumeroFactura VARCHAR(100) = NULL,
    @Observaciones NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Insertar entrada
        INSERT INTO EntradasProductos (ProductoId, ProveedorId, Cantidad, PrecioUnitario, NumeroFactura, Observaciones)
        VALUES (@ProductoId, @ProveedorId, @Cantidad, @PrecioUnitario, @NumeroFactura, @Observaciones);
        
        -- Actualizar stock
        EXEC sp_AjustarStock @ProductoId, @Cantidad, NULL;
        
        COMMIT TRANSACTION;
        
        SELECT SCOPE_IDENTITY() AS Id;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Obtener entradas por rango de fechas
CREATE OR ALTER PROCEDURE sp_ObtenerEntradasPorFecha
    @FechaInicio DATETIME2,
    @FechaFin DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        e.Id,
        e.ProductoId,
        e.ProveedorId,
        e.Cantidad,
        e.PrecioUnitario,
        e.FechaEntrada,
        e.NumeroFactura,
        e.Observaciones,
        p.Codigo AS ProductoCodigo,
        p.Nombre AS ProductoNombre,
        pr.Codigo AS ProveedorCodigo,
        pr.Nombre AS ProveedorNombre,
        c.Nombre AS CategoriaNombre,
        (e.Cantidad * e.PrecioUnitario) AS ValorTotal
    FROM EntradasProductos e
    INNER JOIN Productos p ON e.ProductoId = p.Id
    INNER JOIN Proveedores pr ON e.ProveedorId = pr.Id
    LEFT JOIN Categorias c ON p.CategoriaId = c.Id
    WHERE e.FechaEntrada BETWEEN @FechaInicio AND @FechaFin
    ORDER BY e.FechaEntrada DESC;
END
GO

-- =============================================
-- PROCEDIMIENTOS PARA SALIDAS
-- =============================================

-- Registrar salida de producto
CREATE OR ALTER PROCEDURE sp_RegistrarSalida
    @ProductoId UNIQUEIDENTIFIER,
    @Cantidad INT,
    @Motivo NVARCHAR(200),
    @Responsable NVARCHAR(200) = NULL,
    @Observaciones NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    DECLARE @StockActual INT;
    
    BEGIN TRY
        -- Verificar stock disponible
        SELECT @StockActual = ISNULL(Cantidad, 0) 
        FROM Stock 
        WHERE ProductoId = @ProductoId;
        
        IF @StockActual < @Cantidad
        BEGIN
            RAISERROR('Stock insuficiente. No hay suficiente inventario disponible.', 16, 1);
            RETURN;
        END
        
        -- Insertar salida
        INSERT INTO SalidasProductos (ProductoId, Cantidad, Motivo, Responsable, Observaciones)
        VALUES (@ProductoId, @Cantidad, @Motivo, @Responsable, @Observaciones);
        
        -- Actualizar stock (restar directamente)
        UPDATE Stock 
        SET 
            Cantidad = @StockActual - @Cantidad,
            FechaUltimaActualizacion = GETUTCDATE()
        WHERE ProductoId = @ProductoId;
        
        COMMIT TRANSACTION;
        
        SELECT SCOPE_IDENTITY() AS Id;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Obtener salidas por rango de fechas
CREATE OR ALTER PROCEDURE sp_ObtenerSalidasPorFecha
    @FechaInicio DATETIME2,
    @FechaFin DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        s.Id,
        s.ProductoId,
        s.Cantidad,
        s.Motivo,
        s.FechaSalida,
        s.Responsable,
        s.Observaciones,
        p.Codigo AS ProductoCodigo,
        p.Nombre AS ProductoNombre,
        p.Precio,
        c.Nombre AS CategoriaNombre,
        (s.Cantidad * p.Precio) AS ValorTotal
    FROM SalidasProductos s
    INNER JOIN Productos p ON s.ProductoId = p.Id
    LEFT JOIN Categorias c ON p.CategoriaId = c.Id
    WHERE s.FechaSalida BETWEEN @FechaInicio AND @FechaFin
    ORDER BY s.FechaSalida DESC;
END
GO

-- =============================================
-- PROCEDIMIENTOS DE REPORTES
-- =============================================

-- Reporte de productos con stock bajo
CREATE OR ALTER PROCEDURE sp_ProductosStockBajo
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.Id,
        p.Codigo,
        p.Nombre,
        p.StockMinimo,
        s.Cantidad AS StockActual,
        c.Nombre AS CategoriaNombre,
        (s.Cantidad * p.Precio) AS ValorInventario
    FROM Productos p
    INNER JOIN Stock s ON p.Id = s.ProductoId
    LEFT JOIN Categorias c ON p.CategoriaId = c.Id
    WHERE p.Activo = 1 AND s.Cantidad <= p.StockMinimo
    ORDER BY s.Cantidad ASC;
END
GO

-- Reporte de productos agotados
CREATE OR ALTER PROCEDURE sp_ProductosAgotados
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.Id,
        p.Codigo,
        p.Nombre,
        p.StockMinimo,
        s.Cantidad AS StockActual,
        c.Nombre AS CategoriaNombre
    FROM Productos p
    INNER JOIN Stock s ON p.Id = s.ProductoId
    LEFT JOIN Categorias c ON p.CategoriaId = c.Id
    WHERE p.Activo = 1 AND s.Cantidad = 0
    ORDER BY p.Nombre;
END
GO

-- =============================================
-- MENSAJE FINAL
-- =============================================

PRINT '=============================================';
PRINT 'PROCEDIMIENTOS ALMACENADOS CREADOS EXITOSAMENTE';
PRINT '=============================================';
PRINT 'Procedimientos creados: 20';
PRINT '- Productos: 5 procedimientos';
PRINT '- Categorías: 2 procedimientos';
PRINT '- Proveedores: 2 procedimientos';
PRINT '- Stock: 2 procedimientos';
PRINT '- Entradas: 2 procedimientos';
PRINT '- Salidas: 2 procedimientos';
PRINT '- Reportes: 2 procedimientos';
PRINT '=============================================';
