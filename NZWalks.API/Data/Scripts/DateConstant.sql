CREATE TABLE [ARP].[DateConstants](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Value] [datetime] NOT NULL,
	[IsSystemDate] [bit] NULL,
 CONSTRAINT [PK_DateConstants] PRIMARY KEY ([Id])
 )
