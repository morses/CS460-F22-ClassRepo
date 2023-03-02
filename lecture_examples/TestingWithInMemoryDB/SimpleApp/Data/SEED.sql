INSERT INTO [Color](HexValue)
	VALUES
	('0xFF0000'),
	('0x00FF00'),
	('0x0000FF');

INSERT INTO [UserLog](TimeStamp,IPAddress,UserAgent,ASPNetIdentityId,ColorID)
	VALUES
	('12/04/2017 09:04:22','0.0.0.0','Mozilla/5.0 (iPhone14,3; U; CPU iPhone OS 15_0 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) Version/10.0 Mobile/19A346 Safari/602.1',NULL,1),
	('12/04/2017 08:44:03','127.0.0.1','Mozilla/5.0 (PlayStation; PlayStation 5/2.26) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0 Safari/605.1.15','4b7959dc-2e9f-4fa9-ad38-d49ea70c8d32',1),
	('10/23/2020 00:12:13','FE80:CD00:0000:0CDE:1257:0000:211E:729C','Mozilla/5.0 (Linux; Android 12) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/80.0.3987.119','52f959dc-3e9a-4fa9-ad38-d49ea7448d32',2),
	('04/01/2019 12:32:00','127.0.0.1','Mozilla/5.0 (PlayStation; PlayStation 5/2.26) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0 Safari/605.1.15','4b7959dc-2e9f-4fa9-ad38-d49ea70c8d32',3);
