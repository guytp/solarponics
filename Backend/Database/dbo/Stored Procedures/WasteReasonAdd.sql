CREATE PROCEDURE [dbo].[WasteReasonAdd]
(
	@reason NVARCHAR(200),
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON

	SET XACT_ABORT ON
	BEGIN TRAN

	INSERT INTO [WasteReason] (Reason) VALUES(@reason)
	DECLARE @id INT
	SELECT @id = SCOPE_IDENTITY()
	
	
	EXEC AuditAdd @table = 'WasteReason', @column = 'Reason', @action = 'Add', @userId = @userId, @key = @id, @newValue = @reason

	COMMIT TRAN

	SELECT @id [Id]

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[WasteReasonAdd] TO [WebApi]
GO