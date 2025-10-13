# ✈️ C# Test MVC — Sistema de Gestión de Vuelos

## 🧩 Descripción

Proyecto de simulacro desarrollado en **C# con ASP.NET MVC**, creado como preparación para la **prueba de desempeño del módulo de C#**.
El sistema implementa una estructura MVC funcional para la **gestión de vuelos, usuarios y reservas**, aplicando principios de **POO, LINQ, Entity Framework Core** y conexión a base de datos mediante **DefaultConnection**.

---

## ⚙️ Requisitos Previos

| Herramienta       | Versión Recomendada                                  |
| ----------------- | ---------------------------------------------------- |
| **.NET SDK**      | 8.0 o superior                                       |
| **IDE**           | Visual Studio / Visual Studio Code / JetBrains Rider |
| **Base de Datos** | MySQL o PostgreSQL                                   |
| **ORM**           | Entity Framework Core                                |
| **Navegador**     | Chrome, Edge o Firefox (última versión)              |

> 🧠 **Nota:** Antes de ejecutar el proyecto, asegúrese de tener configurada correctamente la cadena de conexión en la variable **`DefaultConnection`** dentro del archivo `appsettings.json`, con los datos de su base de datos.

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=skytravel_db;user=root;password=tu_contraseña;"
}
```

---

## 🚀 Cómo ejecutar el proyecto

1. **Clonar el repositorio**

   ```bash
   git clone "link del repo"
   #Despues de clonarlo no olvides abrir el proyecto en tu entorno de desarrollo y habre la terminal
   ```

2. **Instala dependencias**

   ```bash
   #Esto se puede hacer desde entorno visual
   # Instalar Entity Framework Core
    dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
    
    # Instalar las herramientas de diseño (para migrations)
    dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
    
    # Instalar el proveedor MySQL con Pomelo
    dotnet add package Pomelo.EntityFrameworkCore.MySql --version 9.0.0
   ```

3. **Restaurar dependencias**

   ```bash
   dotnet restore
   ```

4. **Aplicar las migraciones a la base de datos**

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Ejecutar el proyecto**

   ```bash
   dotnet watch run
   ```

---

## 🧱 Estructura del Proyecto

```
├── Controllers
│   ├── FlightController.cs
│   ├── UserController.cs
│   ├── PrincipalController.cs
│   ├── HomeController.cs
│   └── ReservFlightController.cs
│
├── Infraestructure
│   └── AppDbContext.cs
│
├── Models
│   ├── Flight.cs
│   ├── ReservFlight.cs
│   └── User.cs
│
├── Views
│   ├── Flight
│   ├── Passenger
│   └── Reservation
│
├── wwwroot
│   └── css / js / img
│
└── Program.cs
```

---

## 📊 Diagramas

### 🧩 Diagrama de Clases

*(Inserte aquí el enlace o imagen del diagrama de clases)*

> Ejemplo:
> ![Diagrama de Clases](./Diagrams/Diagrama_Clases.png)

### 🧩 Diagrama de Casos de Uso

*(Inserte aquí el enlace o imagen del diagrama de casos de uso)*

> Ejemplo:
> ![Diagrama de Casos de Uso](./Diagrams/Diagrama_CasosDeUso.png)

---

## 🧠 Funcionalidades Clave

* **Gestión de Vuelos:** Registrar, editar, eliminar y listar vuelos.
* **Gestión de Usuarios:** Registro y administración de pasajeros.
* **Gestión de Reservas:** Crear, cancelar o finalizar reservas con validaciones de disponibilidad.
* **Validaciones de negocio:**

  * Códigos de vuelo y documentos únicos.
  * Reservas dentro del rango de 30 días.
  * Control de asientos disponibles.

---

## 🧮 Tecnologías Utilizadas

* **C# / ASP.NET MVC**
* **Entity Framework Core**
* **LINQ**
* **MySQL / PostgreSQL**
* **Bootstrap / Razor Views**

---

## 👤 Autor

**Nombre:** Miguel Angel Angarita
**Proyecto:** Simulacro de prueba de desempeño — C# MVC
**Correo:** [faragon@example.com](mailto:faragon@example.com)
**Fecha:** Octubre 2025

---

> ☕ “El buen código es como un buen espresso: concentrado, potente y mejor si se sirve caliente.”
> — *J.A.R.V.I.S., asistente personal de Faragon*


