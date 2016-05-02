/*
	Se agrego la columna TotalConIva que acumula el total del detalle mas su respectivo iva.
	Importante para la parte de reporting.
*/
update DetalleComprobanteVenta set TotalConIVA = Total * 1.21 where TipoIva_id = 94
update DetalleComprobanteVenta set TotalConIVA = Total * 1.27 where TipoIva_id = 95
update DetalleComprobanteVenta set TotalConIVA = Total * 1.105 where TipoIva_id = 93
update DetalleComprobanteVenta set TotalConIVA = Total where TipoIva_id = 90 or TipoIva_id = 91

