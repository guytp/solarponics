CREATE TABLE [dbo].[Reading] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [SensorId] INT             NOT NULL,
    [Reading]  DECIMAL (12, 4) NOT NULL,
    [Time]     DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([SensorId]) REFERENCES [dbo].[Sensor] ([Id])
);

