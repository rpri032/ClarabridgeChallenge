CREATE PROCEDURE [dbo].[pressRelease_Get]
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SELECT
		PR.[Id],
		PR.[Title],
		PR.[DescriptionHtml],
		PR.[DatePublished],
		PR.[DateCreated],
		PR.[DateUpdated]
	FROM
		[dbo].[PressRelease] PR (NOLOCK)
	WHERE
		PR.[Id] = @Id
	
END