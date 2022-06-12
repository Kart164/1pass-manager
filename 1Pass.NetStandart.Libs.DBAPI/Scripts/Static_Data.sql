INSERT INTO "Services" ("Id","Name")
    SELECT 0,'No service' WHERE (SELECT COUNT(*) FROM "Config")=0;
INSERT INTO "Config" ("Name", "Value") 
    SELECT 'DBVersion','0.0' WHERE (SELECT COUNT(*) FROM "Config")=0;