CREATE PROCEDURE [dbo].[SensorModuleDelete]
(
	@id INT
)
AS
BEGIN
	SET NOCOUNT ON
	UPDATE SensorModule SET IsDeleted = 1 WHERE Id = @id
	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[SensorModuleDelete] TO [WebApi]
GO