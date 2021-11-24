CREATE PROCEDURE [dbo].[GrainSpawnInnoculate]
(
	@id INT,
	@cultureId INT,
	@userId INT,
	@additionalNotes NVARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	IF NOT EXISTS (SELECT 1 FROM GrainSpawn WHERE Id = @id AND CultureId IS NULL)
		THROW 50001, 'No non-innoculated culture found for this ID', 1;

	DECLARE @existingNotes NVARCHAR(MAX)
	SELECT TOP 1 @existingNotes = Notes FROM [GrainSpawn] WHERE Id = @id
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
	DECLARE @createDate DATETIME = GETUTCDATE()

	UPDATE [GrainSpawn]
		SET
			CultureId = @cultureId,
			InnoculateUserId = @userId,
			InnoculateDate = @createDate
		WHERE Id = @id
		
	EXEC AuditAdd @table = 'GrainSpawn', @column = 'CultureId', @action = 'Update', @userId = @userId, @key = @id, @newValue = @cultureId, @oldValue = NULL
	EXEC AuditAdd @table = 'GrainSpawn', @column = 'InnoculateUserId', @action = 'Update', @userId = @userId, @key = @id, @newValue = @userId, @oldValue = NULL
	EXEC AuditAdd @table = 'GrainSpawn', @column = 'InnoculateDate', @action = 'Update', @userId = @userId, @key = @id, @newValue = @createDate, @oldValue = NULL
	IF @notes <> @existingNotes
		EXEC AuditAdd @table = 'GrainSpawn', @column = 'Notes', @action = 'Update', @userId = @userId, @key = @id, @newValue = @notes, @oldValue = @existingNotes

	COMMIT TRAN

	RETURN 0 
END
GO

GRANT EXECUTE ON [dbo].[GrainSpawnInnoculate] TO [WebApi]