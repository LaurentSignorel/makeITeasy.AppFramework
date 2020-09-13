﻿CREATE TABLE [dbo].[Car]
(
	[ID] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [BrandId] INT NOT NULL, 
    [ReleaseYear] INT NOT NULL, 
    [version] ROWVERSION NULL, 
    CONSTRAINT [FK_Car_ToBrand] FOREIGN KEY ([BrandId]) REFERENCES [Brand]([ID])
)
