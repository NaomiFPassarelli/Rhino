_SelectedGridRows = [];
_GridSelector = null;
/*
    Formatter para armar el checkbox con el valor del id del modelo.
*/
function SelectCheckboxFormatter(cellvalue, options, rowObject)
{
    return '<input type="checkbox" value="' + rowObject.Id + '" class="SelectCheckboxes" />'
}

function Drop_Number(cellvalue, options, rowObject)
{
    var CantidadRestantes = parseFloat(rowObject.CantidadAbonos) - parseFloat(rowObject.CantidadAbonosEfectivos);
    var drop = '<select id=' + options.colModel.formatoptions.nameSelect + ' name=' + options.colModel.formatoptions.nameSelect + ' onchange=changeselected(this,this.value);>';
    for (var i = 1; i <= CantidadRestantes; i++)
    {
        if (i == 1) {
            drop += '<option value=' + i + ' selected="selected">' + i + '</option>';
        } else {
            drop += '<option value=' + i + '>' + i + '</option>';
        }
    }
    drop += '</select>';
    return drop;
}

function changeselected(e, value)
{
    debugger;
    //$($("select#" + e.name + "class=" + $(e).attr("class") + " option")).removeAttr("selected");
    //$($("select#" + e.name + "class=" + $(e).attr("class") + " option")[--value]).attr("selected", "selected");
    $(e.childNodes).removeAttr("selected");
    $($(e.childNodes)[--value]).attr("selected", "selected");
}

/*
    Formatter para armar el checkbox con el valor del id del modelo
    Chequeado o no dependiendo de otros id que vienen en formatoptions: { GridRestringida: } como array
*/
function CheckboxFormatter(cellvalue, options, rowObject) {
    if (options.colModel.formatoptions.GridRestringida.indexOf(rowObject.Id.toString()) >= 0) {
        return '<input type="checkbox" value="' + rowObject.Id + '" class="SelectCheckboxes" checked="checked"/>'
    }
    else
    {
        return '<input type="checkbox" value="' + rowObject.Id + '" class="SelectCheckboxes" />'
    }
}

function RadioFormatter(cellvalue, options, rowObject) {
    if (options.colModel.formatoptions.SelectedRows != null && options.colModel.formatoptions.SelectedRows == rowObject.Id.toString()) {
        return '<input type="radio" value="' + rowObject.Id + '" class="SelectCheckboxes" checked="checked"/>'
    }
    else {
        return '<input type="radio" value="' + rowObject.Id + '" class="SelectCheckboxes" />'
    }
}


function DefaultStarFormatter(cellvalue, options, rowObject) {
    if (rowObject.Predeterminado)
    {
        return '<span class="defaulter" data-id="' + rowObject.Id + '"><i class="fa fa-star is-default"></i></span>';
    }
    else
    {
        return '<span class="defaulter" data-id="' + rowObject.Id + '"><i class="fa fa-star-o not-default"></i></span>';
    }
}
/*
    Formatter para cualquier abm que necesite Editar y Eliminar
*/
function ABM_EditarYEliminar( cellvalue, options, rowObject)
{
    var btns = '';
    btns +='<a class="boton boton-i BtnEditar" title="Editar"><i class="fa fa-edit i-blue"></i></a>'
    btns += ' <a class="boton boton-i BtnEliminar" title="Eliminar"><i class="fa fa-trash-o i-red"></i></a>';
    return btns;
}
/*
    Formatter para cualquier abm que necesite Eliminar
*/
function ABM_Eliminar(cellvalue, options, rowObject) {
    return '<a class="boton boton-i BtnEliminar" title="Eliminar"><i class="fa fa-trash-o i-red"></i></a>';
}

//TODO ver si es util es de Sueldos/Recibos/Nuevo
//function ABM_Eliminar_EsDefault(cellvalue, options, rowObject) {
//    var ret = '';
//    ret += '<a class="boton boton-i BtnVerDialog" title="Detalle Dialogo" data-Id="' + rowObject.Id + '"><i class="fa fa-files-o i-green"></i></a>';
//    ret += '<a class="boton boton-i BtnEliminar" title="Eliminar"><i class="fa fa-trash-o i-red"></i></a>';
//    return ret;
//}

