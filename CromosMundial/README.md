# Cromos del Mundial

Proyecto en ASP.NET Core MVC con Entity Framework Core.

## Modelos
- Pais
- Equipo (pertenece a un Pais)
- Jugador (pertenece a un Equipo)
- Album
- Cromo (pertenece a un Jugador y a un Album)

## Base de datos
Ejecutar el script `Scripts/CrearBaseDeDatos.sql` en SQL Server Management Studio.
También se puede crear con migraciones: `dotnet ef database update`.

La cadena de conexion esta en `appsettings.json`.

## Ejecutar
`dotnet run` o F5 en Visual Studio.
