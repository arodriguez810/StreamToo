$.validator.setDefaults({
	ignore: '.ignore',
	onkeyup: false,
	//onfocusout: false,
	onsubmit: true
});

String.prototype.Format = function () {
	var args = arguments;
	return this.replace(/\{\{|\}\}|\{(\d+)\}/g, function (m, n) {
		if (m == "{{") { return "{"; }
		if (m == "}}") { return "}"; }
		return args[n];
	});
}

var config = {
	'.chosen-select': {},
	'.chosen-select-deselect': { allow_single_deselect: true },
	'.chosen-select-no-single': { disable_search_threshold: 10 },
	'.chosen-select-no-results': { no_results_text: 'No encontrado' },
	'.chosen-select-width': { width: "95%" }
}
for (var selector in config) {
	$(selector).chosen(config[selector]);
}

//Datepicker initializations
$('.date').datepicker({
	dateFormat: 'yy/mm/dd',
	changeMonth: true,
	changeYear: true
});

$('.time').timepicker({
	timeFormat: 'H:i'
});

$('.futurePicker').datepicker({
	dateFormat: 'yy/mm/dd',
	changeMonth: true,
	changeYear: true,
	minDate: 1
});

//Autonumeric Spinner for version select
$("input[name*='versionSelect']").TouchSpin({
	min: 1,
	max: 1000000
});

$('.tags').tagsInput({ width: 'auto' });

$(".chosen").chosen(/*{ maxSelectedOptions: 2 }*/);
$(".chosen-deselect").chosen({ allowSingleDeselect: true });
$(".chosen").chosen().change();
$(".chosen").trigger('chosen:updated');

$(".confirmateComposite").focusout(function () {
	$(".validateComposite").removeData("previousValue");
});

$(document).ajaxSend(function (event, request, settings) {
	if (settings.url.toLowerCase().indexOf('/home/check') == -1) {
		$("#dvLoading").show();
	}
});

$('.excesiveRequest').dialog({
	autoOpen: false,
	width: 'auto',
	title: 'Error en su petición',
	maxWidth: 600,
	height: 'auto',
	modal: true,
	fluid: true, //new option
	resizable: false,
	buttons: {
		"OK": function () {
			$(this).dialog("close");
		}
	}
});

$('.unAuthorize').dialog({
	title: 'Alerta',
	autoOpen: false,
	width: 'auto',
	maxWidth: 600,
	height: 'auto',
	modal: true,
	fluid: true, //new option
	resizable: false,
	buttons: {
		"OK": function () {
			$(this).dialog("close");
		}
	}
});


$(document).ajaxComplete(function () {
	$("#dvLoading").hide();
	$('.eis_table_metro tr > td > a.button_link').addClass('hideOption');
	$('.eis_table_metro tr').not(':first').on('mouseenter',
		function () {
			$(this).find('a.button_link').removeClass('hideOption');
		}).on('mouseleave',
		function () {
			$(this).find('a.button_link').addClass('hideOption');
		});
});

$(document).ajaxStop(function () {
	$("#dvLoading").hide();
});

function updateDropdown(data, url, dropdownElement, chosenElement) {
	var html = '<option value=""></option>';
	var select = "";
	$.ajax({
		url: url,
		type: 'POST',
		data: JSON.stringify(data),
		dataType: 'json',
		contentType: 'application/json',
		success: function (data) {
			if (data == undefined || data == "") {
			}
			else {
				$.each(data, function (key, row) {
					//fill the dropdown
					select = (row.Selected) ? ' selected = "true" ' : "";
					html +=
						'<option value="' + row.Value + '"' + select + '>'
						+ row.Text +
						'</option>';
				});
				dropdownElement.html(html);
				if (chosenElement != null)
					chosenElement.trigger('chosen:updated');
			}
		}
	});
}

