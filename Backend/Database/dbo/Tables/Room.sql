CREATE TABLE [dbo].[Room] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [LocationId] INT           NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id])
);

