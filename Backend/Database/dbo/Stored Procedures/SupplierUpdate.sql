CREATE PROCEDURE [dbo].[SupplierUpdate]
(
	@id INT,
	@name NVARCHAR(200),
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @existingName NVARCHAR(200)
	SELECT @existingName = [Name] FROM Supplier WHERE Id = @id
	IF (@existingName IS NULL OR @existingName = @name)
		RETURN 0

	BEGIN TRAN
	UPDATE [Supplier] SET [Name] = @name WHERE Id = @id

	EXEC AuditAdd @table = 'Supplier', @column = 'Name', @action = 'Update', @userId = @userId, @key = @id, @newValue = @name, @oldValue = @existingName

	COMMIT TRAN

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[SupplierUpdate] TO [WebApi]