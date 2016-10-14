CREATE PROCEDURE [dbo].[PressRelease_Add]
	@Id UNIQUEIDENTIFIER, 
	@Title NVARCHAR(50),
	@DescriptionHtml NVARCHAR(MAX),
	@DatePublished DATETIME,
	@DateCreated DATETIME
AS

INSERT INTO [dbo].[PressRelease] (
	[Id],
	[Title],
	[DescriptionHtml],
	[DatePublished],
	[DateCreated]
) VALUES (
	@Id,
	@Title,
	@DescriptionHtml,
	@DatePublished,
	@DateCreated)

RETURN