function Usuarios_Actions(cellvalue, options, rowObject) {
    return '<a class="boton boton-i BtnDeshabilitar" title="Deshabilitar" data-Id="' + rowObject.Id + '"><i class="fa fa-lock i-red"></i> Deshabilitar</a>';
}

function DireccionFormatter(cellvalue, options, rowObject) {
    var dir = rowObject.Direccion;
    if (rowObject.Numero != "" && typeof rowObject.Numero != "undefined" && rowObject.Numero != null) dir += " " + rowObject.Numero;
    if (rowObject.Piso != "" && typeof rowObject.Piso != "undefined" && rowObject.Piso != null) dir += " " + rowObject.Piso;
    if (rowObject.Departamento != "" && typeof rowObject.Departamento != "undefined" && rowObject.Departamento != null) dir += rowObject.Departamento;
    if (rowObject.CodigoPostal != "" && typeof rowObject.CodigoPostal != "undefined" && rowObject.CodigoPostal != null) dir += " (" + rowObject.CodigoPostal + ")";
    return dir;   
}

function formatterFacturaLetraNumero(cellvalue, options, rowObject) {
    return rowObject.Letra + rowObject.Numero;
}

function AccionesFacturasFormatter(cellvalue, options, rowObject) {
    var ret = '';
    ret += '<a class="boton boton-i BtnVerNuevaVentana" title="Detalle Nueva Ventana" data-Id="' + rowObject.Id + '"><i class="fa fa-search-plus i-blue"></i></a>';
    ret += '<a class="boton boton-i BtnVerDialog" title="Detalle Dialogo" data-Id="' + rowObject.Id + '"><i class="fa fa-files-o i-green"></i></a>';
    return ret;
}

function FreeFormatter(cellvalue, options, rowObject) {
    var ret = '';
    var url = options.colModel.formatoptions.urlAction + "/" + rowObject.Id;
    ret += '<a class="boton boton-i BtnVerNuevaVentana" title=';
    ret += options.colModel.formatoptions.title;
    ret += ' target="_blank" data-Id="' + rowObject.Id + '" href="' + url + '"><i class="';
    ret += options.colModel.formatoptions.icon + '"></i></a>';
    return ret;
}

function formatterNumberToString(cellvalue,opt,row)
{
    return cellvalue > 0 ? cellvalue : "";
}

