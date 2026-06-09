# Proyecto-Web-Q2
Proyecto Web API (ASP.NET Core / .NET) — repositorio de desarrollo del grupo.

## Estructura del proyecto
```
Proyecto-Web-Q2/
├── Controllers/   # Controladores de la API
├── Models/        # Modelos de dominio
├── DTOs/          # Objetos de transferencia de datos
├── Services/      # Lógica de negocio / servicios
├── Config/        # Configuración / credenciales (ignorada por Git)
├── Program.cs     # Punto de entrada
└── .gitignore
```
> Las carpetas vacías se conservan con un archivo `.gitkeep` (Git ignora carpetas vacías).

## Ramas
- `main` — versión estable (protegida, requiere 2 aprobaciones para mergear).
- `develop` — rama de desarrollo.

## Ejecutar
```bash
dotnet run
```

## Restaurar Dependencias(Packages)
``` bash
dotnet restore
```

## Miembros del Equipo No.6

| # | Nombre | GitHub | Rol |
|---|--------|--------|-----|
| 1 | Oscar Yahir Oliva Hernandez | [@Olivaaa8](https://github.com/Olivaaa8) | Líder |
| 2 | Anthony David Dubon Sarmiento | [@AnthonyDubon](https://github.com/AnthonyDubon) | Miembro |
| 3 | Victor Neptali Orellana Carranza | [@victorore2006-lab](https://github.com/victorore2006-lab) | Miembro |
| 4 | Raul Isaac Moncada Vasquez | [@numbpi](https://github.com/numbpi) | Miembro |

## Paquetes NuGet

| Paquete | Versión |
|---------|---------|
| `Google.Cloud.Firestore` | `4.2.0` |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | `10.0.8` |
| `Microsoft.AspNetCore.OpenApi` | `10.0.7` |
| `Scalar.AspNetCore` | `2.14.11` |

