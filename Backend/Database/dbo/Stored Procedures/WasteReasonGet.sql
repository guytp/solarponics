CREATE PROCEDURE [dbo].[WasteReasonGet]
AS
BEGIN
	SET NOCOUNT ON
	SELECT Id, [Reason] FROM WasteReason WHERE IsDeleted = 0 ORDER BY [Reason]
	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[WasteReasonGet] TO [WebApi]