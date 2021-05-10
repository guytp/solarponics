
CREATE PROCEDURE dbo.SensorModuleGetAll
AS
	EXEC dbo.SensorModuleGetById @id = NULL, @allowNullId = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SensorModuleGetAll] TO [WebApi]
    AS [dbo];

