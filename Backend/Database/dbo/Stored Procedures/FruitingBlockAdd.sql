CREATE PROCEDURE [dbo].[FruitingBlockAdd]
(
	@userId INT,
	@recipeId INT,
	@date DATETIME,
	@weight DECIMAL(5, 3),
	@notes NVARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO [FruitingBlock]
		(RecipeId, CreateUserId, CreateDate, [Weight], Notes)
		VALUES(@recipeId, @userId, @date, @weight, @notes)

	DECLARE @id INT
	SELECT @id = SCOPE_IDENTITY()
	
	EXEC AuditAdd @table = 'FruitingBlock', @column = 'CreateUserId', @action = 'Add', @userId = @userId, @key = @id, @newValue = @userId
	EXEC AuditAdd @table = 'FruitingBlock', @column = 'RecipeId', @action = 'Add', @userId = @userId, @key = @id, @newValue = @recipeId
	EXEC AuditAdd @table = 'FruitingBlock', @column = 'Weight', @action = 'Add', @userId = @userId, @key = @id, @newValue = @weight
	EXEC AuditAdd @table = 'FruitingBlock', @column = 'CreateDate', @action = 'Add', @userId = @userId, @key = @id, @newValue = @date
	EXEC AuditAdd @table = 'FruitingBlock', @column = 'Notes', @action = 'Add', @userId = @userId, @key = @id, @newValue = @notes
	
	SELECT @id [Id]

	COMMIT TRAN

	RETURN 0 
END
GO

GRANT EXECUTE ON [dbo].[FruitingBlockAdd] TO [WebApi]