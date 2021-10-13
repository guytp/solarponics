CREATE PROCEDURE [dbo].[RoomUpdate]
(
	@id INT,
	@name NVARCHAR(50),
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON

	SET XACT_ABORT ON
	BEGIN TRAN

	DECLARE @existingName NVARCHAR(50)
	SELECT TOP 1 @existingName = [Name] FROM [Room] WHERE Id = @id
	IF @existingName IS NULL
		RETURN 0

	UPDATE [Room] SET [Name] = @name WHERE Id = @id

	EXEC AuditAdd @table = 'Room', @column = 'Name', @action = 'Update', @userId = @userId, @key = @id, @newValue = @name, @oldValue = @existingName

	COMMIT TRAN


	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[RoomUpdate] TO [WebApi]