function formatterRecibo_RemNoRemDesc(cellvalue, options, rowObject) {
    if (rowObject.TipoLiquidacion == "Footer") {
        return cellvalue;
    }

    var TipoLiquidacion;
    var ValorSobre;
    var Porcentaje;
    var Suma;
    var importe = 0;
    var ValorMin = 0;
    if (rowObject.Adicional != undefined && rowObject.Adicional != null) {
        //Es vista Detalle, sino es Nuevo
        TipoLiquidacion = String(rowObject.Adicional.TipoLiquidacion);
        importe = rowObject.Total; //Es sobre el AdicionalesRecibo
        Porcentaje = rowObject.Adicional.Porcentaje;
        Suma = rowObject.Adicional.Suma;
    } else {
        TipoLiquidacion = String(rowObject.TipoLiquidacion);
        ValorSobre = rowObject.Valor;
        Porcentaje = rowObject.Porcentaje;
        Suma = rowObject.Suma;
        ValorMin = rowObject.ValorMin;
    }
    //rowObject.TipoLiquidacion = String(rowObject.TipoLiquidacion);
    if ((options.colModel.index == "Remunerativo" && TipoLiquidacion != "0")
        || (options.colModel.index == "NoRemunerativo" && TipoLiquidacion != "1")
        || (options.colModel.index == "Descuento" && TipoLiquidacion != "2")) {
        return "";
    } else {
        if (importe == 0) {
            if (/*IdAdicional >= 3007 && IdAdicional <= 3012 || */ (ValorMin != null && ($.isNumeric(ValorMin)) && ValorSobre != null && ($.isNumeric(ValorSobre)))) {
                //es util para diferencia de obra socia, diferencia de cuota sindical
                if (rowObject.Adicional_Id >= 3007 && rowObject.Adicional_Id <= 3012) {
                    if (rowObject.Adicional_Id >= 3007 && rowObject.Adicional_Id <= 3009 && Recibo.ValorAnteriorDiferenciaSindical != 0 && (-1 * ValorSobre >= ValorMin)) {
                        //devuelvo diferencia sindical anterior
                        importe = -1 * Recibo.ValorAnteriorDiferenciaSindical;
                        if (importe < 0) {
                            importe *= -1;
                        }
                        Suma = true;
                    } else if (rowObject.Adicional_Id >= 3007 && rowObject.Adicional_Id <= 3009 && Recibo.ValorAnteriorDiferenciaSindical != 0 && ((-1 * ValorSobre < ValorMin && parseFloat(ValorMin) + parseFloat(ValorSobre)) < Recibo.ValorAnteriorDiferenciaSindical)) {
                        //devuelvo diferencia sindical anterior
                        importe = parseFloat(ValorMin) + parseFloat(ValorSobre) + parseFloat(Recibo.ValorAnteriorDiferenciaSindical);
                        if (importe < 0) {
                            importe *= -1;
                        }
                        Suma = true;
                    } else if (rowObject.Adicional_Id >= 3007 && rowObject.Adicional_Id <= 3009 && Recibo.ValorAnteriorDiferenciaSindical != 0 && ((-1 * ValorSobre < ValorMin && parseFloat(ValorMin) + parseFloat(ValorSobre)) > Recibo.ValorAnteriorDiferenciaSindical)) {
                        //no devuelve hay diferencia nueva
                        importe = parseFloat(ValorMin) + parseFloat(ValorSobre) + parseFloat(Recibo.ValorAnteriorDiferenciaSindical);
                    } else if (rowObject.Adicional_Id >= 3010 && rowObject.Adicional_Id <= 3012 && Recibo.ValorAnteriorDiferenciaObraSocial != 0 && (-1 * ValorSobre >= ValorMin)) {
                        //devuelvo diferencia ObraSocial anterior
                        importe = -1 * Recibo.ValorAnteriorDiferenciaObraSocial;
                        if (importe < 0) {
                            importe *= -1;
                        }
                        Suma = true;
                    } else if (rowObject.Adicional_Id >= 3010 && rowObject.Adicional_Id <= 3012 && Recibo.ValorAnteriorDiferenciaObraSocial != 0 && ((-1 * ValorSobre < ValorMin && parseFloat(ValorMin) + parseFloat(ValorSobre)) < Recibo.ValorAnteriorDiferenciaObraSocial)) {
                        //devuelvo diferencia ObraSocial anterior
                        importe = parseFloat(ValorMin) + parseFloat(ValorSobre) + parseFloat(Recibo.ValorAnteriorDiferenciaObraSocial);
                        if (importe < 0) {
                            importe *= -1;
                        }
                        Suma = true;
                    } else if (rowObject.Adicional_Id >= 3010 && rowObject.Adicional_Id <= 3012 && Recibo.ValorAnteriorDiferenciaObraSocial != 0 && ((-1 * ValorSobre < ValorMin && parseFloat(ValorMin) + parseFloat(ValorSobre)) > Recibo.ValorAnteriorDiferenciaObraSocial)) {
                        //no devuelve hay diferencia nueva
                        importe = parseFloat(ValorMin) + parseFloat(ValorSobre) + parseFloat(Recibo.ValorAnteriorDiferenciaObraSocial);
                    } else {
                        //no se llego al minimo
                        importeMin = parseFloat(ValorMin) + parseFloat(ValorSobre); // porque el ValorSobre esta en negativo
                        if (importeMin > 0) {
                            importe = importeMin;
                        }
                        //si es mayor a cero es porque ya se saldo y no hay que devolver ni agregar diferencia
                    }

                }
                
            } else if (ValorSobre != null && ($.isNumeric(ValorSobre)) && Porcentaje != null && ($.isNumeric(Porcentaje)) && rowObject.Unidades != null && ($.isNumeric(rowObject.Unidades))) {
                //es util para horas extras
                importe = (rowObject.Unidades * ValorSobre) * Porcentaje / 100;
            } else if (ValorSobre != null && ($.isNumeric(ValorSobre)) && rowObject.Unidades != null && ($.isNumeric(rowObject.Unidades))) {
                //es util para sueldo, dias, horas
                importe = rowObject.Unidades * ValorSobre;
            } else if (ValorSobre != null && ($.isNumeric(ValorSobre)) && Porcentaje != null && ($.isNumeric(Porcentaje))) {
                //es util para antiguedad, cuota sindical, obra social, ley
                importe = ValorSobre * Porcentaje / 100;
            } else {
                importe = ValorSobre;
            }
        }
        if(importe != null && ($.isNumeric(importe)))
        {
            importe = parseFloat(importe).toFixed(2);
        }

        //TODO hacer este cambio y probarlo
        //if (importe > 0) {
        //    importeSuma = importe;
        //    importeResta = -1 * importe;
        //} else {
        //    importeResta = importe;
        //    importeSuma = -1 * importe;
        //}

        importeSuma = importe;
        if (importeSuma > 0) {
            importeResta = "-" + importeSuma;
        } else {
            importeResta = importeSuma;
            //porque en la vista Detalle ya viene con el negativo
        }
        
        //switch (rowObject.Adicional_Id && (rowObject.Adicional == undefined || rowObject.Adicional == null))
        if ((rowObject.Adicional == undefined || rowObject.Adicional == null))
        {
            switch (rowObject.Adicional_Id) {
                //TODO SQL
                case 6: //antiguedad
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                    valorAntiguedad = importe;
                    break;
                case 1004: //premio asistencia y puntualidad
                    valorPremioAsistenciaYPuntualidad = importe;
                    break;
                case 1005: //premio asistencia perfecta 
                    valorPremioAsistenciaPerfecta = importe;
                    break;
                case 1006:
                case 5012:
                case 5013:
                    valorRefrigerio = importe;
                    break;
                case 4007:
                    valorHorasExtras = importe;
                    break;
                case 4008:
                case 4009:
                    valorHorasExtras = importe;
                    break;
                case 9011:
                    valorDiaCortador = importe;
                    break;
                default:
                    break;
            }

        }
        
        return ((Suma == "true" || Suma == true) ? importeSuma : importeResta);


        //TODO TIENE SENTIDO ESTE SWITCH?
        //switch (rowObject.TipoLiquidacion) {
        //    case "0":
        //        return (rowObject.Suma == "true" ? importeSuma : importeResta);
        //        break;
        //    case "1":
        //        return (rowObject.Suma == "true" ? importeSuma : importeResta);
        //        break;
        //    case "2":
        //        return (rowObject.Suma == "true" ? importeSuma : importeResta);
        //        break;
        //}
    }
}


