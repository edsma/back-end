-- =============================================
-- Author:		Edwin Arley Tangarife Tobón
-- Create date: 11 de mayo del 2022
-- Description:	Procedimiento para eliminar información
-- =============================================
CREATE PROCEDURE [dbo].[DeletePoliza]
	-- Add the parameters for the stored procedure here
	@IdPoliza int
AS
BEGIN
/****** Script for SelectTopNRows command from SSMS  ******/
	DELETE  
  FROM [dbo].[Polizas]
  WHERE Id = @IdPoliza;

END
