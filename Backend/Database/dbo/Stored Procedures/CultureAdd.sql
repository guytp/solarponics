CREATE PROCEDURE [dbo].[CultureAdd]
(
	@supplierId INT,
	@parentCultureId INT,
	@userId INT,
	@recipeId INT,
	@mediumType INT,
	@orderDate DATETIME,
	@strain NVARCHAR(500),
	@generation INT,
	@notes NVARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	BEGIN TRAN

	DECLARE @createDate DATETIME = GETUTCDATE()

	INSERT INTO Culture
		(SupplierId, ParentCultureId, RecipeId, UserId, MediumType, OrderDate, CreateDate, Strain, Notes, Generation)
		VALUES(@supplierId, @parentCultureId, @recipeId, @userId, @mediumType, @orderDate, @createDate, @strain, @notes, @generation)

	DECLARE @id INT
	SELECT @id = SCOPE_IDENTITY()
	
	EXEC AuditAdd @table = 'Culture', @column = 'Name', @action = 'Add', @userId = @userId, @key = @id, @newValue = @supplierId
	EXEC AuditAdd @table = 'Culture', @column = 'ParentCultureId', @action = 'Add', @userId = @userId, @key = @id, @newValue = @parentCultureId
	EXEC AuditAdd @table = 'Culture', @column = 'UserId', @action = 'Add', @userId = @userId, @key = @id, @newValue = @userId
	EXEC AuditAdd @table = 'Culture', @column = 'RecipeId', @action = 'Add', @userId = @userId, @key = @id, @newValue = @recipeId
	EXEC AuditAdd @table = 'Culture', @column = 'MediumType', @action = 'Add', @userId = @userId, @key = @id, @newValue = @mediumType
	EXEC AuditAdd @table = 'Culture', @column = 'OrderDate', @action = 'Add', @userId = @userId, @key = @id, @newValue = @orderDate
	EXEC AuditAdd @table = 'Culture', @column = 'CreateDate', @action = 'Add', @userId = @userId, @key = @id, @newValue = @createDate
	EXEC AuditAdd @table = 'Culture', @column = 'Strain', @action = 'Add', @userId = @userId, @key = @id, @newValue = @strain
	EXEC AuditAdd @table = 'Culture', @column = 'Notes', @action = 'Add', @userId = @userId, @key = @id, @newValue = @notes
	EXEC AuditAdd @table = 'Culture', @column = 'Generation', @action = 'Add', @userId = @userId, @key = @id, @newValue = @generation
	
	SELECT @id [Id]

	COMMIT TRAN

	RETURN 0 
END
GO

GRANT EXECUTE ON [dbo].[CultureAdd] TO [WebApi]