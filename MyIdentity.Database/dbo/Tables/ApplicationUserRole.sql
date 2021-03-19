CREATE TABLE [dbo].[ApplicationUserRole]
(
	[UserId] NVARCHAR(128) NOT NULL,
	[RoleId] NVARCHAR(128) NOT NULL
    PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_ApplicationUserRole_User] FOREIGN KEY ([UserId]) REFERENCES [ApplicationUser]([Id]),
    CONSTRAINT [FK_ApplicationUserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [ApplicationRole]([Id])
)