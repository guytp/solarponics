CREATE PROCEDURE [dbo].[GrainSpawnAddMix]
(
	@id INT,
	@userId INT,
	@date DATETIME,
	@notes NVARCHAR(MAX)
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

	BEGIN TRAN

	INSERT INTO GrainSpawnMix
		(Id, [Date], UserId)
		VALUES(@id, @date, @userId)

	EXEC AuditAdd @table = 'GrainSpawnMix', @column = 'Id', @action = 'Add', @userId = @userId, @key = @id, @newValue = @id
	EXEC AuditAdd @table = 'GrainSpawnMix', @column = 'UserId', @action = 'Add', @userId = @userId, @key = @id, @newValue = @userId
	EXEC AuditAdd @table = 'GrainSpawnMix', @column = 'Date', @action = 'Add', @userId = @userId, @key = @id, @newValue = @date

	IF (@notes IS NOT NULL AND @notes <> '')
	BEGIN
		DECLARE @existingNotes NVARCHAR(MAX)
		SELECT TOP 1 @existingNotes = Notes FROM [GrainSpawn] WHERE Id = @id
		DECLARE @newNotes NVARCHAR(MAX)
		IF (@existingNotes IS NOT NULL)
			SELECT @newNotes = @existingNotes
		IF (@newNotes IS NULL)
			SELECT @newNotes = @notes
		ELSE
			SELECT @newNotes = @newNotes + CHAR(13) + CHAR(10) + @notes

		UPDATE GrainSpawn SET Notes = @newNotes WHERE Id = @id
		EXEC AuditAdd @table = 'GrainSpawn', @column = 'Notes', @action = 'Update', @userId = @userId, @key = @id, @newValue = @newNotes, @oldValue = @existingNotes
	END

	COMMIT TRAN

	RETURN 0 
END
GO

GRANT EXECUTE ON [dbo].[GrainSpawnAddMix] TO [WebApi]