function updateDropdownSeleted(data, url, dropdownElement, chosenElement, selected) {
	var html = '<option value=""></option>';
	var select = "";
	$.ajax({
		url: url,
		type: 'POST',
		data: JSON.stringify(data),
		dataType: 'json',
		contentType: 'application/json',
		success: function (data) {
			if (data == undefined || data == "") {
			}
			else {
				$.each(data, function (key, row) {
					//fill the dropdown
					select = (row.Selected) ? ' selected = "true" ' : "";
					html +=
						'<option value="' + row.Value + '"' + select + '>'
						+ row.Text +
						'</option>';
				});
				dropdownElement.html(html);

				if (chosenElement != null) {
					var id = dropdownElement.attr("id");
					$('#' + id + ' option').each(function () {
						if (selected.indexOf("(" + $(this).val() + ")") > -1) {
							$(this).attr("selected", true);
						} else {
							$(this).attr("selected", false);
						}
					});
					chosenElement.trigger('chosen:updated');
				}
			}
		}
	});
}


function organizeMenu() {

	var menu = $('.close_menu');
	var status = getCookie('menuStatus');
	if (status == null)
		return false;

	if (status == 'close')
		closeMenuFunction(menu);
}

function closeMenuFunction(menus) {
	jQuery(menus).addClass("menu_closed");
	jQuery(this).data('status', 'close');
	setCookie('menuStatus', 'close', 7);
}

function openMenuFunction(menus) {
	jQuery(menus).removeClass("menu_closed");
	jQuery(this).data('status', 'open');
	setCookie('menuStatus', 'open', 7);
}

/** Cookies functions **/
function setCookie(name, value, days) {
	if (days) {
		var date = new Date();
		date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
		var expires = "; expires=" + date.toGMTString();
	}
	else var expires = "";
	document.cookie = name + "=" + value + expires + "; path=/";
}

function getCookie(name) {
	var nameEQ = name + "=";
	var ca = document.cookie.split(';');
	for (var i = 0; i < ca.length; i++) {
		var c = ca[i];
		while (c.charAt(0) == ' ') c = c.substring(1, c.length);
		if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
	}
	return null;
}

$(document).ready(function () {

    
    //Evita que se introduzca letras
    $(".ELND").keydown(function (event) {
        if (event.shiftKey) {
            event.preventDefault();
        }

        if (event.keyCode == 46 || event.keyCode == 8) {
        }
        else {
            if (event.keyCode < 95) {
                if (event.keyCode < 48 || event.keyCode > 57) {
                    event.preventDefault();
                }
            }
            else {
                if (event.keyCode < 96 || event.keyCode > 105) {
                    event.preventDefault();
                }
            }
        }
    });

    //Evita que peguen un texto
    $(".ELND").bind('paste', function (e) {
        $("#ELnd_1").val($("#ELnd_1").val().replace($("#ELnd_1").val(), ""));
    });

    $("#sortable").sortable();

    $(".updateMyTable").change(function () {
        myTable.fnDraw();
    });

	$(".first.paginate_button, .last.paginate_button").hide();
	organizeMenu();

	$('.datetimepicker').datetimepicker({
		language: 'en',
		maskInput: true
	});

	$('.datetimepicker').keypress(function () {
		return false;
	});

	jQuery("li#colapsar_menu > a.close_menu").click(function () {

		var menus = $('.close_menu');
		var status = jQuery(this).data('status');

		if (getCookie('menuStatus') != null)
			status = getCookie('menuStatus');

		if (status == 'open')
			closeMenuFunction(menus);
		else
			openMenuFunction(menus);

	});


	$(document).tooltip({
		position: {
			my: "center bottom-20",
			at: "center top",
			using: function (position, feedback) {
				$(this).css(position);
				$("<div>")
				  .addClass("arrow")
				  .addClass(feedback.vertical)
				  .addClass(feedback.horizontal)
				  .appendTo(this);
			}
		}
	});
	$('.eis_table_metro tr > td > a.button_link').addClass('hideOption');
	$('.eis_table_metro tr').not(':first').on('mouseenter',
		function () {
			$(this).find('a.button_link').removeClass('hideOption');
		}).on('mouseleave',
		function () {
			$(this).find('a.button_link').addClass('hideOption');
		});
	//Datepicker initializations
	$('.defaultPicker').datepicker({
		dateFormat: 'yy/mm/dd',
		changeMonth: true,
		changeYear: true
	});

	$('.name').attr('maxLength', '45');

	$('.fieldInput').css("position", "absolute").css("left", "-9999px");

	$('.eraseInput').on("click", function (event) {
		event.preventDefault();
		resetInput($(".fieldInput"));
		$(".falseInputFile").val("");
		$(this).hide();
	});

	$('.falseInputFile').val($('.hiddenName').val());
	if ($('.falseInputFile').val() == '') {
		$('.eraseInput').hide();
	}

	$('.btSubmit').on("click", function (event) {
		if ($('.hiddenName').val() &&
			$('.hiddenName').val() != $('.falseInputFile').val()) {
			$('.fileChange').dialog("open");
			event.preventDefault();
		}
		else {
			$('.standard_form').submit();
		}
	});
	$('.fieldInput').on("change", function () {
		if ($(this).val() != '' || $('.falseInputFile').val() != '') {
			$('.eraseInput').show();
		}
	});

	if ($('#customError').val()) {
		$('.excesiveRequest').dialog("open");
	}

	$('form input:text').each(function () {
		var me = $(this);
		if (/^\d+(?:\,\d{1,2})?$/.test(me.val())) {
			me.val(me.val().replace(',', '.'));
		}
	});


});

