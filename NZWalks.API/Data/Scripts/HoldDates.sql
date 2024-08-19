 CREATE TABLE [dbo].[HoldDates](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[BeginHold] [datetime] NULL,
	[Release] [datetime] NULL,
 CONSTRAINT [PK_HoldDates] PRIMARY KEY ([Id])
 );

 INSERT INTO [dbo].[HoldDates] VALUES(1,'May - June','All new escape enrollment','2024-05-05','2024-07-15');
 INSERT INTO [dbo].[HoldDates] VALUES(2,'June - July','All existing annual / escape downward corrections','2024-06-21','2024-07-15');
 INSERT INTO [dbo].[HoldDates] VALUES(3,'July - Sep','Current year escape / annual corrections','2024-06-21','2024-09-15');
