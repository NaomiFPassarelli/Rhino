$(document).ready(function () {
    $(document).on('click', '.alert-close', function () {
        $(this).closest('.alert-box').fadeOut();
    })
})

/*

    Post de ajax por el cual toda la app va a usar.
*/
$.AjaxPost = function (params) {
    if (typeof (params.withoutLoader) === "undefined" || !params.withoutLoader) {
        $.ShowLoader();
    }   
    defParams = {
        async: true,
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        error: function (e) {
            $.HideLoader();
            if (e.ErrorMessage) {
                $.NewAlert("error", e.ErrorMessage);
            }
        }
    };
    var postParams = $.extend(defParams, params);

    // Si no viene por formData hacerle stringify.
    if (typeof (params.hasFormData) === "undefined" || !params.hasFormData) {
        postParams.data = JSON.stringify(postParams.data, null, 2);
    }
    
    _successCallback = params.success;
    postParams.success = function (data) {
        $.HideLoader();
        if ($.isFunction(_successCallback) && _successCallback != null) {
            _successCallback(data);
        }
       
    }
    return $.ajax(postParams);
}

/* 
    Genera una nueva alerta del sistema que se posiciona arriba, error, success, info, warning
*/
$.fn.NewAlert = function (type, msg) {
    var html = '<div class="alert-box ' + type + '">';
    html += '<p class="message">' + msg + '</p>';
    html += '<p class="close"><a title="Cerrar" class="alert-close">Cerrar</a></p>';
    html += '</div>';
    $(this).html(html);

}

/* 
    Binding del enter a una funcion callback 
*/
$.fn.onEnterKey = function (fnc) {
    $(this).keypress(function (ev) {
        var keycode = (ev.keyCode ? ev.keyCode : ev.which);
        if (keycode == '13') {
            fnc.call(this, ev);
        }
    })
}
$.fn.onDocumentEnterKey = function (fnc) {
    $(document).keypress(function (ev) {
        var keycode = (ev.keyCode ? ev.keyCode : ev.which);
        if (keycode == '13') {
            fnc.call(this, ev);
        }
    });
}



$.parseCurrency = function(value){
    if (isNaN(parseFloat(value))) 
        return 0;
    return parseFloat(value).toFixed(2);
}
$.formatCurrency = function(value){
    return accounting.formatMoney(value, "$", 2, ".", ",");
}
$.unformatCurrency = function (value) {
    return accounting.unformat(value,",");
}

Array.prototype.diff = function (a) {
    return this.filter(function (i) { return a.indexOf(i) < 0; });
};

Number.prototype.toFixedNumber = function (decimals) {
    return parseFloat(this.toFixed(decimals));
};

$.ExportGridToExcel = function (GridId, Filename) {
    alert('16/01/2015 - Funcion deprecated, utilizar -> JQGridExportExcel.Export');
}

$.ExportTableToExcel = function (html, filename) {
    var uri = 'data:application/vnd.ms-excel;base64,'
        , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
        , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
    var ctx = { worksheet: filename, table: html }
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");
    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
    {
        ieexcel.document.open("txt/html", "replace");
        ieexcel.document.write(html);
        ieexcel.document.close();
        ieexcel.focus();
        return ieexcel.document.execCommand("SaveAs", true, filename + ".xls");
    } else {
        document.getElementById("exportToExcel_dump").href = uri + btoa(format(template, ctx));
        document.getElementById("exportToExcel_dump").download = filename;
        document.getElementById("exportToExcel_dump").click();
    }

}

$.ValidateDates = function (start, end) {
    daystart = ((start).split("/"))[0];
    monthstart = ((start).split("/"))[1];
    yearstart = ((start).split("/"))[2];

    _start = new Date(yearstart, monthstart - 1, daystart);

    dayend = ((end).split("/"))[0];
    monthend = ((end).split("/"))[1];
    yearend = ((end).split("/"))[2];

    _end = new Date(yearend, monthend - 1, dayend);

    if ( _start > _end ) {
        return { success: false, message: 'La Fecha Desde debe ser menor o igual a la Fecha Hasta' };
    } else {
        return { success: true };
    }
}

$.CompletarFiltroDates = function (start, end) {
    var start_current = new Date();
    var end_current = new Date();
    if (start.val() == "" && end.val() == "") {
        start.val(DateAdd('m', start_current, -1).format("dd/mm/yyyy"));
        end.val(end_current.format("dd/mm/yyyy"));
    }
    if (end.val() == "") {
        end.val(end_current.format("dd/mm/yyyy"));
    }
    if (start.val() == "") {
        day = ((end.val()).split("/"))[0];
        month = ((end.val()).split("/"))[1] - 1;
        year = ((end.val()).split("/"))[2];

        date_start = new Date(year, month - 1, day);
        start.val(date_start.format("dd/mm/yyyy"));
    }
}

