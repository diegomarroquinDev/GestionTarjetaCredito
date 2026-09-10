# GestionTarjetaCredito

GestionTarjetaCredito es una solución web para la gestión de tarjetas de crédito desarrollada con ASP.NET Core, .NET 6, SQL Server y una arquitectura por capas orientada a buenas prácticas de desarrollo. El sistema permite consultar el estado de cuenta de una tarjeta de crédito, registrar compras, registrar pagos, consultar movimientos mensuales, calcular saldos y administrar parámetros financieros configurables como el porcentaje de interés y el porcentaje de cuota mínima.

La solución fue estructurada en varios proyectos para mantener una adecuada separación de responsabilidades. `GestionTarjetaCredito.Domain` contiene las entidades principales del dominio, como `CardHolder`, `CreditCard`, Transaction y FinancialConfiguration, además del enum TransactionType. GestionTarjetaCredito.Application contiene los casos de uso, DTOs, interfaces, validaciones, excepciones, perfiles de AutoMapper y la implementación del patrón CQRS utilizando MediatR. `GestionTarjetaCredito.Infrastructure` contiene la persistencia, los repositorios, la conexión con SQL Server, Dapper y la implementación de UnitOfWork. `GestionTarjetaCredito.Api` expone los servicios REST mediante ASP.NET Core Web API, Swagger y manejo global de excepciones. Finalmente, `GestionTarjetaCredito.Mvc` contiene la interfaz gráfica desarrollada con ASP.NET Core MVC, Razor, Bootstrap y HttpClient para consumir la API.

Entre las principales funcionalidades del sistema se encuentra la consulta del estado de cuenta de una tarjeta de crédito, mostrando el nombre del titular, el número de tarjeta enmascarado, el límite de crédito, el saldo utilizado, el crédito disponible, las compras realizadas durante el mes actual, las compras del mes anterior, el porcentaje de interés configurado, el interés calculado, la cuota mínima, el pago total y el pago de contado con intereses. El sistema también permite registrar nuevas compras y pagos, aplicando reglas de negocio para impedir que una compra exceda el crédito disponible o que un pago sea mayor al saldo pendiente. Además, permite consultar todos los movimientos de una tarjeta durante un mes determinado, mostrando compras y pagos ordenados desde la fecha más reciente hasta la más antigua.

Los parámetros financieros utilizados por la aplicación no se encuentran definidos directamente en el código fuente. El porcentaje de interés y el porcentaje utilizado para calcular la cuota mínima se almacenan en la tabla `FinancialConfigurations`, lo que permite modificarlos sin necesidad de recompilar la aplicación. Los valores iniciales definidos en los datos de demostración son `INTEREST_PERCENTAGE = 25` y `MIN_PAYMENT_PERCENTAGE = 5`. Estos valores también pueden ser modificados desde la interfaz web mediante la pantalla de configuración financiera.

La aplicación utiliza CQRS para separar claramente las operaciones de consulta de las operaciones que modifican información. Entre las consultas implementadas se encuentran `GetCreditCardStatementQuery` y `GetMonthlyTransactionsQuery`, mientras que entre los comandos se encuentran `CreatePurchaseCommand`, `CreatePaymentCommand` y `UpdateFinancialConfigurationCommand`. MediatR se utiliza como mediador entre los Controllers y los respectivos Handlers. FluentValidation se integra mediante un `ValidationBehavior` dentro del pipeline de MediatR, permitiendo centralizar las validaciones y evitar lógica repetida dentro de los Controllers. AutoMapper se utiliza para convertir entidades del dominio en DTOs, por ejemplo `FinancialConfiguration` a `FinancialConfigurationDto` y `Transaction` a `TransactionDto`.

El acceso a SQL Server se realiza mediante Dapper y `Microsoft.Data.SqlClient`. La solución utiliza el patrón Repository para evitar que la capa Application conozca detalles concretos de persistencia. Las interfaces `ICreditCardRepository`, `ITransactionRepository` e `IFinancialConfigurationRepository` se encuentran definidas en Application, mientras que sus implementaciones se encuentran en Infrastructure. También se implementa UnitOfWork para controlar operaciones transaccionales como el registro de compras y pagos. Cuando se inicia una operación de escritura se crea una transacción SQL y, si la operación se completa correctamente, se ejecuta `Commit`; si ocurre un error, se realiza `Rollback`.

La interacción con la base de datos se realiza mediante procedimientos almacenados. Entre los principales procedimientos utilizados se encuentran `sp_CreditCard_GetById`, `sp_Transaction_GetByCreditCardId`, `sp_Transaction_GetMonthly`, `sp_Transaction_CreatePurchase`, `sp_Transaction_CreatePayment`, `sp_FinancialConfiguration_GetByCode`, `sp_FinancialConfiguration_GetAll` y `sp_FinancialConfiguration_Update`. Las consultas ejecutadas mediante Dapper utilizan parámetros, evitando concatenación directa de valores dentro de instrucciones SQL.

La base de datos utiliza las tablas `CardHolders`, `CreditCards`, `Transactions` y `FinancialConfigurations`. Las relaciones entre las tablas se encuentran protegidas mediante llaves primarias y llaves foráneas, además de restricciones `CHECK`, valores `DEFAULT`, restricciones `UNIQUE` e índices. La tabla `Transactions` utiliza `TransactionType = 1` para compras y `TransactionType = 2` para pagos. Los montos monetarios utilizan tipos `DECIMAL` y las fechas utilizan `DATETIME2`.

