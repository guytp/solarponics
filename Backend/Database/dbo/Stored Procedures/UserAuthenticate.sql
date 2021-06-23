CREATE PROCEDURE [dbo].[UserAuthenticate]
	@userId SMALLINT,
	@pin SMALLINT
AS
BEGIN
	SELECT TOP 1 [Id], [UserId], [Name] FROM [User] WHERE UserId = @userId AND Pin = @pin
END
RETURN 0

GRANT EXECUTE
    ON OBJECT::[dbo].[UserAuthenticate] TO [WebApi]
    AS [dbo];

