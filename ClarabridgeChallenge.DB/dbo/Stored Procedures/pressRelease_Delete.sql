CREATE PROCEDURE [dbo].[pressRelease_Delete]
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	DELETE 
	FROM 
		[dbo].[PressRelease]
	WHERE
		[Id] = @Id
END