CREATE PROCEDURE [dbo].[FruitingBlockGet]
(
	@id INT = null
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	SELECT
		fb.Id, GrainSpawnId, fb.RecipeId, fb.CreateUserId, fb.InnoculateUserId, fb.IncubateShelfPlaceUserId, fb.FruitingShelfPlaceUserId, fb.CreateDate, fb.InnoculateDate, fb.IncubateShelfPlaceDate, fb.FruitingShelfPlaceDate, fb.[Weight], fb.Notes, cu.[Name] CreateUser, iu.[Name] [InnoculateUser], isu.[Name] [IncubateShelfPlaceUser], fsu.[Name] [FruitingShelfPlaceUser], c.[Strain], c.[Generation], c.MediumType, r.[Name] RecipeName, fs.[Name] [FruitingShelfName], [is].[Name] IncubateShelfName, [is].[Id] IncubateShelfId, fs.[Id] FruitingShelfId
	FROM [FruitingBlock] fb
	JOIN [User] cu ON cu.Id = fb.CreateUserId
	JOIN Recipe r ON r.Id = fb.RecipeId
	LEFT OUTER JOIN [User] iu ON iu.Id = fb.InnoculateUserId
	LEFT OUTER JOIN GrainSpawn g ON g.Id = fb.GrainSpawnId
	LEFT OUTER JOIN Culture c ON c.Id = g.CultureId
	LEFT OUTER JOIN [Shelf] [is] ON [is].Id = fb.[IncubateShelfId]
	LEFT OUTER JOIN [Shelf] fs ON fs.Id = fb.FruitingShelfId
	LEFT OUTER JOIN [User] isu ON isu.Id = fb.IncubateShelfPlaceUserId
	LEFT OUTER JOIN [User] fsu ON fsu.Id = fb.FruitingShelfPlaceUserId
	WHERE
		@id IS NULL OR fb.Id = @id
	ORDER BY
		c.Id DESC

	RETURN 0 
END
GO

GRANT EXECUTE ON [dbo].[FruitingBlockGet] TO [WebApi]