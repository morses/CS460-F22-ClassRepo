CREATE TABLE [UserLog] (
  [ID]                  INT           NOT NULL IDENTITY(1, 1) PRIMARY KEY,
  [TimeStamp]			DATETIME	  NOT NULL,
  [IPAddress]           VARCHAR(45)   NOT NULL,
  [UserAgent]           NVARCHAR(150),
  [ASPNetIdentityId]    NVARCHAR(450)
);
