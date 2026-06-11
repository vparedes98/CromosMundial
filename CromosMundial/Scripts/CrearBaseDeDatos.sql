-- Cromos del Mundial - crear base de datos

USE master;
GO

IF DB_ID('CromosMundialDb') IS NOT NULL
BEGIN
    ALTER DATABASE CromosMundialDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CromosMundialDb;
END
GO

CREATE DATABASE CromosMundialDb;
GO

USE CromosMundialDb;
GO

-- Tablas

CREATE TABLE Paises (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(80) NOT NULL,
    Continente NVARCHAR(40) NOT NULL,
    CodigoFifa NVARCHAR(3) NOT NULL,
    RankingFifa INT NOT NULL
);
GO

CREATE TABLE Equipos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(80) NOT NULL,
    DirectorTecnico NVARCHAR(80) NOT NULL,
    AnioFundacion INT NOT NULL,
    Logo NVARCHAR(200) NULL,
    GrupoMundialista NVARCHAR(1) NOT NULL,
    PaisId INT NOT NULL FOREIGN KEY REFERENCES Paises(Id)
);
GO

CREATE TABLE Jugadores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Posicion NVARCHAR(40) NOT NULL,
    NumeroCamiseta INT NOT NULL,
    FechaNacimiento DATETIME2 NOT NULL,
    EquipoId INT NOT NULL FOREIGN KEY REFERENCES Equipos(Id)
);
GO

CREATE TABLE Albumes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Anio INT NOT NULL,
    CantidadCromos INT NOT NULL,
    EdicionEspecial BIT NOT NULL
);
GO

CREATE TABLE Cromos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NumeroCromo INT NOT NULL,
    Edicion NVARCHAR(60) NOT NULL,
    ValorMercado DECIMAL(18,2) NOT NULL,
    Foto NVARCHAR(200) NULL,
    JugadorId INT NOT NULL FOREIGN KEY REFERENCES Jugadores(Id),
    AlbumId INT NOT NULL FOREIGN KEY REFERENCES Albumes(Id)
);
GO

-- Datos de ejemplo

INSERT INTO Paises (Nombre, Continente, CodigoFifa, RankingFifa) VALUES
(N'Argentina', N'América', N'ARG', 1),
(N'Francia', N'Europa', N'FRA', 2),
(N'Brasil', N'América', N'BRA', 3),
(N'Inglaterra', N'Europa', N'ENG', 4),
(N'Bélgica', N'Europa', N'BEL', 5),
(N'Países Bajos', N'Europa', N'NED', 6),
(N'Portugal', N'Europa', N'POR', 7),
(N'España', N'Europa', N'ESP', 8),
(N'Italia', N'Europa', N'ITA', 9),
(N'Croacia', N'Europa', N'CRO', 10);
GO

INSERT INTO Equipos (Nombre, DirectorTecnico, AnioFundacion, Logo, GrupoMundialista, PaisId) VALUES
(N'Selección Argentina', N'Lionel Scaloni', 1893, N'/imagenes/arg.png', N'C', 1),
(N'Selección de Francia', N'Didier Deschamps', 1904, N'/imagenes/fra.png', N'D', 2),
(N'Selección de Brasil', N'Dorival Júnior', 1914, N'/imagenes/bra.png', N'G', 3),
(N'Selección de Inglaterra', N'Thomas Tuchel', 1863, N'/imagenes/eng.png', N'F', 4),
(N'Selección de Bélgica', N'Domenico Tedesco', 1895, N'/imagenes/bel.png', N'E', 5),
(N'Selección de Países Bajos', N'Ronald Koeman', 1889, N'/imagenes/ned.png', N'A', 6),
(N'Selección de Portugal', N'Roberto Martínez', 1914, N'/imagenes/por.png', N'H', 7),
(N'Selección de España', N'Luis de la Fuente', 1913, N'/imagenes/esp.png', N'B', 8),
(N'Selección de Italia', N'Luciano Spalletti', 1898, N'/imagenes/ita.png', N'I', 9),
(N'Selección de Croacia', N'Zlatko Dalic', 1912, N'/imagenes/cro.png', N'J', 10);
GO

INSERT INTO Jugadores (Nombre, Posicion, NumeroCamiseta, FechaNacimiento, EquipoId) VALUES
(N'Lionel Messi', N'Delantero', 10, '1987-06-24', 1),
(N'Emiliano Martínez', N'Portero', 23, '1992-09-02', 1),
(N'Julián Álvarez', N'Delantero', 9, '2000-01-31', 1),
(N'Enzo Fernández', N'Mediocampista', 24, '2001-01-17', 1),
(N'Rodrigo De Paul', N'Mediocampista', 7, '1994-05-24', 1),
(N'Kylian Mbappé', N'Delantero', 10, '1998-12-20', 2),
(N'Antoine Griezmann', N'Delantero', 7, '1991-03-21', 2),
(N'Aurélien Tchouaméni', N'Mediocampista', 8, '2000-01-27', 2),
(N'Mike Maignan', N'Portero', 16, '1995-07-03', 2),
(N'Vinícius Júnior', N'Extremo', 7, '2000-07-12', 3),
(N'Rodrygo', N'Extremo', 10, '2001-01-09', 3),
(N'Casemiro', N'Mediocampista', 5, '1992-02-23', 3),
(N'Alisson Becker', N'Portero', 1, '1992-10-02', 3),
(N'Jude Bellingham', N'Mediocampista', 10, '2003-06-29', 4),
(N'Kevin De Bruyne', N'Mediocampista', 7, '1991-06-28', 5),
(N'Virgil van Dijk', N'Defensa', 4, '1991-07-08', 6),
(N'Cristiano Ronaldo', N'Delantero', 7, '1985-02-05', 7),
(N'Lamine Yamal', N'Extremo', 19, '2007-07-13', 8),
(N'Gianluigi Donnarumma', N'Portero', 21, '1999-02-25', 9),
(N'Luka Modric', N'Mediocampista', 10, '1985-09-09', 10);
GO

INSERT INTO Albumes (Nombre, Anio, CantidadCromos, EdicionEspecial) VALUES
(N'Mundial 2026', 2026, 700, 1),
(N'Clásicos del Mundial', 2022, 500, 0);
GO

INSERT INTO Cromos (NumeroCromo, Edicion, ValorMercado, Foto, JugadorId, AlbumId) VALUES
(100, N'Oro', 50.00, N'/imagenes/messi.jpg', 1, 1),
(101, N'Normal', 5.00, N'/imagenes/martinez.jpg', 2, 1),
(102, N'Plata', 12.00, N'/imagenes/mbappe.jpg', 6, 1),
(103, N'Oro', 45.00, N'/imagenes/vinicius.jpg', 10, 1),
(104, N'Plata', 30.00, N'/imagenes/bellingham.jpg', 14, 1),
(105, N'Oro', 60.00, N'/imagenes/ronaldo.jpg', 17, 1),
(106, N'Normal', 8.00, N'/imagenes/debruyne.jpg', 15, 1),
(107, N'Plata', 35.00, N'/imagenes/yamal.jpg', 18, 1),
(108, N'Normal', 10.00, N'/imagenes/modric.jpg', 20, 1),
(200, N'Normal', 6.50, N'/imagenes/jalvarez.jpg', 3, 2),
(201, N'Plata', 16.00, N'/imagenes/rodrygo.jpg', 11, 2),
(202, N'Normal', 9.00, N'/imagenes/vandijk.jpg', 16, 2);
GO
