create procedure SensorModuleAdd
(
	@roomId INT,
	@serialNumber NVARCHAR(50),
	@name NVARCHAR(50)
)
as
begin
	SET NOCOUNT ON

	INSERT INTO [SensorModule] (RoomId, SerialNumber, [Name]) VALUES(@roomId, @serialNumber, @name)

	SELECT SCOPE_IDENTITY() Id
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SensorModuleAdd] TO [WebApi]
    AS [dbo];

