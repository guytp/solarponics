CREATE TABLE [dbo].[GrainSpawnMix]
(
	[Id] INT NOT NULL,
	[Date] DATETIME NOT NULL,
	[UserId] INT NOT NULL,
	CONSTRAINT PK_GrainSpawnMix PRIMARY KEY (Id, [Date]),
	CONSTRAINT FK_GrainSpawnMix_GrainSpawn FOREIGN KEY (Id) REFERENCES [GrainSpawn](Id),
	CONSTRAINT FK_GrainSpawnMix_User FOREIGN KEY (UserId) REFERENCES [User](Id)
)