//Validation
//$('[data-toggle="tooltip"]').tooltip();

$(document).on('click', '.bb', function () {
    alert(lastScreen);
    location.href = lastScreen;
    return false;
});

function validateForm(id) {
    validJson = new Array();
    validJson["rules"] = {};
    validJson["messages"] = {};
    validJson["errorPlacement"] = function (error, element) {
        error.insertAfter(element.parent());
    };
    $("form#" + id + " [name]").each(function () {
        me = $(this);
        if (me.data('validation') != undefined) {
            validJson["rules"][me.attr("name")] = {};
            validJson["messages"][me.attr("name")] = {};
            var validations = me.data('validation').split(';');
            for (var item in validations) {
                var keyValues = validations[item].split(':');
                if (keyValues.length <= 1) {
                    validJson["rules"][me.attr("name")][keyValues[0]] = true;
                }
                else {
                    if (keyValues[0] == "remote") {
                        var remoteJson = {};
                        var remoteUrl = keyValues[1].split('>')[0];
                        var remoteRules = keyValues[1].split('>')[1];
                        var tableName = remoteRules.split('?')[0];
                        var fields = remoteRules.split('?')[1].split('&');

                        remoteJson['url'] = remoteUrl.split('=')[0];
                        remoteJson['type'] = remoteUrl.split('=')[1];
                        remoteJson['data'] = {};
                        remoteJson['data']['tableName'] = tableName;
                        for (var item in fields) {
                            remoteJson['data'][fields[item]] = new Function(fields[item], "return $('.ajaxForm #" + fields[item] + "').val();");
                        }
                        validJson["rules"][me.attr("name")][keyValues[0]] = eval("(" + stringify(remoteJson) + ")");

                        var remoteMessage = $('.ajaxForm #' + fields[item]).data('remotemessage');

                        if (remoteMessage) {
                            validJson["messages"][me.attr("name")][keyValues[0]] = remoteMessage;
                        } else
                            if (fields.length == 1) {
                                validJson["messages"][me.attr("name")][keyValues[0]] = tj("remote").replace('{0}', fields[0]);
                            } else {
                                var Listfields = remoteRules.split('?')[1];
                                validJson["messages"][me.attr("name")][keyValues[0]] = tj("remotePlus").replace('{0}', replaceAll('&', ', ', Listfields));
                            }
                    } else {
                        validJson["rules"][me.attr("name")][keyValues[0]] = eval(keyValues[1]);
                    }
                }
                if (keyValues[0] != "remote")
                    validJson["messages"][me.attr("name")][keyValues[0]] = tj(keyValues[0]);
            }
        }
    });
    $("#" + id).validate(validJson);
}

function replaceAll(find, replace, str) {
    return str.replace(new RegExp(find, 'g'), replace);
}

var stringify = function (obj, prop) {
    var placeholder = '____PLACEHOLDER____';
    var fns = [];
    var json = JSON.stringify(obj, function (key, value) {
        if (typeof value === 'function') {
            fns.push(value);
            return placeholder;
        }
        return value;
    }, 2);
    json = json.replace(new RegExp('"' + placeholder + '"', 'g'), function (_) {
        return fns.shift();
    });
    return json;
};

function isValid(id) {
    return $("#" + id).valid();
}

$(document).ready(function () {

    var loadingColor = ["loadingBlue", "loadingRed", "loadingGray", "loadingOrange", "loadingGreen"];
    $(document).ajaxComplete(function () {
        loading(false);
    });

    $(document).ajaxStop(function () {
        loading(false);
    });

    $(document).ajaxSend(function (event, request, settings) {
        if (settings.url.toLowerCase().indexOf('/home/check') == -1) {
            $("#dvLoading").removeClass();
            var color = loadingColor[Math.floor(Math.random() * loadingColor.length)];
            $("#dvLoading").addClass(color);
            loading(true);
        }
    });

});

function loading(show) {
    if (show) {
        $("#dvLoading").show();
    }
    else {
        $("#dvLoading").hide();
    }
}



function invokebaseDynamicModal(action, selector) {
    $me = $(selector);
    $refi = $me.parent().find("#refi").val();
    if ($refi == undefined)
        $refi = "";
    if (action) {
        $me.data('loading-text', "<i class='fa fa-refresh fa-spin'></i>");
        $me.button('loading');
        $('#baseDynamicModalBody').load(action, function () {
            options = {
                backdrop: true,
                keyboard: true
            };
            $('#baseDynamicModalObject').modal(options);

            $info = $("#modalInfo" + $refi);
            if ($info == undefined) {
                $info = $("#modalInfo");
            }
            $('#baseGenericModalTitle').html("");
            if ($info.data("modalimg")) {
                $('#baseGenericModalTitle').append("<img src='" + $info.data("modalimg") + "'>");
            }

            if ($info.data("width") != undefined) {

                if (!ismobile)
                    $('.modal-dialog').attr("style", "width: " + $info.data("width") + " !important;")
                else
                    $('.modal-dialog').attr("style", "width: " + $info.data("mobilewidth") + " !important;")
            } else {
                $('.modal-dialog').attr("style", "")
            }

            if ($info.data("editing") == "True") {
                $('#baseGenericModalTitle').append($info.data("editmodaltitle"));
            } else {
                $('#baseGenericModalTitle').append($info.data("modaltitle"));
            }
            $me.button('reset');
        });
    }
    return false;
}

