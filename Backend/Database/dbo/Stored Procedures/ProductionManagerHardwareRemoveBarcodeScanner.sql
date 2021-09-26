CREATE PROCEDURE [dbo].[ProductionManagerHardwareRemoveBarcodeScanner]
(
	@machineName NVARCHAR(100),
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON
	
	EXEC ProductionManagerHardwareRemoveSerialDevice
		@machineName = @machineName,
		@deviceType = 'BarcodeScanner',
		@userId = @userId

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[ProductionManagerHardwareRemoveBarcodeScanner] TO [WebApi]