/*
	Se agrego la columna Tipo al articulo.
*/
update Articulo set Tipo = 'Producto'

/*
	Se hace nulleable el mes de prestacion de un Comprobante Venta
*/
ALTER TABLE [dbo].[ComprobanteVenta] ALTER COLUMN [MesPrestacion] NVARCHAR (255) NULL;