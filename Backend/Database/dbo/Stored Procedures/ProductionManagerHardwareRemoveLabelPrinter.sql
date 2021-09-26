CREATE PROCEDURE [dbo].[ProductionManagerHardwareRemoveLabelPrinter]
(
	@machineName NVARCHAR(100),
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @id INT
	SELECT TOP 1 @id = dev.Id FROM ProductionManagerHardwarePrinter dev JOIN ProductionManager pm ON pm.Id = dev.ProductionManagerId WHERE pm.MachineName = @machineName AND dev.PrinterType = 'Label'

	IF @id IS NULL
		RETURN 0
	
	SET XACT_ABORT ON
	BEGIN TRAN

	DECLARE @hardwareId INT
	DELETE FROM ProductionManagerHardwarePrinter WHERE Id = @id
	EXEC AuditAdd @table = 'ProductionManagerHardwarePrinter', @action = 'Delete', @userId = @userId, @key = @id

	COMMIT TRAN

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[ProductionManagerHardwareRemoveLabelPrinter] TO [WebApi]