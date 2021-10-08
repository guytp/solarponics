CREATE PROCEDURE [dbo].[CultureGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	SELECT Id, SupplierId, ParentCultureId, RecipeId, UserId, MediumType, OrderDate, CreateDate, Strain, Notes FROM [Culture] WHERE Id = @id

	RETURN 0 
END
GO

GRANT EXECUTE ON [dbo].[CultureGet] TO [WebApi]