CREATE PROCEDURE [dbo].[ReadingCurrentGet]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT TOP 1 [Reading], [Time] FROM [Reading] WHERE [SensorId] = @id ORDER BY [Time] DESC

	RETURN 0
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReadingCurrentGet] TO [WebApi]
    AS [dbo];