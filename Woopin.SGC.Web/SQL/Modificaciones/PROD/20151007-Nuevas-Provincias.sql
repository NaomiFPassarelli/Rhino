/*
Post-Deployment Script Template							
*/

SET DATEFORMAT ymd

SET IDENTITY_INSERT dbo.Localizacion ON;
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(20,'Santiago del Estero','Santiago del Estero',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(21,'Chaco','Chaco',40,1,0)
SET IDENTITY_INSERT dbo.Localizacion OFF;
