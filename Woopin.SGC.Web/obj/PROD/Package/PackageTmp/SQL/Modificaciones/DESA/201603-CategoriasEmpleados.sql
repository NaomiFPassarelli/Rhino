/*
Post-Deployment Script Template							
*/

SET IDENTITY_INSERT dbo.Combo ON;
insert into Combo(Id,Nombre, Activo) values(15,'Categorias de Empleados', 'true')
insert into Combo(Id,Nombre, Activo) values(16,'Tareas del Empleado', 'true')
SET IDENTITY_INSERT dbo.Combo OFF;


SET IDENTITY_INSERT dbo.ComboItem ON;
--160 a 229 categorias de empleados 
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(160,'Tizador', 5000, null, 'true', 15)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(161,'Empleados c', 3000, null, 'true', 15)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(162,'Encimador', 1000, null, 'true', 15)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(163,'Cortador', 5000, null, 'true', 15)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(164,'Empleado c produccion', 3000, null, 'true', 15)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(165,'Oficial', 1000, null, 'true', 15)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(166,'Medio oficial', 5000, null, 'true', 15)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(167,'Oficial calificado', 3000, null, 'true', 15)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(168,'Empleado c', 1000, null, 'true', 15)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(169,'Administrativo', 3000, null, 'true', 15)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(170,'Modelista', 1000, null, 'true', 15)


--230 a 239 tareas del empleado
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(230,'Administracion', null, null, 'true', 16)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(231,'Tizador', null, null, 'true', 16)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(232,'Produccion', null, null, 'true', 16)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(233,'Encimador', null, null, 'true', 16)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(234,'Cortador', null, null, 'true', 16)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(235,'Trabajo A Maquina', null, null, 'true', 16)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(236,'Operador De Maquinas', null, null, 'true', 16)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(237,'Confeccion De Prendas De Vestir, Excepto Piel,Cueros Y Sucedanos, Pilotos E Impermeables', null, null, 'true', 16)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(238,'Modelista', null, null, 'true', 16)
SET IDENTITY_INSERT dbo.ComboItem OFF;


SET IDENTITY_INSERT dbo.Combo ON;
insert into Combo(Id,Nombre, Activo) values(17,'Sexo', 'true')
SET IDENTITY_INSERT dbo.Combo OFF;


SET IDENTITY_INSERT dbo.ComboItem ON;
--240 a 241 sexo
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(240,'Hombre', null, null, 'true', 17)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(241,'Mujer', null, null, 'true', 17)
SET IDENTITY_INSERT dbo.ComboItem OFF;




SET IDENTITY_INSERT dbo.Combo ON;
insert into Combo(Id,Nombre, Activo) values(18,'Estado Civil', 'true')
SET IDENTITY_INSERT dbo.Combo OFF;


SET IDENTITY_INSERT dbo.ComboItem ON;
--242 a 247 Estado Civil
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(242,'Soltero', null, null, 'true', 18)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(243,'Casado', null, null, 'true', 18)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(244,'Divorciado', null, null, 'true', 18)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(245,'Viudo', null, null, 'true', 18)
SET IDENTITY_INSERT dbo.ComboItem OFF;




SET IDENTITY_INSERT dbo.Combo ON;
insert into Combo(Id,Nombre, Activo) values(19,'Sindicato', 'true')
SET IDENTITY_INSERT dbo.Combo OFF;


SET IDENTITY_INSERT dbo.ComboItem ON;
--250 a 289 Sindicato
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(251,'UCI', null, null, 'true', 19)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(252,'SETIA', null, null, 'true', 19)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(253,'SOIVA', null, null, 'true', 19)
SET IDENTITY_INSERT dbo.ComboItem OFF;


SET IDENTITY_INSERT dbo.Combo ON;
insert into Combo(Id,Nombre, Activo) values(20,'Obra Social', 'true')
SET IDENTITY_INSERT dbo.Combo OFF;


SET IDENTITY_INSERT dbo.ComboItem ON;
--290 a 330 Obra Social
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(291,'OSETYA', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(292,'OSPCN', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(293,'UCI', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(294,'OSPIV', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(295,'OSCEP', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(296,'OSPIF', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(297,'OSECAC', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(298,'O.S.De Relojeros Y Joyeros', null, null, 'true', 20)
SET IDENTITY_INSERT dbo.ComboItem OFF;