window.resetInput = function (e) {
	e.wrap('<form>').closest('form').get(0).reset();
	e.unwrap();
}

$(document).on('click', '.filetrigger', function () {
	inputfile = $(this).prevAll('input:file');

	$(inputfile).trigger('click');

	$(inputfile).change(function () {
		$('#filename').val($(this).val());
	});
});

$('.fileChange').dialog({
	autoOpen: false,
	width: 540,
	height: 200,
	resizable: false,
	modal: true,
	buttons: {
		"Guardar": function () {
			$(this).dialog("close");
			$('.standard_form').submit();
		},
		"Cancel": function () {
			$(this).dialog("close");
		}
	}
});

$('.nestedDropDown').change(function () {
	alert($("#" + this.Attr("id") + " :selected").val());
	alert($(this).data.childDropdownID);
});

var oLanguage = {
	"sLengthMenu": "Mostrar _MENU_ registros por páginas",
	"sZeroRecords": "No hay registros entontrados.",
	"sInfo": "Mostrando _START_ a _END_ de _TOTAL_ registros",
	"sInfoEmpty": "Monstrando 0 a 0 de 0 records",
	"sInfoFiltered": "(filtrado desde _MAX_ total registros)",
	"sSearch": " ",
	"oPaginate": {
		"sNext": "Siguiente",
		"sPrevious": "Anterior",
		"sFirst": "Primero",
		"sLast": "Último"
	}
};



jQuery.fn.dataTableExt.oApi.fnSetFilteringDelay = function (oSettings, iDelay) {
	var
_that = this,
iDelay = (typeof iDelay == 'undefined') ? 250 : iDelay;

	this.each(function (i) {
		$.fn.dataTableExt.iApiIndex = i;
		var
	$this = this,
	oTimerId = null,
	sPreviousSearch = null,
	anControl = $('input', _that.fnSettings().aanFeatures.f);

		anControl.unbind('keyup').bind('keyup', function () {
			var $$this = $this;

			if (sPreviousSearch === null || sPreviousSearch != anControl.val()) {
				window.clearTimeout(oTimerId);
				sPreviousSearch = anControl.val();
				oTimerId = window.setTimeout(function () {
					$.fn.dataTableExt.iApiIndex = i;
					_that.fnFilter(anControl.val());
				}, iDelay);
			}
		});

		return this;
	});
	return this;
}

