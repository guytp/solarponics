CREATE PROCEDURE [dbo].[AuditAdd]
(
	@table NVARCHAR(200),
	@column NVARCHAR(200) = NULL,
	@action NVARCHAR(20),
	@userId INT,
	@key INT,
	@oldValue NVARCHAR(MAX) = NULL,
	@newValue NVARCHAR(MAX) = NULL
)
AS
BEGIN
	SET NOCOUNT ON

	IF @oldValue = @newValue
		RETURN 0

	INSERT INTO [Audit] ([Table], [Column], [Action], [UserId], [Key], [OldValue], [NewValue]) VALUES(@table, @column, @action, @userId, @key, @oldValue, @newValue)

	RETURN 0
END