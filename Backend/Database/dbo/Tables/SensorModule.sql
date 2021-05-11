CREATE TABLE [dbo].[SensorModule] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [RoomId]           INT              NOT NULL,
    [SerialNumber]     NVARCHAR (50)    NOT NULL,
    [Name]             NVARCHAR (50)    NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Room] ([Id])
);

