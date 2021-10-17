CREATE PROCEDURE [dbo].[AutoclaveAdd]
(
	@name NVARCHAR(100),
	@details NVARCHAR(MAX),
	@roomId INT,
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON

	SET XACT_ABORT ON
	BEGIN TRAN

	INSERT INTO [Autoclave] (RoomId, [Name], [Details]) VALUES(@roomId, @name, @details)
	DECLARE @id INT
	SELECT @id = SCOPE_IDENTITY()
	
	
	EXEC AuditAdd @table = 'Autoclave', @column = 'RoomId', @action = 'Add', @userId = @userId, @key = @id, @newValue = @roomId
	EXEC AuditAdd @table = 'Autoclave', @column = 'Name', @action = 'Add', @userId = @userId, @key = @id, @newValue = @name
	EXEC AuditAdd @table = 'Autoclave', @column = 'Details', @action = 'Add', @userId = @userId, @key = @id, @newValue = @details

	COMMIT TRAN

	SELECT @id [Id]

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[AutoclaveAdd] TO [WebApi]
GO