CREATE TABLE [dbo].[Sensor] (
    [Id]                 INT              IDENTITY (1, 1) NOT NULL,
    [SensorModuleId]     INT              NOT NULL,
    [Type]               NVARCHAR (20)    NOT NULL,
    [Number]             TINYINT          NOT NULL,
    [WarningLowBelow]    DECIMAL (12, 4)  NOT NULL,
    [CriticalLowBelow]   DECIMAL (12, 4)  NOT NULL,
    [WarningHighAbove]   DECIMAL (12, 4)  NOT NULL,
    [CriticalHighAbove]  DECIMAL (12, 4)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([SensorModuleId]) REFERENCES [dbo].[SensorModule] ([Id])
);