function formatterPago_AntiDesc(cellvalue, options, rowObject) {
    if (rowObject.Adicional_Id >= 6 && rowObject.Adicional_Id <= 18) {
    }
    if (rowObject.TipoLiquidacion == "Footer") {
        return cellvalue;
    }

    var TipoLiquidacion;
    var ValorSobre;
    var Porcentaje;
    var Suma;
    var importe = 0;
    var ValorMin = 0;
    if (rowObject.Adicional != undefined && rowObject.Adicional != null) {
        //Es vista Detalle, sino es Nuevo
        TipoLiquidacion = String(rowObject.Adicional.TipoLiquidacion);
        importe = rowObject.Total; //Es sobre el AdicionalesRecibo
        Porcentaje = rowObject.Adicional.Porcentaje;
        Suma = rowObject.Adicional.Suma;
    } else {
        TipoLiquidacion = String(rowObject.TipoLiquidacion);
        ValorSobre = rowObject.Valor;
        Porcentaje = rowObject.Porcentaje;
        Suma = rowObject.Suma;
        ValorMin = rowObject.ValorMin;
    }
    //rowObject.TipoLiquidacion = String(rowObject.TipoLiquidacion);
    if ((options.colModel.index == "Remunerativo" && TipoLiquidacion != "0")
        || (options.colModel.index == "NoRemunerativo" && TipoLiquidacion != "1")
        || (options.colModel.index == "Descuento" && TipoLiquidacion != "2")) {
        return "";
    } else {
        if (importe == 0) {
            if (/*IdAdicional >= 3007 && IdAdicional <= 3012 || */ (ValorMin != null && ($.isNumeric(ValorMin)) && ValorSobre != null && ($.isNumeric(ValorSobre)))) {
                //es util para diferencia de obra socia, diferencia de cuota sindical
                if (rowObject.Adicional_Id >= 3007 && rowObject.Adicional_Id <= 3012) {
                    if (rowObject.Adicional_Id >= 3007 && rowObject.Adicional_Id <= 3009 && Recibo.ValorAnteriorDiferenciaSindical != 0 && (-1 * ValorSobre >= ValorMin)) {
                        //devuelvo diferencia sindical anterior
                        importe = -1 * Recibo.ValorAnteriorDiferenciaSindical;
                        if (importe < 0) {
                            importe *= -1;
                        }
                        Suma = true;
                    } else if (rowObject.Adicional_Id >= 3007 && rowObject.Adicional_Id <= 3009 && Recibo.ValorAnteriorDiferenciaSindical != 0 && ((-1 * ValorSobre < ValorMin && parseFloat(ValorMin) + parseFloat(ValorSobre)) < Recibo.ValorAnteriorDiferenciaSindical)) {
                        //devuelvo diferencia sindical anterior
                        importe = parseFloat(ValorMin) + parseFloat(ValorSobre) + parseFloat(Recibo.ValorAnteriorDiferenciaSindical);
                        if (importe < 0) {
                            importe *= -1;
                        }
                        Suma = true;
                    } else if (rowObject.Adicional_Id >= 3007 && rowObject.Adicional_Id <= 3009 && Recibo.ValorAnteriorDiferenciaSindical != 0 && ((-1 * ValorSobre < ValorMin && parseFloat(ValorMin) + parseFloat(ValorSobre)) > Recibo.ValorAnteriorDiferenciaSindical)) {
                        //no devuelve hay diferencia nueva
                        importe = parseFloat(ValorMin) + parseFloat(ValorSobre) + parseFloat(Recibo.ValorAnteriorDiferenciaSindical);
                    } else if (rowObject.Adicional_Id >= 3010 && rowObject.Adicional_Id <= 3012 && Recibo.ValorAnteriorDiferenciaObraSocial != 0 && (-1 * ValorSobre >= ValorMin)) {
                        //devuelvo diferencia ObraSocial anterior
                        importe = -1 * Recibo.ValorAnteriorDiferenciaObraSocial;
                        if (importe < 0) {
                            importe *= -1;
                        }
                        Suma = true;
                    } else if (rowObject.Adicional_Id >= 3010 && rowObject.Adicional_Id <= 3012 && Recibo.ValorAnteriorDiferenciaObraSocial != 0 && ((-1 * ValorSobre < ValorMin && parseFloat(ValorMin) + parseFloat(ValorSobre)) < Recibo.ValorAnteriorDiferenciaObraSocial)) {
                        //devuelvo diferencia ObraSocial anterior
                        importe = parseFloat(ValorMin) + parseFloat(ValorSobre) + parseFloat(Recibo.ValorAnteriorDiferenciaObraSocial);
                        if (importe < 0) {
                            importe *= -1;
                        }
                        Suma = true;
                    } else if (rowObject.Adicional_Id >= 3010 && rowObject.Adicional_Id <= 3012 && Recibo.ValorAnteriorDiferenciaObraSocial != 0 && ((-1 * ValorSobre < ValorMin && parseFloat(ValorMin) + parseFloat(ValorSobre)) > Recibo.ValorAnteriorDiferenciaObraSocial)) {
                        //no devuelve hay diferencia nueva
                        importe = parseFloat(ValorMin) + parseFloat(ValorSobre) + parseFloat(Recibo.ValorAnteriorDiferenciaObraSocial);
                    } else {
                        //no se llego al minimo
                        importeMin = parseFloat(ValorMin) + parseFloat(ValorSobre); // porque el ValorSobre esta en negativo
                        if (importeMin > 0) {
                            importe = importeMin;
                        }
                        //si es mayor a cero es porque ya se saldo y no hay que devolver ni agregar diferencia
                    }

                }

            } else if (ValorSobre != null && ($.isNumeric(ValorSobre)) && rowObject.Unidades != null && ($.isNumeric(rowObject.Unidades))) {
                //es util para sueldo, dias, horas
                importe = rowObject.Unidades * ValorSobre;
            } else if (ValorSobre != null && ($.isNumeric(ValorSobre)) && Porcentaje != null && ($.isNumeric(Porcentaje))) {
                //es util para antiguedad, cuota sindical, obra social, ley
                importe = ValorSobre * Porcentaje / 100;
            } else {
                importe = ValorSobre;

            }
        }
        if (importe != null && ($.isNumeric(importe))) {
            importe = parseFloat(importe).toFixed(2);
        }

        //TODO hacer este cambio y probarlo
        //if (importe > 0) {
        //    importeSuma = importe;
        //    importeResta = -1 * importe;
        //} else {
        //    importeResta = importe;
        //    importeSuma = -1 * importe;
        //}

        importeSuma = importe;
        if (importeSuma > 0) {
            importeResta = "-" + importeSuma;
        } else {
            importeResta = importeSuma;
            //porque en la vista Detalle ya viene con el negativo
        }

        //switch (rowObject.Adicional_Id && (rowObject.Adicional == undefined || rowObject.Adicional == null))
        if ((rowObject.Adicional == undefined || rowObject.Adicional == null)) {
            switch (rowObject.Adicional_Id) {
                //TODO SQL
                case 6: //antiguedad
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                    valorAntiguedad = importe;
                    break;
                case 1004: //premio asistencia y puntualidad
                    valorPremioAsistenciaYPuntualidad = importe;
                    break;
                case 1005: //premio asistencia perfecta 
                    valorPremioAsistenciaPerfecta = importe;
                    break;
                case 1006:
                case 5012:
                case 5013:
                    valorRefrigerio = importe;
                    break;
                case 4007:
                    valorHorasExtras = importe;
                    break;
                case 4008:
                case 4009:
                    valorHorasExtras = importe;
                    break;
                case 9011:
                    valorDiaCortador = importe;
                    break;
                default:
                    break;
            }

        }

        return ((Suma == "true" || Suma == true) ? importeSuma : importeResta);


        //TODO TIENE SENTIDO ESTE SWITCH?
        //switch (rowObject.TipoLiquidacion) {
        //    case "0":
        //        return (rowObject.Suma == "true" ? importeSuma : importeResta);
        //        break;
        //    case "1":
        //        return (rowObject.Suma == "true" ? importeSuma : importeResta);
        //        break;
        //    case "2":
        //        return (rowObject.Suma == "true" ? importeSuma : importeResta);
        //        break;
        //}
    }
}



