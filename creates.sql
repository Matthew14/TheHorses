ALTER TABLE race_result DROP CONSTRAINT FK_race_result_race;
DROP TABLE dbo.race;
--
ALTER TABLE race_result DROP CONSTRAINT FK_race_result_horse;
DROP TABLE dbo.horse;
--
DROP TABLE dbo.race_result;

-- Let's create:
CREATE TABLE race (id int IDENTITY PRIMARY KEY, name varchar(200), venue varchar(100), race_time datetime);
CREATE TABLE horse (id int IDENTITY PRIMARY KEY, name varchar(200));
CREATE TABLE race_result(
	race_id int constraint FK_race_result_race REFERENCES race(id), 
	horse_id int constraint FK_race_result_horse REFERENCES horse(id),
	position int
	CONSTRAINT PK_race_result PRIMARY KEY NONCLUSTERED ([race_id], [horse_id])
);