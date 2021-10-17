CREATE PROCEDURE [dbo].[ProductionManagerHardwareSetLabelPrinter]
(
	@machineName NVARCHAR(100),
	@driverName NVARCHAR(500),
	@queueName NVARCHAR(200),
	@printerType NVARCHAR(100),
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @id INT
	SELECT TOP 1 @id = Id FROM ProductionManager WHERE MachineName = @machineName
	
	SET XACT_ABORT ON
	BEGIN TRAN

	IF (@id IS NULL)
	BEGIN
		INSERT INTO ProductionManager (MachineName) VALUES(@machineName)
		SELECT @id = SCOPE_IDENTITY()
	END

	DECLARE @hardwareId INT
		DECLARE @labelCombined NVARCHAR(105) = ('Label' + @printerType)
	SELECT TOP 1 @hardwareId = Id FROM ProductionManagerHardwarePrinter WHERE ProductionManagerId = @id AND PrinterType = @labelCombined

	IF (@hardwareId IS NULL)
	BEGIN
		INSERT INTO ProductionManagerHardwarePrinter
				(ProductionManagerId, PrinterType, DriverName, QueueName)
			VALUES
				(@id, @labelCombined, @driverName, @queueName)
		SELECT @hardwareId = SCOPE_IDENTITY()
		
		EXEC AuditAdd @table = 'ProductionManagerHardwarePrinter', @action = 'Add', @userId = @userId, @key = @hardwareId,  @column = 'ProductionManagerId',  @newValue = @id
		EXEC AuditAdd @table = 'ProductionManagerHardwarePrinter', @action = 'Add', @userId = @userId, @key = @hardwareId,  @column = 'PrinterType',  @newValue = @labelCombined
		EXEC AuditAdd @table = 'ProductionManagerHardwarePrinter', @action = 'Add', @userId = @userId, @key = @hardwareId,  @column = 'DriverName',  @newValue = @driverName
		EXEC AuditAdd @table = 'ProductionManagerHardwarePrinter', @action = 'Add', @userId = @userId, @key = @hardwareId,  @column = 'QueueName',  @newValue = @queueName
	END
	ELSE
	BEGIN
		DECLARE @existingDriverName NVARCHAR(500)
		DECLARE @existingQueueName NVARCHAR(200)
		
		SELECT
				TOP 1
					@existingDriverName = DriverName,
					@existingQueueName = QueueName
				FROM ProductionManagerHardwarePrinter
				WHERE Id = @hardwareId
		
		UPDATE ProductionManagerHardwarePrinter
			SET
				DriverName = @driverName,
				QueueName = @queueName
			WHERE Id = @hardwareId

		IF @existingDriverName != @driverName
			EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Update', @userId = @userId, @key = @hardwareId,  @column = 'DriverName',  @newValue = @driverName, @oldValue = @existingDriverName
		IF @existingQueueName != @queueName
			EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Update', @userId = @userId, @key = @hardwareId,  @column = 'QueueName',  @newValue = @queueName, @oldValue = @existingQueueName
	END

	COMMIT TRAN

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[ProductionManagerHardwareSetLabelPrinter] TO [WebApi]