CREATE PROCEDURE [dbo].[GrainSpawnAdd]
(
	@userId INT,
	@recipeId INT,
	@weight DECIMAL(5, 3),
	@notes NVARCHAR(MAX),
	@date DATETIME
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO GrainSpawn
		(RecipeId, CreateUserId, CreateDate, [Weight], Notes)
		VALUES(@recipeId, @userId, @date, @weight, @notes)

	DECLARE @id INT
	SELECT @id = SCOPE_IDENTITY()
	
	EXEC AuditAdd @table = 'GrainSpawn', @column = 'CreateUserId', @action = 'Add', @userId = @userId, @key = @id, @newValue = @userId
	EXEC AuditAdd @table = 'GrainSpawn', @column = 'RecipeId', @action = 'Add', @userId = @userId, @key = @id, @newValue = @recipeId
	EXEC AuditAdd @table = 'GrainSpawn', @column = 'Weight', @action = 'Add', @userId = @userId, @key = @id, @newValue = @weight
	EXEC AuditAdd @table = 'GrainSpawn', @column = 'CreateDate', @action = 'Add', @userId = @userId, @key = @id, @newValue = @date
	EXEC AuditAdd @table = 'GrainSpawn', @column = 'Notes', @action = 'Add', @userId = @userId, @key = @id, @newValue = @notes
	
	SELECT @id [Id]

	COMMIT TRAN

	RETURN 0 
END
GO

GRANT EXECUTE ON [dbo].[GrainSpawnAdd] TO [WebApi]