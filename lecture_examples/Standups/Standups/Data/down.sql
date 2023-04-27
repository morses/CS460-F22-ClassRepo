
-- #######################################
-- #            Our Model Tables         #
-- #######################################
ALTER TABLE [dbo].[SUPUsers] DROP CONSTRAINT [FK_dbo.SUPUsers_dbo.SUPGroups_ID];
ALTER TABLE [dbo].[SUPMeetings] DROP CONSTRAINT [FK_dbo.SUPMeetings_dbo.SUPUsers_ID];
ALTER TABLE [dbo].[SUPGroups] DROP CONSTRAINT [FK_dbo.SUPGroups_dbo.SUPAcademicYears_ID];

ALTER TABLE [dbo].[SUPCommentRatings] DROP CONSTRAINT [FK_dbo.SUPCommentRatings_dbo.SUPComments_ID];
ALTER TABLE [dbo].[SUPCommentRatings] DROP CONSTRAINT [FK_dbo.SUPCommentRatings_dbo.SUPUsers_ID];
--ALTER TABLE [dbo].[SUPComments] DROP CONSTRAINT [FK_dbo.SUPComments_dbo.SUPUsers_ID];
ALTER TABLE [dbo].[SUPComments] DROP CONSTRAINT [FK_dbo.SUPComments_dbo.SUPQuestions_ID];

DROP TABLE [dbo].[SUPGroups];
DROP TABLE [dbo].[SUPAdvisors];
DROP TABLE [dbo].[SUPUsers];
DROP TABLE [dbo].[SUPMeetings];

DROP TABLE [dbo].[SUPCommentRatings];
DROP TABLE [dbo].[SUPComments];
DROP TABLE [dbo].[SUPQuestions];
DROP TABLE [dbo].[SUPAcademicYears];