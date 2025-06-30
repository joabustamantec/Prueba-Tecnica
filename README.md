# Proyecto de Prueba Regiones y Comunas

Este repositorio contiene dos proyectos que trabajan juntos:

1. **API REST** (`Proyecto.API`)  
2. **Aplicación MVC** (`Proyecto.WebMVC`)

---

## Requisitos

- .NET 8 SDK (o superior)
- SQL Server 2012 (o superior) con la base de datos `ProyectoBD` y los _stored procedures_ ya creados.
- Visual Studio 2022 (o tu IDE preferido).

---

## 1. Ejecutar la API

1. Abre la solución en tu IDE y selecciona como proyecto de inicio **`Proyecto.API`**.
2. Asegúrate de que la _connection string_ en `appsettings.json` apunte a tu servidor de SQL Server.
3. Ejecuta la API (F5 o `dotnet run`).  
   Por defecto levantará en un puerto como `https://localhost:7213/`.

## 2. Ejecutar la aplicación MVC

1. Abre la solución y selecciona **`Proyecto.WebMVC`** como proyecto de inicio.
2. Antes de arrancar, ve al archivo `Program.cs` en la raíz de `Proyecto.WebMVC` y localiza la línea:

   
si tu puerto no es `https://localhost:7213/` hice una linea de conexion simple en el archivo program.cs en la linea 6 donde solo tienes que cambiarla por la tuya.
   ```csharp
   client.BaseAddress = new Uri("https://localhost:7213/"); // Cambia esto si usas otro puerto

