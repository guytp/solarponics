
CREATE PROCEDURE SensorModuleProvisioningQueueDelete
(
	@serialNumber NVARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM SensorModuleProvisioningQueue WHERE SerialNumber = @serialNumber
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SensorModuleProvisioningQueueDelete] TO [WebApi]
    AS [dbo];

