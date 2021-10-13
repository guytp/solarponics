CREATE PROCEDURE [dbo].[RoomAdd]
(
	@locationId INT,
	@name NVARCHAR(50),
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @id INT
	SET XACT_ABORT ON
	BEGIN TRAN
	INSERT INTO [Room] (LocationId, [Name]) VALUES(@locationId, @name)
	SELECT @id = SCOPE_IDENTITY()

	EXEC AuditAdd @table = 'Room', @column = 'LocationId', @action = 'Add', @userId = @userId, @key = @id, @newValue = @locationId
	EXEC AuditAdd @table = 'Room', @column = 'Name', @action = 'Add', @userId = @userId, @key = @id, @newValue = @name

	SELECT @id [Id]
	COMMIT TRAN


	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[RoomAdd] TO [WebApi]