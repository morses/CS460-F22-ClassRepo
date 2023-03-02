CREATE TABLE [UserLog] (
  [ID]                  INT           NOT NULL IDENTITY(1, 1) PRIMARY KEY,
  [TimeStamp]			DATETIME	  NOT NULL,
  [IPAddress]           VARCHAR(45)   NOT NULL,
  [UserAgent]           NVARCHAR(150),
  [ASPNetIdentityId]    NVARCHAR(450),
  [ColorID]				INT		      NOT NULL
);

CREATE TABLE [Color] (
	[ID]         INT           NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[HexValue]	 VARCHAR(10)   NOT NULL
);

ALTER TABLE [UserLog] ADD CONSTRAINT [UserLog_Fk_Color] FOREIGN KEY ([ColorID]) REFERENCES [Color] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;