function invokebaseDynamicModalChild(action, selector) {
    $me = $(selector);
    if (action) {
        //este se usa
        $me.data('loading-text', "<i class='fa fa-refresh fa-spin'></i>");
        $me.button('loading');
        //angular.element('#baseDynamicModalBody').load(action, function () {
        $('#baseDynamicModalBodyChild').load(action, function () {

            options = {
                backdrop: true,
                keyboard: true
            };

            $('#baseDynamicModalObjectChild').modal(options);


            $info = $("#modalInfoChild");
            $('#baseGenericModalTitleChild').html("");
            if ($info.data("modalimg")) {
                $('#baseGenericModalTitleChild').append("<img src='" + $info.data("modalimg") + "'>");
            }

            if ($info.data("width") != undefined) {
                if (!ismobile)
                    $('.modal-dialog').attr("style", "width: " + $info.data("width") + " !important;")
                else
                    $('.modal-dialog').attr("style", "width: " + $info.data("mobilewidth") + " !important;")
            } else {
                $('.modal-dialog').attr("style", "")
            }

            if ($info.data("editing") == "True") {
                $('#baseGenericModalTitleChild').append($info.data("editmodaltitle"));
            } else {
                $('#baseGenericModalTitleChild').append($info.data("modaltitle"));
            }
            $me.button('reset');
        });
    }
    return false;
}

//$('#baseDynamicModalObject').on('shown.bs.modal', function () {    
//    $(this).removeData('bs.modal');    
//});


$(document).on('click', '.refreshTable', function (e) {
    $me = $(this);
});

$(document).on('click', '.baseDynamicModal', function (e) {
    //verdadero modal
    
    $me = $(this);
    $refi = $me.parent().find("#refi").val();
    if ($refi == undefined)
        $refi = "";
    if ($me.data("action")) {
        $me.data('loading-text', "<i class='fa fa-refresh fa-spin'></i>");
        $me.button('loading');
        $.ajax({
            url: $me.data("action"),
            type: 'POST',
            dataType: 'text',
            success: function (data, textStatus, jqXHR) {

                $me.data("update", "true");
                $('#baseDynamicModalBody').html(data);




                options = {
                    backdrop: $me.data("backdrop") || true,
                    keyboard: $me.data("keyboard") || true
                };

                $('#baseDynamicModalObject').modal(options);


                $info = $("#modalInfo" + $refi);
                if ($info == undefined || $info.length == 0)
                    $info = $("#modalInfo");

                var formClose = $info.parent().find('.formClose:eq(0)');
                formClose.hide();
                $(".fromFilter").show();
                $(".fromColumns").show();
                $(".fromBack").show();

                $info.data('ismodal', true);
                $('#baseGenericModalTitle').html("");
                if ($info.data("modalimg")) {
                    $('#baseGenericModalTitle').append("<img src='" + $info.data("modalimg") + "'>");
                }

                if ($info.data("width") != undefined) {
                    if (!ismobile)
                        $('.modal-dialog').attr("style", "width: " + $info.data("width") + " !important;")
                    else
                        $('.modal-dialog').attr("style", "width: " + $info.data("mobilewidth") + " !important;")
                } else {
                    $('.modal-dialog').attr("style", "")
                }

                if ($info.data("editing") == "True") {
                    $('#baseGenericModalTitle').append($info.data("editmodaltitle"));
                } else {
                    $('#baseGenericModalTitle').append($info.data("modaltitle"));
                }
                $me.button('reset');
            },
            error: function (data) {
                $me.button('reset');
            }
        });
    }
    return false;
});

$(document).on('click', '.baseDynamicModalChild', function (e) {
    $me = $(this);
    //este tambien
    if ($me.data("action")) {
        $me.data('loading-text', "<i class='fa fa-refresh fa-spin'></i>");
        $me.button('loading');
        //angular.element('#baseDynamicModalBody').load($me.data("action"), function () {
        $.ajax({
            url: $me.data("action"),
            type: 'POST',
            dataType: 'text',
            success: function (data, textStatus, jqXHR) {
                $('#baseDynamicModalBodyChild').html(data);
                options = {
                    backdrop: $me.data("backdrop") || true,
                    keyboard: $me.data("keyboard") || true
                };

                $('#baseDynamicModalObjectChild').modal(options);


                $info = $("#modalInfoChild");
                $('#baseGenericModalTitleChild').html("");
                if ($info.data("modalimg")) {
                    $('#baseGenericModalTitleChild').append("<img src='" + $info.data("modalimg") + "'>");
                }
                if ($info.data("editing") == "True") {
                    $('#baseGenericModalTitleChild').append($info.data("editmodaltitle"));
                } else {
                    $('#baseGenericModalTitleChild').append($info.data("modaltitle"));
                }
                $me.button('reset');
            },
            error: function (data) {
                $me.button('reset');
            }
        });


        //$('#baseDynamicModalBody').load($me.data("action"), function () {


        //});
    }
    return false;
});

