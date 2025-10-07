# Sistema de Inventario con Arquitectura Hexagonal

Sistema de gestión de inventario implementando **Arquitectura Hexagonal** con separación clara de capas y **Inyección de Dependencias**.

## Patrones Implementados

- **Arquitectura Hexagonal (Ports & Adapters)**: Separación entre dominio, aplicación e infraestructura
- **Inyección de Dependencias**: Configuración de servicios y repositorios
- **Publish-Subscribe**: Sistema de eventos de dominio para comunicación desacoplada
- **Microservicios**: API REST independiente y aplicación Blazor

## 📁 Estructura del Proyecto

```
ProyectoInventario/
├── Domain/                          # Capa de Dominio
│   └── Domain/Models/               # Entidades: Producto, Stock, Categoria, Proveedor
│   └── Domain/Ports/                # Interfaces: IRepositorio*, IEventBus, IServicio*
│   └── Domain/Events/               # Eventos de dominio: StockActualizadoEvent, etc.
├── Aplication/                      # Capa de Aplicación
│   └── Service/Servicios/           # Servicios de negocio con publicación de eventos
├── Infraestructura/                 # Capa de Infraestructura
│   ├── Api/                         # API REST
│   │   └── Infrastructure/          # Implementaciones de infraestructura
│   │       ├── EventBus/            # InMemoryEventBus
│   │       ├── EventHandlers/       # Manejadores de eventos
│   │       └── Services/            # Servicios de infraestructura
│   └── App/                         # App Blazor
└── Scripts/                         # Scripts de base de datos
    ├── 01_CreateDatabase.sql
    └── 02_StoredProcedures.sql
```

## 🗄️ Base de Datos

**Script**: `Scripts/01_CreateDatabase.sql`
- Tablas: Productos, Stock, Categorias, Proveedores, EntradasProductos, SalidasProductos
- Datos iniciales de ejemplo

## 🚀 Configuración del Entorno

### 📋 Prerrequisitos
- **.NET 8 SDK** instalado
- **SQL Server** (LocalDB, Express o Developer Edition)
- **Visual Studio 2022** o **Visual Studio Code** con extensión C#
- **SQL Server Management Studio (SSMS)** o **Azure Data Studio**

### 📝 1. Configuración de Archivos appsettings

**IMPORTANTE**: Los archivos `appsettings.json` y `appsettings.Development.json` NO están incluidos en el repositorio por razones de seguridad. Debes crearlos manualmente en cada proyecto.

> **Nota**: Estos archivos están excluidos del repositorio mediante `.gitignore` para proteger información sensible como cadenas de conexión y credenciales.

#### Crear appsettings.json para la API REST
**Ubicación**: `Infraestructura/Api/Api/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=InventarioDB;Integrated Security=True;Encrypt=False;"
  },
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "UserName": "tu-email@gmail.com",
    "Password": "tu-password",
    "EnableSsl": true
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

#### Crear appsettings.Development.json para la API REST
**Ubicación**: `Infraestructura/Api/Api/appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=InventarioDB;Integrated Security=True;Encrypt=False;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

#### Crear appsettings.json para la App Blazor
**Ubicación**: `Infraestructura/App/App/App/appsettings.json`

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7133",
    "Timeout": 30,
    "RetryCount": 3
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning",
      "System.Net.Http.HttpClient": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=InventarioDB;Integrated Security=True;Trusted_Connection=True;Encrypt=False;"
  }
}
```

#### Crear appsettings.Development.json para la App Blazor
**Ubicación**: `Infraestructura/App/App/App/appsettings.Development.json`

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7133",
    "Timeout": 30,
    "RetryCount": 3
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore": "Information",
      "System.Net.Http.HttpClient": "Information",
      "AppBlazor": "Debug"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=InventarioDB;Integrated Security=True;Trusted_Connection=True;Encrypt=False;"
  }
}
```

### 🗄️ 2. Configuración de Base de Datos

**IMPORTANTE**: Los scripts deben ejecutarse en el orden especificado para evitar errores de dependencias.

#### Paso 1: Crear la Base de Datos
```sql
-- Ubicación: Scripts/01_CreateDatabase.sql
-- Ejecutar en SQL Server Management Studio
-- Este script crea:
--   - Base de datos "InventarioDB"
--   - Todas las tablas del dominio
--   - Sistema de eventos y notificaciones
--   - Stored procedures y triggers
--   - Datos iniciales de ejemplo
```

#### Paso 2: Configurar Stored Procedures
```sql
-- Ubicación: Scripts/02_StoredProcedures.sql
-- Ejecutar  procedimientos almacenados
-- para optimización de consultas
```

### 🔧 3. Configuración de Proyectos

#### Proyecto API REST
**Ubicación**: `Infraestructura/Api/Api/`

**Configuración**:
1. Abrir `Infraestructura/Api/Api.sln` en Visual Studio
2. Verificar la cadena de conexión en `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=InventarioDB;Integrated Security=True;Encrypt=False;"
     }
   }
   ```
3. Restaurar paquetes NuGet: `dotnet restore`
4. Compilar: `dotnet build`
5. Ejecutar: `dotnet run` o F5 en Visual Studio

**Puerto por defecto**: `https://localhost:7133` o `http://localhost:5162`

#### Proyecto App Blazor
**Ubicación**: `Infraestructura/App/App/`

