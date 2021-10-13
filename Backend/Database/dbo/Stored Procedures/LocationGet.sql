CREATE PROCEDURE [dbo].[LocationGet]
AS
BEGIN
	SET NOCOUNT ON

	SELECT Id, [Name] FROM [Location] ORDER BY [Name]

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[LocationGet] TO [WebApi]