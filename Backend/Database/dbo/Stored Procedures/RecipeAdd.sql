CREATE PROCEDURE [dbo].[RecipeAdd]
(
	@name NVARCHAR(200),
	@type TINYINT,
	@text NVARCHAR(MAX),
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON

	SET XACT_ABORT ON

	BEGIN TRAN
	INSERT INTO Recipe ([Name], [Type], [Text]) VALUES(@name, @type, @text)

	DECLARE @id INT
	SELECT @id = SCOPE_IDENTITY()
	
	EXEC AuditAdd @table = 'Recipe', @column = 'Name', @action = 'Add', @userId = @userId, @key = @id, @newValue = @name
	EXEC AuditAdd @table = 'Recipe', @column = 'Type', @action = 'Add', @userId = @userId, @key = @id, @newValue = @type
	EXEC AuditAdd @table = 'Recipe', @column = 'Text', @action = 'Add', @userId = @userId, @key = @id, @newValue = @text

	COMMIT TRAN

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[RecipeAdd] TO [WebApi]