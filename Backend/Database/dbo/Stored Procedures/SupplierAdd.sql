CREATE PROCEDURE [dbo].[SupplierAdd]
(
	@name NVARCHAR(200),
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON

	SET XACT_ABORT ON

	BEGIN TRAN
	INSERT INTO Supplier ([Name]) VALUES(@name)

	DECLARE @id INT
	SELECT @id = SCOPE_IDENTITY()

	EXEC AuditAdd @table = 'Supplier', @column = 'Name', @action = 'Add', @userId = @userId, @key = @id, @newValue = @name


	COMMIT TRAN

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[SupplierAdd] TO [WebApi]