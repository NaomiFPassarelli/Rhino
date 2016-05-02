/*
    Configura a un boton de formulario para que mande por ajax el formulario que lo contiene.Previamente valida.
    Parametros: Los propios de ajax
                ++ enter : Si le bindea el evento del enter al boton.
*/
$.fn.FormSubmit = function (params) {
    var btn = $(this);
    var form = btn.closest('form');
    if (form.length == 0) // Esta en un dialog
    {
        form = btn.closest('.button-pane').parent('div').children('form');
    }
    $(this).click(function () {
        SetupFormParams(form, params);
    });
    if (params.enter != undefined && params.enter) {
        form.onEnterKey(function (e) {
            SetupFormParams(form, params);
            e.preventDefault();
        });
    }
}
function SetupFormParams(form,params)
{
    if (!form.valid()) return;

    // Check if form is submiting a file.
    var sendData = null;
    if (form.attr('enctype') == "multipart/form-data") {
        sendData = form.serializeFormData();
    }
    else {
        sendData = form.serializeFormJSON();
        for (prop in sendData) {
            if (prop.indexOf('.Id') != -1 && sendData[prop] == "") {
                delete sendData[prop];
            }
        }
    }
     

    var formParams = {
        url: form.attr('action'),
        type: form.attr('method'),
        data: sendData
    }

    if (form.attr('enctype') == "multipart/form-data") {
        formParams = $.extend(formParams, {
            contentType: false,
            processData: false,
            hasFormData: true
        });
    }

    var postParams = $.extend(formParams, params);
    $.AjaxPost(postParams);
}

/*
    Dado como selector a un grupo de checkbox, te devuelve sus valores.
*/
$.fn.GetCheckboxValues = function () {
    var SelectedIds = [];
    $(this).each(function () {
        SelectedIds.push($(this).val());
    });
    return SelectedIds;
}

$.fn.ClearForm = function () {
    var form = $(this);
    form.find('input').val('');
    form.find('textarea').val('');
    form.find('select').val('');
    form.find('input[type=checkbox]').attr('checked', 'false');
    form.find('.select2-container').select2('val', '')
}

$.fn.ClearFormNuevo = function () {
    
    $('form input.textinput.value-def').each(function (i, e) { $(e).attr('value') })
    $('form input').each(function (i, e) {
        if (!$(e).hasClass('value-def')) {
            $(e).val("");
        }
    });
    $('form select').each(function (i, e) { $($($(e).find('option'))[0]).attr("selected", "selected") });
    $('form .select2-container').each(function (i, e) {
        select2Clear = $($(e).next('input'));
        select2Clear.select2("val", "");
        IdClear = select2Clear.attr('id').split("_")[0];
        $("#" + IdClear + "ID").val("");
    });
}

$.fn.serializeFormJSON = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

$.fn.serializeFormData = function () {
    var a = this.serializeArray();
    var formData = new FormData();
    $.each(a, function () {
        formData.append(this.name, this.value);
    });
    var files = $('form').find('input[type=file]');
    $.each(files, function (index,item) {
        if (item.files.length > 0) {
            for (var i = 0; i < item.files.length; i++) {
                formData.append(item.name, item.files[i]);
            }
        }
    });
    return formData;
};

$.makeErrorMsgList = function (msg, list) {
    var str = msg;
    if (list != null) {
        str += "</br>";
        str += "<ul>"
        $.each(list, function (index, item) {
            str += "<li>" + item.ErrorMessage + "</li>";
        })
        str += "<ul>"
    }
    return str;
}

// Matiene un campo en formato de moneda

$.fn.currencyFormat = function () {
    this.each(function (i) {
        $(this).change(function (e) {
            if (isNaN(parseFloat(this.value))) return;
            this.value = parseFloat(this.value).toFixed(2);
        });
    });
    return this; //for chaining
}

$.fn.reParseForm = function () {
    var form = $(this)
            .removeData("validator") /* added by the raw jquery.validate plugin */
            .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin */

    $.validator.unobtrusive.parse(form);
}

$.SetupUniqueNonQuitableForm = function (beforeQuitTitle) {
    // Poner input en pag.
    //<input type="hidden" id="needrefresh" value="0">

    window.onload = function () {
        var e = document.getElementById("needrefresh");
        if (e.value == "0") e.value = "1";
        else { e.value = "0"; location.reload(); }
    }
    if ($('#needrefresh').val() == '0') {
        window.onbeforeunload = function () {
            return beforeQuitTitle;
        };
    }
    // Importante!!!
    // En caso de necesitar redirigir eliminar el onbeforeunload antes de hacerlo
    // Ejemplo:  window.onbeforeunload = null;
}


$.fn.fillSelect = function (opt) {
    var select = $(this);
    var postParams = {
        url: opt.url,
        data: opt.data
    };
    // Utilizamos promise por que con success del ajax llegaba el primero y el segundo con el mismo contexto.
    var promise = $.AjaxPost(postParams);
    promise.done(function (ret) {
        if (opt.placeholder != null) {
            select.append($("<option>").text(opt.placeholder))
        }

        $.each(ret.Data.Items, function () {
            var option = $("<option>").attr('value', this.id).text(this.text).attr('data-ad', this.additionalData);

            if (ret.Data.Items.length == 1 || opt.selectedValue == this.id) {
                option.attr('selected', 'selected')
            }

            select.append(option);
        });

        if (ret.Data.Items.length == 1) {
            select.trigger('change');
        }
    });
}