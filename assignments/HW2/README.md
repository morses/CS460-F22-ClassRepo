seed.sql is the entire seed script in one file.  If that's causing problems for you please use the seed\_1.sql, seed\_2.sql and seed\_3.sql files, running them one after another.  I just broke up the large file into 3 separate ones.  

Also, a Blue's Clues show had a description that didn't fit into my NVARCHAR(2048) so I increased that to NVARCHAR(MAX) which will definitely hold it.  If you already ran it with the earlier version, please rerun it.
