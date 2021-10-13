CREATE PROCEDURE [dbo].[LocationAdd]
(
	@name NVARCHAR(50),
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @id INT
	SET XACT_ABORT ON
	BEGIN TRAN
	INSERT INTO [Location] ([Name]) VALUES(@name)
	SELECT @id = SCOPE_IDENTITY()

	EXEC AuditAdd @table = 'Location', @column = 'Name', @action = 'Add', @userId = @userId, @key = @id, @newValue = @name

	SELECT @id [Id]
	COMMIT TRAN


	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[LocationAdd] TO [WebApi]