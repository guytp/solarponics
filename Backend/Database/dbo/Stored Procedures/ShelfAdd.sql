CREATE PROCEDURE [dbo].[ShelfAdd]
(
	@name NVARCHAR(100),
	@roomId INT,
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON

	SET XACT_ABORT ON
	BEGIN TRAN

	INSERT INTO [Shelf] (RoomId, [Name]) VALUES(@roomId, @name)
	DECLARE @id INT
	SELECT @id = SCOPE_IDENTITY()
	
	
	EXEC AuditAdd @table = 'Shelf', @column = 'RoomId', @action = 'Add', @userId = @userId, @key = @id, @newValue = @roomId
	EXEC AuditAdd @table = 'Shelf', @column = 'Name', @action = 'Add', @userId = @userId, @key = @id, @newValue = @name

	COMMIT TRAN

	SELECT @id [Id]

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[ShelfAdd] TO [WebApi]
GO