function Action_Aceptar(cellvalue, options, rowObject)
{
    return "<a class='boton boton-i place-right Realizar' title='Realizar'><span><i class='fa fa-check-circle-o'></i></span></a>";
}

//TODO creo que no lo usa nadie
function Action_Imprimir(cellvalue, options, rowObject)
{
        var ret = '';
        var url = options.colModel.formatoptions.urlAction + "/" + rowObject.Id;
        ret += '<a class="boton boton-i BtnImprimir" title="Imprimir" target="_blank" data-Id="' + rowObject.Id + '" href="' + url + '"><i class="fa fa-print i-blue"></i></a>';
        return ret;
        //a = "<a class='boton boton-i BtnImprimir place-left' style='border-bottom-color: #2a7442;' title='Imprimir' href=";
        //a += "'" + "@Url.Action(" + options.colModel.formatoptions.View + "," + options.colModel.formatoptions.Controller + ")?Id=" + rowObject.Id + "'"
        //a += "><i class='fa fa-print' style='padding: 2px 5px 0px 5px; color: #666666;'><br /><span style='color: #666666; font-size: 12px; padding: 0px; margin: 0px;'></span></i></a>";
        //return a;
}

//function formatterRecibo_Adicionales(cellvalue, options, rowObject) {

//    Adicional_Adicionales = new Array();
//    //New = { "Sobre": [ret.Data_AAs] };
//    Adicional_Adicionales.push(ret.Data_AAs);

