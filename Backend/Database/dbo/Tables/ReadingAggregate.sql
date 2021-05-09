CREATE TABLE [dbo].[ReadingAggregate] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [SensorId]       INT             NOT NULL,
    [ReadingMinimum] DECIMAL (12, 4) NOT NULL,
    [ReadingMaximum] DECIMAL (12, 4) NOT NULL,
    [ReadingAverage] DECIMAL (12, 4) NOT NULL,
    [NumberReadings] INT             NOT NULL,
    [PeriodBegin]    DATETIME        NOT NULL,
    [PeriodType]     NVARCHAR (3)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([SensorId]) REFERENCES [dbo].[Sensor] ([Id])
);

