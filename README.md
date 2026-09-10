# GestionTarjetaCredito

Sistema de gestión de tarjetas de crédito desarrollado como una solución web completa utilizando ASP.NET Core, arquitectura por capas, CQRS, MediatR, Dapper y SQL Server.

El sistema permite consultar el estado de cuenta de una tarjeta, registrar compras, registrar pagos, consultar movimientos mensuales y administrar parámetros financieros configurables.

---

## Objetivo

Desarrollar una solución para la gestión básica de tarjetas de crédito aplicando buenas prácticas de arquitectura, separación de responsabilidades, validaciones, manejo centralizado de errores y acceso a datos mediante procedimientos almacenados.

---

## Funcionalidades principales

El sistema actualmente permite:

- Consultar el estado de cuenta de una tarjeta.
- Mostrar el titular y número de tarjeta enmascarado.
- Consultar límite de crédito.
- Calcular saldo utilizado.
- Calcular crédito disponible.
- Consultar compras del mes actual.
- Consultar compras del mes anterior.
- Calcular interés bonificable.
- Calcular cuota mínima.
- Calcular pago total.
- Calcular pago con intereses.
- Registrar compras.
- Registrar pagos.
- Validar que una compra no exceda el crédito disponible.
- Validar que un pago no exceda el saldo pendiente.
- Consultar movimientos mensuales.
- Diferenciar compras y pagos.
- Administrar parámetros financieros.
- Modificar el porcentaje de interés.
- Modificar el porcentaje de cuota mínima.
- Manejar errores de forma centralizada.
- Consumir la API desde una aplicación MVC con Razor.

---

## Arquitectura

La solución está dividida en los siguientes proyectos:

```text
GestionTarjetaCredito
│
├── GestionTarjetaCredito.Api
├── GestionTarjetaCredito.Application
├── GestionTarjetaCredito.Domain
├── GestionTarjetaCredito.Infrastructure
├── GestionTarjetaCredito.Mvc
│
└── database