$(document).on('click', '.baseDropDownRefresh', function (e) {
    e.preventDefault();
    $me = $(this);
    $baseSelect = $me.parent().prev("div").find(".baseSelect.select2.select2-offscreen:eq(0)");
    $where = $baseSelect.data("where");
    $table = $baseSelect.data("table");
    $text = $baseSelect.data("text");
    $value = $baseSelect.data("value");
    $action = $baseSelect.data("action");
    $groupby = $baseSelect.data("groupby");
    $selected = new Array();
    jQuery.ajaxSettings.traditional = true;
    $("#" + $baseSelect.attr("id") + " option:selected").each(function () {
        $selected.push($(this).val());
    });
    $me.data('loading-text', "<i class='fa fa-refresh fa-spin'></i>");
    $me.button('loading');
    $.ajax({
        url: $action,
        type: 'POST',
        dataType: 'json',
        data: { where: $where, table: $table, text: $text, value: $value, selected: $selected, groupby: $groupby },
        success: function (data, textStatus, jqXHR) {
            $baseSelect.empty();
            groupCheck = "";
            first = true;
            finalString = "";
            $.each(data, function (i, item) {
                if (item.Group) {
                    if (groupCheck != item.Group) {
                        groupCheck = item.Group;
                        if (!first)
                            finalString += "</optgroup>";
                        finalString += '<optgroup label="' + item.Group + '">';
                        first = false;
                    }
                }
                finalString += '<option ' + (item.Selected ? "selected" : "") + ' value="' + item.Value + '">' + item.Text + '</option>';
            });
            $baseSelect.append(finalString);
            $me.button('reset');
            //pageSetUp();
        },
        error: function (data) {
            $me.button('reset');
        }
    });
    return false;
});

$(document).on('click', '.baseSelectorButtonsRefresh', function (e) {
    e.preventDefault();
    $me = $(this);
    $baseSelect = $me.parent().prev(".baseSelectorButtons");
    $where = $baseSelect.data("where");
    $table = $baseSelect.data("table");
    $text = $baseSelect.data("text");
    $value = $baseSelect.data("value");
    $action = $baseSelect.data("action");
    $col = $baseSelect.data("col");
    $row = $baseSelect.data("row");
    $toggleoff = $baseSelect.data("toggleoff");
    $toggleon = $baseSelect.data("toggleon");
    $name = $baseSelect.data("name");
    $type = $baseSelect.data("type");
    $toggle = $baseSelect.data("toggle");
    $selected = new Array();
    $baseSelect.find("input[type=" + $type + "]:checked").each(function () {
        $selected.push($(this).val());
    });
    jQuery.ajaxSettings.traditional = true;
    $me.data('loading-text', "<i class='fa fa-refresh fa-spin'></i>");
    $me.button('loading');
    $.ajax({
        url: $action,
        type: 'POST',
        dataType: 'text',
        data: {
            where: $where, table: $table, text: $text, value: $value, selected: $selected,
            col: $col, row: $row, toggleoff: $toggleoff, toggleon: $toggleon, name: $name, type: $type, toggle: $toggle
        },
        success: function (data, textStatus, jqXHR) {
            $me.parent().prev(".baseSelectorButtons").html(data);
            $me.button('reset');
            //pageSetUp();
        },
        error: function (data) {
            $me.button('reset');
        }
    });
    return false;
});

$buttonClicked = null;
$buttonMadaled = null;
$(document).on('click', 'form button', function (e) {
    $buttonClicked = $(this);
});

$(document).on('click', '.baseDynamicModal', function (e) {
    $buttonMadaled = $(this);
});

$(document).on('click', '.baseCheck', function (e) {
    var id = $(this).attr("id");
    var checked = $(this).prop("checked");
    $(this).val(checked ? true : false);
    $("input[name='" + id + "']:hidden").val(checked ? true : false);
});


$(document).on('click', '.formClose', function (e) {
    $me = $(this);
    $begin = $me.data('begin');
    if ($begin) {
        eval($begin);
    }
    $('#baseDynamicModalObject').modal('hide');
    if ($me.attr("href") != undefined)
        if ($me.attr("href").indexOf("#") !== -1)
            location.href = $me.attr("href");
    return false;
});

$(document).on('click', '.formCloseChild', function (e) {
    $me = $(this);
    $begin = $me.data('begin');
    if ($begin) {
        eval($begin);
    }
    $('#baseDynamicModalObjectChild').modal('hide');
    return false;
});

$('#baseDynamicModalObject').on('hidden.bs.modal', function () {
    $me = $(this);
    $refi = $me.parent().find("#refi").val();
    if ($refi == undefined)
        $refi = "";
    $me.removeData();

    //este tambien    
    $info = $("#modalInfo" + $refi);
    if ($info == undefined) {
        $info = $("#modalInfo");
    }
    if ($info.data("onclose")) {
        eval($info.data("onclose"));
    }
    $('#baseDynamicModalBody').html('');
});

