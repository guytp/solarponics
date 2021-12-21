CREATE PROCEDURE [dbo].[GrainSpawnShelfPlace]
(
	@id INT,
	@shelfId INT,
	@userId INT,
	@additionalNotes NVARCHAR(MAX),
	@date DATETIME
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	IF NOT EXISTS (SELECT 1 FROM GrainSpawn WHERE Id = @id AND CultureId IS NOT NULL)
		THROW 50001, 'No innoculated culture found for this ID', 1;

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

	UPDATE [GrainSpawn]
		SET
			ShelfId = @shelfId,
			ShelfPlaceUserId = @userId,
			ShelfPlaceDate = @date
		WHERE Id = @id
		
	EXEC AuditAdd @table = 'GrainSpawn', @column = 'ShelfId', @action = 'Update', @userId = @userId, @key = @id, @newValue = @shelfId, @oldValue = NULL
	EXEC AuditAdd @table = 'GrainSpawn', @column = 'ShelfPlaceUserId', @action = 'Update', @userId = @userId, @key = @id, @newValue = @userId, @oldValue = NULL
	EXEC AuditAdd @table = 'GrainSpawn', @column = 'ShelfPlaceDate', @action = 'Update', @userId = @userId, @key = @id, @newValue = @date, @oldValue = NULL
	IF @notes <> @existingNotes
		EXEC AuditAdd @table = 'GrainSpawn', @column = 'Notes', @action = 'Update', @userId = @userId, @key = @id, @newValue = @notes, @oldValue = @existingNotes

	COMMIT TRAN

	RETURN 0 
END
GO

GRANT EXECUTE ON [dbo].[GrainSpawnShelfPlace] TO [WebApi]