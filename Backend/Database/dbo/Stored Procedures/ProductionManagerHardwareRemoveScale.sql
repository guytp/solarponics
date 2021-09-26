CREATE PROCEDURE [dbo].[ProductionManagerHardwareRemoveScale]
(
	@machineName NVARCHAR(100),
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON
	
	EXEC ProductionManagerHardwareRemoveSerialDevice
		@machineName = @machineName,
		@deviceType = 'Scale',
		@userId = @userId

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[ProductionManagerHardwareRemoveScale] TO [WebApi]