$('.csTableFilters').on('hidden.bs.modal', function () {
    $me = $(this);
    $refi = $me.parent().find("#refi").val();
    if ($refi == undefined)
        $refi = "";
    $info = $("#modalInfo" + $refi);
    if ($info == undefined) {
        $info = $("#modalInfo");
    }
    if ($info.data("onclose")) {
        eval($info.data("onclose"));
    }
});


function reopenModal() {
    //este tambien
    $('#baseDynamicModalObject').modal('show');
}


$(document).on('click', '.SmallBox', function (e) {
    var $this = $(this);
    $this.remove();
});

$(document).on('submit', '.ajaxForm', function (e) {
    var formObj = $(this);
    var $submit = $buttonClicked;
    var callBack = $submit.data("begin") || "";
    if (callBack != "") {
        eval(callBack);
        return false;
    }
    var JsonBack = formObj.data("jsonback") || "";
    var target = formObj.data("target") || "";
    var redirect = formObj.data("redirect") || "";
    var formURL = formObj.attr("action");
    var formData = new FormData(this);
    var type = formObj.data("type") || "POST";
    var contentType = formObj.data("contenttype") || false;
    var successMessage = formObj.data("successmessage") || "";
    var errorMessage = formObj.data("errormessage") || "";
    $submit.data('loading-text', "<i class='fa fa-refresh fa-spin'></i>");
    $submit.button('loading');
    if (target != "") {
        $(target).html();
        $(target).append('<h1 class="licheck"><i class="fa fa-cog fa-spin"></i> ' + "" + '...</h1>');
    }

    $.ajax({
        url: formURL,
        type: type,
        data: formData,
        mimeType: "multipart/form-data",
        contentType: contentType,
        cache: false,
        processData: false,
        success: function (data, textStatus, jqXHR) {
            if (JsonBack != "") {
                $submit.button('reset');
                eval(JsonBack + "(data);");
            }
            if (target != "") {
                $submit.button('reset');
                $(target).html(data);
                var callBack = $submit.data("success") || "";
                if (callBack != "") {
                    eval(callBack);
                }
                return;
            }
            data = eval("(" + data + ")");
            if (data) {

                if (!formObj.hasClass("NocurrentID")) {
                    eval(formObj.attr("id").replace("-", "_") + "_currentID='" + data.id + "';");
                }
            }
            $submit.button('reset');

            BaseLastID = data.id;


            wasMessage = true;
            var close = true;

            if (data.severScript) {
                eval(data.severScript);
            }

            if (data.Message) {
                wasMessage = false;
                $.smallBox({
                    title: data.Message,
                    color: "#b94a48",
                    iconSmall: "glyphicon glyphicon-remove  bounce animated",
                    timeout: 5000
                });
                close = false;
            }
            else if (data.MessageSucess) {
                wasMessage = true;
                $.smallBox({
                    title: data.MessageSucess,
                    color: "#5F895F",
                    iconSmall: "fa fa-check bounce animated",
                    timeout: 5000
                });

            } else {
                if (successMessage) {
                    $.smallBox({
                        title: successMessage,
                        color: "#5F895F",
                        iconSmall: "fa fa-check bounce animated",
                        timeout: 5000
                    });
                }
            }
            if (close) {
                var callBack = $submit.data("success") || "";
                if (callBack != "") {
                    eval(callBack);
                }
            }

            if (redirect != "") {
                location.href = redirect;
            }
        },
        error: function (data) {
            $submit.button('reset');
            if (target != "") {
                $(target).html("");
                var callBack = $submit.data("success") || "";
                if (callBack != "") {
                    eval(callBack);
                }
                return;
            }
            if (data.Message) {
                $.smallBox({
                    title: data.Message,
                    color: "#b94a48",
                    iconSmall: "fa fa-check bounce animated",
                    timeout: 5000
                });
            } else {
                if (errorMessage) {
                    $.smallBox({
                        title: errorMessage,
                        color: "#b94a48",
                        iconSmall: "fa fa-check bounce animated",
                        timeout: 5000
                    });
                }
            }
            var callBack = $submit.data("error") || "";
            if (callBack != "") {
                eval(callBack);
            }
        }
    });
    e.preventDefault();
});

$(document).on('click', '.noclick', function (e) {
    return false;
});

$(document).on('dblclick', '.editlinks', function (e) {
    $me = $(this);
    var ifButton = $me.parent().parent().find("td a.btn-circle:eq(0)");
    if (ifButton.length > 0) {
        location.href = $me.parent().parent().find("td a.btn-circle:eq(0)").attr('href');
    } else {
        $me.parent().parent().find("td button.btn-circle:eq(0)").trigger("click");
    }

    return false;
});

