CREATE PROCEDURE [dbo].[spCreateNewUser]
	@id nvarchar(128),
	@firstName nvarchar(50),
	@lastName nvarchar(50),
	@emailAddress nvarchar(256),
	@createdDate Datetime2
AS
begin
insert into [dbo].[User](id, FirstName, LastName, EmailAddress, CreatedDate)
values (@id,@firstName,@lastName, @emailaddress, @createdDate)
end
