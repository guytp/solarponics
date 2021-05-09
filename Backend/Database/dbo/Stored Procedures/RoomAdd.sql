create procedure RoomAdd
(
	@locationId INT,
	@name NVARCHAR(50)
)
as
begin
	SET NOCOUNT ON

	INSERT INTO [Room] (LocationId, [Name]) VALUES(@locationId, @name)

	SELECT SCOPE_IDENTITY() Id
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RoomAdd] TO [WebApi]
    AS [dbo];

