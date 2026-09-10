# GestionTarjetaCredito

Aplicación web para la gestión de tarjetas de crédito desarrollada con **.NET 6, ASP.NET Core Web API, MVC, Razor, SQL Server y Dapper**.

El sistema permite consultar el estado de cuenta, registrar compras y pagos, consultar movimientos mensuales, administrar parámetros financieros y exportar información a Excel y PDF.

## Funcionalidades

- Consulta de estado de cuenta.
- Cálculo de saldo utilizado y disponible.
- Cálculo de cuota mínima e interés bonificable.
- Registro de compras.
- Registro de pagos.
- Consulta de movimientos mensuales.
- Configuración de porcentaje de interés y pago mínimo.
- Exportación de movimientos a Excel.
- Exportación del estado de cuenta a PDF.
- Manejo global de errores y validaciones.

## Arquitectura

La solución está dividida en los siguientes proyectos:

GestionTarjetaCredito.Api
GestionTarjetaCredito.Application
GestionTarjetaCredito.Domain
GestionTarjetaCredito.Infrastructure
GestionTarjetaCredito.Mvc

Se aplican los siguientes patrones y prácticas:
- Arquitectura por capas.
- CQRS con MediatR.
- Repository Pattern.
- Unit of Work.
- FluentValidation.
- AutoMapper.
- DTOs para la API.
- ViewModels para MVC.
- Dependency Injection.
- Stored Procedures con SQL Server.
- Swagger / OpenAPI.
Tecnologías
- .NET 6
- C#
- ASP.NET Core Web API
- ASP.NET Core MVC
- Razor
- SQL Server
- Dapper
- MediatR
- FluentValidation
- AutoMapper
- Swagger
- ClosedXML
- QuestPDF
- Bootstrap

  Base de datos
Los scripts se encuentran en la carpeta:
database
Ejecutar en el siguiente orden:
01_CreateDatabase.sql
02_CreateTables.sql
03_CreateStoredProcedures.sql
04_SeedData.sql
La base de datos contiene las tablas principales:
CardHolders
CreditCards
Transactions
FinancialConfigurations

Configuración
La cadena de conexión se administra mediante User Secrets en el proyecto:
GestionTarjetaCredito.Api
Ejemplo:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SERVIDOR;Database=GestionTarjetaCreditoDb;User Id=USUARIO;Password=CONTRASEÑA;TrustServerCertificate=True;"
  }
}
La URL de la API utilizada por el MVC se configura en:
GestionTarjetaCredito.Mvc/appsettings.json
Ejemplo:
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7299/"
  }
}
Ejecución
En Visual Studio configurar como proyectos de inicio:
GestionTarjetaCredito.Api
GestionTarjetaCredito.Mvc

Ejecutar ambos proyectos simultáneamente.
Swagger estará disponible en una URL similar a:
https://localhost:7299/swagger

Endpoints principales
GET /api/credit-cards/{id}/statement
GET /api/credit-cards/{id}/transactions?year={year}&month={month}
POST /api/credit-cards/{id}/purchases
POST /api/credit-cards/{id}/payments
GET /api/configurations
PUT /api/configurations/{id}

Postman
La colección de Postman se encuentra en:
postman/GestionTarjetaCredito.postman_collection.json

Datos de prueba
El script 04_SeedData.sql crea una tarjeta de demostración con:
CreditCardId: 1
Titular: Cliente Demostración
Límite de crédito: $1,000.00

Los valores financieros iniciales son:
INTEREST_PERCENTAGE = 25
MIN_PAYMENT_PERCENTAGE = 5

Seguridad
- Credenciales fuera del repositorio mediante User Secrets.
- Número de tarjeta enmascarado.
- Consultas parametrizadas.
- Validaciones con FluentValidation.
- Manejo centralizado de excepciones.
- AntiForgeryToken en formularios MVC.

  
Colección y documentación
El repositorio incluye:
- Código fuente completo.
- Scripts de base de datos.
- Colección de Postman.
- Swagger.
- Documentación de arquitectura y ejecución.
Proyecto desarrollado como evaluación técnica utilizando buenas prácticas de desarrollo en .NET.
