CREATE PROCEDURE dbo.SensorModuleGetByUniqueIdentifier
(
	@uniqueIdentifier UNIQUEIDENTIFIER
)
AS
BEGIN
	DECLARE @id INT
	SELECT TOP 1 @id = Id FROM SensorModule WHERE UniqueIdentifier = @uniqueIdentifier

	SELECT
		sm.Id, sm.[Name], r.[Name] Room, l.[Name] [Location], sm.[UniqueIdentifier], sm.SerialNumber
		FROM SensorModule sm
		JOIN Room r ON sm.RoomId = r.Id
		JOIN [Location] l ON l.Id = r.LocationId
		WHERE
			sm.Id = @id

	SELECT
		s.Id, s.[Type], s.Number
		FROM Sensor s
		WHERE
			s.SensorModuleId = @id
		ORDER BY [Type], [Number]
END