CREATE TABLE [dbo].[ApplicationRole]
(
	[Id] NVARCHAR(128) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(256) NOT NULL,
    [NormalizedName] NVARCHAR(256) NOT NULL
)

GO

CREATE INDEX [IX_ApplicationRole_NormalizedName] ON [dbo].[ApplicationRole] ([NormalizedName])