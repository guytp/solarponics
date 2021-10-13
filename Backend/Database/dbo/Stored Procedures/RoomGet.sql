CREATE PROCEDURE [dbo].[RoomGet]
AS
BEGIN
	SET NOCOUNT ON

	SELECT Id, LocationId, [Name] FROM [Room] ORDER BY LocationId, [Name]

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[RoomGet] TO [WebApi]