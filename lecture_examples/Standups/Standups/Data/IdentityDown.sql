ALTER TABLE [dbo].[AspNetRoleClaims] DROP CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId];
ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId];
ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId];
ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId];
ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId];
ALTER TABLE [dbo].[AspNetUserTokens] DROP CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId];

DROP TABLE [dbo].[AspNetRoles];
DROP TABLE [dbo].[AspNetUsers];
DROP TABLE [dbo].[AspNetRoleClaims];
DROP TABLE [dbo].[AspNetUserClaims];
DROP TABLE [dbo].[AspNetUserLogins];
DROP TABLE [dbo].[AspNetUserRoles];
DROP TABLE [dbo].[AspNetUserTokens];