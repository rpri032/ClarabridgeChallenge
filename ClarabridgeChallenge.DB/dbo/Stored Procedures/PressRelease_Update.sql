CREATE PROCEDURE [dbo].[PressRelease_Update]
	@Id UNIQUEIDENTIFIER, 
	@Title NVARCHAR(50),
	@DescriptionHtml NVARCHAR(MAX),
	@DatePublished DATETIME,
	@DateUpdated DATETIME
AS

UPDATE [dbo].[PressRelease]
SET
	[Title] = @Title,
	[DescriptionHtml] = @DescriptionHtml,
	[DatePublished] = @DatePublished,
	[DateUpdated] = @DateUpdated
WHERE
	[Id] = @Id

RETURN