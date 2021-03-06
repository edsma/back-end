-- =============================================
-- Author:		Edwin Arley Tangarife Tobón
-- Create date: 11 de mayo del 2022
-- Description:	Procedimiento para consultar información
-- =============================================
CREATE PROCEDURE [dbo].[ConsultarPoliza]
	-- Add the parameters for the stored procedure here
	@Valor varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from 
	Polizas
	where PlacaAutoCliente = @Valor OR NumeroPoliza = @Valor
END