function ajaxAlert($elem, properties, beginFN, endFN) {
    if (properties == undefined) {
        $properties = {
            message: "Are you sure?",
            errorMessage: "Can execute this action",
            successMessage: "Action execute successfully",
            title: "Remote Action",
            action: "#",
            buttons: "[No][Yes]",
            color: "txt-color-orange",
            icon: "<i class='glyphicon glyphicon-warning-sign " + "txt-color-orange" + "'></i> ",
            btnActions: "Yes=>;&No=>"
        };
    } else {
        $properties = properties;
    }
    var $message = $properties.message;
    var $errorMessage = $properties.errormessage;
    var $successMessage = $properties.successmessage;
    var $title = $properties.title;
    var $action = $properties.action;
    var $buttons = $properties.buttons;
    var $color = $properties.color;
    var $icon = $properties.icon;
    var $btnActions = $properties.btnActions;

    $.SmartMessageBox({
        title: $icon + $title,
        content: $message,
        buttons: $buttons
    }, function (ButtonPressed) {
        var actions = $btnActions.split('&');
        for (var action in actions) {
            var btnPresed = actions[action].split('=>')[0];
            var code = actions[action].split('=>')[1];
            if (ButtonPressed == btnPresed) {
                var realPresed = btnPresed;
                var realcode = code;
                if (realcode.indexOf('%') === -1) {
                    if (realcode != '')
                        eval(realcode);
                } else {
                    if (beginFN != undefined)
                        beginFN();
                    $.ajax({
                        url: $action,
                        type: 'GET',
                        dataType: 'json',
                        success: function (data, textStatus, jqXHR) {


                            if (endFN != undefined)
                                endFN();
                            if (realcode.replace('%', '') != '')
                                eval(realcode.replace('%', ''));

                            var close = true;
                            if (data.Message) {
                                $.smallBox({
                                    title: data.Message,
                                    color: "#b94a48",
                                    iconSmall: "glyphicon glyphicon-remove  bounce animated",
                                    timeout: 5000
                                });
                                close = false;
                            }
                            else if (data.MessageSucess) {
                                $.smallBox({
                                    title: data.MessageSucess,
                                    color: "#5F895F",
                                    iconSmall: "fa fa-check bounce animated",
                                    timeout: 5000
                                });

                            } else {
                                if (successMessage) {
                                    $.smallBox({
                                        title: successMessage,
                                        color: "#5F895F",
                                        iconSmall: "fa fa-check bounce animated",
                                        timeout: 5000
                                    });
                                }
                            }
                        },
                        error: function (data) {
                            if (endFN != undefined)
                                endFN();
                            if (data.message) {
                                $.smallBox({
                                    title: data.message,
                                    color: "#b94a48",
                                    iconSmall: "fa fa-check bounce animated",
                                    timeout: 5000
                                });
                            }

                            if ($errorMessage) {
                                $.smallBox({
                                    title: $errorMessage,
                                    color: "#b94a48",
                                    iconSmall: "fa fa-check bounce animated",
                                    timeout: 5000
                                });
                            }
                        }
                    });
                }
            }
        }
    });
    return false;
}

$(document).on('click', '.delete', function (e) {
    $elem = $(this);
    var $submit = $elem;
    var $message = $elem.data("message");
    var $errorMessage = $elem.data("errormessage");
    var $successMessage = $elem.data("successmessage");
    var $title = $elem.data("title");
    var $action = $elem.data("action");
    var $buttons = $elem.data("buttons");
    var $color = $elem.data("color") || "txt-color-orange";
    var $icon = $elem.data("icon") || "<i class='glyphicon glyphicon-warning-sign " + $color + "'></i> ";
    var $btnActions = $elem.data("btnactions");
    $submit.data('loading-text', "<i class='fa fa-refresh fa-spin'></i>");
    $submit.button('loading');
    $.SmartMessageBox({
        title: $icon + $title,
        content: $message,
        buttons: $buttons
    }, function (ButtonPressed) {
        var actions = $btnActions.split('&');
        for (var action in actions) {
            var btnPresed = actions[action].split('=>')[0];
            var code = actions[action].split('=>')[1];
            if (ButtonPressed == btnPresed) {
                var realPresed = btnPresed;
                var realcode = code;
                if (realcode.indexOf('%') === -1) {
                    if (realcode != '')
                        eval(realcode);
                    $submit.button('reset');
                } else {
                    $.ajax({
                        url: $action,
                        type: 'GET',
                        dataType: 'json',
                        success: function (data, textStatus, jqXHR) {
                            $submit.button('reset');
                            if (realcode.replace('%', '') != '')
                                eval(realcode.replace('%', ''));


                            var close = true;
                            if (data.Message) {
                                $.smallBox({
                                    title: data.Message,
                                    color: "#b94a48",
                                    iconSmall: "glyphicon glyphicon-remove  bounce animated",
                                    timeout: 5000
                                });
                                close = false;
                            }
                            else if (data.MessageSucess) {
                                $.smallBox({
                                    title: data.MessageSucess,
                                    color: "#5F895F",
                                    iconSmall: "fa fa-check bounce animated",
                                    timeout: 5000
                                });

                            } else {
                                if (successMessage != undefined) {
                                    if (successMessage) {
                                        $.smallBox({
                                            title: successMessage,
                                            color: "#5F895F",
                                            iconSmall: "fa fa-check bounce animated",
                                            timeout: 5000
                                        });
                                    }
                                }
                            }
                        },
                        error: function (data) {
                            $submit.button('reset');
                            if (data.message) {
                                $.smallBox({
                                    title: data.message,
                                    color: "#b94a48",
                                    iconSmall: "fa fa-check bounce animated",
                                    timeout: 5000
                                });
                            }

                            if ($errorMessage) {
                                $.smallBox({
                                    title: $errorMessage,
                                    color: "#b94a48",
                                    iconSmall: "fa fa-check bounce animated",
                                    timeout: 5000
                                });
                            }
                        }
                    });
                }
            }
        }
    });
    return false;
});

