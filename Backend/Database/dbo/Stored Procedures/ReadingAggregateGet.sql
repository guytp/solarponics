CREATE PROCEDURE [dbo].[ReadingAggregateGet]
(
	@id INT,
	@timeframe NVARCHAR(3)
)
AS
BEGIN
	with bottom as
	(
		SELECT TOP 35
			ReadingMinimum [Minimum], ReadingMaximum [Maximum], ReadingAverage [Average], PeriodBegin [Time]
			FROM ReadingAggregate ra (NOLOCK)
			WHERE
				ra.SensorId = @id AND
				ra.PeriodType = @timeframe
			ORDER BY
				ra.PeriodBegin DESC
	)
		SELECT * FROM bottom ORDER BY [Time] ASC
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReadingAggregateGet] TO [WebApi]
    AS [dbo];