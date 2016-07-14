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
--290 a 339 Obra Social
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(291,'OSETYA', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(292,'OSPCN', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(293,'UCI', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(294,'OSPIV', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(295,'OSCEP', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(296,'OSPIF', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(297,'OSECAC', null, null, 'true', 20)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(298,'O.S.De Relojeros Y Joyeros', null, null, 'true', 20)
SET IDENTITY_INSERT dbo.ComboItem OFF;


SET IDENTITY_INSERT dbo.Combo ON;
insert into Combo(Id,Nombre, Activo) values(21,'Banco Deposito', 'true')
SET IDENTITY_INSERT dbo.Combo OFF;

SET IDENTITY_INSERT dbo.ComboItem ON;
--340 a 359 Banco Deposito
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(340,'Banco Credicoop', null, null, 'true', 21)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(341,'Banco Galicia', null, null, 'true', 21)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(342,'Banco HSBC', null, null, 'true', 21)
insert into ComboItem(Id,Data, AdditionalData, AfipData, Activo, Combo_id) values(343,'Banco ICBC', null, null, 'true', 21)
SET IDENTITY_INSERT dbo.ComboItem OFF;


SET IDENTITY_INSERT [dbo].[Adicional] ON
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (1, N'Sueldo Mensual', NULL, NULL, 1, N'Remunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (2, N'Sueldo Basico', NULL, NULL, 1, N'Remunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (3, N'Falta Justificada', NULL, NULL, 1, N'Remunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (4, N'Falta Injustificada', NULL, NULL, 0, N'Remunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (5, N'Descuento de Horas', NULL, NULL, 0, N'Remunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (6, N'Antiguedad', CAST(0.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'0')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (7, N'Antiguedad', CAST(1.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'1')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (8, N'Antiguedad', CAST(4.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'2')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (9, N'Antiguedad', CAST(8.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'3')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (10, N'Antiguedad', CAST(10.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'5')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (11, N'Antiguedad', CAST(14.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'8')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (12, N'Antiguedad', CAST(16.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'10')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (13, N'Antiguedad', CAST(18.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'15')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (14, N'Antiguedad', CAST(20.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'20')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (15, N'Antiguedad', CAST(22.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'25')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (16, N'Antiguedad', CAST(25.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'30')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (17, N'Antiguedad', CAST(28.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'35')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (18, N'Antiguedad', CAST(35.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'40')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (1004, N'Premio asistencia y puntualidad', CAST(20.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (1005, N'Asistencia perfecta', CAST(5.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (1006, N'Refrigerio', NULL, CAST(35.90000 AS Decimal(19, 5)), 1, N'Remunerativo', 1, N'251')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (1007, N'Obra Social', CAST(3.00000 AS Decimal(19, 5)), NULL, 0, N'Descuento', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (1008, N'Jubilacion', CAST(11.00000 AS Decimal(19, 5)), NULL, 0, N'Descuento', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (1009, N'Ley 19032', CAST(3.00000 AS Decimal(19, 5)), NULL, 0, N'Descuento', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (1010, N'Cuota Sindical', CAST(3.00000 AS Decimal(19, 5)), NULL, 0, N'Descuento', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (2006, N'Anticipo', NULL, NULL, 0, N'Descuento', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (2008, N'Redondeo', NULL, NULL, 1, N'NoRemunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (3006, N'Suma No Remunerativa', NULL, NULL, 1, N'NoRemunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (3007, N'Diferencia de cuota sindical', NULL, CAST(550.00000 AS Decimal(19, 5)), 0, N'Descuento', 1, N'251')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (3008, N'Diferencia de cuota sindical', NULL, CAST(450.00000 AS Decimal(19, 5)), 0, N'Descuento', 1, N'252')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (3009, N'Diferencia de cuota sindical', NULL, CAST(400.00000 AS Decimal(19, 5)), 0, N'Descuento', 1, N'253')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (3010, N'Diferencia de obra social', NULL, CAST(400.00000 AS Decimal(19, 5)), 0, N'Descuento', 1, N'251')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (3011, N'Diferencia de obra social', NULL, CAST(500.00000 AS Decimal(19, 5)), 0, N'Descuento', 1, N'252')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (3012, N'Diferencia de obra social', NULL, CAST(550.00000 AS Decimal(19, 5)), 0, N'Descuento', 1, N'253')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (4007, N'Horas Extras', NULL, CAST(120.00000 AS Decimal(19, 5)), 1, N'Remunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (4008, N'Premio por produccion', CAST(22.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'251')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (4009, N'Premio por produccion', CAST(20.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'253')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (4010, N'SAC', CAST(50.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (5011, N'Beneficiario Obra Social', NULL, NULL, 0, N'Descuento', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (5012, N'Refrigerio', NULL, CAST(200.00000 AS Decimal(19, 5)), 1, N'Remunerativo', 1, N'252')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (5013, N'Refrigerio', NULL, CAST(300.00000 AS Decimal(19, 5)), 1, N'Remunerativo', 1, N'253')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (5014, N'Viaticos', NULL, NULL, 1, N'NoRemunerativo', 1, N'251')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (5015, N'Viaticos', NULL, NULL, 1, N'NoRemunerativo', 1, N'252')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (5016, N'Viaticos', NULL, NULL, 1, N'NoRemunerativo', 1, N'253')
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (6011, N'Suspension', NULL, NULL, 0, N'Remunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (6012, N'ART', NULL, NULL, 0, N'Remunerativo', 1, NULL)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription]) VALUES (6013, N'Licencia por Enfermedad', NULL, NULL, 0, N'Remunerativo', 1, NULL)
SET IDENTITY_INSERT [dbo].[Adicional] OFF



