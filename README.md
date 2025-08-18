# Billetera Digital API

API REST construida en **.NET Core** para la gestión de una billetera digital personal.

Permite a cada usuario administrar su saldo, registrar gastos, clasificar categorías y consultar un historial de movimientos, con integración de tipo de cambio en tiempo real.
----------------------------------
## 🚀 **Tecnologías principales**

- [ASP.NET](http://asp.net/) Core 8
- Entity Framework Core
- SQL Server
- JWT Authentication
- FluentValidation
- Swagger/OpenAPI
- Consumo de API externa de tipo de cambio (ExchangeRate-API)
  --------------------------------
  ## ⚙️ **Funcionalidades**

- **Autenticación y autorización** con JWT.
- **CRUD de Saldo:** cargar, actualizar, consultar y eliminar saldo.
- **Registro de Gastos:** registrar gastos, actualizarlos y eliminarlos.
- **Gestión de Categorías:** crear, editar y borrar categorías de gasto.
- **Historial de movimientos:** consulta detallada del historial de ingresos y gastos.
- **Integración API externa:** consulta de tipo de cambio para ver el saldo en múltiples monedas.
- **Validación robusta:** control de entradas con FluentValidation.
- **Rutas protegidas:** todas las operaciones requieren token válido.
  ----------------------------
  ## 📌 **Estructura del proyecto**

/BilleteraApp

├── Controllers.

├── Services.

├── Models.

├── Dtos.

├── Validators.

├── Program.cs.

├── appsettings.json.
----------------------------
## **Requisitos**

- .NET 8 SDK
- SQL Server
- Clave API gratuita de [ExchangeRate-API](https://www.exchangerate-api.com/)
-------------------------
## **Variables de entorno**

Configurar `appsettings.json`.
      {
        "ConnectionStrings": {
          "DefaultConnection": "Server=localhost;Database=BilleteraDb;Trusted_Connection=True;"
        },
        "Jwt": {
          "Key": "TU_CLAVE_SECRETA",
          "Issuer": "TU_ISSUER",
          "Audience": "TU_AUDIENCE"
        },
        "ExchangeRateApiKey": "TU_API_KEY_EXCHANGE"
      }

---------------------------
## **Ejecutar el proyecto**

1. Clonar el repositorio:
bash.
git clone https://github.com/TU_USUARIO/TU_REPO.git

2. Restaurar dependencias:

bash.
dotnet restore.

3.Ejecutar migraciones:
bash.
dotnet ef database update.

4.Correr la aplicación:
bash.
dotnet run.
