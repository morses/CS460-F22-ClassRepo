
-- #######################################
-- #            Our Model Tables         #
-- #######################################

-- ########### SUPAdvisors ###########
CREATE TABLE [dbo].[SUPAdvisors]
(
	[ID] INT IDENTITY (1,1) NOT NULL,
	[FirstName] NVARCHAR (50) NOT NULL,
	[LastName] NVARCHAR (50) NOT NULL,
	CONSTRAINT [PK_dbo.SUPAdvisors] PRIMARY KEY CLUSTERED ([ID] ASC)
);

-- ########### SUPGroups ###########
CREATE TABLE [dbo].[SUPGroups]
(
	[ID] INT IDENTITY (1,1) NOT NULL,
	[Name] NVARCHAR (50) NOT NULL,
	[Motto] NVARCHAR (100) NOT NULL,
	[SUPAdvisorID] INT,
	[SUPAcademicYearID] INT NOT NULL,
	CONSTRAINT [PK_dbo.SUPGroups] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_dbo.SUPGroups_dbo.SUPAdvisors_ID] FOREIGN KEY ([SUPAdvisorID]) 
		REFERENCES [dbo].[SUPAdvisors] ([ID])
);

-- ########### SUPUsers ###########
CREATE TABLE [dbo].[SUPUsers]
(
	[ID] INT IDENTITY (1,1)		NOT NULL,
	[FirstName] NVARCHAR (50)	NOT NULL,
	[LastName] NVARCHAR (50)	NOT NULL,
	[SUPGroupID] INT,
	[ASPNetIdentityID] NVARCHAR (128) NOT NULL,			-- Id into Identity User table, but NOT a FK on purpose
	CONSTRAINT [PK_dbo.SUPUsers] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_dbo.SUPUsers_dbo.SUPGroups_ID] FOREIGN KEY ([SUPGroupID]) 
		REFERENCES [dbo].[SUPGroups] ([ID])
);

-- ########### SUPMeetings ###########
-- finally, the table of meeting reports.
CREATE TABLE [dbo].[SUPMeetings]
(
	[ID] INT IDENTITY (1,1)		NOT NULL,
	[SubmissionDate] DATETIME	NOT NULL,
	[SUPUserID] INT				NOT NULL,
	[Completed] NVARCHAR(1000)	NOT NULL,
	[Planning] NVARCHAR(1000)	NOT NULL,
	[Obstacles] NVARCHAR(1000)	NOT NULL,
	CONSTRAINT [PK_dbo.SUPMeetings] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_dbo.SUPMeetings_dbo.SUPUsers_ID] FOREIGN KEY ([SUPUserID]) 
		REFERENCES [dbo].[SUPUsers] ([ID])
);


-- ########### SUPQuestions ############
-- Questions that generate comments
CREATE TABLE [dbo].[SUPQuestions]
(
	[ID] INT IDENTITY (1,1)			NOT NULL,
	[SubmissionDate] DATETIME		NOT NULL,
	[Question]		 NVARCHAR(1000)	NOT NULL,
	[Active]		 INT			NOT NULL,
	CONSTRAINT [PK_dbo.SUPQuestions] PRIMARY KEY CLUSTERED ([ID] ASC)
);

-- ########### SUPComments ###########
-- Comments from students
CREATE TABLE [dbo].[SUPComments]
(
	[ID] INT IDENTITY (1,1)			NOT NULL,
--	[SUPUserID]		 INT			NOT NULL,		-- leaving this out means the architecture enforces that we don't know who submitted the comment
	[SUPQuestionID]  INT			NOT NULL,
	[SubmissionDate] DATETIME		NOT NULL,
	[Comment]		 NVARCHAR(1000)	NOT NULL,
	[AdvisorRating]  INT			NOT NULL,
	CONSTRAINT [PK_dbo.SUPComments] PRIMARY KEY CLUSTERED ([ID] ASC),
--	CONSTRAINT [FK_dbo.SUPComments_dbo.SUPUsers_ID] FOREIGN KEY ([SUPUserID]) REFERENCES [dbo].[SUPUsers] ([ID]),
	CONSTRAINT [FK_dbo.SUPComments_dbo.SUPQuestions_ID] FOREIGN KEY ([SUPQuestionID]) REFERENCES [dbo].[SUPQuestions] ([ID])
);

-- ########### SUPCommentRatings ###########
-- Ratings by the students on questions
CREATE TABLE [dbo].[SUPCommentRatings]
(
	[ID] INT IDENTITY (1,1)			NOT NULL,
	[SUPCommentID]    INT			NOT NULL,
	[SUPRaterUserID]  INT			NOT NULL,
	[RatingDate]	  DATETIME		NOT NULL,
	[RatingValue]	  INT			NOT NULL,
	CONSTRAINT [PK_dbo.SUPCommentRatings] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_dbo.SUPCommentRatings_dbo.SUPComments_ID] FOREIGN KEY ([SUPCommentID]) REFERENCES [dbo].[SUPComments] ([ID]),
	CONSTRAINT [FK_dbo.SUPCommentRatings_dbo.SUPUsers_ID] FOREIGN KEY ([SUPRaterUserID]) REFERENCES [dbo].[SUPUsers] ([ID])
);

-- ########### SUPAcademicYears ###########
-- Academic Year table, so we know which students/groups to display each year
CREATE TABLE [dbo].[SUPAcademicYears]
(
	[ID]	  INT IDENTITY (1,1)	NOT NULL,
	[Year]    CHAR (9)				NOT NULL,
	CONSTRAINT [PK_dbo.SUPAcademicYears] PRIMARY KEY CLUSTERED ([ID] ASC)
);

ALTER TABLE [dbo].[SUPGroups] ADD CONSTRAINT [FK_dbo.SUPGroups_dbo.SUPAcademicYears_ID] FOREIGN KEY ([SUPAcademicYearID]) REFERENCES [dbo].[SUPAcademicYears] ([ID]); 