CREATE PROCEDURE [dbo].[SensorModuleGetModbusTcp]
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		sm.Id, sm.[Name], r.[Name] Room, l.[Name] [Location], mb.IpAddress, mb.[Port], SerialNumber
		FROM SensorModule sm
		JOIN SensorModuleModbusTcp mb ON mb.Id = sm.Id
		JOIN Room r ON r.Id = sm.RoomId
		JOIN Location l ON l.Id = r.LocationId
		WHERE
			sm.Driver = 'ModbusTcp' AND
			sm.IsDeleted = 0
		ORDER BY sm.[Name]
	SELECT s.Id, SensorModuleId, [Type], [Number], WarningLowBelow, CriticalLowBelow, WarningHighAbove, CriticalHighAbove FROM Sensor s JOIN SensorModule sm ON sm.Id = s.SensorModuleId WHERE sm.Driver = 'ModbusTcp' AND sm.IsDeleted = 0
	RETURN 0
END


GRANT EXECUTE ON [dbo].[SensorModuleGetModbusTcp] TO [ModbusIngestionProxy]