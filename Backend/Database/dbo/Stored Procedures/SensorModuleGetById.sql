
CREATE PROCEDURE dbo.SensorModuleGetById
(
	@id INT,
	@allowNullId BIT = 0
)
AS
BEGIN
	SELECT
		sm.Id, sm.[Name], r.[Name] Room, l.[Name] [Location], sm.SerialNumber
		FROM SensorModule sm
		JOIN Room r ON sm.RoomId = r.Id
		JOIN [Location] l ON l.Id = r.LocationId
		WHERE
			((@id IS NULL AND @allowNullId = 1) OR sm.Id = @id) AND
			IsDeleted =0
		ORDER BY l.[Name], r.[Name], sm.[Name]

	SELECT
		s.Id, s.SensorModuleId, s.[Type], s.Number, s.CriticalLowBelow, s.WarningLowBelow, s.WarningHighAbove, s.CriticalHighAbove
		FROM Sensor s
		JOIN SensorModule sm ON sm.Id = s.SensorModuleId
		WHERE
			((@id IS NULL AND @allowNullId = 1) OR s.SensorModuleId = @id) AND
			sm.IsDeleted = 0
		ORDER BY [Type], [Number]
END