setInterval(function () {
    $('.ajaxBadge').each(function () {
        $this = $(this);
        var $badaction = $this.data("action");
        $query = $this.data("query");
        $.ajax({
            url: $badaction,
            type: 'POST',
            dataType: 'json',
            data: {
                query: $query
            },
            success: function (data, textStatus, jqXHR) {
                if (data == "0") {
                    $this.css("display", "none");
                }
                $this.html(data);
            },
            error: function (data) {
            }
        });
        return false;
    });
}, 3000);

if ($.fn.modal != undefined)
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };


if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;

        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined' ? args[number] : match;
        });
    };
}

(function ($) {
    $.buildMorris = function (el, options) {
        var base = this;
        base.$el = $(el);
        base.el = el;
        base.$el.data("buildMorris", base);
        base.init = function () {
            base.options = $.extend({}, $.buildMorris.defaultOptions, options);
            base.options.json.element = base.$el.attr('id');
            base.doMorris();
        };
        base.doMorris = function () {
            switch (base.options.type) {
                case 'line': {
                    Morris.Line(base.options.json);
                    break;
                }
                case 'area': {
                    Morris.Area(base.options.json);
                    break;
                }
                case 'bar': {
                    Morris.Bar(base.options.json);
                    break;
                }
                case 'donut': {
                    base.options.json.formatter = function (x) { return numberFormat(x, 2); };
                    Morris.Donut(base.options.json);
                    break;
                }
            }
        };
        base.init();
    };
    $.buildMorris.defaultOptions = {
        type: 'line',
        json: {}
    };

    $.fn.buildMorris = function (options) {
        return this.each(function () {
            (new $.buildMorris(this, options));
        });
    };

    $.fn.buildMorris.loadMorris = function (callback) {
        loadScript('js/plugin/morris/raphael-min.js', function () {
            loadScript('js/plugin/morris/morris.js', callback);
        });
    };
})(jQuery);

function numberFormat(nStr, decimal) {
    if (typeof nStr == "undefined") {
        return nStr;
    }
    nStr = nStr.toFixed(decimal);
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}


(function ($) {
    $.buildInlineChart = function (el, options) {
        var base = this;
        base.$el = $(el);
        base.el = el;
        base.$el.data("buildInlineChart", base);
        base.init = function () {
            base.options = $.extend({}, $.buildInlineChart.defaultOptions, options);
            base.options.json.element = base.$el.attr('class');
            base.doInlineChart();
        };

        base.doInlineChart = function () {
            debugger
            switch (base.options.type) {
                case 'line': {
                    InlineChart.line(base.options.json);
                    break;
                }
                case 'bar': {
                    InlineChart.bar(base.options.json);
                    break;
                }
                case 'pie': {
                    InlineChart.pie(base.options.json);
                    break;
                }
            }

        };

        base.init();
    };
    $.buildInlineChart.defaultOptions = {
        type: 'bar',
        json: {}
    };

    $.fn.buildInlineChart = function (options) {
        return this.each(function () {
            (new $.buildInlineChart(this, options));
        });
    };

    $.fn.buildInlineChart.loadInlineChart = function (callback) {

    };

})(jQuery);

(function ($) {
    $.buildFlot = function (el, options) {
        var base = this;
        base.$el = $(el);
        base.el = el;
        base.$el.data("buildFlot", base);
        base.init = function () {
            base.options = $.extend({}, $.buildFlot.defaultOptions, options);
            base.options.json.element = base.$el.attr('id');
        };

        base.init();
    };
    $.buildFlot.defaultOptions = {
        type: 'bar',
        json: {}
    };

    $.fn.buildFlot = function (options) {
        return this.each(function () {
            (new $.buildFlot(this, options));
        });
    };

    $.fn.buildFlot.loadFlot = function (callback) {
        loadScript("js/plugin/flot/jquery.flot.cust.js", loadFlotResize);

        function loadFlotResize() {
            loadScript("js/plugin/flot/jquery.flot.resize.js", loadFlotFillbetween);
        }
        function loadFlotFillbetween() {
            loadScript("js/plugin/flot/jquery.flot.fillbetween.js", loadFlotOrderBar);
        }
        function loadFlotOrderBar() {
            loadScript("js/plugin/flot/jquery.flot.orderBar.js", loadFlotPie);
        }
        function loadFlotPie() {
            loadScript("js/plugin/flot/jquery.flot.pie.js", loadFlotToolTip);
        }
        function loadFlotToolTip() {
            loadScript("js/plugin/flot/jquery.flot.tooltip.js", callback);
        }
    };

})(jQuery);

