CREATE PROCEDURE [dbo].[FruitingBlockInnoculate]
(
	@id INT,
	@grainSpawnId INT,
	@date DATETIME,
	@userId INT,
	@additionalNotes NVARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	IF NOT EXISTS (SELECT 1 FROM FruitingBlock WHERE Id = @id AND GrainSpawnId IS NULL)
		THROW 50001, 'No non-innoculated grain spawn found for this ID', 1;

	DECLARE @existingNotes NVARCHAR(MAX)
	SELECT TOP 1 @existingNotes = Notes FROM [FruitingBlock] WHERE Id = @id
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

	UPDATE [FruitingBlock]
		SET
			GrainSpawnId = @grainSpawnId,
			InnoculateUserId = @userId,
			InnoculateDate = @date
		WHERE Id = @id
		
	EXEC AuditAdd @table = 'FruitingBlock', @column = 'GrainSpawnId', @action = 'Update', @userId = @userId, @key = @id, @newValue = @grainSpawnId, @oldValue = NULL
	EXEC AuditAdd @table = 'FruitingBlock', @column = 'InnoculateUserId', @action = 'Update', @userId = @userId, @key = @id, @newValue = @userId, @oldValue = NULL
	EXEC AuditAdd @table = 'FruitingBlock', @column = 'InnoculateDate', @action = 'Update', @userId = @userId, @key = @id, @newValue = @date, @oldValue = NULL
	IF @notes <> @existingNotes
		EXEC AuditAdd @table = 'FruitingBlock', @column = 'Notes', @action = 'Update', @userId = @userId, @key = @id, @newValue = @notes, @oldValue = @existingNotes

	COMMIT TRAN

	RETURN 0 
END
GO

GRANT EXECUTE ON [dbo].[FruitingBlockInnoculate] TO [WebApi]