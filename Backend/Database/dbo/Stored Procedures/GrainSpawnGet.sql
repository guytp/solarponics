CREATE PROCEDURE [dbo].[GrainSpawnGet]
(
	@id INT = null
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	SELECT
		g.Id, CultureId, g.RecipeId, g.CreateUserId, g.InnoculateUserId, g.ShelfPlaceUserId, g.CreateDate, g.InnoculateDate, g.ShelfPlaceDate, [Weight], g.Notes, cu.[Name] CreateUser, iu.[Name] [InnoculateUser], su.[Name] [ShelfPlaceUser], c.[Strain], c.[Generation], c.MediumType, r.[Name] RecipeName, s.[Name] [ShelfName], s.[Id] ShelfId
	FROM [GrainSpawn] g
	JOIN [User] cu ON cu.Id = g.CreateUserId
	JOIN Recipe r ON r.Id = g.RecipeId
	LEFT OUTER JOIN [User] iu ON iu.Id = g.InnoculateUserId
	LEFT OUTER JOIN Culture c ON c.Id = g.CultureId
	LEFT OUTER JOIN [Shelf] s ON s.Id = g.ShelfId
	LEFT OUTER JOIN [User] su ON su.Id = g.ShelfPlaceUserId
	WHERE
		@id IS NULL OR g.Id = @id
	ORDER BY
		c.Id DESC

	RETURN 0 
END
GO

GRANT EXECUTE ON [dbo].[GrainSpawnGet] TO [WebApi]