if ($.validator != undefined) {
    $.validator.addMethod('precision', function (value, element, param) {
        var precission = param;
        return value.split('.')[0].length <= precission;
    }, '');

    $.validator.addMethod('onlyLetters', function (value, element, param) {
        if (!value.match(/^[a-zA-Z_áéíóúñ\s]*$/)) {
            return false;
        }
        else {
            return true;
        }
    }, 'Este campo solo acepta letras.');

    $.validator.addMethod('strongPassword', function (value, element, param) {
        if (param) {
            if (value.length < 8) {
                return false;
            }
            if (!value.match(/[A-z]/)) {
                return false;
            }
            if (!value.match(/\d/)) {
                return false;
            }
            return true;
        }
        return false;
    }, 'The password must contain 8 character with letters and numbers');

    $.validator.addMethod('maxLetter', function (value, element, param) {

        if (param) {
            if (value.length > param) {
                return false;
            }
            if (!value.match(/^[a-zA-Z_áéíóúñ\s]*$/)) {
                return false;
            }

            return true;
        }
        return false;
    }, 'Este campo acepta como máximo {0} letras.');

    $.validator.addMethod('minLetter', function (value, element, param) {
        if (param) {
            if (value.length < param) {
                return false;
            }
            if (!value.match(/^[a-zA-Z_áéíóúñ\s]*$/)) {
                return false;
            }
            return true;
        }
        return false;
    }, 'Este campo acepta como mínimo {0} letras.');

    $.validator.addMethod('maxNum', function (value, element, param) {
        if (param) {
            if (value.length > param) {
                return false;
            }
            if (!value.match(/^\+?(0|[1-9]\d*)$/)) {
                return false;
            }

            return true;
        }
        return false;
    }, 'Este campo acepta como máximo {0} números enteros.');

    $.validator.addMethod('minNum', function (value, element, param) {
        if (param) {
            if (value.length < param) {
                return false;
            }

            if (!value.match(/^\+?(0|[1-9]\d*)$/)) {
                return false;
            }

            return true;
        }
        return false;
    }, 'Este campo acepta como mínimo {0} números enteros.');

    $.validator.addMethod('scale', function (value, element, param) {
        var scale = param;
        if (value.split('.').length > 1) {
            return value.split('.')[1].length <= scale;
        } else {
            return true;
        }
    }, '');
}

$(document).on("change", "#projectStatu", function () {
    var $this = $(this);
    var idurl = location.href.split('/');
    idurl = idurl[idurl.length - 1];
    $properties = {
        message: "Are you sure you want to change the status of this project?",
        errorMessage: "Could not change status",
        successMessage: "Status changed successfully",
        title: "Change Status",
        action: "CPProject/ChangeStatus?id=" + idurl + "&status=" + $this.val(),
        buttons: "[No][Yes]",
        color: "txt-color-orange",
        icon: "<i class='glyphicon glyphicon-warning-sign " + "txt-color-orange" + "'></i> ",
        btnActions: "Yes=>%location.reload();&No=>"
    };
    ajaxAlert($this, $properties,
    function () {
        $this.parent().parent().find("label").html("<b style='color:red'>Changing Status...<b>");
    }, function () {

    });
});

$(document).on('click', '.ModalComment', function (e) {
    $me = $(this);
    var action = $me.data("action");
    if (action) {
        $me.data('loading-text', "<i class='fa fa-refresh fa-spin'></i>");
        $me.button('loading');

        $('#ModalCommentBody').load(action, function () {
            options = {
                backdrop: true,
                keyboard: true
            };
            $('#myModalComment').modal(options);
            $me.button('reset');
        });
    }
    return false;
});

function AddChenks(table) {
    $('#' + table + ' tr:first ').append("<td><input type='checkbox' id='checkallmultiple'/></td>");
    $('#' + table + ' tr:not(:first)').each(function () {
        $(this).append("<td> <input type='checkbox' name='checkmultiple' value='1' /> </td>");

    });
}

$(document).on("click", "#configAdmColumns", function () {
    var $this = $(this);
    var currentCOntrol = $this;
    var intro = false;
    while (!intro) {
        var currentCOntrol = currentCOntrol.parent();
        var found = currentCOntrol.find(".admCols:eq(0)");
        if (found.length > 0) {
            found.click();
            intro = true;
        }
    }
});

var es_firefox = navigator.userAgent.toLowerCase().indexOf('firefox') > -1;
if (es_firefox) {
    // alert("El navegador que se está utilizando es Firefox");
    $(document).ready(function () {
        $(document).on("keypress", ".blockletter", function (e) {
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        });
    });
}