Dentro del repositorio se incluye una carpeta llamada `database`, la cual contiene los scripts necesarios para reconstruir completamente la base de datos. Los archivos deben ejecutarse en el siguiente orden: `01_CreateDatabase.sql`, `02_CreateTables.sql`, `03_CreateStoredProcedures.sql` y `04_SeedData.sql`. El primer script crea la base `GestionTarjetaCreditoDb`, el segundo crea las tablas, relaciones, restricciones e índices, el tercero crea los procedimientos almacenados y el cuarto inserta información de demostración. El seed crea una tarjeta de prueba con `CreditCardId = 1`, un titular llamado `Cliente Demostración`, un límite de crédito de `$1,000.00`, configuraciones financieras iniciales y movimientos correspondientes al mes actual y al mes anterior.

Por razones de seguridad, la cadena de conexión real de SQL Server no se almacena dentro del repositorio. Durante el desarrollo se utiliza User Secrets de ASP.NET Core. Para configurarlo en Visual Studio se debe hacer clic derecho sobre `GestionTarjetaCredito.Api` y seleccionar `Administrar secretos de usuario`. Dentro de `secrets.json` se debe configurar una estructura similar a la siguiente:

json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SERVIDOR;Database=GestionTarjetaCreditoDb;User Id=USUARIO;Password=CONTRASEÑA;TrustServerCertificate=True;"
  }
}
Los valores SERVIDOR, USUARIO y CONTRASEÑA deben reemplazarse por los datos correspondientes al entorno local. Las credenciales reales no deben incluirse en GitHub.
El proyecto MVC consume la API mediante HttpClient. La URL base se encuentra configurada en GestionTarjetaCredito.Mvc/appsettings.json mediante la propiedad ApiSettings:BaseUrl. Un ejemplo de configuración es:
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7299/"
  }
}
Si la API utiliza un puerto distinto, solamente debe modificarse ese valor y no es necesario realizar cambios en el código fuente.
Para ejecutar la solución se recomienda utilizar Visual Studio 2022 con el SDK de .NET 6 instalado, SQL Server, SQL Server Management Studio y Git. Primero se debe clonar el repositorio utilizando git clone <URL_DEL_REPOSITORIO> y abrir el archivo GestionTarjetaCredito.sln. Después se deben ejecutar los cuatro scripts de base de datos en el orden especificado, configurar User Secrets con la cadena de conexión correspondiente y verificar que la URL de la API configurada en el MVC coincida con el puerto utilizado por el proyecto API. En Visual Studio se deben configurar GestionTarjetaCredito.Api y GestionTarjetaCredito.Mvc como varios proyectos de inicio para que ambos se ejecuten simultáneamente.
La API cuenta con documentación interactiva mediante Swagger/OpenAPI. Al ejecutar GestionTarjetaCredito.Api se puede acceder a una dirección similar a https://localhost:7299/swagger, aunque el puerto puede variar según el entorno. Entre los principales endpoints se encuentran GET /api/credit-cards/{id}/statement para consultar el estado de cuenta, GET /api/credit-cards/{id}/transactions?year={year}&month={month} para consultar los movimientos mensuales, POST /api/credit-cards/{id}/purchases para registrar compras, POST /api/credit-cards/{id}/payments para registrar pagos, GET /api/configurations para consultar los parámetros financieros y PUT /api/configurations/{id} para modificarlos.
Un ejemplo para registrar una compra es:
{
  "transactionDate": "2026-09-09T14:30:00",
  "description": "Supermercado",
  "amount": 50.00
}
y un ejemplo para registrar un pago es:
{
  "transactionDate": "2026-09-09T15:15:00",
  "amount": 50.00
}
La API implementa un middleware global para el manejo de excepciones, evitando bloques try/catch repetidos dentro de los Controllers. Los errores de validación y reglas de negocio generan respuestas 400 Bad Request, los recursos inexistentes generan 404 Not Found y los errores no controlados generan 500 Internal Server Error. El formato estándar de respuesta de error es similar a:
{
  "status": 400,
  "message": "Mensaje del error",
  "errors": null
}
Cuando se produce un error de FluentValidation, la propiedad errors incluye la lista de campos y mensajes correspondientes.
También se aplican medidas básicas de seguridad y buenas prácticas como el uso de HTTPS durante el desarrollo, User Secrets para evitar publicar credenciales, consultas SQL parametrizadas mediante Dapper, procedimientos almacenados, AntiForgeryToken en los formularios MVC, validación de entradas y enmascaramiento del número de tarjeta. La API no expone el número completo de la tarjeta al frontend; en su lugar se presenta en un formato similar a **** **** **** 4587.
La aplicación web MVC incluye las pantallas de estado de cuenta, registro de compras, registro de pagos, movimientos mensuales y configuración financiera. La interfaz utiliza Razor, Bootstrap y estilos personalizados orientados a una experiencia visual de tipo bancario. El estado de cuenta muestra una representación visual de la tarjeta de crédito junto con los principales indicadores financieros y los cálculos correspondientes.
El flujo general de una consulta es MVC o Swagger → API Controller → MediatR → ValidationBehavior → Query Handler → Repository → Dapper → Stored Procedure → SQL Server. Para las operaciones de compra y pago se incorpora además UnitOfWork para controlar la transacción SQL mediante BeginTransaction, Commit y Rollback.
El proyecto utiliza convenciones consistentes como CreatedDate, UpdatedDate y TransactionDate para los campos de fecha, y el sufijo Async para las operaciones asíncronas como GetByIdAsync, GetMonthlyTransactionsAsync, CreatePurchaseAsync y CreatePaymentAsync.
Este proyecto fue desarrollado como ejercicio de evaluación técnica aplicando ASP.NET Core, SQL Server, arquitectura por capas, principios SOLID, CQRS, MediatR, FluentValidation, AutoMapper, Repository Pattern, UnitOfWork, Dapper, Stored Procedures, Swagger, MVC y buenas prácticas de desarrollo de software.

https://github.com/diegomarroquinDev/GestionTarjetaCredito
