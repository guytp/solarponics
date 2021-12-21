CREATE PROCEDURE [dbo].[RecipeAdd]
(
	@name NVARCHAR(MAX),
	@type TINYINT,
	@text NVARCHAR(MAX),
	@unitsCreated INT,
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON

	SET XACT_ABORT ON

	BEGIN TRAN
	INSERT INTO Recipe ([Name], [Type], [Text], [UnitsCreated]) VALUES(@name, @type, @text, @unitsCreated)

	DECLARE @id INT
	SELECT @id = SCOPE_IDENTITY()
	
	EXEC AuditAdd @table = 'Recipe', @column = 'Name', @action = 'Add', @userId = @userId, @key = @id, @newValue = @name
	EXEC AuditAdd @table = 'Recipe', @column = 'Type', @action = 'Add', @userId = @userId, @key = @id, @newValue = @type
	EXEC AuditAdd @table = 'Recipe', @column = 'UnitsCreated', @action = 'Add', @userId = @userId, @key = @id, @newValue = @unitsCreated
	EXEC AuditAdd @table = 'Recipe', @column = 'Text', @action = 'Add', @userId = @userId, @key = @id, @newValue = @text

	COMMIT TRAN

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[RecipeAdd] TO [WebApi]