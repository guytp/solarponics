CREATE PROCEDURE dbo.SensorModuleGetByUniqueIdentifier
(
	@uniqueIdentifier UNIQUEIDENTIFIER
)
AS
BEGIN
	DECLARE @id INT
	SELECT TOP 1 @id = Id FROM SensorModule WHERE UniqueIdentifier = @uniqueIdentifier
	EXEC dbo.SensorModuleGetById @id = @id
END