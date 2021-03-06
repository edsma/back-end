-- =============================================
-- Author:		Edwin Arley Tangarife Tobón
-- Create date: 11 de mayo del 2022
-- Description:	Procedimiento para almacenar información
-- =============================================
CREATE PROCEDURE [dbo].[GetPolizas]
	@NombreCliente VARCHAR(MAX),
	@IdentificacionCliente VARCHAR(MAX),
	@FechaNacimientoCliente VARCHAR(MAX),
	@FechaPoliza VARCHAR(MAX),
	@CoberturasCubiertas VARCHAR(MAX),
	@ValorMaximoPoliza varchar(max),
	@NombrePoliza VARCHAR(MAX),
	@NombreCiudadCliente VARCHAR(MAX),
	@DireccionCliente VARCHAR(MAX),
	@PlacaAutoCliente VARCHAR(MAX),
	@VehiculoCuentaInspeccion bit

AS
BEGIN
DECLARE @FechaServidor date;
DECLARE @FechaPolizaConvertida date;

set @FechaServidor = Getdate();
set @FechaPolizaConvertida = CAST(@FechaPoliza AS datetime)

IF @FechaPolizaConvertida <= @FechaServidor
	BEGIN
	INSERT INTO [dbo].[Polizas]
           ([NombreCliente]
           ,[IdentificacionCliente]
           ,[FechaNacimientoCliente]
           ,[FechaPoliza]
           ,[CoberturasCubiertas]
           ,[ValorMaximoPoliza]
           ,[NombrePoliza]
           ,[NombreCiudadCliente]
           ,[DireccionCliente]
           ,[PlacaAutoCliente]
           ,[VehiculoCuentaInspeccion])
     VALUES
           (@NombreCliente
           ,@IdentificacionCliente
           ,CAST(@FechaNacimientoCliente AS datetime)
           ,@FechaPolizaConvertida
           ,@CoberturasCubiertas
           ,CAST(@ValorMaximoPoliza AS DECIMAL(10, 4))
           ,@NombrePoliza
           ,@NombreCiudadCliente
           ,@DireccionCliente
           ,@PlacaAutoCliente
           ,@VehiculoCuentaInspeccion)

	END
	

END