function RefreshTable(tableId, urlData, dataValEdit, dataUrlEdit, dataValDelete, dataUrlDelete, params) {
	// Retrieve the new data with $.getJSON. You could use it ajax too
	return $.getJSON(urlData, null, function (json) {
		table = $(tableId).dataTable();
		oSettings = table.fnSettings();
		table.fnClearTable(this);
		for (var i = 0; i < json.aaData.length; i++) {
			var links = '';
			links +=
				(!dataValEdit) ? '' : '<a href="javascript:void(0);" class="edit_link button_link editAction" title="Edit" data-val="' + dataValEdit + json.aaData[i][params] + '" data-url="' + dataUrlEdit + '" >Edit</a>';
			links +=
				(!dataValDelete) ? '' : '<a href="javascript:void(0);" class="delete_link button_link deleteAction" title="Delete" data-val="' + dataValDelete + json.aaData[i][params] + '" data-url="' + dataUrlDelete + '" >Delete</a>';
			json.aaData[i][params] = links;
			table.oApi._fnAddData(oSettings, json.aaData[i]);
		}

		oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
		table.fnDraw();
		$('.dataTables_wrapper').addClass('mn');
	});
}

//function RefreshTable(tableId, urlData, data, dataValEdit, dataUrlEdit, dataValDelete, dataUrlDelete, params) {
//    // Retrieve the new data with $.getJSON. You could use it ajax too
//    return $.getJSON(urlData, data, function (json) {
//        table = $(tableId).dataTable();
//        oSettings = table.fnSettings();
//        //alert(json.aaData.length);
//        table.fnClearTable(this);
//        for (var i = 0; i < json.aaData.length; i++) {
//            var links = '';
//            links +=
//				(!dataValEdit) ? '' : '<a href="javascript:void(0);" class="edit_link button_link editAction" title="Edit" data-val="' + dataValEdit + json.aaData[i][params] + '" data-url="' + dataUrlEdit + '" >Edit</a>';
//            links +=
//				(!dataValDelete) ? '' : '<a href="javascript:void(0);" class="delete_link button_link deleteAction" title="Delete" data-val="' + dataValDelete + json.aaData[i][params] + '" data-url="' + dataUrlDelete + '" >Delete</a>';

//            json.aaData[i][params] = links;
//            alert(json.aaData[i][params]);
//            table.oApi._fnAddData(oSettings, json.aaData[i][params]);
//        }

//        oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
//        table.fnDraw();
//        $('.dataTables_wrapper').addClass('mn');
//    });
//}

function printObject(o) {
	var out = '';
	for (var p in o) {
		var display = o[p];
		out += p + ' => ' + display + '\n';
	}
	alert(out);
}

function printObjectDump(o) {
	var out = '';
	for (var p in o) {
		var display = o[p];
		if (display instanceof Array) {
			display = printObjectReturn(display);
			alert(display);
		}
		out += p + ' => ' + display + '\n';
	}
	alert(out);
}

function printObjectReturn(o) {
	var out = '';
	for (var p in o) {
		var display = o[p];
		if (display instanceof Array) {
			display = printObjectReturn(display);
		}
		out += p + ' => ' + display + '\n';
	}
	return out;
}

function fnShowHide(datatableID, iCol, visible) {
	datatableID.fnSetColumnVis(iCol, visible);
}

/*** Function to parse dynamic loaded content with unobstrusive jQuery validation ***/
function fnValidateDynamicContent(element) {
	var currForm = element.closest("form");
	$('.standard_form').removeData("validator");
	$('.standard_form').removeData("unobtrusiveValidation");
	$.validator.unobtrusive.parse(currForm);
	// This line is important and added for client side validation to trigger, without this it didn't fire client side errors.
	$('.standard_form').validate();
}

$('[readonly]').each(function (e) {
	$(this).attr('onfocus', 'this.blur()');
});

