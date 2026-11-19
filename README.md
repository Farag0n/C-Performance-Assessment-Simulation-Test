# ✈️ C# Test MVC — Sistema de Gestión de Vuelos

## 🧩 Descripción

Proyecto de simulacro desarrollado en **C# con ASP.NET Core MVC** para practicar la gestión de vuelos, usuarios y reservas con **POO, LINQ y Entity Framework Core**. La aplicación usa vistas Razor para la interfaz web, MySQL como base de datos y QuestPDF para generar los tiquetes de reserva.

## ⚙️ Requisitos previos

| Herramienta | Versión / detalle |
| --- | --- |
| **.NET SDK** | 9.0 |
| **Base de datos** | MySQL compatible con Pomelo.EntityFrameworkCore.MySql 9 |
| **IDE** | Visual Studio, Visual Studio Code o JetBrains Rider |
| **Navegador** | Chrome, Edge o Firefox |

El proyecto ya declara Entity Framework Core 9, su proveedor MySQL y las bibliotecas de PDF en `SkyTravel.csproj`; no es necesario agregarlas manualmente.

## 🧠 Cómo funciona

La aplicación sigue el patrón MVC:

1. `Program.cs` registra las vistas MVC y `AppDbContext` en el contenedor de dependencias, configura MySQL y define la ruta inicial como `Principal/Index`.
2. Los controladores reciben `AppDbContext` por inyección de dependencias y ejecutan las operaciones de consulta y persistencia con Entity Framework Core.
3. Las entidades `Flight`, `User` y `ReservFlight` representan los vuelos, pasajeros y reservas. Las relaciones de una reserva con su vuelo y usuario se resuelven mediante sus claves foráneas.
4. Las vistas Razor en `Views/` muestran formularios y listados; los archivos estáticos (CSS, JavaScript e imágenes) se sirven desde `wwwroot/`.

### Funcionalidades y reglas que implementa el código

- **Vuelos:** listar, crear, editar y eliminar.
- **Usuarios:** listar, crear, editar y eliminar; al crear, se valida que el documento no esté repetido.
- **Reservas:** crear, listar, editar y eliminar; se impide que un usuario tenga más de una reserva y se exige que el vuelo salga dentro de 20 días o más.
- **Tiquetes:** generar un PDF con los datos del pasajero, el vuelo, la fecha y el estado del pago.

> La regla implementada en el código es de **20 días** (no 30). La capacidad del vuelo está modelada como texto y al eliminar una reserva se incrementa si ese valor se puede convertir a número; crear una reserva no decrementa la capacidad.

## 🚀 Cómo ejecutar el proyecto

Los siguientes comandos están escritos para PowerShell y se ejecutan desde la raíz del repositorio:

1. **Clonar el repositorio y entrar al proyecto**

   ```powershell
   git clone https://github.com/Farag0n/C-Performance-Assessment-Simulation-Test.git
   cd C-Performance-Assessment-Simulation-Test\SkyTravel
   ```

2. **Configurar MySQL**

   Inicia MySQL y crea una base de datos vacía llamada `sky_travel`. Configura la cadena de conexión local con User Secrets para no guardar contraseñas en archivos versionados:

   ```powershell
   $env:ASPNETCORE_ENVIRONMENT = "Development"
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "server=localhost;database=sky_travel;user=TU_USUARIO;password=TU_CLAVE;"
   ```

   Sustituye `TU_USUARIO` y `TU_CLAVE` por las credenciales locales de MySQL. Mantén esta variable de entorno en la terminal donde ejecutarás los comandos de Entity Framework para que se carguen los User Secrets.

3. **Restaurar paquetes y preparar Entity Framework**

   ```powershell
   dotnet restore
   dotnet tool install --global dotnet-ef --version 9.0.9
   ```

   Si ya tienes `dotnet-ef` instalado, actualízalo a la versión compatible con EF Core 9:

   ```powershell
   dotnet tool update --global dotnet-ef --version 9.0.9
   ```

4. **Crear el esquema y aplicar la migración**

   Este repositorio no incluye migraciones iniciales. Para una base de datos nueva:

   ```powershell
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

   Si tu base de datos ya contiene tablas o datos, no generes ni apliques una migración inicial sin revisar primero cómo coincide el esquema existente con los modelos.

5. **Ejecutar la aplicación**

   ```powershell
   dotnet watch run
   ```

   Abre la URL que aparece en la terminal (con el perfil HTTP configurado normalmente es `http://localhost:5069`). La aplicación necesita que MySQL esté disponible al arrancar.

## 🧱 Estructura del proyecto

```text
SkyTravel/
├── Controllers/       # Acciones MVC para vuelos, usuarios y reservas
├── Infraestructure/   # AppDbContext y acceso a datos con EF Core
├── Models/            # Entidades Flight, User y ReservFlight
├── Views/             # Vistas Razor organizadas por controlador
├── Properties/        # Perfiles locales de ejecución
├── wwwroot/           # CSS, JavaScript, Bootstrap y otros archivos estáticos
├── appsettings.json   # Configuración no secreta
├── Program.cs         # Inicio, servicios, base de datos y rutas
└── SkyTravel.csproj   # Target .NET 9 y dependencias NuGet
```

## 📊 Diagramas

### 🧩 Diagrama de clases

*(Inserte aquí el enlace o imagen del diagrama de clases)*

> Ejemplo: `![Diagrama de Clases](./Diagrams/Diagrama_Clases.png)`

### 🧩 Diagrama de casos de uso

*(Inserte aquí el enlace o imagen del diagrama de casos de uso)*

> Ejemplo: `![Diagrama de Casos de Uso](./Diagrams/Diagrama_CasosDeUso.png)`

## 🧮 Tecnologías utilizadas

- **C# / ASP.NET Core MVC / Razor**
- **Entity Framework Core 9 / LINQ**
- **MySQL con Pomelo**
- **QuestPDF**
- **Bootstrap**

## 👤 Autor

**Nombre:** Miguel Angel Angarita

**Proyecto:** Simulacro de prueba de desempeño — C# MVC

**Correo:** [faragon@example.com](mailto:faragon@example.com)

**Fecha:** Octubre 2025

---

> ☕ “El buen código es como un buen espresso: concentrado, potente y mejor si se sirve caliente.”
> — *J.A.R.V.I.S., asistente personal de Faragon*
