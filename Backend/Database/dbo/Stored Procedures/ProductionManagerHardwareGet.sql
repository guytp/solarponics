CREATE PROCEDURE [dbo].[ProductionManagerHardwareGet]
(
	@machineName NVARCHAR(100)
)
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT TOP 1 DriverName, SerialPort, BaudRate, Parity, DataBits, StopBits FROM ProductionManagerHardwareSerialDevice sd JOIN ProductionManager pm ON pm.Id = sd.ProductionManagerId WHERE pm.MachineName = @machineName AND sd.DeviceType = 'BarcodeScanner'

	SELECT TOP 1 DriverName, QueueName FROM ProductionManagerHardwarePrinter pr JOIN ProductionManager pm ON pm.Id = pr.ProductionManagerId WHERE pm.MachineName = @machineName AND pr.PrinterType = 'LabelSmall'

	SELECT TOP 1 DriverName, QueueName FROM ProductionManagerHardwarePrinter pr JOIN ProductionManager pm ON pm.Id = pr.ProductionManagerId WHERE pm.MachineName = @machineName AND pr.PrinterType = 'LabelLarge'

    SELECT TOP 1 DriverName, SerialPort, BaudRate, Parity, DataBits, StopBits FROM ProductionManagerHardwareSerialDevice sd JOIN ProductionManager pm ON pm.Id = sd.ProductionManagerId WHERE pm.MachineName = @machineName AND sd.DeviceType = 'Scale'

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[ProductionManagerHardwareGet] TO [WebApi]