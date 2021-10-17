CREATE PROCEDURE [dbo].[AutoclaveDelete]
(
	@id INT,
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON
	
	SET XACT_ABORT ON
	BEGIN TRAN

	IF NOT EXISTS (SELECT 1 FROM Autoclave WHERE Id = @id AND IsDeleted = 0)
		RETURN 0

	UPDATE Autoclave SET IsDeleted = 1 WHERE Id = @id
	EXEC AuditAdd @table = 'Autoclave', @column = 'IsDeleted', @action = 'Update', @userId = @userId, @key = @id, @newValue = '1', @oldValue = '0'
	EXEC AuditAdd @table = 'Autoclave', @action = 'Delete', @userId = @userId, @key = @id

	COMMIT TRAN

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[AutoclaveDelete] TO [WebApi]