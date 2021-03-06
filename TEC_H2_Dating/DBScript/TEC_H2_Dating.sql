-- CTRL + SHIFT + E = Execute script

USE master

GO

IF EXISTS(select * from sys.databases where name='TEC_H2_Dating')
ALTER DATABASE TEC_H2_Dating SET SINGLE_USER WITH ROLLBACK IMMEDIATE

IF EXISTS(select * from sys.databases where name='TEC_H2_Dating')
DROP DATABASE TEC_H2_Dating

CREATE DATABASE TEC_H2_Dating

GO

USE TEC_H2_Dating

GO

CREATE TABLE Users (
	userID int IDENTITY(1,1) PRIMARY KEY,
    created datetime DEFAULT GETDATE(),
	username nvarchar(50) NOT NULL,
    userpassword nvarchar(256) NOT NULL,
    email nvarchar(320) NOT NULL
)

GO

CREATE TABLE Regions (
	regionID int IDENTITY(1,1) PRIMARY KEY,
	regionName nvarchar(50)
)

GO

CREATE TABLE RS_ProfileInterests (
    interestId int,
    profileID int
)

CREATE TABLE Interests (
    interestID int IDENTITY(1,1) PRIMARY KEY,
    interestName nvarchar(180) NOT NULL,
   -- profileID int NOT NULL -- FKey bliver tilføjet efter Profiles table er oprettet
)

GO

CREATE TABLE Profiles (
    profileID int IDENTITY(1,1) PRIMARY KEY,
    userID int NOT NULL FOREIGN KEY REFERENCES Users(userID),
    profileFirstName nvarchar(50) NOT NULL,
    profileLastName nvarchar(50) NOT NULL,
    profileBio nvarchar(280) NOT NULL,
    active bit NOT NULL default 1,
    sex bit NOT NULL,
    age int,
    country nvarchar(50),
    city nvarchar(50),
    regionId int NOT NULL FOREIGN KEY REFERENCES Regions(regionId)
)

GO

-- ALTER TABLE Interests ADD FOREIGN KEY (interestID) REFERENCES Profiles(profileID); -- Tilføj FKey nu hvor Profiles table er oprettet

GO

CREATE TABLE Payment_Methods (
    PM_typeID int IDENTITY(1,1) PRIMARY KEY,
    PM_typ nvarchar(50) NOT NULL   
)

GO

CREATE TABLE Payment_Information (
    PM_typeID int NOT NULL FOREIGN KEY REFERENCES Payment_Methods(PM_typeID),
    userID int NOT NULL FOREIGN KEY REFERENCES Users(userID),
    active bit NOT NULL DEFAULT 1,
    created datetime DEFAULT GETDATE()
)

GO

CREATE TABLE Images (
    imageID int IDENTITY(1,1) PRIMARY KEY,
    userID int NOT NULL FOREIGN KEY REFERENCES Users(userID),
    imageFile varbinary(max),
    created datetime DEFAULT GETDATE()
)

GO

CREATE TABLE Messages (
    messageID int IDENTITY(1,1) PRIMARY KEY,
    profileID int NOT NULL FOREIGN KEY REFERENCES Profiles(profileID),
    created datetime DEFAULT GETDATE(),
    content nvarchar(600) NOT NULL
)

GO

CREATE TABLE Matches (
    matchID int IDENTITY(1,1) PRIMARY KEY,
    profileID int NOT NULL FOREIGN KEY REFERENCES Profiles(profileID),
    created datetime DEFAULT GETDATE()
)

GO



INSERT INTO Regions (regionName)
VALUES 	('Nordjylland'),
		('Midtjylland'),
		('Sønderjylland'),
		('Fyn'),
		('Lolland'),
		('Falster'),
		('Møn'),
		('Sydsjælland'),
		('Vestsjælland'),
		('Nordsjælland'),
		('København')
		
/* GO

INSERT INTO Users (Username, userpassword, email)
VALUES ('Bruger', '376f30fadbf88b322a6fe61adda5bb72010ba720c74f7b588c0fe86d9dafb4a1', 'brugermail@mail.com')
GO

INSERT INTO Users (Username, userpassword, email)
VALUES ('Bruger2', '376f30fadbf88b322a6fe61adda5bb72010ba720c74f7b588c0fe86d9dafb4a1', 'bruger2mail@mail.com') */
GO

INSERT INTO Interests (InterestName)
VALUES 	('Musik'),
		('Mad'),
		('Rejser'),
		('Mad Moneyz'),
		('Biler'),
		('Netflix'),
		('Lange gåture på stranden'),
		('Bjergbestigning'),
		('Ekstrem-sport'),
		('Surfing'),
		('En times offentlig trasnport i en kold bus mandag morgen'),
		('Mode'),
		('Gaming'),
		('Brætspil'),
		('Fitness')

/* GO

INSERT INTO Profiles (userID, profileFirstName, profileLastName, profileBio, sex, age, country, city, regionId)
VALUES (1, 'Bruger', 'brugeren', 'min bio', 1, 19, 'Danmark', 'Snekkersten', '1')

GO

INSERT INTO Profiles (userID, profileFirstName, profileLastName, profileBio, sex, age, country, city, regionId)
VALUES (2, 'Brugerto', 'brugerento', 'bruger2 min bio', 1, 19, 'Danmark', 'Snekkersten', '2') */

GO

Create Index IX_Profiles_Age
ON Profiles (Age ASC)

