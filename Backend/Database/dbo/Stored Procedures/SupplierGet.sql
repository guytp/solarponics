CREATE PROCEDURE [dbo].[SupplierGet]
AS
BEGIN
	SET NOCOUNT ON
	SELECT [Id], [Name] FROM Supplier WHERE IsDeleted = 0 ORDER BY [Name]
	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[SupplierGet] TO [WebApi]