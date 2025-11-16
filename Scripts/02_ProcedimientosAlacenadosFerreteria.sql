USE FerreteriaDB;



-------------------------------
--      Categorias
-------------------------------

-- INSERT
GO

CREATE OR ALTER PROCEDURE sp_Categorias_Insert
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(250) = NULL
AS
BEGIN
    INSERT INTO Categorias (Nombre, Descripcion)
    VALUES (@Nombre, @Descripcion);
    SELECT SCOPE_IDENTITY() AS IdCategoria;
END;
GO

-- SELECT ALL
CREATE OR ALTER PROCEDURE sp_Categorias_GetAll
AS
BEGIN
    SELECT * FROM Categorias;
END;
GO

CREATE OR ALTER PROCEDURE sp_Categorias_GetActive
AS
BEGIN
    SELECT * FROM Categorias
    WHERE Activo = 1;
END;

GO

-- SELECT BY ID
CREATE OR ALTER PROCEDURE sp_Categorias_GetById
    @IdCategoria INT
AS
BEGIN
    SELECT * FROM Categorias WHERE IdCategoria = @IdCategoria;
END;
GO

-- UPDATE
CREATE OR ALTER PROCEDURE sp_Categorias_Update
    @IdCategoria INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(250) = NULL
AS
BEGIN
    UPDATE Categorias
    SET Nombre = @Nombre,
        Descripcion = @Descripcion
    WHERE IdCategoria = @IdCategoria;
END;
GO

-- DELETE
CREATE OR ALTER PROCEDURE sp_Categorias_Delete
    @IdCategoria INT
AS
BEGIN
    DELETE FROM Categorias WHERE IdCategoria = @IdCategoria;
END;
GO

-------------------------------
--      Productos
-------------------------------

CREATE OR ALTER PROCEDURE sp_Productos_Insert
    @Nombre NVARCHAR(150),
    @Descripcion NVARCHAR(300) = NULL,
    @PrecioCompra DECIMAL(10,2),
    @PrecioVenta DECIMAL(10,2),
    @StockActual INT,
    @StockMinimo INT,
    @IdCategoria INT
AS
BEGIN
    INSERT INTO Productos (Nombre, Descripcion, PrecioCompra, PrecioVenta, StockActual, StockMinimo, IdCategoria)
    VALUES (@Nombre, @Descripcion, @PrecioCompra, @PrecioVenta, @StockActual, @StockMinimo, @IdCategoria);
    SELECT SCOPE_IDENTITY() AS IdProducto;
END;
GO

CREATE OR ALTER PROCEDURE sp_Productos_GetAll
AS
BEGIN
    SELECT p.*, c.Nombre AS Categoria
    FROM Productos p
    INNER JOIN Categorias c ON p.IdCategoria = c.IdCategoria;
END;

GO

CREATE OR ALTER PROCEDURE sp_Productos_GetActive
AS
BEGIN
    SELECT p.*, c.Nombre AS Categoria
    FROM Productos p
    INNER JOIN Categorias c ON p.IdCategoria = c.IdCategoria
    WHERE P.Activo = 1;
END;

GO

CREATE OR ALTER PROCEDURE sp_Productos_GetById
    @IdProducto INT
AS
BEGIN
    SELECT * FROM Productos WHERE IdProducto = @IdProducto;
END;
GO

CREATE OR ALTER PROCEDURE sp_Productos_Update
    @IdProducto INT,
    @Nombre NVARCHAR(150),
    @Descripcion NVARCHAR(300),
    @PrecioCompra DECIMAL(10,2),
    @PrecioVenta DECIMAL(10,2),
    @StockActual INT,
    @StockMinimo INT,
    @IdCategoria INT,
    @Activo BIT
AS
BEGIN
    UPDATE Productos
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        PrecioCompra = @PrecioCompra,
        PrecioVenta = @PrecioVenta,
        StockActual = @StockActual,
        StockMinimo = @StockMinimo,
        IdCategoria = @IdCategoria,
        Activo = @Activo
    WHERE IdProducto = @IdProducto;
