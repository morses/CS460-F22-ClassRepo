CREATE TABLE [Person] 
(
  [ID]			INT          NOT NULL IDENTITY(1, 1) PRIMARY KEY,
  [Age]			INT			 NOT NULL,
  [Name]		NVARCHAR(50) NOT NULL,
  [Anniversary] DATETIME	 NOT NULL
);
