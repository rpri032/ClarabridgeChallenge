CREATE PROCEDURE [dbo].[pressRelease_GetAll]
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
	ORDER BY
		PR.[DatePublished] DESC

END