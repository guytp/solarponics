CREATE PROCEDURE ReadingAggregateGet
(
	@id INT,
	@timeframe NVARCHAR(3)
)
AS
BEGIN
	SELECT
		ReadingMinimum [Minimum], ReadingMaximum [Maximum], ReadingAverage [Average], PeriodBegin [Time]
		FROM ReadingAggregate ra
		WHERE
			ra.SensorId = @id AND
			ra.PeriodType = @timeframe
		ORDER BY
			ra.PeriodBegin ASC
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReadingAggregateGet] TO [WebApi]
    AS [dbo];