function eisModal() {
	this.selector = "#dialog";
	this.delegate = "";
	this.masterID = "";
	this.eventDelegate = "click";
	this.editDelegate = "";
	this.editEventDelegate = "click";
	this.tableSelector = ".eis_table";
	this.autoOpen = false;
	this.title = '';
	this.width = 540;
	this.height = 400;
	this.resizable = false;
	this.draggable = false;
	this.modal = true;
	this.loaded = false;
	this.data = null;

	this.fieldSet = [];
	this.action = "";

	this.sendPost;
	this.placeModal = function (clearFunc, saveFunc, cancelFunc, extracFunc) {
		this.loaded = true;
		$(this.selector).dialog({
			autoOpen: this.autoOpen,
			title: this.title,
			width: this.width,
			height: this.height,
			resizable: this.resizable,
			draggable: this.draggable,
			modal: this.modal,
			buttons: {
				"Guardar": saveFunc,
				"Cancelar": cancelFunc
			}
		});

		eval('$("' + this.delegate + '").' + this.eventDelegate + '(function () { $("' + this.selector + '").dialog("open"); clearFunc(); });');

		if (this.editDelegate != "") {
			$(this.tableSelector).on(this.editEventDelegate, this.editDelegate, function (e) {
				var formyData = "";
				var actions = $(this).attr("href");
				var thisID = actions.split('/')[actions.split('/').length - 1];
				$(dialog.masterID).val(thisID);
				$.get(actions, {}, function (data) {
					extracFunc(data);
				});
				return false;
			});
		}

		//eval('$("' + this.editDelegate + '").on("'+ this.editEventDelegate +'", "' + this.editDelegate + '",' + 'function () { $("' + this.selector + '").dialog("open"); return false; });');
	};

	this.cloaseDialog = function () {
		if (!this.loaded) {
			alert("U must use placeModal before use cloaseDialog function.");
			return;
		}
		$(this.selector).dialog("close");
	}

	this.openDialog = function () {
		if (!this.loaded) {
			alert("U must use placeModal before use openDialog function.");
			return;
		}
		$(this.selector).dialog("open");
	}

	this.sendPost = function (clearFunc, table) {
		if (this.fieldSet.length == 0) {
			alert("This modal not has fields"); return;
		}

		for (var item in this.fieldSet) {
			$(this.fieldSet[item]).addClass("formData" + this.selector.replace("#", ""));
		}

		$.post(this.action, $(".formData" + this.selector.replace("#", "")).serialize(), function (data) {
			table.fnDraw();
			alert(data);
			if (data.indexOf('existe') < 0) {
				clearFunc();
				dialog.cloaseDialog();
			}

		});
	}

}

function loadTableLink() {
    var options = new Array('table.table a.edit_link');

    for (var selector in options) {
        $(options[selector]).each(function () {
            var me = $(this);
            var url = me.attr('href');
            me.parent('td').addClass('button-column');
            me.parent('td').parent('tr').children('td').not("[class='button-column']").each(function () {
                var old = $(this).html();
                var html = "<a href='" + url + "'>" + old + "</>";
                if (!$(this).children().length) {
                    $(this).html(html);
                }
            });
        });
    }

}


/**
 * Validate email function with regualr expression
 * 
 * If email isn't valid then return false
 * 
 * @param email
 * @return Boolean
 */
function isEmail(email) {
	var emailReg = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
	var valid = emailReg.test(email);

	if (!valid) {
		return false;
	} else {
		return true;
	}
}

function scale(selector, scale) {
    $(selector).css('transform', 'scale(' + scale + ')');
    $(selector).css('-ms-transform', 'scale(' + scale + ')');
    $(selector).css('-moz-transform', 'scale(' + scale + ')');
    $(selector).css('-webkit-transform', 'scale(' + scale + ')');
    $(selector).css('-o-transform', 'scale(' + scale + ')');
}

$(window).resize(function () {
    $(".ui-dialog-content:visible").each(function () {
        $(this).dialog("option", "position", $(this).dialog("option", "position"));
    });
});

$(window).scroll(function () {
    $(".ui-dialog-content:visible").each(function () {
        $(this).dialog("option", "position", $(this).dialog("option", "position"));
    });
});

function trimy(str, trimy) {
    var newStr = "";
    eval('newStr = str.replace(/(^' + trimy + ')|(' + trimy + '$)/g, "");');
    return newStr;
}

$('.showInModal').click(function () {
    $('#dialogContent').load(this.href, function () {
        $('#dialogDiv').modal({
            backdrop: 'static',
            keyboard: true
        }, 'show');
        bindForm(this);
    });
    return false;
});

function loadURLPOST(url, container) {
    $(container).load(url, function () {
        bindForm(this);
    });
}

function bindForm2(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    eval($(this).data('after'));
                } else {
                    $('#dialogContent').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#dialogDiv').modal('hide');
                    // Refresh:
                    // location.reload();
                } else {
                    $('#dialogContent').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}