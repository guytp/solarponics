CREATE PROCEDURE [dbo].[RecipeUpdate]
(
	@id INT,
	@name NVARCHAR(200),
	@type TINYINT,
	@text NVARCHAR(MAX),
	@unitsCreated INT,
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON

	SET XACT_ABORT ON
	
	DECLARE @existingName NVARCHAR(200)
	DECLARE @existingType TINYINT
	DECLARE @existingText NVARCHAR(MAX)
	DECLARE @existingUnitsCreated INT
	SELECT @existingName = [Name], @existingType = [Type], @existingText = [Text], @existingUnitsCreated = UnitsCreated FROM Recipe WHERE Id = @id
	IF (@existingName IS NULL)
		RETURN 0

	BEGIN TRAN
	UPDATE [Recipe]
		SET
			[Name] = @name,
			[Type] = @type,
			[Text] = @text,
			[UnitsCreated] = @unitsCreated
		WHERE
			Id = @id
			
	EXEC AuditAdd @table = 'Recipe', @column = 'Name', @action = 'Update', @userId = @userId, @key = @id, @newValue = @name, @oldValue = @existingName
	EXEC AuditAdd @table = 'Recipe', @column = 'Type', @action = 'Update', @userId = @userId, @key = @id, @newValue = @type, @oldValue = @existingType
	EXEC AuditAdd @table = 'Recipe', @column = 'Text', @action = 'Update', @userId = @userId, @key = @id, @newValue = @text, @oldValue = @existingText
	EXEC AuditAdd @table = 'Recipe', @column = 'UnitsCreated', @action = 'Update', @userId = @userId, @key = @id, @newValue = @unitsCreated, @oldValue = @existingUnitsCreated

	COMMIT TRAN

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[RecipeUpdate] TO [WebApi]