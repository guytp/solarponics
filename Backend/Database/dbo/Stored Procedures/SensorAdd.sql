create procedure SensorAdd
(
	@sensorModuleId INT,
	@type NVARCHAR(20),
	@number TINYINT
)
as
begin
	SET NOCOUNT ON

	INSERT INTO [Sensor] (SensorModuleId, [Type], [Number]) VALUES(@sensorModuleId, @type, @number)

	SELECT SCOPE_IDENTITY() Id
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SensorAdd] TO [WebApi]
    AS [dbo];