END;
GO

CREATE OR ALTER PROCEDURE sp_Productos_Delete
    @IdProducto INT
AS
BEGIN
    DELETE FROM Productos WHERE IdProducto = @IdProducto;
END;
GO

-------------------------------
--      Proveedores
-------------------------------

CREATE OR ALTER PROCEDURE sp_Proveedores_Insert
    @Nombre NVARCHAR(150),
    @Telefono NVARCHAR(20),
    @Email NVARCHAR(100),
    @Direccion NVARCHAR(250)
AS
BEGIN
    INSERT INTO Proveedores (Nombre, Telefono, Email, Direccion)
    VALUES (@Nombre, @Telefono, @Email, @Direccion);
    SELECT SCOPE_IDENTITY() AS IdProveedor;
END;
GO

CREATE OR ALTER PROCEDURE sp_Proveedores_GetAll
AS
BEGIN
    SELECT * FROM Proveedores;
END;
GO

CREATE OR ALTER PROCEDURE sp_Proveedores_GetById
    @IdProveedor INT
AS
BEGIN
    SELECT * FROM Proveedores WHERE IdProveedor = @IdProveedor;
END;
GO

CREATE OR ALTER PROCEDURE sp_Proveedores_Update
    @IdProveedor INT,
    @Nombre NVARCHAR(150),
    @Telefono NVARCHAR(20),
    @Email NVARCHAR(100),
    @Direccion NVARCHAR(250)
AS
BEGIN
    UPDATE Proveedores
    SET Nombre = @Nombre,
        Telefono = @Telefono,
        Email = @Email,
        Direccion = @Direccion
    WHERE IdProveedor = @IdProveedor;

    SELECT IdProveedor FROM Proveedores
    WHERE IdProveedor = @IdProveedor;
END;
GO

CREATE OR ALTER PROCEDURE sp_Proveedores_Delete
    @IdProveedor INT
AS
BEGIN
    DELETE FROM Proveedores WHERE IdProveedor = @IdProveedor;
END;
GO


-------------------------------
--      Clientes
-------------------------------

CREATE OR ALTER PROCEDURE sp_Clientes_Insert
    @Nombre NVARCHAR(150),
    @Telefono NVARCHAR(20),
    @Email NVARCHAR(100),
    @Direccion NVARCHAR(250)
AS
BEGIN
    INSERT INTO Clientes (Nombre, Telefono, Email, Direccion)
    VALUES (@Nombre, @Telefono, @Email, @Direccion);
    SELECT SCOPE_IDENTITY() AS IdCliente;
END;
GO

CREATE OR ALTER PROCEDURE sp_Clientes_GetAll
AS
BEGIN
    SELECT * FROM Clientes;
END;
GO

CREATE OR ALTER PROCEDURE sp_Clientes_GetById
    @IdCliente INT
AS
BEGIN
    SELECT * FROM Clientes WHERE IdCliente = @IdCliente;
END;
GO

CREATE OR ALTER PROCEDURE sp_Clientes_Update
    @IdCliente INT,
    @Nombre NVARCHAR(150),
    @Telefono NVARCHAR(20),
    @Email NVARCHAR(100),
    @Direccion NVARCHAR(250)
AS
BEGIN
    UPDATE Clientes
    SET Nombre = @Nombre,
        Telefono = @Telefono,
        Email = @Email,
        Direccion = @Direccion
    WHERE IdCliente = @IdCliente;

    SELECT IdCliente FROM Cliente
    WHERE IdCliente = @IdCliente;
END;
GO

CREATE OR ALTER PROCEDURE sp_Clientes_Delete
    @IdCliente INT
AS
BEGIN
    DELETE FROM Clientes WHERE IdCliente = @IdCliente;
END;
GO


-------------------------------
--      Empleados
-------------------------------

