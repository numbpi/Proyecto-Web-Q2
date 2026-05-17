# Entrega — Tarea: Repositorio + Reglas de Seguridad

## Repositorio

- **URL:** https://github.com/keven-bardales/Proyecto-Web-Q2
- En este repositorio se llevará el historial de desarrollo del proyecto.

## Estructura del proyecto

Proyecto ASP.NET Core Web API (.NET). Carpetas creadas según lo que necesita el
proyecto (las carpetas vacías se conservan con `.gitkeep` porque Git ignora
carpetas vacías):

- `Controllers/` — controladores de la API
- `Models/` — modelos de dominio
- `DTOs/` — objetos de transferencia de datos
- `Services/` — lógica de negocio
- `Config/` — configuración / credenciales (ignorada por `.gitignore`)

## .gitignore

Tomado del repositorio de clase y adaptado para .NET. Evita subir binarios,
configuración sensible y credenciales:

```
bin/
obj/
Config/*
!Config/.gitkeep
appsettings.json
appsettings.Development.json
```

## Ramas

- `main` — versión estable (protegida).
- `develop` — rama de desarrollo creada a partir de `main` para trabajar sin
  afectar la versión estable.

## Reglas de seguridad (branch protection en `main`)

- ✅ Requiere Pull Request antes de mergear.
- ✅ **Requiere 2 aprobaciones** para aprobar código.
- ✅ Aplica también a administradores.
- ✅ Bloquea force-push y borrado de la rama.

## Colaboradores invitados

- numbpi
- AnthonyDubon
- Olivaaa8
- victorore2006-lab

---

## Capturas requeridas (pegar aquí)

1. **Repositorio + historial de commits**
   _(captura de la pestaña Code y la pestaña Commits)_

2. **Ramas `main` y `develop`**
   _(captura del selector de ramas)_

3. **Colaboradores del proyecto**
   _(Settings → Collaborators con la lista de invitados)_

4. **Reglas de seguridad creadas**
   _(Settings → Branches mostrando "Require a pull request" + "Require 2 approvals")_

> Nota: la conexión con Firebase NO se incluye en esta entrega (aún no se ha
> visto en clase, según indicación del ingeniero).