var es_chrome = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;
if (es_chrome) {
    //alert("El navegador que se está utilizando es Chrome");
    $(document).on("keypress", ".blockletter", function (key) {
        try {
            //if ($(this).val() == "")
            //    $(this).val(1);
            if (parseInt($(this).val()) <= 0) {
                $(this).val(1);
                return false;
            }
            if (key.charCode < 48 || key.charCode > 57) {
                if (key.charCode == 8 || key.charCode == 46) {
                    return true;
                }
                return false;
            };
        } catch (err) {
            $(this).val(1);
        }
    });
}

function clearAllINterval() {
    var interval_id = window.setInterval("", 9999);
    for (var i = 1; i < interval_id; i++)
        window.clearInterval(i);
}

isModal = false;
function HideRelation(formID, from) {
    if (from != "") {
        $("#" + formID).find("select").each(function (index, ele) {
            var $meselect = $(this);
            if ($meselect.attr("id") != undefined) {
                var arryNin = from.split(':');
                if (arryNin.length > 1) {
                    var tableselect = "_" + arryNin[0].split('_')[1];
                    if ($meselect.attr("id").indexOf(tableselect) !== -1) {
                        $("#" + $meselect.attr("id")).val(arryNin[1]).trigger("change");
                        $meselect.select2('val', arryNin[1]).trigger("change");
                        var data = $meselect.select2('data');
                        var parent = $meselect.parent();
                        var topParent = parent.parent();
                        topParent.append('<label class="input"> <i class="icon-append fa fa-check-square"></i><input readonly="readonly" type="text" value="{0}"></label>'.format(data.text));
                        var title = topParent.parent().parent().parent().parent().parent().find('.panel-heading').find('.panel-title');
                        title.append(" of {0}'s {1}".format(arryNin[0].split('_')[1], data.text));
                        parent.hide();
                    }
                }
            }
        });
    }
}

//Sound
function Play(name) {
    var audioElement = document.createElement("audio");
    if (navigator.userAgent.match("Firefox/")) {
        audioElement.setAttribute("src", "sound/" + name + ".ogg")

    } else {
        audioElement.setAttribute("src", "sound/" + name + ".mp3")
    }
    $.get();
    audioElement.addEventListener("load", function () {
        audioElement.play()
    }, true);
    audioElement.pause();
    audioElement.play()
}

//Prices
function addInternal(control, d, formula, reset) {
    var $control = $("input[name='{0}']".format(control));
    if (reset == true && reset != undefined)
        $control.val("$" + eval(formula).toFixed(2));
    else {
        var current = parseFloat($control.val());
        $control.val("$" + (eval(formula) + current).toFixed(2));
    }
}

function val(control) {
    try {
        var $control = $("input[name='{0}']".format(control));
        return parseFloat($control.val().replace("$", ""));
    } catch (err) {
        console.log(control);
    }
}

function addPrice(obj) {
    var countControl = obj.countControl;
    countControl = countControl == undefined ? false : countControl;
    var control = obj.control;
    var name = (countControl ? "input" : "") + obj.name;
    var reset = obj.reset;
    var separator = obj.separator;

    separator = separator == undefined ? '=>' : separator;
    reset = reset == undefined ? false : reset;
    var $control = $('#' + control);
    var value = reset ? 0 : parseFloat($control.val().replace('$', ''));
    var valueNew = 0;
    var counts = 1;
    if (countControl) {
        var $counts = $("input[name='count{0}']".format(obj.name));
        counts = $counts.val() != "" ? parseInt($counts.val()) : 0;
        if (counts <= 0) {
            return;
        }
    }
    var finalSearch = $("select[name='{0}']".format(name));
    var ammounts = null;
    if (finalSearch.length == 0)
        ammounts = { text: "=>{0}".format($("input[name='{0}']".format(name)).val()) };
    else
        ammounts = $("select[name='{0}']".format(name)).select2('data');

    if (ammounts != "" && ammounts != null) {
        if (ammounts != null)
            if (ammounts.constructor !== Array) {
                var oldammounts = ammounts;
                ammounts = new Array();
                ammounts.push(oldammounts);
            }
        for (var i in ammounts) {
            var item = ammounts[i];
            var has = item.text.constructor != "function Function() { [native code] }";
            if (has) {
                var itemsplit = item.text.split(separator);
                if (itemsplit != undefined) {
                    var itemsplit = cleanArray(itemsplit);
                    if (itemsplit.length > 1) {
                        var finalItemSplit = itemsplit[1].replace("$", "").trim().split('/')[0];
                        valueNew += parseFloat(finalItemSplit);
                    }
                }
            }
        }
    }
    var finalCalc = (counts * valueNew);
    finalCalc += value;
    $control.val('$' + (finalCalc).toFixed(2));
}

function cleanArray(actual) {
    var newArray = new Array();
    for (var i = 0; i < actual.length; i++) {
        if (actual[i]) {
            newArray.push(actual[i]);
        }
    }
    return newArray;
}