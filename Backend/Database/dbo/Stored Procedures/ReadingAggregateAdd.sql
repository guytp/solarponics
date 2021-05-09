create procedure ReadingAggregateAdd
(
	@sensorId INT,
	@reading DECIMAL(12, 4),
	@time DATETIME,
	@periodType VARCHAR(3)
)
as
begin
	-- Determine start time for this period based off time
	DECLARE @timeframe INT
	IF @periodType = '1M'
		SELECT @timeframe = 60
	ELSE IF @periodType = '5M'
		SELECT @timeframe = 300
	ELSE IF @periodType = '15M'
		SELECT @timeframe = 900
	ELSE IF @periodType = '30M'
		SELECT @timeframe = 1800
	ELSE IF @periodType = '1H'
		SELECT @timeframe = 3600
	ELSE IF @periodType = '4H'
		SELECT @timeframe = 14400
	ELSE IF @periodType = '12H'
		SELECT @timeframe = 43200
	ELSE IF @periodType = '1D'
		SELECT @timeframe = 86400
	DECLARE @startDt DATETIME = convert(DATETIME, convert(NVARCHAR(MAX), DATEPART(year, @time)) + '-01-01 00:00:00')
	DECLARE @secondsElapsed INT = DATEDIFF(second, @startDt, @time)
	DECLARE @barsElapsed INT = @secondsElapsed / @timeframe
	DECLARE @secondsToStartOfBar INT = @barsElapsed * @timeframe
	DECLARE @periodBegin DATETIME = DATEADD(second, @secondsToStartOfBar, @startDt)

	-- If existing aggregate doesn't exist, add one now and return
	IF NOT EXISTS (SELECT 1 FROM ReadingAggregate WHERE SensorId = @sensorId AND PeriodType = @periodType AND PeriodBegin = @periodBegin)
	BEGIN
		INSERT
			INTO ReadingAggregate
			(SensorId, ReadingMinimum, ReadingMaximum, ReadingAverage, NumberReadings, PeriodBegin, PeriodType)
			VALUES(@sensorId, @reading, @reading, @reading, 1, @periodBegin, @periodType)

			RETURN 0
	END

	-- Otherwise we can update the existing
	DECLARE @readingMinimum DECIMAL(12, 4)
	DECLARE @readingMaximum DECIMAL(12, 4)
	DECLARE @readingAverage DECIMAL(12, 4)
	DECLARE @numberReadings DECIMAL(12, 4)
	DECLARE @id INT
	SELECT
		TOP 1
			@id = Id,
			@readingMinimum = ReadingMinimum,
			@readingMaximum = ReadingMaximum,
			@readingAverage = ReadingAverage,
			@numberReadings = NumberReadings
		FROM
			ReadingAggregate
		WHERE
			SensorId = @sensorId AND
			PeriodType = @periodType AND
			PeriodBegin = @periodBegin

	UPDATE ReadingAggregate
		SET
			ReadingMinimum = CASE WHEN @reading < @readingMinimum THEN @reading ELSE @readingMinimum END,
			ReadingMaximum = CASE WHEN @reading > @readingMaximum THEN @reading ELSE @readingMaximum END,
			ReadingAverage = ((@readingAverage * @numberReadings) + @reading) / (@numberReadings + 1),
			NumberReadings = @numberReadings + 1
		WHERE
			SensorId = @sensorId AND
			PeriodType = @periodType AND
			PeriodBegin = @periodBegin
end
