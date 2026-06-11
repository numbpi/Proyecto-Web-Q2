# Proyecto-Web-Q2
Backend del proyecto de clase con .NET

## Estructura del backend
```
backend/
├── Controllers/   # Controladores de la API
├── Models/        # Modelos de dominio
├── DTOs/          # Objetos de transferencia de datos
├── Services/      # Lógica de negocio / servicios
├── Config/        # credenciales (ignorada por Git)
├── Program.cs     # Punto de entrada
└── .gitignore
```
> Las carpetas vacías se conservan con un archivo `.gitkeep` (Git ignora carpetas vacías).

## Ejecutar
```bash
dotnet run
```

## Restaurar Dependencias(Packages)
``` bash
dotnet restore
```

## Paquetes NuGet

| Paquete | Versión |
|---------|---------|
| `Google.Cloud.Firestore` | `4.2.0` |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | `10.0.8` |
| `Microsoft.AspNetCore.OpenApi` | `10.0.7` |
| `Scalar.AspNetCore` | `2.14.11` |

