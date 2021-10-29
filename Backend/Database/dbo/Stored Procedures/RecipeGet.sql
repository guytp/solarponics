CREATE PROCEDURE [dbo].[RecipeGet]
AS
BEGIN
	SET NOCOUNT ON
	SELECT [Id], [Name], [Type], [Text], [UnitsCreated] FROM Recipe WHERE IsDeleted = 0 ORDER BY [Name]
	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[RecipeGet] TO [WebApi]