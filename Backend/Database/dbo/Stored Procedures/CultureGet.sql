﻿CREATE PROCEDURE [dbo].[CultureGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	SELECT
	TOP 1
		c.Id, SupplierId, ParentCultureId, RecipeId, c.UserId, c.InnoculateUserId, MediumType, OrderDate, CreateDate, Strain, Notes, Generation, cu.[Name] CreateUser, iu.[Name] [InnoculateUser], c.InnoculateDate
	FROM [Culture] c
	JOIN [User] cu ON cu.Id = c.UserId
	LEFT OUTER JOIN [User] iu ON iu.Id = c.InnoculateUserId
	WHERE
		c.Id = @id

	RETURN 0 
END
GO

GRANT EXECUTE ON [dbo].[CultureGet] TO [WebApi]