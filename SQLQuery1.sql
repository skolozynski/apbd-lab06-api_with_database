CREATE TABLE Animal(
	IdAnimal INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(200) NOT NULL,
	Description nvarchar(200) NULL,
	Category nvarchar(200) NOT NULL,
	Area nvarchar(200) NOT NULL
);

INSERT INTO Animal VALUES (
	'Animal1', 'Desc1', 'Cat1', 'Area1'
);
INSERT INTO Animal VALUES (
	'Animal2', 'Desc2', 'Cat2', 'Area2'
);
INSERT INTO Animal VALUES (
	'Animal3', 'Desc3', 'Cat3', 'Area3'
);

SELECT * FROM Animal;
