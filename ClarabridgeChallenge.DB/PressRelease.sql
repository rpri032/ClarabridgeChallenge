CREATE TABLE [dbo].[PressRelease]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(50) NOT NULL, 
    [DescriptionHtml] NVARCHAR(MAX) NOT NULL, 
    [DatePublished] DATETIME NOT NULL, 
    [DateCreated] DATETIME NOT NULL, 
    [DateUpdated] DATETIME NULL
)