//    if (options.colModel.formatoptions.SelectedRows != null && options.colModel.formatoptions.SelectedRows == rowObject.Id.toString()) {
//        return '<input type="radio" value="' + rowObject.Id + '" class="SelectCheckboxes" checked="checked"/>'
//    }
//    else {
//        return '<input type="radio" value="' + rowObject.Id + '" class="SelectCheckboxes" />'
//    }

//}











$.fn.CreateCommonGrid = function(params)
{
    _GridSelector = $(this);
    var gridId = _GridSelector.attr('id');
    var defaults = {
        ajaxGridOptions: { contentType: 'application/json; charset=utf-8' },
        datatype: 'json',
        serializeGridData: function (postData) {
            return JSON.stringify(postData);
        },
        mtype: 'POST',
        ignoreCase: true,
        jsonReader: {
            root: function (obj) { return obj.Records; },
            page: function (obj) { return obj.Page; },
            total: function (obj) { return obj.TotalPages; },
            records: function (obj) { return obj.TotalRecords; },
            repeatitems: false,
            id: 'Id'
        },
        loadonce: true,
        height: 400,
        sortable: true,
        rowList: [20, 40, 60],
        rowNum: 20,
        viewrecords: true,
        shrinkToFit: true,
        autowidth: true,
        onPaging: function () {
            $.HoldSelectedIDs(gridId)
        },
        loadComplete: function () {
            if (params.loadCompleteCtm != null) {
                params.loadCompleteCtm();
            }
            this.p.lastSelected = lastSelected; // set this.p.lastSelected
            $.HoldSelectedIDs(gridId);

            // Scrollbar offset detection
            if ($('#gbox_' + gridId + ' .ui-jqgrid-bdiv').hasVerticalScrollbar()) {
                $('#gbox_' + gridId + ' .ui-jqgrid-hbox').addClass('grid-has-scroll');
            }
            else {
                $('#gbox_' + gridId + ' .ui-jqgrid-hbox').removeClass('grid-has-scroll');
            }
        }
    }
    var par = $.extend(defaults, params);
    $(this).jqGrid(par);
}

