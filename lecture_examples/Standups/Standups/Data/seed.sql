-- populate academic years table
INSERT INTO [dbo].[SUPAcademicYears] (Year) VALUES 
	('2016-2017'),
	('2017-2018'),
	('2018-2019'),
	('2019-2020'),
	('2020-2021'),
	('2021-2022'),
	('2022-2023'),
	('2023-2024'),
	('2024-2025'),
	('2025-2026');

INSERT INTO [dbo].[SUPAdvisors] (FirstName,LastName) VALUES
	('Scot','Morse'),
	('Becka','Morgan'),
	('Luke','Cordova');

INSERT INTO [dbo].[SUPGroups] (Name,Motto,SUPAdvisorID,SUPAcademicYearID) VALUES
	('Panda Programmers','Yum, bamboo!',1,7),
	('Night Owl','The night is ours, the future is yours!',1,7),
	('Otter Productions','do as you otter',1,7),
	('1.21GB','Git''r done before running OUTATIME',1,7),
	('Caiman Lizard','We''re not dangerous',1,7),
	('Team++','We''re bringing the tea',1,7);
