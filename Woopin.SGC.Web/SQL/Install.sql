SET DATEFORMAT ymd
/*
	Limpiar tablas ordenadamente
*/
delete from UsuarioOrganizacion
delete from ListaPreciosItem
delete from webpages_UsersInRoles
delete from webpages_Membership
delete from webpages_Roles
delete from UsuarioOrganizacion
delete from GrupoEgreso
delete from GrupoIngreso
delete from Chequera
delete from ImputacionVenta
delete from ImputacionCompra
delete from ComprobanteRetencion
delete from BloqueoContable
delete from DepositoItem
delete from Deposito
delete from CancelacionTarjeta
delete from PagoTarjeta
delete from TarjetaCredito
delete from Transferencia
delete from Cheque
delete from OtroEgresoPago
delete from OtroEgresoItem
delete from OtroEgreso
delete from OrdenPagoValorItem
delete from CobranzaValorItem
delete from OrdenPagoValorItem
delete from CobranzaComprobanteItem
delete from Cobranza
delete from OrdenPagoComprobanteItem
delete from OrdenPago
delete from ValorIngresado
delete from MovimientoFondo
delete from DetalleComprobanteCompra
delete from ComprobanteCompra
delete from MovimientoFondo
delete from DetalleComprobanteVenta
delete from ObservComprobanteVenta
delete from ComprobanteVenta
delete from AsientoItem
delete from Asiento
delete from ChequePropio
delete from Ejercicio
delete from HistorialCaja
delete from HistorialCuentaBancaria
delete from Caja
delete from CuentaBancaria
delete from Banco
delete from Proveedor
delete from Cliente
delete from GrupoEconomico
delete from CategoriaIVA
delete from Sucursal
delete from Retencion
delete from Valor
delete from Moneda
delete from Articulo
delete from RubroCompra
delete from Cuenta
delete from Talonario
update usuario set OrganizacionActual_id = null
delete from Organizacion
delete from Usuario
delete from Localizacion
delete from ComboItem
delete from Combo
delete from OrganizacionModulo
delete from Localidad
delete from Concepto



DBCC CHECKIDENT ('Combo',RESEED, 0)
SET IDENTITY_INSERT dbo.Combo ON;
insert into Combo (Id,Nombre,Activo) values(1,'Letra de Comprobante', 1)
--insert into Combo (Id,Nombre,Activo) values(2,'Condicion de Compra',1) -- DISPONIBLE!
insert into Combo (Id,Nombre,Activo) values(3,'Movimiento de Fondos', 1)
insert into Combo (Id,Nombre,Activo) values(4,'Paises',1)
insert into Combo (Id,Nombre,Activo) values(5,'Tipo Comprobantes Venta', 1)
insert into Combo (Id,Nombre,Activo) values(6,'Condicion de Compra y Venta',1)
insert into Combo (Id,Nombre,Activo) values(7,'Tipo de Valores Tesoreria',1)
insert into Combo (Id,Nombre,Activo) values(8,'Tipo Comprobante Compra',1)
insert into Combo (Id,Nombre,Activo) values(9,'Tipos de IVA',1)
insert into Combo (Id,Nombre,Activo) values(10,'Tipos de Cobranzas',1)
insert into Combo (Id,Nombre,Activo) values(11,'Tipos de Ordenes de Pago',1)
insert into Combo (Id,Nombre,Activo) values(12,'Actividad de la Organizaci√≥n',1)
insert into Combo (Id,Nombre,Activo) values(13,'Categoria IVA de la Organizaci√≥n',1)
insert into Combo (Id,Nombre,Activo) values(14,'Unidades de Medida',1)
INSERT INTO [dbo].[Combo] ([Id], [Nombre], [Activo]) VALUES (15, N'Categorias de Empleados', 1)
INSERT INTO [dbo].[Combo] ([Id], [Nombre], [Activo]) VALUES (16, N'Tareas del Empleado', 1)
INSERT INTO [dbo].[Combo] ([Id], [Nombre], [Activo]) VALUES (17, N'Sexo', 1)
INSERT INTO [dbo].[Combo] ([Id], [Nombre], [Activo]) VALUES (18, N'Estado Civil', 1)
INSERT INTO [dbo].[Combo] ([Id], [Nombre], [Activo]) VALUES (19, N'Sindicato', 1)
INSERT INTO [dbo].[Combo] ([Id], [Nombre], [Activo]) VALUES (20, N'Obra Social', 1)
INSERT INTO [dbo].[Combo] ([Id], [Nombre], [Activo]) VALUES (21, N'Banco Deposito', 1)
SET IDENTITY_INSERT dbo.Combo OFF;