$.CantidadDeDiasEntreFechas = function (start, end) {
    //TODO validar que las fechas no esten vacias, sino completarlas, y validar que el start sea menor que end

    dayStart = (start.split("/"))[0];
    monthStart = (start.split("/"))[1];
    yearStart = (start.split("/"))[2];
    var dateStart = new Date(yearStart, monthStart - 1, dayStart);

    dayEnd = (end.split("/"))[0];
    monthEnd = (end.split("/"))[1];
    yearEnd = (end.split("/"))[2];
    var dateEnd = new Date(yearEnd, monthEnd - 1, dayEnd);

    //TODO HAY UNA FUNCION QUE HACE LA DIFERENCIA ENTRE FECHAS REPLICAR EN CALCULARANTIGUEDAD!

    // Calculate days between dates
    var millisecondsPerDay = 86400 * 1000; // Day in milliseconds
    dateStart.setHours(0, 0, 0, 1);  // Start just after midnight
    dateEnd.setHours(23, 59, 59, 999);  // End just before midnight
    var diff = dateEnd - dateStart;  // Milliseconds between datetime objects   
    var days = Math.ceil(diff / millisecondsPerDay);

    return days;
},

$.CalcularDiasLaborales = function (start, end) {
    //TODO validar que las fechas no esten vacias, sino completarlas, y validar que el start sea menor que end

    dayStart = (start.split("/"))[0];
    monthStart = (start.split("/"))[1];
    yearStart = (start.split("/"))[2];
    var dateStart = new Date(yearStart, monthStart - 1, dayStart);

    dayEnd = (end.split("/"))[0];
    monthEnd = (end.split("/"))[1];
    yearEnd = (end.split("/"))[2];
    var dateEnd = new Date(yearEnd, monthEnd - 1, dayEnd);

    //TODO HAY UNA FUNCION QUE HACE LA DIFERENCIA ENTRE FECHAS REPLICAR EN CALCULARANTIGUEDAD!

    // Calculate days between dates
    var millisecondsPerDay = 86400 * 1000; // Day in milliseconds
    dateStart.setHours(0, 0, 0, 1);  // Start just after midnight
    dateEnd.setHours(23, 59, 59, 999);  // End just before midnight
    var diff = dateEnd - dateStart;  // Milliseconds between datetime objects   
    var days = Math.ceil(diff / millisecondsPerDay);

    // Subtract two weekend days for every week in between
    var weeks = Math.floor(days / 7);
    var days = days - (weeks * 2);

    // Handle special cases
    var startDay = dateStart.getDay();
    var endDay = dateEnd.getDay();

    // Remove weekend not previously removed.   
    if (startDay - endDay > 1)
        days = days - 2;

    // Remove start day if span starts on Sunday but ends before Saturday
    if (startDay == 0 && endDay != 6)
        days = days - 1;

    // Remove end day if span ends on Saturday but starts after Sunday
    if (endDay == 6 && startDay != 0)
        days = days - 1;

    return days;
},



$.fn.hasVerticalScrollbar = function () {
    var divnode = this.get(0);
    if (divnode.scrollHeight > divnode.clientHeight)
        return true;
}



// Funciones de Fecha

function AddBusinessDays(date, weekDaysToAdd) {
    var curdate = date;
    var realDaysToAdd = 0;
    while (weekDaysToAdd > 0) {
        curdate.setDate(curdate.getDate() + 1);
        realDaysToAdd++;
        //check if current day is business day
        if (noWeekendsOrHolidays(curdate)[0]) {
            weekDaysToAdd--;
        }
    }
    return realDaysToAdd;

}
function noWeekendsOrHolidays(date) {
    return $.datepicker.noWeekends(date);
}

function DateAdd(ItemType, DateToWorkOn, ValueToBeAdded)
{
    switch (ItemType)
    {
        //date portion         
        case 'd': //add days
            DateToWorkOn.setDate(DateToWorkOn.getDate() + ValueToBeAdded)
            break;
        case 'm': //add months
            DateToWorkOn.setMonth(DateToWorkOn.getMonth() + ValueToBeAdded)
            break;
        case 'y': //add years
            DateToWorkOn.setYear(DateToWorkOn.getFullYear() + ValueToBeAdded)
            break;
            //time portion         
        case 'h': //add days
            DateToWorkOn.setHours(DateToWorkOn.getHours() + ValueToBeAdded)
            break;
        case 'n': //add minutes
            DateToWorkOn.setMinutes(DateToWorkOn.getMinutes() + ValueToBeAdded)
            break;
        case 's': //add seconds
            DateToWorkOn.setSeconds(DateToWorkOn.getSeconds() + ValueToBeAdded)
            break;
    }
    return DateToWorkOn;
}

function ClearSelectedRows(GridName) {
    _SelectedGridRows[GridName] = [];
    $('#SelectCount').html('0');
}

