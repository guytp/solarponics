create procedure SensorModuleAdd
(
	@roomId INT,
	@uniqueIdentifier UNIQUEIDENTIFIER,
	@serialNumber NVARCHAR(50),
	@name NVARCHAR(50)
)
as
begin
	SET NOCOUNT ON

	INSERT INTO [SensorModule] (RoomId, [UniqueIdentifier], SerialNumber, [Name]) VALUES(@roomId, @uniqueIdentifier, @serialNumber, @name)

	SELECT SCOPE_IDENTITY() Id
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SensorModuleAdd] TO [WebApi]
    AS [dbo];

