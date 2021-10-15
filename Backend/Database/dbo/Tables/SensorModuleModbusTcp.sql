CREATE TABLE [dbo].[SensorModuleModbusTcp] (
    [Id]               INT              NOT NULL,
    [IpAddress]        NVARCHAR (39)    NOT NULL,
    [Port]             SMALLINT         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Id]) REFERENCES [dbo].[SensorModuleModbusTcp] ([Id])
);

