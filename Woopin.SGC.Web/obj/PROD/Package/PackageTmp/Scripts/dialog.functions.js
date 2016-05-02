/*
    Abre dialog de JQUI haciendole get a la url que le manda. Parsea el dialog por si tiene validaciones.
*/
$.fn.OpenFormDialog = function (geturl, title, opt) {
    $.ShowLoader();
    var DialogSelector = $(this);
    var DefOpt = {
        resizable: true,
        draggable: true,
        modal: true,
        width: 500,
        height: 400,
        title: title,
        close: function (event, ui) {
            DialogSelector.parent('.ui-dialog').html('');
            DialogSelector.html('');
            _OpenDialog = null;
        },
        open: function () {
            $.HideLoader();
            DialogSelector.parent('.ui-dialog').append($('.button-pane').detach());
            var form = DialogSelector
                .removeData("validator") /* added by the raw jquery.validate plugin */
                .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin */
            $.validator.unobtrusive.parse(form);
            _OpenDialog = DialogSelector;
            DialogSelector.position({
                my: "center",
                at: "center",
                of: window
            });
            if (opt != null && opt.openCallback != null) {
                opt.openCallback();
            }
        }
    };
    var Options = $.extend(DefOpt, opt);
    $.get(geturl, function (data) {
        DialogSelector.html(data);
        DialogSelector.dialog(Options);
    });
}
$.fn.OpenDetailDialog = function (geturl, title, opt) {
    $.ShowLoader();
    var DialogSelector = $(this);
    var DefOpt = {
        resizable: true,
        draggable: true,
        modal: true,
        width: 500,
        height: 400,
        title: title,
        close: function (event, ui) {
            DialogSelector.parent('.ui-dialog').html('');
            _OpenDialog = null;
        },
        open: function () {
            $.HideLoader();
            DialogSelector.parent('.ui-dialog').append($('.button-pane').detach());
            _OpenDialog = DialogSelector;
            DialogSelector.position({
                my: "center",
                at: "center",
                of: window
            });
            if (opt != null && opt.openCallback != null) {
                opt.openCallback();
            }
        }
    };
    var Options = $.extend(DefOpt, opt);
    $.get(geturl, function (data) {
        DialogSelector.html(data);
        DialogSelector.dialog(Options);
    });
}

$.CloseOpenedDialog = function () {
    _OpenDialog.dialog("close");
}