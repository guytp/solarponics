CREATE PROCEDURE [dbo].[AutoclaveGet]
AS
BEGIN
	SET NOCOUNT ON
	SELECT a.Id, RoomId, a.[Name], Details, r.[Name] RoomName, l.[Name] LocationName FROM [Autoclave] a JOIN [Room] r ON r.Id = a.RoomId JOIN [Location] l ON l.Id = r.LocationId WHERE IsDeleted = 0 ORDER BY a.[Name]
	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[AutoclaveGet] TO [WebApi]