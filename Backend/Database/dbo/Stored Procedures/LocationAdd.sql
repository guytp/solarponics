create procedure LocationAdd
(
	@name NVARCHAR(50)
)
as
begin
	SET NOCOUNT ON

	INSERT INTO [Location] ([Name]) VALUES(@name)

	SELECT SCOPE_IDENTITY() Id
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LocationAdd] TO [WebApi]
    AS [dbo];