$.HoldSelectedIDs = function (gridId) {
    if (_SelectedGridRows[gridId] == null) return;
    //Seleccionar de la pagina Nueva los que ya estaban seleccionados
    var CheckPage = _GridSelector.find('.SelectCheckboxes');
    for (var countBox = 0; countBox <= CheckPage.length ; countBox++) {
        if (_SelectedGridRows[gridId].indexOf($(CheckPage[countBox]).val()) >= 0) {
            $(CheckPage[countBox]).prop("checked", true);
        }
    }
}

$.SetupSearchTextbox = function (GridId, rulesFields, prefix) {
    var tbxSearchId = 'TbxSearch';
    var btnSearchId = 'BtnSearch';
    var btnClearSearchId = 'BtnClearSearch';
    if (prefix != null) {
        tbxSearchId = prefix + "_" + tbxSearchId;
        btnSearchId = prefix + "_" + btnSearchId;
        btnClearSearchId = prefix + "_" + btnClearSearchId;
    }

    function SearchGrid() {
        var searchFiler = $('#' + tbxSearchId).val(), grid = $(GridId), f;
        if (searchFiler.length === 0) {
            grid[0].p.search = false;
            $.extend(grid[0].p.postData, { filters: "" });
        }
        f = { groupOp: "OR", rules: [] };
        $.each(rulesFields, function (i, v) {
            f.rules.push({ field: v, op: "cn", data: searchFiler });
        });
        grid[0].p.search = true;
        $.extend(grid[0].p.postData, { filters: JSON.stringify(f) });
        grid.trigger("reloadGrid", [{ page: 1, current: true }]);
    }
    $('#' + btnSearchId).click(SearchGrid);
    $('#' + tbxSearchId).onEnterKey(SearchGrid);
    $('#' + btnClearSearchId).click(function () {
        $('#' + tbxSearchId).val('');
        $('#' + btnClearSearchId).addClass('hide');
        SearchGrid();
    });
    $('#' + tbxSearchId).keypress(function (ev) {
        var keycode = (ev.keyCode ? ev.keyCode : ev.which);
        if (keycode != '13') {
            if ($('#' + tbxSearchId).length != 0) {
                $('#' + btnClearSearchId).removeClass('hide');
            }
            else {
                $('#' + btnClearSearchId).addClass('hide');
            }
        }
    })
}

