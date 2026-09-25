USE Neptuno;
GO

/* Preparación para eliminación lógica. */
IF COL_LENGTH('dbo.proveedores', 'Activo') IS NULL
BEGIN
    ALTER TABLE dbo.proveedores
        ADD Activo bit NOT NULL
            CONSTRAINT DF_proveedores_Activo DEFAULT (1) WITH VALUES;
END;
GO

/* La carga original contiene 29 proveedores; los nuevos comienzan en 30. */
IF NOT EXISTS (SELECT 1 FROM sys.sequences WHERE name = 'SeqProveedores' AND schema_id = SCHEMA_ID('dbo'))
    EXEC('CREATE SEQUENCE dbo.SeqProveedores AS int START WITH 30 INCREMENT BY 1');
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedores_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idProveedor, nombreCompañia, nombrecontacto, cargocontacto,
           direccion, ciudad, region, codPostal, pais, telefono, fax,
           paginaprincipal, Activo
    FROM dbo.proveedores
    WHERE Activo = 1
    ORDER BY nombreCompañia;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedores_Buscar
    @NombreContacto varchar(30) = NULL,
    @Ciudad varchar(15) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idProveedor, nombreCompañia, nombrecontacto, cargocontacto,
           direccion, ciudad, region, codPostal, pais, telefono, fax,
           paginaprincipal, Activo
    FROM dbo.proveedores
    WHERE Activo = 1
      AND (NULLIF(LTRIM(RTRIM(@NombreContacto)), '') IS NULL
           OR nombrecontacto LIKE '%' + LTRIM(RTRIM(@NombreContacto)) + '%')
      AND (NULLIF(LTRIM(RTRIM(@Ciudad)), '') IS NULL
           OR ciudad LIKE '%' + LTRIM(RTRIM(@Ciudad)) + '%')
    ORDER BY nombreCompañia;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedores_ObtenerPorId
    @IdProveedor int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idProveedor, nombreCompañia, nombrecontacto, cargocontacto,
           direccion, ciudad, region, codPostal, pais, telefono, fax,
           paginaprincipal, Activo
    FROM dbo.proveedores
    WHERE idProveedor = @IdProveedor AND Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedores_Insertar
    @NombreCompania varchar(40), @NombreContacto varchar(30), @CargoContacto varchar(30),
    @Direccion varchar(60), @Ciudad varchar(15), @Region varchar(15), @CodPostal varchar(10),
    @Pais varchar(15), @Telefono varchar(24), @Fax varchar(24), @PaginaPrincipal text
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IdProveedor int = NEXT VALUE FOR dbo.SeqProveedores;
    INSERT INTO dbo.proveedores
        (idProveedor, nombreCompañia, nombrecontacto, cargocontacto, direccion, ciudad,
         region, codPostal, pais, telefono, fax, paginaprincipal, Activo)
    VALUES
        (@IdProveedor, @NombreCompania, @NombreContacto, @CargoContacto, @Direccion, @Ciudad,
         @Region, @CodPostal, @Pais, @Telefono, @Fax, @PaginaPrincipal, 1);
    SELECT @IdProveedor AS IdProveedor;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedores_Actualizar
    @IdProveedor int, @NombreCompania varchar(40), @NombreContacto varchar(30), @CargoContacto varchar(30),
    @Direccion varchar(60), @Ciudad varchar(15), @Region varchar(15), @CodPostal varchar(10),
    @Pais varchar(15), @Telefono varchar(24), @Fax varchar(24), @PaginaPrincipal text
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.proveedores
    SET nombreCompañia = @NombreCompania, nombrecontacto = @NombreContacto,
        cargocontacto = @CargoContacto, direccion = @Direccion, ciudad = @Ciudad,
        region = @Region, codPostal = @CodPostal, pais = @Pais, telefono = @Telefono,
        fax = @Fax, paginaprincipal = @PaginaPrincipal
    WHERE idProveedor = @IdProveedor AND Activo = 1;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_Proveedores_Eliminar
    @IdProveedor int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.proveedores SET Activo = 0
    WHERE idProveedor = @IdProveedor AND Activo = 1;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO
