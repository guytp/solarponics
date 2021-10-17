CREATE PROCEDURE [dbo].[ShelfDelete]
(
	@id INT,
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON
	
	SET XACT_ABORT ON
	BEGIN TRAN

	IF NOT EXISTS (SELECT 1 FROM Shelf WHERE Id = @id AND IsDeleted = 0)
		RETURN 0

	UPDATE Shelf SET IsDeleted = 1 WHERE Id = @id
	EXEC AuditAdd @table = 'Shelf', @column = 'IsDeleted', @action = 'Update', @userId = @userId, @key = @id, @newValue = '1', @oldValue = '0'
	EXEC AuditAdd @table = 'Shelf', @action = 'Delete', @userId = @userId, @key = @id

	COMMIT TRAN

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[ShelfDelete] TO [WebApi]