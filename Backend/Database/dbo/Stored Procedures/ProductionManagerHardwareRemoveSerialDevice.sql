CREATE PROCEDURE [dbo].[ProductionManagerHardwareRemoveSerialDevice]
(
	@machineName NVARCHAR(100),
	@deviceType NVARCHAR(50),
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @id INT
	SELECT TOP 1 @id = dev.Id FROM ProductionManagerHardwareSerialDevice dev JOIN ProductionManager pm ON pm.Id = dev.ProductionManagerId WHERE pm.MachineName = @machineName AND dev.DeviceType = @deviceType

	IF @id IS NULL
		RETURN 0
	
	SET XACT_ABORT ON
	BEGIN TRAN

	DECLARE @hardwareId INT
	DELETE FROM ProductionManagerHardwareSerialDevice WHERE Id = @id
	EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Delete', @userId = @userId, @key = @id

	COMMIT TRAN

	RETURN 0
END
GO