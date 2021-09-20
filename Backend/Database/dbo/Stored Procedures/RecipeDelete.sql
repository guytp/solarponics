CREATE PROCEDURE [dbo].[RecipeDelete]
(
	@id INT,
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @existingDeleted BIT
	SELECT @existingDeleted = IsDeleted FROM Recipe WHERE Id = @id
	IF @existingDeleted IS NULL OR @existingDeleted = 1
		RETURN 0

	SET XACT_ABORT ON
	BEGIN TRAN
	UPDATE [Recipe] SET IsDeleted = 1 WHERE Id = @id
	EXEC AuditAdd @table = 'Recipe', @column = 'IsDeleted', @action = 'Update', @userId = @userId, @key = @id, @oldValue = 0, @newValue = 1
	EXEC AuditAdd @table = 'Recipe', @action = 'Delete', @userId = @userId, @key = @id
	COMMIT TRAN
	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[RecipeDelete] TO [WebApi]