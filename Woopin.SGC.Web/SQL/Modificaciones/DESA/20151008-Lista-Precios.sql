/*
Post-Deployment Script Template			
				
*/

SET DATEFORMAT ymd

insert into ListaPreciosItem(Articulo_id, Organizacion_Id, Precio, Cliente_id, grupo_id)
	select Id,Organizacion_id,0,null,null
	from articulo