$.SetupSelectCount = function (gridId) {
    $(document).on('change', '.SelectCheckboxes', function () {
        var actual = parseInt($('#SelectCount').html());
        if ($(this).is(':checked')) {
            $('#SelectCount').html(++actual);
            if (_SelectedGridRows[gridId] == null)
            {
                _SelectedGridRows[gridId] = [];
            }
            _SelectedGridRows[gridId].push($(this).val());
        }
        else {
            $('#SelectCount').html(--actual);
            var pos = $.inArray($(this).val(), _SelectedGridRows[gridId]);
            if (pos != -1) {
                _SelectedGridRows[gridId].splice(pos, 1);
            }
        }
    });
    $('#SelectCount').click(function () {
        var gridSelector = $('#' + gridId);
        f = { groupOp: "OR", rules: [] };
        $.each(_SelectedGridRows[gridId], function (i, v) {
            f.rules.push({ field: 'Id', op: "eq", data: v });
        });
        gridSelector[0].p.search = true;
        $.extend(gridSelector[0].p.postData, { filters: JSON.stringify(f) });
        gridSelector.trigger("reloadGrid", [{ page: 1, current: true }]);
    });
    //$('#SelectAll').attr('onclick','$.SelectAllCheckboxClick(event)')
    //$.SelectAllCheckboxClick = function (e) {
    //    //e = e||event;/* get IE event ( not passed ) */
    //    //e.stopPropagation? e.stopPropagation() : e.cancelBubble = true;
    //    //Seleccionar de la pagina Nueva los que ya estaban seleccionados
    //    var CheckPage = _GridSelector.find('.SelectCheckboxes');
    //    for (var countBox = 0; countBox <= CheckPage.length ; countBox++) {
    //        $(CheckPage[countBox]).prop("checked", $(this).is(':checked'));
    //    }
    //}
}

$.SetupSetDefaulter = function (setUrl) {
    $(document).on('click', '.defaulter', function () {
        var IsDefault = $(this).children('i').hasClass('is-default');
        if (!IsDefault)
        {
            var Defaulter = $(this);
            var Id = $(this).data("id");
            var params = {
                url: setUrl,
                data: { Id: Id },
                success: function (ret) {
                    if (ret.Success) {
                        $('.defaulter i.is-default').removeClass('is-default fa-star').addClass('not-default fa-star-o');
                        Defaulter.children('i').removeClass('not-default fa-star-o').addClass('is-default fa-star');
                    }
                }
            }
            $.AjaxPost(params);
        }
    });
}

// Armo funcion para futura logica cuando hay mas de una grilla.
$.GetSelectedRows = function (GridId) {
    return _SelectedGridRows[GridId];
}

$.fn.resizeGrid = function (gridContainerId) {
    var gridId = $(this).attr('id');
    // GPG - 26-07-2015: Saque 
    // - parseInt($("#gbox_" + gridId).css("borderRightWidth")) - parseInt($("#gbox_" + gridId).css("borderLeftWidth"));
    // porque quedaba sin el borde y con el scroll horizontal.
    var width = $("#" + gridContainerId).outerWidth();
    $(this).setGridWidth(width);
}

GridCurrencyTemplate = {
    align: 'right', sorttype: 'number', editable: true,
    searchoptions: { sopt: ['eq', 'ne', 'lt', 'le', 'gt', 'ge', 'nu', 'nn', 'in', 'ni'] },
    formatter: function (v) {
        return $.formatCurrency(v);
    },
    unformat: function (v) {
        return $.unformatCurrency(v);
    }
};

// Cambia la funcion de JqGrid From para poder tener los elementos filtrados en el parametro lastSelected.
// se los puede llamar desde :$grid.jqGrid('getGridParam', 'lastSelected')

var oldFrom = $.jgrid.from,
    lastSelected;

$.jgrid.from = function (source, initalQuery) {
    var result = oldFrom.call(this, source, initalQuery),
        old_select = result.select;
    result.select = function (f) {
        lastSelected = old_select.call(this, f);
        return lastSelected;
    };
    return result;
};
