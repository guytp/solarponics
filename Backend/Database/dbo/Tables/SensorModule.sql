CREATE TABLE [dbo].[SensorModule] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [RoomId]           INT              NOT NULL,
    [SerialNumber]     NVARCHAR (50)    NOT NULL,
    [Driver]           NVARCHAR (50)    NOT NULL,
    [Name]             NVARCHAR (50)    NOT NULL,
	[IsDeleted]        BIT NOT NULL DEFAULT(0)
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Room] ([Id])
);