**Configuración**:
1. Abrir `Infraestructura/App/App.sln` en Visual Studio
2. Verificar la configuración de la API en `appsettings.json`:
   ```json
   {
     "ApiSettings": {
       "BaseUrl": "https://localhost:7133",
       "Timeout": 30,
       "RetryCount": 3
     }
   }
   ```
3. Restaurar paquetes NuGet: `dotnet restore`
4. Compilar: `dotnet build`
5. Ejecutar: `dotnet run` o F5 en Visual Studio

**Puerto por defecto**: `https://localhost:7187` o `http://localhost:5198`

### 🔄 4. Orden de Ejecución

#### Secuencia Correcta:
1. **SQL Server** debe estar ejecutándose
2. **Ejecutar Scripts** (en orden):
   - `Scripts/01_CreateDatabase.sql`
   - `Scripts/02_StoredProcedures.sql` (opcional)
3. **Ejecutar API REST**:
   - Navegar a `Infraestructura/Api/Api/`
   - Ejecutar: `dotnet run`
   - Verificar en: `https://localhost:7133/swagger`
4. **Ejecutar App Blazor**:
   - Navegar a `Infraestructura/App/App/`
   - Ejecutar: `dotnet run`
   - Abrir en navegador: `https://localhost:7187`

### 🛠️ 5. Verificación del Entorno

#### Verificar API REST:
```bash
# Probar endpoint de salud
curl https://localhost:7133/api/inventario/productos

# O abrir en navegador
https://localhost:7133/swagger
```

#### Verificar App Blazor:
- Abrir `https://localhost:7187` en el navegador
- Deberías ver la interfaz de gestión de inventario
- Verificar que se cargan los datos desde la API

### ⚠️ Solución de Problemas Comunes

#### Error de Conexión a Base de Datos:
- Verificar que SQL Server esté ejecutándose
- Comprobar la cadena de conexión en `appsettings.json`
- Asegurar que la base de datos `InventarioDB` existe

#### Error de Conexión API ↔ App Blazor:
- Verificar que la API esté ejecutándose en el puerto correcto
- Comprobar la URL de la API en `appsettings.json` de la App Blazor
- Verificar que no hay conflictos de puertos

#### Error de CORS:
- La API ya tiene configurado CORS para permitir la App Blazor
- Si persiste, verificar la configuración en `Program.cs` de la API

### 🔧 6. Configuración Avanzada

#### Variables de Entorno:
```bash
# Para desarrollo
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__DefaultConnection="Data Source=localhost\\SQLEXPRESS;Initial Catalog=InventarioDB;Integrated Security=True;Encrypt=False;"
```

#### Configuración de Producción:
- Actualizar cadenas de conexión
- Configurar HTTPS
- Configurar logging
- Configurar CORS para dominios específicos

## 📊 Funcionalidades

- **Gestión de Productos**: CRUD completo con validaciones
- **Gestión de Stock**: Control de inventario y movimientos
- **Gestión de Categorías**: Organización de productos
- **Gestión de Proveedores**: Información de proveedores
- **Sistema de Eventos**: Notificaciones automáticas y promociones
- **API REST**: Endpoints para todas las operaciones
- **Interfaz Web**: App Blazor para gestión visual

## 🔄 Sistema de Eventos (Publish-Subscribe)

### Eventos de Dominio Implementados:
- **StockActualizadoEvent**: Se publica cuando cambia la cantidad de stock
- **StockBajoEvent**: Se publica cuando el stock está por debajo del mínimo
- **ProductoAgotadoEvent**: Se publica cuando un producto se agota completamente

### Manejadores de Eventos:
- **StockEventHandler**: Procesa eventos de stock y ejecuta acciones automáticas:
  - Envía notificaciones por email cuando el stock está bajo
  - Crea promociones automáticas para productos con exceso de stock
  - Genera órdenes de compra urgentes para productos agotados

### Servicios de Infraestructura:
- **ServicioNotificacionEmail**: Envío de notificaciones por email
- **ServicioPromocionesExterno**: Gestión de promociones automáticas
- **ServicioProveedoresExterno**: Gestión de órdenes de compra
- **InMemoryEventBus**: Bus de eventos en memoria para desarrollo

## 🛠️ Tecnologías

- **.NET 8**, **Entity Framework Core**, **SQL Server**
- **Blazor Server**, **ASP.NET Core Web API**
- **Arquitectura Hexagonal**, **Patrón Publish-Subscribe**
- **Inyección de Dependencias**, **Event-Driven Architecture**

## 🚀 Ejemplo de Uso del Sistema de Eventos

### Flujo Automático:
1. **Usuario actualiza stock** → Se ejecuta `ServicioInventario.AgregarStockAsync()`
2. **Se publica evento** → `StockActualizadoEvent` se envía al Event Bus
3. **Manejador procesa evento** → `StockEventHandler` recibe el evento
4. **Acciones automáticas**:
   - Si stock < mínimo → Envía notificación por email
   - Si stock > máximo → Crea promoción automática
   - Si stock = 0 → Genera orden de compra urgente

### Beneficios:
- **Desacoplamiento**: Los servicios no conocen quién procesa los eventos
- **Extensibilidad**: Fácil agregar nuevos manejadores sin modificar código existente
- **Escalabilidad**: Preparado para microservicios y sistemas distribuidos
- **Mantenibilidad**: Lógica de negocio separada de infraestructura
