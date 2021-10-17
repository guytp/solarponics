CREATE PROCEDURE [dbo].[ShelfGet]
AS
BEGIN
	SET NOCOUNT ON
	SELECT s.Id, RoomId, s.[Name], r.[Name] RoomName, l.[Name] LocationName FROM [Shelf] s JOIN [Room] r ON r.Id = s.RoomId JOIN [Location] l ON l.Id = r.LocationId WHERE IsDeleted = 0 ORDER BY s.[Name]
	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[ShelfGet] TO [WebApi]