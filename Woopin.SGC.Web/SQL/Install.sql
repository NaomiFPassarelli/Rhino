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
insert into Combo (Id,Nombre,Activo) values(12,'Actividad de la Organización',1)
insert into Combo (Id,Nombre,Activo) values(13,'Categoria IVA de la Organización',1)
insert into Combo (Id,Nombre,Activo) values(14,'Unidades de Medida',1)
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
-- del 30 al 39 los movimientos de fondos
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(30,'Deposito',3,'1',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(31,'Extracción',3,'-1',1)
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo) values(32,'Transferencia entre Bancos',3,'0',1)
-- del 40 al 49 los paises
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(40,'Argentina',4,null,1,'200')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(41,'Brazil',4,null,1,'203')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(42,'Chile',4,null,1, '208')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(43,'Uruguay',4,null,1, '225')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(44,'Ecuador',4,null,1, '210')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(45,'Venezuela',4,null,1, '226')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(46,'Paraguay',4,null,1, '221')
insert into ComboItem (Id,Data,Combo_Id,AdditionalData,Activo, AfipData) values(47,'Peru',4,null,1, '222')
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
SET IDENTITY_INSERT dbo.ComboItem OFF;
DBCC CHECKIDENT ('ComboItem',RESEED, 200)


DBCC CHECKIDENT ('Localizacion',RESEED, 0)
SET IDENTITY_INSERT dbo.Localizacion ON;
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(1,'C.A.B.A.','Buenos Aires',40,1,1)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(2,'Buenos Aires','Buenos Aires',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(3,'Cordoba','Cordoba',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(4,'Rosario','Santa Fe',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(5,'Corrientes','Corrientes',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(6,'Formosa','Formosa',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(7,'Entre Rios','Entre Rios',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(8,'Misiones','Misiones',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(9,'Chubut','Chubut',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(10,'Mendoza','Mendoza',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(11,'San Juan','San Juan',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(12,'Santa Fe','Santa Fe',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(13,'Tierra del Fuego','Tierra del Fuego',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(14,'San Luis','San Luis',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(15,'Montevideo','Montevideo',43,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(16,'Santiago de Chile','Santiago de Chile',42,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(17,'Asunción','Asunción',46,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(18,'Lima','Lima',47,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(19,'La Pampa','La Pampa',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(20,'Santiago del Estero','Santiago del Estero',40,1,0)
insert into Localizacion (Id,Nombre, Provincia, Pais_id,Activo,Predeterminado) values(21,'Chaco','Chaco',40,1,0)
SET IDENTITY_INSERT dbo.Localizacion OFF;

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
	VALUES (1, N'Woopin SRL', N'20-35961444-7', N'123-123-123', N'woopin@woopin.com.ar', N'1530682244', N'Nicolas Avellaneda3265', N'1636', 1, 1, 121, 130, N'Woopin',1)
SET IDENTITY_INSERT [dbo].[Organizacion] OFF

update Usuario set OrganizacionActual_id = 1 where Id = 1
insert into UsuarioOrganizacion(Usuario_id,Organizacion_id)	values(1,1)




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
SET IDENTITY_INSERT dbo.Cuenta ON;
-- Este plan inicial esta atado a la contabilidad de piroska, luego cambiarlo por uno comun.
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(1,1,0,0,0,'1.0.0.0','Activo',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(2,2,0,0,0,'2.0.0.0','Pasivo',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(3,3,0,0,0,'3.0.0.0','Patrimonio Neto',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(4,4,0,0,0,'4.0.0.0','Resultado',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(5,1,1,0,0,'1.1.0.0','Activo Corriente',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(6,1,2,0,0,'1.2.0.0','Activo No Corriente',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(7,1,3,0,0,'1.3.0.0','Cuentas Transitorias',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(8,2,1,0,0,'2.1.0.0','Pasivo Corriente',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(9,2,2,0,0,'2.2.0.0','Pasivo No Corriente',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(10,4,1,0,0,'4.1.0.0','Ingresos',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(11,4,2,0,0,'4.2.0.0','Egresos',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(12,1,1,1,0,'1.1.1.0','Caja y Bancos',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(13,1,1,2,0,'1.1.2.0','Credito Por Ventas',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(14,1,1,3,0,'1.1.3.0','Otros Creditos',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(15,1,2,1,0,'1.2.1.0','Bienes de Uso',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(16,2,1,1,0,'2.1.1.0','Deudas Comerciales',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(17,2,1,2,0,'2.1.2.0','Deudas Sociales',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(18,2,1,3,0,'2.1.3.0','Deudas Fiscales',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(19,2,1,4,0,'2.1.4.0','Deudas Bancarias',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(20,2,1,5,0,'2.1.5.0','Otras Deudas',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(21,4,1,1,0,'4.1.1.0','Ingresos Por Ventas',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(22,4,1,2,0,'4.1.2.0','Otros Ingresos',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(23,4,2,1,0,'4.2.1.0','Gastos Administración',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(24,4,2,2,0,'4.2.2.0','Gastos Comerciales',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(25,4,2,3,0,'4.2.3.0','Gastos Financieros',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(26,4,2,4,0,'4.2.4.0','Costos',1)
insert into Cuenta (Id,Rubro,Corriente,SubRubro,Numero,Codigo,Nombre,Organizacion_id) values(27,3,0,0,1,'3.001','Capital Social',1)
SET IDENTITY_INSERT dbo.Cuenta OFF;


DBCC CHECKIDENT ('CategoriaIVA',RESEED, 0)
SET IDENTITY_INSERT [dbo].[CategoriaIVA] ON
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (1, 'RI', 'Responsable Inscripto', 0, 0, 0, 'IVA Responsable Inscripto', 1, 1, 1,1)
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (2, 'CF', 'Consumidor Final', 0, 0, 0, 'Consumidor Final', 1, 0, 2,2)
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (3, 'EX', 'Exento', 0, 0, 0, 'IVA Sujeto Exento', 1, 0, 2,2)
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (4, 'EP', 'Exportación', 0, 0, 0, 'Cliente del Exterior', 1, 0, 4,4)
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (5, 'MO', 'Responsable Monotributista', 0, 0, 0, 'Responsable Monotributista', 1, 0, 3,2)
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (6, 'NI', 'Responsable No Inscripto', 0, 0, 0, 'IVA Responsable No Inscripto', 1, 0, 1,1)
INSERT INTO [dbo].[CategoriaIVA] ([Id], [Abreviatura], [Nombre], [Discrimina], [LiquidaInternos], [ExentoIva], [ResponsabilidadAfip], [Activo], [Predeterminado], [LetraCompras_id], [LetraVentas_id]) 
	VALUES (7, 'ET', 'No categorizado', 0, 0, 0, 'Sujeto no caracterizado', 1, 0, 2,2)
SET IDENTITY_INSERT [dbo].[CategoriaIVA] OFF



SET IDENTITY_INSERT [dbo].[Valor] ON
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (1, N'Efectivo Pesos', 1, 1, 71,1)
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (2, N'Tarjeta de Debito', 1, 1, 72,1)
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (3, N'Cheque Propio', 1, 1, 73,1)
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (4, N'Cheque Terceros', 1, 1, 70,1)
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (5, N'Transferencia', 1, 1, 72,1)
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (6, N'Tarjeta de Credito', 1, 1, 74,1)
INSERT INTO [dbo].[Valor] ([Id], [Nombre], [Activo], [Moneda_id], [TipoValor_id],Organizacion_Id) VALUES (7, N'Retención', 1, 1, 75,1)
SET IDENTITY_INSERT [dbo].[Valor] OFF

SET IDENTITY_INSERT [dbo].[Ejercicio] ON
INSERT INTO [dbo].[Ejercicio] ([Id], [Inicio], [Final], [Nombre], [Activo], [Cerrado], Organizacion_Id) VALUES (7, N'2014-10-01 00:00:00', N'2015-09-30 00:00:00', N'Ejercicio 2015', 1, 0,1)
SET IDENTITY_INSERT [dbo].[Ejercicio] OFF