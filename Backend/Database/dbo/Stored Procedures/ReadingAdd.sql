create procedure ReadingAdd
(
	@sensorId INT,
	@reading DECIMAL(12, 4),
	@time DATETIME
)
as
begin
	SET NOCOUNT ON

	SET XACT_ABORT ON
	BEGIN TRAN ReadingAdd
	INSERT INTO [Reading] (SensorId, Reading, [Time]) VALUES(@sensorId, @reading, @time)

	-- Add to aggregate periods for 1M, 5M, 15M, 30M, 1H, 4H, 12H, 1D
	EXEC ReadingAggregateAdd
		@sensorId = @sensorId,
		@reading = @reading,
		@time = @time,
		@periodType = '1M'
	EXEC ReadingAggregateAdd
		@sensorId = @sensorId,
		@reading = @reading,
		@time = @time,
		@periodType = '5M'
	EXEC ReadingAggregateAdd
		@sensorId = @sensorId,
		@reading = @reading,
		@time = @time,
		@periodType = '15M'
	EXEC ReadingAggregateAdd
		@sensorId = @sensorId,
		@reading = @reading,
		@time = @time,
		@periodType = '30M'
	EXEC ReadingAggregateAdd
		@sensorId = @sensorId,
		@reading = @reading,
		@time = @time,
		@periodType = '1H'
	EXEC ReadingAggregateAdd
		@sensorId = @sensorId,
		@reading = @reading,
		@time = @time,
		@periodType = '4H'
	EXEC ReadingAggregateAdd
		@sensorId = @sensorId,
		@reading = @reading,
		@time = @time,
		@periodType = '12H'
	EXEC ReadingAggregateAdd
		@sensorId = @sensorId,
		@reading = @reading,
		@time = @time,
		@periodType = '1D'
	COMMIT TRAN ReadingAdd
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReadingAdd] TO [IngestionServer]
    AS [dbo];

