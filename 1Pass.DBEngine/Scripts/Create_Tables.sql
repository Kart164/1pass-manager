﻿CREATE TABLE IF NOT EXISTS "Services" (
	"Id"	INTEGER NOT NULL,
	"Name"	TEXT NOT NULL UNIQUE,
	"LastUpdate" TEXT NOT NULL DEFAULT CURRENT_DATE,
	PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "Accounts" (
	"Id"	INTEGER NOT NULL,
	"Username"	TEXT NOT NULL,
	"Password"	TEXT NOT NULL,
	"ServiceId"	INTEGER NOT NULL DEFAULT 0,
	"LastUpdate" TEXT NOT NULL DEFAULT CURRENT_DATE,
	FOREIGN KEY("ServiceId") REFERENCES "Services"("Id") ON DELETE SET DEFAULT,
	PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TABLE "Config" (
	"Name"	TEXT NOT NULL UNIQUE,
	"Value"	TEXT
);