DBCC CHECKIDENT ('ComboItem',RESEED, 0)
SET IDENTITY_INSERT dbo.ComboItem ON;
-- del 1 al 10 son las letras de comprobante
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(1,'A',1,null,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(2,'B',1,null,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(3,'C',1,null,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(4,'E',1,null,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(5,'M',1,null,1)
-- del 60 al 69 Condiciones de venta
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(10,'Contado',6,'0',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(11,'7 Dias',6,'7',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(12,'15 Dias',6,'15',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(13,'30 Dias',6,'30',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(14,'45 Dias',6,'45',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(15,'60 Dias',6,'60',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(16,'90 Dias',6,'90',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(17,'120 Dias',6,'120',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(18,'150 Dias',6,'150',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(19,'Cuenta Corriente',6,'0',1)
-- del 30 al 39 los movimientos de fondos
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(30,'Deposito',3,'1',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(31,'Extracci√≥n',3,'-1',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(32,'Transferencia entre Bancos',3,'0',1)
-- del 40 al 49 los paises
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(40,'Argentina',4,null,1,'200')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(41,'Brazil',4,null,1,'203')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(42,'Chile',4,null,1, '208')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(43,'Uruguay',4,null,1, '225')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(44,'Ecuador',4,null,1, '210')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(45,'Venezuela',4,null,1, '226')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(46,'Paraguay',4,null,1, '221')
<<<<<<< HEAD
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(47,'Per˙',4,null,1, '222')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(48,'Otro PaÌs',4,null,1,null)
=======
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(47,'Per√∫',4,null,1, '222')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(48,'Otro Pa√≠s',4,null,1,null)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
-- del 50 al 69 tipos de comprobantes venta
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(51,'Factura',5,'1',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(52,'Nota de Credito',5,'-1',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(53,'Nota de Debito',5,'1',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(54,'FE Factura A',5,'1',1,'1')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(55,'FE Factura B',5,'1',1,'6')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(56,'FE Factura C',5,'1',1,'11')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(57,'FE Nota de Credito A',5,'-1',1,'3')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(58,'FE Nota de Credito B',5,'-1',1,'8')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(59,'FE Nota de Credito C',5,'-1',1,'13')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(60,'FE Nota de Debito A',5,'1',1,'2')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(61,'FE Nota de Debito B',5,'1',1,'7')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(62,'FE Nota de Debito C',5,'1',1,'12')
-- del 70 al 79 Tipo de Valor
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(70,'Cheque',7,null,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(71,'Efectivo',7,null,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(72,'Transferencia',7,null,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(73,'Cheque Propio',7,null,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(74,'Tarjeta de Credito',7,null,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(75,'Retencion',7,null,1)
-- del 80 al 89 Tipos de Comprobante Compra
--insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(80,'Sin Comprobante',8,1,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(81,'Factura',8,1,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(82,'Nota de Debito',8,1,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(83,'Nota de Credito',8,-1,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(84,'Ticket',8,1,1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(85,'Ticket Factura',8,1,1)
-- del 90 al 99 Tipos de iva
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(90,'No Gravado',9,-1,1,'1')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(91,'Exento',9,0,1,'3')
--insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(92,'0%',9,0,1,'3')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(93,'10,5%',9,10.5,1,'4')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(94,'21%',9,21,1,'5')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(95,'27%',9,27,1,'6')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(96,'5%',9,5,1,'8')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(97,'2.5%',9,2.5,1,'9')
-- del 100 al 109 Tipos de Tipos de Cobranzas
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(100,'Recibo',10,null,1)
--insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(101,'FE Recibo A',10,null,1,'4')
--insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(102,'FE Recibo B',10,null,1,'9')
--insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(103,'FE Recibo C',10,null,1,'15')
-- del 110 al 119 Tipos de Ordenes de Pago
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(110,'Orden de Pago',11,0,1)
-- del 120 - Tipo de Servicio
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo,AfipData) values(120,'Venta de Productos',12,0,1,'1')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo,AfipData) values(121,'Venta de Servicios',12,0,1,'2')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo,AfipData) values(122,'Venta de Productos y Servicios',12,0,1,'3')
-- del 130 - Tipo categoria iva de la organizacion
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo,AfipData) values(130,'Responsable Inscripto',13,0,1,null)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo,AfipData) values(131,'Monotributista',13,0,1,null)
-- del 140 - Unidades de Medida.
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo,AfipData) values(140,'Unidad',14,0,1,null)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo,AfipData) values(141,'m2',14,0,1,null)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo,AfipData) values(142,'m3',14,0,1,null)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo,AfipData) values(143,'cm',14,0,1,null)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo,AfipData) values(144,'l',14,0,1,null)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo,AfipData) values(145,'ml',14,0,1,null)
--Categoria
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (160, N'Tizador', N'5000', NULL, 1, 15)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (161, N'Empleados c', N'3000', NULL, 1, 15)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (162, N'Encimador', N'1000', NULL, 1, 15)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (163, N'Cortador', N'5000', NULL, 1, 15)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (164, N'Empleado c produccion', N'3000', NULL, 1, 15)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (165, N'Oficial', N'1000', NULL, 1, 15)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (166, N'Medio oficial', N'5000', NULL, 1, 15)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (167, N'Oficial calificado', N'3000', NULL, 1, 15)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (168, N'Empleado c', N'1000', NULL, 1, 15)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (169, N'Administrativo', N'3000', NULL, 1, 15)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (170, N'Modelista', N'1000', NULL, 1, 15)
--Tarea
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (230, N'Administracion', NULL, NULL, 1, 16)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (231, N'Tizador', NULL, NULL, 1, 16)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (232, N'Produccion', NULL, NULL, 1, 16)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (233, N'Encimador', NULL, NULL, 1, 16)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (234, N'Cortador', NULL, NULL, 1, 16)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (235, N'Trabajo A Maquina', NULL, NULL, 1, 16)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (236, N'Operador De Maquinas', NULL, NULL, 1, 16)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (237, N'Confeccion De Prendas De Vestir, Excepto Piel,Cueros Y Sucedanos, Pilotos E Impermeables', NULL, NULL, 1, 16)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (238, N'Modelista', NULL, NULL, 1, 16)
--Genero
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (240, N'Hombre', NULL, NULL, 1, 17)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (241, N'Mujer', NULL, NULL, 1, 17)
--Estado Civil
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (242, N'Soltero', NULL, NULL, 1, 18)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (243, N'Casado', NULL, NULL, 1, 18)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (244, N'Divorciado', NULL, NULL, 1, 18)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (245, N'Viudo', NULL, NULL, 1, 18)
--Sindicato
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (251, N'UCI', NULL, NULL, 1, 19)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (252, N'SETIA', NULL, NULL, 1, 19)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (253, N'SOIVA', NULL, NULL, 1, 19)
--Obra Social
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (291, N'OSETYA', NULL, NULL, 1, 20)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (292, N'OSPCN', NULL, NULL, 1, 20)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (293, N'UCI', NULL, NULL, 1, 20)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (294, N'OSPIV', NULL, NULL, 1, 20)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (295, N'OSCEP', NULL, NULL, 1, 20)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (296, N'OSPIF', NULL, NULL, 1, 20)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (297, N'OSECAC', NULL, NULL, 1, 20)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (298, N'O.S.De Relojeros Y Joyeros', NULL, NULL, 1, 20)
--Banco Deposito
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (340, N'Banco Credicoop', NULL, NULL, 1, 21)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (341, N'Banco Galicia', NULL, NULL, 1, 21)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (342, N'Banco HSBC', NULL, NULL, 1, 21)
INSERT INTO [dbo].[ComboItem] ([Id], [Data], [AdditionalData], [AfipData], [Activo], [Combo_id]) VALUES (343, N'Banco ICBC', NULL, NULL, 1, 21)
SET IDENTITY_INSERT dbo.ComboItem OFF;
DBCC CHECKIDENT ('ComboItem',RESEED, 200)


SET IDENTITY_INSERT [dbo].[Localizacion] ON
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (1, N'Buenos Aires', N'Buenos Aires', 1, 1, 40)
<<<<<<< HEAD
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (2, N'CÛrdoba', N'CÛrdoba', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (3, N'Corrientes', N'Corrientes', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (4, N'Formosa', N'Formosa', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (5, N'Entre RÌos', N'Entre RÌos', 1, 0, 40)
=======
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (2, N'C√≥rdoba', N'C√≥rdoba', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (3, N'Corrientes', N'Corrientes', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (4, N'Formosa', N'Formosa', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (5, N'Entre R√≠os', N'Entre R√≠os', 1, 0, 40)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (6, N'Misiones', N'Misiones', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (7, N'Chubut', N'Chubut', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (8, N'Mendoza', N'Mendoza', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (9, N'San Juan', N'San Juan', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (10, N'Santa Fe', N'Santa Fe', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (11, N'Tierra del Fuego', N'Tierra del Fuego', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (12, N'San Luis', N'San Luis', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (13, N'La Pampa', N'La Pampa', 1, 0, 40)
<<<<<<< HEAD
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (14, N'Tucum·n', N'Tucum·n', 1, 0, 40)
=======
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (14, N'Tucum√°n', N'Tucum√°n', 1, 0, 40)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (15, N'Jujuy', N'Jujuy', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (16, N'Chaco', N'Chaco', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (17, N'Salta', N'Salta', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (18, N'Santa Cruz', N'Santa Cruz', 1, 0, 40)
<<<<<<< HEAD
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (19, N'RÌo Negro', N'RÌo Negro', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (20, N'Catamarca', N'Catamarca', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (21, N'NeuquÈn', N'NeuquÈn', 1, 0, 40)
=======
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (19, N'R√≠o Negro', N'R√≠o Negro', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (20, N'Catamarca', N'Catamarca', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (21, N'Neuqu√©n', N'Neuqu√©n', 1, 0, 40)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (22, N'Santiago del Estero', N'Santiago del Estero', 1, 0, 40)
INSERT INTO [dbo].[Localizacion] ([Id], [Nombre], [Provincia], [Activo], [Predeterminado], [Pais_id]) VALUES (23, N'La Rioja', N'La Rioja', 1, 0, 40)
SET IDENTITY_INSERT [dbo].[Localizacion] OFF


SET IDENTITY_INSERT [dbo].[Localidad] ON
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (1, N'17 de Agosto', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (2, N'25 de Mayo', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (3, N'9 de Julio / La NiÒa', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (3, N'9 de Julio / La Ni√±a', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (4, N'Acassuso', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (5, N'Aguas Verdes', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (6, N'Alberti', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (7, N'Arenas Verdes', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (8, N'Arrecifes', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (9, N'Avellaneda', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (10, N'Ayacucho', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (11, N'Azul', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (12, N'BahÌa Blanca', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (13, N'BahÌa San Blas', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (12, N'Bah√≠a Blanca', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (13, N'Bah√≠a San Blas', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (14, N'Balcarce', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (15, N'Balneario Marisol', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (16, N'Balneario Orense', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (17, N'Balneario Reta', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (18, N'Balneario San Cayetano', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (19, N'Baradero', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (20, N'Bella Vista', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (21, N'Benito Ju·rez', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (21, N'Benito Ju√°rez', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (22, N'Berazategui', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (23, N'Berisso', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (24, N'Boulogne', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (25, N'Bragado', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (26, N'Brandsen', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (27, N'Campana', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (28, N'Capilla del SeÒor', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (29, N'Capital Federal', 1, 1, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (30, N'Capit·n Sarmiento', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (28, N'Capilla del Se√±or', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (29, N'Capital Federal', 1, 1, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (30, N'Capit√°n Sarmiento', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (31, N'Carapachay', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (32, N'Carhue', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (33, N'Carlos Keen', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (34, N'Carmen de Areco', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (35, N'Carmen de Patagones', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (36, N'Caseros', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (37, N'Castelar', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (38, N'Castelli', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (39, N'Chacabuco', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (40, N'Chascom˙s', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (41, N'Chivilcoy', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (42, N'City Bell', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (43, N'Ciudadela', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (44, N'ClaromecÛ', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (45, N'ColÛn', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (46, N'Coronel Dorrego', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (47, N'Coronel Pringles', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (48, N'Coronel Su·rez', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (40, N'Chascom√∫s', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (41, N'Chivilcoy', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (42, N'City Bell', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (43, N'Ciudadela', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (44, N'Claromec√≥', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (45, N'Col√≥n', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (46, N'Coronel Dorrego', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (47, N'Coronel Pringles', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (48, N'Coronel Su√°rez', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (49, N'Darregueira', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (50, N'Dunamar', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (51, N'Escobar', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (52, N'Ezeiza', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (53, N'Florencio Varela', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (54, N'Florida', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (55, N'FortÌn Mercedes', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (55, N'Fort√≠n Mercedes', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (56, N'Garin', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (57, N'General Arenales', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (58, N'General Belgrano', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (59, N'General Madariaga', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (60, N'General Villegas', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (61, N'Gral. Daniel Cerri', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (62, N'Gran Buenos Aires', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (63, N'GuaminÌ', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (64, N'Haedo', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (65, N'Huanguelen', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (66, N'Hurlingham', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (67, N'Isla MartÌn GarcÌa', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (68, N'Ituzaingo', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (69, N'JunÌn', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (63, N'Guamin√≠', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (64, N'Haedo', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (65, N'Huanguelen', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (66, N'Hurlingham', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (67, N'Isla Mart√≠n Garc√≠a', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (68, N'Ituzaingo', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (69, N'Jun√≠n', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (70, N'La Plata', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (71, N'La Tablada', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (72, N'Laferrere', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (73, N'Lanus', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (74, N'Laprida', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (75, N'Las Flores', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (76, N'Las Gaviotas', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (77, N'Las Toninas', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (78, N'Lima', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (79, N'Lisandro Olmos', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (80, N'Llavallol', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (81, N'Lobos', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (82, N'Lomas de Zamora', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (83, N'Los Toldos - Gral. Viamonte', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (84, N'Lucila del Mar', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (85, N'Luis GuillÛn', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (86, N'Luj·n', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (87, N'Magdalena', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (88, N'Maip˙', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (89, N'Mar Azul', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (90, N'Mar Chiquita', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (91, N'Mar de AjÛ', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (92, N'Mar de Cobo', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (93, N'Mar del Plata', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (94, N'Mar del Sud', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (95, N'Mar del Tuy˙', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (96, N'Martinez', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (97, N'MÈdanos / Laguna ChasicÛ', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (85, N'Luis Guill√≥n', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (86, N'Luj√°n', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (87, N'Magdalena', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (88, N'Maip√∫', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (89, N'Mar Azul', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (90, N'Mar Chiquita', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (91, N'Mar de Aj√≥', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (92, N'Mar de Cobo', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (93, N'Mar del Plata', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (94, N'Mar del Sud', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (95, N'Mar del Tuy√∫', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (96, N'Martinez', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (97, N'M√©danos / Laguna Chasic√≥', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (98, N'Mercedes', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (99, N'Merlo', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (100, N'Miramar', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (101, N'Monte Hermoso', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (102, N'Moreno', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (103, N'MorÛn', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (103, N'Mor√≥n', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (104, N'Munro', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (105, N'Nada', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (106, N'Navarro', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (107, N'Necochea', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (108, N'Nueva Atlantis', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (109, N'OlavarrÌa', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (109, N'Olavarr√≠a', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (110, N'Olivos', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (111, N'Open Door', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (112, N'Ostende', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (113, N'Pedro Luro', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (114, N'PehuajÛ', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (115, N'Pehuen  CÛ', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (116, N'Pergamino', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (117, N'Pig¸È', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (114, N'Pehuaj√≥', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (115, N'Pehuen  C√≥', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (116, N'Pergamino', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (117, N'Pig√º√©', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (118, N'Pilar', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (119, N'Pinamar', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (120, N'Pinamar', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (121, N'Provincia de Buenos Aires', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (122, N'Puan', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (123, N'Punta Alta', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (124, N'Punta Indio', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (125, N'Punta Lara', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (126, N'QuequÈn', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (127, N'Quilmes', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (128, N'Ramallo', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (129, N'Ramos MejÌa', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (126, N'Quequ√©n', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (127, N'Quilmes', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (128, N'Ramallo', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (129, N'Ramos Mej√≠a', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (130, N'Ranchos', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (131, N'Rauch', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (132, N'Rivadavia', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (133, N'Rojas', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (134, N'Roque PÈrez', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (135, N'Saenz PeÒa', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (134, N'Roque P√©rez', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (135, N'Saenz Pe√±a', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (136, N'Saladillo', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (137, N'Salto', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (138, N'San Antonio de Areco', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (139, N'San Bernardo', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (140, N'San Cayetano', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (141, N'San Clemente del Tuy˙', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (141, N'San Clemente del Tuy√∫', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (142, N'San Fernando', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (143, N'San Isidro', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (144, N'San Justo', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (145, N'San Martin', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (146, N'San Miguel del Monte', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (147, N'San Nicol·s', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (147, N'San Nicol√°s', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (148, N'San Pedro', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (149, N'San Vicente', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (150, N'Santa Clara del Mar', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (151, N'Santa Teresita', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (152, N'SarandÌ', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (153, N'Sierra de la Ventana', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (154, N'Sierra de los Padres', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (155, N'Tandil', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (156, N'TapalquÈ', 1, 0, 1)
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (152, N'Sarand√≠', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (153, N'Sierra de la Ventana', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (154, N'Sierra de los Padres', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (155, N'Tandil', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (156, N'Tapalqu√©', 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (157, N'Temperley', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (158, N'Tigre', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (159, N'Tornquist / Ruta Prov. 76', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (160, N'Trenque Lauquen', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (161, N'Tres Arroyos', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (162, N'Turdera', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (163, N'Valentin Alsina', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (164, N'Vicente Lopez', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (165, N'Victoria', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (166, N'Villa Ballester', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (167, N'Villa Gesell', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (168, N'Villa Lynch', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (169, N'Villa Serrana La Gruta', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (170, N'Villa Ventana', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (171, N'Villalonga', 1, 0, 1)
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (172, N'Wilde', 1, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (173, N'Z·rate', 1, 0, 1)
SET IDENTITY_INSERT [dbo].[Localidad] OFF
=======
INSERT INTO [dbo].[Localidad] ([Id], [Nombre], [Activo], [Predeterminado], [Provincia_id]) VALUES (173, N'Z√°rate', 1, 0, 1)
SET IDENTITY_INSERT [dbo].[Localidad] OFF

SET IDENTITY_INSERT [dbo].[Concepto] ON
INSERT INTO [dbo].[Concepto] ([Id], [Descripcion], [AdditionalDescription], [Valor], [Suma], [TipoConcepto], [Organizacion_id]) VALUES (1, N'Servicios Prestados', NULL, NULL, 1, N'Anticipo', 1)
INSERT INTO [dbo].[Concepto] ([Id], [Descripcion], [AdditionalDescription], [Valor], [Suma], [TipoConcepto], [Organizacion_id]) VALUES (2, N'Reconocimiento Monotributo', NULL, NULL, 1, N'Anticipo', 1)
INSERT INTO [dbo].[Concepto] ([Id], [Descripcion], [AdditionalDescription], [Valor], [Suma], [TipoConcepto], [Organizacion_id]) VALUES (3, N'Adelanto', NULL, NULL, 1, N'Anticipo', 1)
INSERT INTO [dbo].[Concepto] ([Id], [Descripcion], [AdditionalDescription], [Valor], [Suma], [TipoConcepto], [Organizacion_id]) VALUES (4, N'Otros Anticipos', NULL, NULL, 1, N'Anticipo', 1)
SET IDENTITY_INSERT [dbo].[Concepto] OFF

>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e

SET IDENTITY_INSERT [dbo].Usuario ON
insert into Usuario(Id,NombreCompleto,Username,LastLogin,Activo, OrganizacionActual_id)
	values(1, 'Administrador','Administrador',GETDATE(), 1, null)
SET IDENTITY_INSERT [dbo].Usuario OFF

INSERT INTO [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) 
	select 1, GETDATE(), NULL, 1, NULL, 0, N'AGgypb1NDl8Mq/Czo2412jOdLFG3083T1tcNziC4AqI32kuwkI2iaaCclgqh2/J1HQ==', N'2014-10-20 17:43:56', N'', NULL, NULL

SET IDENTITY_INSERT dbo.[webpages_Roles] ON;
INSERT INTO [dbo].[webpages_Roles] (RoleId,RoleName) values (1, 'Administrador')
SET IDENTITY_INSERT dbo.[webpages_Roles] OFF;

INSERT INTO [dbo].[webpages_UsersInRoles] values (1,1)

SET IDENTITY_INSERT [dbo].[Organizacion] ON
INSERT INTO [dbo].[Organizacion] (Id, [NombreFantasia], [CUIT], [IngresosBrutos], [Email], [Telefono], [Domicilio], [CodigoPostal], [Activo], [Provincia_id], [Actividad_id], [Categoria_id], [RazonSocial],[Administrador_id]) 
	VALUES (1, N'Woopin SRL', N'20-35961444-7', N'123-123-123', N'woopin@woopin.com.ar', N'1557488925', N'Aranguren 867', N'1405', 1, 1, 121, 130, N'Woopin',1)
SET IDENTITY_INSERT [dbo].[Organizacion] OFF

update Usuario set OrganizacionActual_id = 1 where Id = 1
insert into UsuarioOrganizacion(Usuario_id,Organizacion_id)	values(1,1)


SET IDENTITY_INSERT [dbo].[OrganizacionModulo] ON
INSERT INTO [dbo].[OrganizacionModulo] ([Id], [ModulosSistemaGestion], [Organizacion_id]) VALUES (1, N'1', 1)
INSERT INTO [dbo].[OrganizacionModulo] ([Id], [ModulosSistemaGestion], [Organizacion_id]) VALUES (2, N'2', 1)
INSERT INTO [dbo].[OrganizacionModulo] ([Id], [ModulosSistemaGestion], [Organizacion_id]) VALUES (3, N'3', 1)
INSERT INTO [dbo].[OrganizacionModulo] ([Id], [ModulosSistemaGestion], [Organizacion_id]) VALUES (4, N'4', 1)
INSERT INTO [dbo].[OrganizacionModulo] ([Id], [ModulosSistemaGestion], [Organizacion_id]) VALUES (5, N'5', 1)
INSERT INTO [dbo].[OrganizacionModulo] ([Id], [ModulosSistemaGestion], [Organizacion_id]) VALUES (6, N'6', 1)
INSERT INTO [dbo].[OrganizacionModulo] ([Id], [ModulosSistemaGestion], [Organizacion_id]) VALUES (7, N'7', 1)
INSERT INTO [dbo].[OrganizacionModulo] ([Id], [ModulosSistemaGestion], [Organizacion_id]) VALUES (8, N'8', 1)
INSERT INTO [dbo].[OrganizacionModulo] ([Id], [ModulosSistemaGestion], [Organizacion_id]) VALUES (9, N'9', 1)
SET IDENTITY_INSERT [dbo].[OrganizacionModulo] OFF
<<<<<<< HEAD


SET IDENTITY_INSERT [dbo].[Concepto] ON
INSERT INTO [dbo].[Concepto] ([Id], [Descripcion], [AdditionalDescription], [Valor], [Suma], [TipoConcepto], [Organizacion_id]) VALUES (1, N'Servicios Prestados', NULL, NULL, 1, N'Anticipo', 1)
INSERT INTO [dbo].[Concepto] ([Id], [Descripcion], [AdditionalDescription], [Valor], [Suma], [TipoConcepto], [Organizacion_id]) VALUES (2, N'Reconocimiento Monotributo', NULL, NULL, 1, N'Anticipo', 1)
INSERT INTO [dbo].[Concepto] ([Id], [Descripcion], [AdditionalDescription], [Valor], [Suma], [TipoConcepto], [Organizacion_id]) VALUES (3, N'Adelanto', NULL, NULL, 1, N'Anticipo', 1)
INSERT INTO [dbo].[Concepto] ([Id], [Descripcion], [AdditionalDescription], [Valor], [Suma], [TipoConcepto], [Organizacion_id]) VALUES (4, N'Otros Anticipos', NULL, NULL, 1, N'Anticipo', 1)
SET IDENTITY_INSERT [dbo].[Concepto] OFF

=======
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e


DBCC CHECKIDENT ('Moneda',RESEED, 0)
SET IDENTITY_INSERT dbo.Moneda ON;
insert into Moneda (Id,Nombre, Abreviatura,CodigoAfip,Signo,Activo,Predeterminado) values(1,'Peso Argentino','ARS','PES','$',1,1)
insert into Moneda (Id,Nombre, Abreviatura,CodigoAfip,Signo,Activo,Predeterminado) values(2,'Real','R','012','$',1,0)
insert into Moneda (Id,Nombre, Abreviatura,CodigoAfip,Signo,Activo,Predeterminado) values(3,'Peso Chileno','CHI','033','$',1,0)
insert into Moneda (Id,Nombre, Abreviatura,CodigoAfip,Signo,Activo,Predeterminado) values(4,'Dolar Estadounidense','USD','DOL','u$d',1,0)
insert into Moneda (Id,Nombre, Abreviatura,CodigoAfip,Signo,Activo,Predeterminado) values(5,'Pesos Uruguayos','URU','011','$',1,0)
insert into Moneda (Id,Nombre, Abreviatura,CodigoAfip,Signo,Activo,Predeterminado) values(6,'Peso Boliviano','BOL','031','$',1,0)
insert into Moneda (Id,Nombre, Abreviatura,CodigoAfip,Signo,Activo,Predeterminado) values(7,'Peso Colombiano','COL','032','$',1,0)
SET IDENTITY_INSERT dbo.Moneda OFF;

DBCC CHECKIDENT ('Cuenta',RESEED, 0)
SET IDENTITY_INSERT [dbo].[Cuenta] ON
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (1, N'1.0.0.0', N'Activo', 1, 0, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (2, N'2.0.0.0', N'Pasivo', 2, 0, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (3, N'3.0.0.0', N'Patrimonio Neto', 3, 0, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (4, N'3.2.0.0', N'Resultado', 3, 2, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (5, N'1.1.0.0', N'Activo Corriente', 1, 1, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (6, N'1.2.0.0', N'Activo No Corriente', 1, 2, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (7, N'1.3.0.0', N'Cuentas Transitorias', 1, 3, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (8, N'2.1.0.0', N'Pasivo Corriente', 2, 1, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (9, N'2.2.0.0', N'Pasivo No Corriente', 2, 2, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (10, N'4.0.0.0', N'Ingresos', 4, 0, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (11, N'1.1.1.0', N'Caja y Bancos', 1, 1, 1, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (12, N'1.1.2.0', N'Credito Por Ventas', 1, 1, 2, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (13, N'1.1.3.0', N'Otros Creditos', 1, 1, 3, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (14, N'1.2.1.0', N'Bienes de Uso', 1, 2, 1, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (15, N'2.1.1.0', N'Deudas Comerciales', 2, 1, 1, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (16, N'2.1.2.0', N'Deudas Sociales', 2, 1, 2, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (17, N'2.1.3.0', N'Deudas Fiscales', 2, 1, 3, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (18, N'2.1.4.0', N'Deudas Bancarias', 2, 1, 4, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (19, N'2.1.5.0', N'Otras Deudas', 2, 1, 5, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (20, N'4.1.0.0', N'Ingresos Por Ventas', 4, 1, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (21, N'4.2.0.0', N'Otros Ingresos', 4, 2, 0, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (22, N'4.2.1.0', N'Gastos AdministraciÛn', 4, 2, 1, 0, 1)
=======
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (22, N'4.2.1.0', N'Gastos Administraci√≥n', 4, 2, 1, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (23, N'4.2.2.0', N'Gastos Comerciales', 4, 2, 2, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (24, N'4.2.3.0', N'Gastos Financieros', 4, 2, 3, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (25, N'4.2.4.0', N'Costos', 4, 2, 4, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (26, N'3.1.0.001', N'Capital Social', 3, 1, 0, 1, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (27, N'5.0.0.0', N'Egresos', 5, 0, 0, 0, 1)
<<<<<<< HEAD
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (28, N'5.1.0.0', N'Gastos de ComercializaciÛn', 5, 1, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (29, N'5.2.0.0', N'Gastos de AdministraciÛn', 5, 2, 0, 0, 1)
=======
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (28, N'5.1.0.0', N'Gastos de Comercializaci√≥n', 5, 1, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (29, N'5.2.0.0', N'Gastos de Administraci√≥n', 5, 2, 0, 0, 1)
>>>>>>> 93eff202c21e24ca31f3cc1a406bab2764c4c66e
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (30, N'5.3.0.0', N'Gastos de Financiamiento', 5, 3, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (31, N'5.4.0.0', N'Otros Egresos', 5, 4, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (32, N'1.1.1.002', N'Valores A Depositar', 1, 1, 1, 2, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (33, N'1.1.2.001', N'Deudores Por Ventas', 1, 1, 2, 1, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (34, N'1.1.3.001', N'IVA Credito Fiscal', 1, 1, 3, 1, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (35, N'2.1.2.001', N'Sueldos A Pagar', 2, 1, 2, 1, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (36, N'2.1.2.002', N'Cargas Sociales A Pagar', 2, 1, 2, 2, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (37, N'2.1.2.003', N'Sindicato A Pagar', 2, 1, 2, 3, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (38, N'3.1.0.0', N'Capital', 3, 1, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (39, N'4.1.0.001', N'Ventas', 4, 1, 0, 1, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (40, N'6.0.0.0', N'Costos', 6, 0, 0, 0, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (41, N'2.1.1.001', N'Proveedores', 2, 1, 1, 1, 1)
INSERT INTO [dbo].[Cuenta] ([Id], [Codigo], [Nombre], [Rubro], [Corriente], [SubRubro], [Numero], [Organizacion_id]) VALUES (42, N'2.1.3.001', N'IVA Debito Fiscal', 2, 1, 3, 1, 1)
SET IDENTITY_INSERT [dbo].[Cuenta] OFF



DBCC CHECKIDENT ('CategoriaIVA',RESEED, 0)
SET IDENTITY_INSERT [dbo].[CategoriaIVA] ON
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (1, 'RI', 'Responsable Inscripto', 0, 0, 0, 'IVA Responsable Inscripto', 1, 1, 1,1)
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (2, 'CF', 'Consumidor Final', 0, 0, 0, 'Consumidor Final', 1, 0, 2,2)
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (3, 'EX', 'Exento', 0, 0, 0, 'IVA Sujeto Exento', 1, 0, 2,2)
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (4, 'EP', 'Exportaci√≥n', 0, 0, 0, 'Cliente del Exterior', 1, 0, 4,4)
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (5, 'MO', 'Responsable Monotributista', 0, 0, 0, 'Responsable Monotributista', 1, 0, 3,2)
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (6, 'NI', 'Responsable No Inscripto', 0, 0, 0, 'IVA Responsable No Inscripto', 1, 0, 1,1)
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (7, 'ET', 'No categorizado', 0, 0, 0, 'Sujeto no caracterizado', 1, 0, 2,2)
SET IDENTITY_INSERT [dbo].[CategoriaIVA] OFF

SET IDENTITY_INSERT [dbo].[Adicional] ON
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (1, N'Sueldo Mensual', NULL, NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (2, N'Sueldo Basico', NULL, NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (3, N'Falta Justificada', NULL, NULL, 1, N'Remunerativo', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (4, N'Falta Injustificada', NULL, NULL, 0, N'Remunerativo', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (5, N'Descuento de Horas', NULL, NULL, 0, N'Remunerativo', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (6, N'Antiguedad', CAST(0.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (7, N'Antiguedad', CAST(1.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (8, N'Antiguedad', CAST(4.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (9, N'Antiguedad', CAST(8.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (10, N'Antiguedad', CAST(10.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (11, N'Antiguedad', CAST(14.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (12, N'Antiguedad', CAST(16.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (13, N'Antiguedad', CAST(18.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (14, N'Antiguedad', CAST(20.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (15, N'Antiguedad', CAST(22.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (16, N'Antiguedad', CAST(25.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (17, N'Antiguedad', CAST(28.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (18, N'Antiguedad', CAST(35.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (1004, N'Premio asistencia y puntualidad', CAST(20.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (1005, N'Asistencia perfecta', CAST(5.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (1006, N'Refrigerio', NULL, CAST(43.50000 AS Decimal(19, 5)), 1, N'Remunerativo', 1, N'251', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (1007, N'Obra Social', CAST(3.00000 AS Decimal(19, 5)), NULL, 0, N'Descuento', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (1008, N'Jubilacion', CAST(11.00000 AS Decimal(19, 5)), NULL, 0, N'Descuento', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (1009, N'Ley 19032', CAST(3.00000 AS Decimal(19, 5)), NULL, 0, N'Descuento', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (1010, N'Cuota Sindical', CAST(3.00000 AS Decimal(19, 5)), NULL, 0, N'Descuento', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (2006, N'Anticipo', NULL, NULL, 0, N'Descuento', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (2008, N'Redondeo', NULL, NULL, 1, N'NoRemunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (3006, N'Suma No Remunerativa', NULL, NULL, 1, N'NoRemunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (3007, N'Diferencia de cuota sindical', NULL, CAST(550.00000 AS Decimal(19, 5)), 0, N'Descuento', 1, N'251', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (3008, N'Diferencia de cuota sindical', NULL, CAST(450.00000 AS Decimal(19, 5)), 0, N'Descuento', 1, N'252', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (3009, N'Diferencia de cuota sindical', NULL, CAST(400.00000 AS Decimal(19, 5)), 0, N'Descuento', 1, N'253', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (3010, N'Diferencia de obra social', NULL, CAST(400.00000 AS Decimal(19, 5)), 0, N'Descuento', 1, N'251', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (3011, N'Diferencia de obra social', NULL, CAST(500.00000 AS Decimal(19, 5)), 0, N'Descuento', 1, N'252', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (3012, N'Diferencia de obra social', NULL, CAST(550.00000 AS Decimal(19, 5)), 0, N'Descuento', 1, N'253', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (4007, N'Horas Extras', CAST(150.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (4008, N'Premio por produccion', CAST(22.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'251', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (4009, N'Premio por produccion', CAST(20.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'253', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (4010, N'SAC', CAST(50.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (5011, N'Beneficiario Obra Social', NULL, NULL, 0, N'Descuento', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (5012, N'Refrigerio', NULL, CAST(43.50000 AS Decimal(19, 5)), 1, N'Remunerativo', 1, N'252', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (5013, N'Refrigerio', NULL, CAST(43.50000 AS Decimal(19, 5)), 1, N'Remunerativo', 1, N'253', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (5014, N'Viaticos', NULL, CAST(43.00000 AS Decimal(19, 5)), 1, N'NoRemunerativo', 1, N'251', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (5015, N'Viaticos', NULL, CAST(43.00000 AS Decimal(19, 5)), 1, N'NoRemunerativo', 1, N'252', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (5016, N'Viaticos', NULL, CAST(43.00000 AS Decimal(19, 5)), 1, N'NoRemunerativo', 1, N'253', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (6011, N'Suspension', NULL, NULL, 0, N'Remunerativo', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (6012, N'ART', NULL, NULL, 1, N'Remunerativo', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (6013, N'Licencia por Enfermedad', NULL, NULL, 1, N'Remunerativo', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (8011, N'Vacaciones', NULL, NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (9011, N'Dia Cortador', CAST(5.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'251', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (10011, N'Dia Nacimiento', NULL, NULL, 1, N'Remunerativo', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (10012, N'Dia Casamiento', NULL, NULL, 1, N'Remunerativo', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (10013, N'Dia Mudanza', NULL, NULL, 1, N'Remunerativo', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (10014, N'Dias Fallecimiento', NULL, NULL, 1, N'Remunerativo', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (10015, N'Horas Justificadas', NULL, NULL, 1, N'Remunerativo', 1, NULL, NULL, 1)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (10016, N'Dia del Gremio', NULL, NULL, 1, N'Remunerativo', 1, N'251', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (10017, N'Dia del Gremio', NULL, NULL, 1, N'Remunerativo', 1, N'253', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (10018, N'Dia del Gremio', NULL, NULL, 1, N'Remunerativo', 1, N'252', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (12016, N'CETIC', CAST(10.00000 AS Decimal(19, 5)), NULL, 1, N'Remunerativo', 1, N'251', NULL, 0)
INSERT INTO [dbo].[Adicional] ([Id], [Descripcion], [Porcentaje], [Valor], [Suma], [TipoLiquidacion], [Organizacion_id], [AdditionalDescription], [Cuenta_id], [OnlyAutomatic]) VALUES (13017, N'Adicional rem sin valor suma', NULL, NULL, 1, N'Remunerativo', 1, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Adicional] OFF


SET IDENTITY_INSERT [dbo].[Valor] ON
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (1, N'Efectivo Pesos', 1, 1, 71,1)
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (2, N'Tarjeta de Debito', 1, 1, 72,1)
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (3, N'Cheque Propio', 1, 1, 73,1)
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (4, N'Cheque Terceros', 1, 1, 70,1)
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (5, N'Transferencia', 1, 1, 72,1)
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (6, N'Tarjeta de Credito', 1, 1, 74,1)
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (7, N'Retenci√≥n', 1, 1, 75,1)
SET IDENTITY_INSERT [dbo].[Valor] OFF

SET IDENTITY_INSERT [dbo].[Ejercicio] ON
INSERT INTO [dbo].[Ejercicio] ([Id], [Inicio], [Final], [Nombre], [Activo], [Cerrado], Organizacion_Id) VALUES (7, N'2014-10-01 00:00:00', N'2015-09-30 00:00:00', N'Ejercicio 2015', 1, 0,1)
SET IDENTITY_INSERT [dbo].[Ejercicio] OFF
