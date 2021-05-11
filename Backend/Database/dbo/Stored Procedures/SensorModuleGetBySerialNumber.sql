CREATE PROCEDURE dbo.SensorModuleGetBySerialNumber
(
	@serialNumber NVARCHAR(50)
)
AS
BEGIN
	DECLARE @id INT
	SELECT TOP 1 @id = Id FROM SensorModule WHERE SerialNumber = @serialNumber
	EXEC dbo.SensorModuleGetById @id = @id
END