CREATE OR ALTER PROCEDURE sp_Empleados_Insert
    @Nombre NVARCHAR(150),
    @Cargo NVARCHAR(50),
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(20)
AS
BEGIN
    INSERT INTO Empleados (Nombre, Cargo, Email, Telefono)
    VALUES (@Nombre, @Cargo, @Email, @Telefono);
    SELECT SCOPE_IDENTITY() AS IdEmpleado;
END;
GO

CREATE OR ALTER PROCEDURE sp_Empleados_GetAll
AS
BEGIN
    SELECT * FROM Empleados;
END;

GO

CREATE OR ALTER PROCEDURE sp_Empleados_GetActive
AS
BEGIN
    SELECT * FROM Empleados
    WHERE Activo = 1;
END;

GO

CREATE OR ALTER PROCEDURE sp_Empleados_GetById
    @IdEmpleado INT
AS
BEGIN
    SELECT * FROM Empleados WHERE IdEmpleado = @IdEmpleado;
END;
GO

CREATE OR ALTER PROCEDURE sp_Empleados_Update
    @IdEmpleado INT,
    @Nombre NVARCHAR(150),
    @Cargo NVARCHAR(50),
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @Activo BIT
AS
BEGIN
    UPDATE Empleados
    SET Nombre = @Nombre,
        Cargo = @Cargo,
        Email = @Email,
        Telefono = @Telefono,
        Activo = @Activo
    WHERE IdEmpleado = @IdEmpleado;
END;
GO

CREATE OR ALTER PROCEDURE sp_Empleados_Delete
    @IdEmpleado INT
AS
BEGIN
    DELETE FROM Empleados WHERE IdEmpleado = @IdEmpleado;
END;
GO

-------------------------------
--      Pedidos y detalle pedidos
-------------------------------

CREATE OR ALTER PROCEDURE sp_Pedidos_Insert
    @IdProveedor INT
AS
BEGIN
    INSERT INTO Pedidos (IdProveedor)
    VALUES (@IdProveedor);
    SELECT SCOPE_IDENTITY() AS IdPedido;
END;
GO

CREATE OR ALTER PROCEDURE sp_Pedidos_GetAll
AS
BEGIN
    SELECT p.*, pr.Nombre AS Proveedor
    FROM Pedidos p
    INNER JOIN Proveedores pr ON p.IdProveedor = pr.IdProveedor;
END;
GO

CREATE OR ALTER PROCEDURE sp_Pedidos_GetById
    @IdPedido INT
AS
BEGIN
    SELECT * FROM Pedidos WHERE IdPedido = @IdPedido;
END;
GO

CREATE OR ALTER PROCEDURE sp_Pedidos_Update
    @IdPedido INT,
    @Estado NVARCHAR(50),
    @Total DECIMAL(12,2)
AS
BEGIN
    UPDATE Pedidos
    SET Estado = @Estado,
        Total = @Total
    WHERE IdPedido = @IdPedido;
END;
GO

CREATE OR ALTER PROCEDURE sp_Pedidos_Delete
    @IdPedido INT
AS
BEGIN
    DELETE FROM Pedidos WHERE IdPedido = @IdPedido;
END;
GO

-- DetallePedidos
CREATE OR ALTER PROCEDURE sp_DetallePedidos_Insert
    @IdPedido INT,
    @IdProducto INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2)
AS
BEGIN
    INSERT INTO DetallePedidos (IdPedido, IdProducto, Cantidad, PrecioUnitario)
    VALUES (@IdPedido, @IdProducto, @Cantidad, @PrecioUnitario);
END;
GO

-------------------------------
--      Facturas, detalleFacturas y pagos
-------------------------------

CREATE OR ALTER PROCEDURE sp_Facturas_Insert
    @IdCliente INT,
    @IdEmpleado INT
AS
BEGIN
    INSERT INTO Facturas (IdCliente, IdEmpleado)
    VALUES (@IdCliente, @IdEmpleado);
    SELECT SCOPE_IDENTITY() AS IdFactura;
END;
GO

