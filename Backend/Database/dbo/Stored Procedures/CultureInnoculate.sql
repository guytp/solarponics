CREATE PROCEDURE [dbo].[CultureInnoculate]
(
	@id INT,
	@parentCultureId INT,
	@userId INT,
	@additionalNotes NVARCHAR(MAX),
	@date DATETIME
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	IF NOT EXISTS (SELECT 1 FROM Culture WHERE Id = @id AND RecipeId IS NOT NULL AND Strain IS NULL AND ParentCultureId IS NULL AND SupplierId IS NULL)
		THROW 50001, 'No non-innoculated culture found for this ID', 1;

	DECLARE @strain NVARCHAR(500)
	DECLARE @existingNotes NVARCHAR(MAX)
	DECLARE @supplierId INT
	SELECT TOP 1 @existingNotes = Notes FROM [Culture] WHERE Id = @id
	DECLARE @notes NVARCHAR(MAX)
	IF (@existingNotes IS NOT NULL)
		SELECT @notes = @existingNotes
	IF (@additionalNotes IS NOT NULL)
	BEGIN
		IF (@notes IS NULL)
			SELECT @notes = @additionalNotes
		ELSE
			SELECT @notes = @notes + CHAR(13) + CHAR(10) + @additionalNotes
	END

	SET XACT_ABORT ON
	BEGIN TRAN
	DECLARE @generation INT
	SELECT TOP 1 @strain = [Strain], @supplierId = SupplierId, @generation = Generation + 1 FROM Culture WHERE Id = @parentCultureId

	UPDATE [Culture]
		SET
			ParentCultureId = @parentCultureId,
			[Strain] = @strain,
			Notes = @notes,
			SupplierId = @supplierId,
			Generation = @generation,
			InnoculateUserId = @userId,
			InnoculateDate = @date
		WHERE Id = @id
		
	EXEC AuditAdd @table = 'Culture', @column = 'ParentCultureId', @action = 'Update', @userId = @userId, @key = @id, @newValue = @parentCultureId, @oldValue = NULL
	EXEC AuditAdd @table = 'Culture', @column = 'Strain', @action = 'Update', @userId = @userId, @key = @id, @newValue = @strain, @oldValue = NULL
	EXEC AuditAdd @table = 'Culture', @column = 'SupplierId', @action = 'Update', @userId = @userId, @key = @id, @newValue = @supplierId, @oldValue = NULL
	EXEC AuditAdd @table = 'Culture', @column = 'Generation', @action = 'Update', @userId = @userId, @key = @id, @newValue = @generation, @oldValue = NULL
	EXEC AuditAdd @table = 'Culture', @column = 'InnoculatedUserId', @action = 'Update', @userId = @userId, @key = @id, @newValue = @userId, @oldValue = NULL
	EXEC AuditAdd @table = 'Culture', @column = 'InnoculateDate', @action = 'Update', @userId = @userId, @key = @id, @newValue = @date, @oldValue = NULL
	IF @notes <> @existingNotes
		EXEC AuditAdd @table = 'Culture', @column = 'Notes', @action = 'Update', @userId = @userId, @key = @id, @newValue = @notes, @oldValue = @existingNotes

	COMMIT TRAN

	RETURN 0 
END
GO

GRANT EXECUTE ON [dbo].[CultureInnoculate] TO [WebApi]