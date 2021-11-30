CREATE TABLE [dbo].[Reading] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [SensorId] INT             NOT NULL,
    [Reading]  DECIMAL (12, 4) NOT NULL,
    [Time]     DATETIME        NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC),
    FOREIGN KEY ([SensorId]) REFERENCES [dbo].[Sensor] ([Id])
);
GO

CREATE INDEX IX_Reading_Time ON dbo.[Reading] ([Time])
GO

CREATE CLUSTERED INDEX IX_Reading_SensorId ON dbo.[Reading] ([SensorId])
GO