CREATE OR ALTER PROCEDURE sp_Facturas_GetAll
AS
BEGIN
    SELECT f.*, c.Nombre AS Cliente, e.Nombre AS Empleado
    FROM Facturas f
    INNER JOIN Clientes c ON f.IdCliente = c.IdCliente
    INNER JOIN Empleados e ON f.IdEmpleado = e.IdEmpleado;
END;
GO

CREATE OR ALTER PROCEDURE sp_Facturas_GetById
    @IdFactura INT
AS
BEGIN
    SELECT f.*, c.Nombre AS Cliente, e.Nombre AS Empleado
    FROM Facturas f
    INNER JOIN Clientes c ON f.IdCliente = c.IdCliente
    INNER JOIN Empleados e ON f.IdEmpleado = e.IdEmpleado
    WHERE IdFactura = @IdFactura;
END;
GO

CREATE OR ALTER PROCEDURE sp_Facturas_Update
    @IdFactura INT,
    @Total DECIMAL(12,2),
    @Estado NVARCHAR(50)
AS
BEGIN
    UPDATE Facturas
    SET Total = @Total,
        Estado = @Estado
    WHERE IdFactura = @IdFactura;
END;
GO

CREATE OR ALTER PROCEDURE sp_Facturas_Delete
    @IdFactura INT
AS
BEGIN
    DELETE FROM Facturas WHERE IdFactura = @IdFactura;
END;

GO
CREATE OR ALTER PROCEDURE dbo.sp_DetalleFacturas_Insert
    @IdFactura INT,
    @IdProducto INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2) = NULL  -- <-- puede venir nulo
AS
BEGIN
    SET NOCOUNT ON;

    -- Si no se especifica el precio, obtenerlo del producto
    IF @PrecioUnitario IS NULL
    BEGIN
        SELECT @PrecioUnitario = P.PrecioVenta
        FROM Productos P
        WHERE P.IdProducto = @IdProducto;
    END;

    -- Insertar el detalle
    INSERT INTO DetalleFacturas (IdFactura, IdProducto, Cantidad, PrecioUnitario)
    VALUES (@IdFactura, @IdProducto, @Cantidad, @PrecioUnitario);

    -- Recalcular el total de la factura con todos sus detalles
    UPDATE F
    SET F.Total = COALESCE((
        SELECT SUM(DF.Cantidad * DF.PrecioUnitario)
        FROM DetalleFacturas DF
        WHERE DF.IdFactura = F.IdFactura
    ), 0)
    FROM Facturas F
    WHERE F.IdFactura = @IdFactura;
END;

GO

-- DetalleFacturas
CREATE OR ALTER PROCEDURE sp_DetalleFacturas_GetAll
AS
BEGIN
    SELECT *, P.Nombre AS NombreProducto FROM DetalleFacturas D
    INNER JOIN Productos P ON P.IdProducto = D.IdProducto    
END;

GO

-- DetalleFacturas
CREATE OR ALTER PROCEDURE sp_DetalleFacturas_GetByIdFactura
    @IdFactura INT
AS
BEGIN
    SELECT D.*, P.Nombre AS NombreProducto FROM DetalleFacturas D
    INNER JOIN Productos P ON D.IdProducto = P.IdProducto
    WHERE IdFactura = @IdFactura
END;

GO

-- Pagos
CREATE OR ALTER PROCEDURE sp_Pagos_Insert
    @IdFactura INT,
    @Monto DECIMAL(12,2),
    @MetodoPago NVARCHAR(50)
AS
BEGIN
    INSERT INTO Pagos (IdFactura, Monto, MetodoPago)
    VALUES (@IdFactura, @Monto, @MetodoPago);
END;
GO

CREATE OR ALTER PROCEDURE sp_Pagos_GetAll
AS
BEGIN
    SELECT p.*, f.IdCliente, f.Total
    FROM Pagos p
    INNER JOIN Facturas f ON p.IdFactura = f.IdFactura;
END;
GO
