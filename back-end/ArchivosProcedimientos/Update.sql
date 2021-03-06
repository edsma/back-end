
-- =============================================
-- Author:		Edwin Arley Tangarife Tobón
-- Create date: 11 de mayo del 2022
-- Description:	Procedimiento para almacenar información
-- =============================================
CREATE PROCEDURE [dbo].[ActualizarPoliza]
	@Id int,
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
	UPDATE Polizas

		set 
		NombreCliente = @NombreCliente,
		IdentificacionCliente = @IdentificacionCliente, 
		FechaNacimientoCliente = @FechaNacimientoCliente,
		FechaPoliza = @FechaPoliza,
		CoberturasCubiertas = @CoberturasCubiertas,
		ValorMaximoPoliza = @ValorMaximoPoliza,
		NombrePoliza = @NombrePoliza,
		NombreCiudadCliente = @NombreCiudadCliente,
		DireccionCliente = @DireccionCliente,
		PlacaAutoCliente = @PlacaAutoCliente,
		@VehiculoCuentaInspeccion = @VehiculoCuentaInspeccion
	Where Id = @Id;
END
