CREATE TABLE [dbo].[Sensor] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [SensorModuleId] INT           NOT NULL,
    [Type]           NVARCHAR (20) NOT NULL,
    [Number]         TINYINT       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([SensorModuleId]) REFERENCES [dbo].[SensorModule] ([Id])
);

