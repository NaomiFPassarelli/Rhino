
/* Migracion completa a la nueva funcion ConfirmationDialog.Show() */
$.fn.ConfirmDialog = function (msg, btns) {
    alert('DEPRECATED!!! 12/2014 . Pasa a ConfirmationDialog')
};


var ConfirmationDialog = {

    CancelCallback: function () {
        $("#dialog-confirm").dialog("close");
        if (ConfirmationDialog.customCancelCallback != null) {
            ConfirmationDialog.customCancelCallback();
        }
    },

    customCancelCallback: null,

    SuccessCallback: function () {
        $("#dialog-confirm").dialog("close");
        if (ConfirmationDialog.customSuccessCallback != null) {
            ConfirmationDialog.customSuccessCallback();
        }
    },

    customSuccessCallback: null,

    Show: function (msg, success, cancel) {
        // Limpia los callbacks
        ConfirmationDialog.customCancelCallback = null;
        ConfirmationDialog.customSuccessCallback = null;


        if (cancel != null) {
            ConfirmationDialog.customCancelCallback = cancel;
        }
        if (success != null) {
            ConfirmationDialog.customSuccessCallback = success;
        }

        var html = '<div class="visual"><i class="fa fa-warning"></i></div>'; // Background
        html += '<div class="detail">' + msg + '</div>'; // Detalle del Mensaje

        var btns = '<div class="button-pane clearfix" style="margin-top:-10px;"><div class="button-set" style="text-align:center;float:none;padding-top:5px;padding-bottom:5px;">'
        btns += '<a class="boton" title="Confirmar" onclick="ConfirmationDialog.SuccessCallback()" style="float:none;"><i class="fa fa-check-circle-o i-green"></i>  Si, Confirmar</a>'
        btns += '<a class="boton" title="Cancelar" onclick="ConfirmationDialog.CancelCallback()" style="float:none;"><i class="fa fa-times-circle-o i-red"></i>  No, Cancelar</a>'
        btns += '<div class="clearfix"></div></div></div>';

        html += btns; // Botones

        $("#dialog-confirm").html(html);
        $("#dialog-confirm").dialog({
            resizable: false,
            height: 100,
            width: 400,
            modal: true,
            open: function () {
                $("#dialog-confirm").parent('.ui-dialog').append($('#dialog-confirm .button-pane').detach());
            },
            close: function () {
                $("#dialog-confirm").parent('.ui-dialog').html('');
            }
        });
        _OpenDialog = $("#dialog-confirm");
    }
}

$.ShowLoader = function () {
    $('body').addClass("loading");
}
$.HideLoader = function () {
    $('body').removeClass("loading");
}

_WarningDialogCallback = $.CloseOpenedDialog;

$.WarningDialog = function (msg, type, callback) {
    if (callback != null) {
        _WarningDialogCallback = function () {
            callback();
            $.CloseOpenedDialog();
        }
    } else {
        // Reset por distintos tipos de warning dialogs en misma pag.
        _WarningDialogCallback = $.CloseOpenedDialog;
    }
    if (type == "error"){
        var icon = 'fa-times-circle';
    }
    else if (type == "info"){
        var icon = 'fa-info-circle';
    }
    else if (type == "success") {
        var icon = 'fa-check-circle';
    }
    else{
        var icon = 'fa-warning';
    }
    var bg = '<div class="visual"><i class="fa ' + icon + '"></i></div>';
    var htmlMsg = '<div class="detail">' + msg + '</div>';
    var btns = '<div class="button-pane clearfix" style="margin-top:-10px;"><div class="button-set" style="text-align:center;float:none;padding-top:5px;padding-bottom:5px;">'
    btns += '<a class="boton" title="Aceptar" onclick="_WarningDialogCallback()" style="float:none;"><i class="fa fa-check-circle-o i-green"></i>  Aceptar</a>'
    btns += '<div class="clearfix"></div></div></div>';
   
    $("#dialog-warning").html(bg + htmlMsg + btns);
    $("#dialog-warning").dialog({
        resizable: false,
        height: 100,
        width: 400,
        modal: true,
        open: function () {
            $("#dialog-warning").parent('.ui-dialog').append($('#dialog-warning .button-pane').detach());
        },
        close: function () {
            $("#dialog-warning").parent('.ui-dialog').html('');
            $(document).off('keypress', OnWarningDialogEnter);
        }
    });
    _OpenDialog = $("#dialog-warning");
    $(document).on('keypress', OnWarningDialogEnter);
};

function OnWarningDialogEnter(ev) {
    var keycode = (ev.keyCode ? ev.keyCode : ev.which);
    if (keycode == '13') {
        _WarningDialogCallback();
    }
}


