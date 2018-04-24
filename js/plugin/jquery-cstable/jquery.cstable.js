(function ($) {

    $.CStable = function (el, options) {
        var base = this;
        base.$el = $(el);
        base.el = el;
        base.$el.data("CStable", base);
        var iUlModal = "";
        base.init = function () {
            base.options = $.extend({}, $.CStable.defaultOptions, options);
            base.options.headerSort = $.cookie(base.getCookieID());
            var $table = $('<table/>', {
                class: base.options.tableClass
            });
            var $tc = $('<div/>', {
                class: base.options.tableContainerClass
            });
            var $tableControl = $('<div/>', {
                class: base.options.tableControlClass
            });
            $.each($(base.options.refreshButton), function (index, element) {
                $me = $(this);
                $me.data('loading-text', "<i class='fa fa-refresh fa-spin'></i>");
                $me.button('loading');
            });
            base.options.mycolumns = new Array();
            $.ajax({
                url: base.options.ajaxUrl,
                type: base.options.ajaxType,
                dataType: base.options.ajaxDataType,
                data: base.options.ajaxData,
                async: true,
                success: function (data) {
                    base.options.alldata = data;
                    if (data.infoData != "")
                        base.options.infoData = eval("(" + data.infoData + ")");

                    $(base.options.refreshButton).button('reset');
                    eval(" rowsCount_" + base.getCookieID() + " = data.count;");
                    eval(" Finalquery = data.query;");
                    if (base.options.new) {
                        base.options.count = 1;
                        base.$el.html('');

                        //Search
                        if (base.options.hasSearch) {
                            base.buildSearch($tableControl);
                        }

                        //Filters
                        base.buildFilters($tableControl);

                        //Show
                        if (base.options.hasShow) {
                            base.buildShow($tableControl);
                        }

                        //button modal
                        if (base.options.noCheckboxes == false) {
                            var namemodal = "mimodal_" + $(base.el).attr("id");
                            base.buildButtonModalColumns($tableControl, namemodal);
                            iUlModal = "gg_" + namemodal;
                        }

                        //Header
                        var rh = new Array(),
                            rhc = new Array(),
                            orhc = new Array();

                        for (var k = 0, s = data.header.length; k < s; k++) {
                            var rows = data.header[k];
                            base.pushBeforeHeader(rh, rows);
                        }


                        rh.push('<tr>');

                        if (base.options.headerSort !== undefined) {
                            if (base.options.headerSort.length != data.columns.length)
                                base.options.headerSort = undefined;
                        }

                        //Variable que se usa en el View para obtener nombre de columnas
                        eval(" columns_" + base.getCookieID() + " = data.columns;");

                        if (base.options.headerSort == undefined) {

                            for (var k = 0, s = data.columns.length; k < s; k++) {
                                var column = data.columns[k];

                                //Si se ha establecido columnas apagadas por Default
                                if (base.options.defaultColumnOff) {
                                    var columnOn = true;

                                    //Recorre el arreglo de los nombre de campos que deben estar apagados
                                    for (var i = 0; i < base.options.columnasDesactivadas.length; i++) {
                                        //Si en el arreglo hay un nombre igual a column


                                        if (column.n == base.options.columnasDesactivadas[i]) {
                                            base.pushHeaderSort(orhc, column, k, false);
                                            base.pushHeader(rhc, column);
                                            columnOn = false;
                                            break;
                                        }
                                    }
                                    //si el column no se encontro en el arreglo anterior
                                    if (columnOn) {
                                        base.pushHeaderSort(orhc, column, k, true);
                                        base.pushHeader(rhc, column);
                                    }
                                }
                                else {
                                    base.pushHeaderSort(orhc, column, k, true);
                                    base.pushHeader(rhc, column);
                                }
                            }
                        } else {
                            for (var i in base.options.headerSort) {
                                var k = base.options.headerSort[i];
                                var column = data.columns[k.v];

                                if (column !== undefined) {
                                    base.pushHeaderSort(orhc, column, k.v, k.c);
                                    if (k.c)
                                        base.pushHeader(rhc, column);
                                }
                            }
                        }

                        var w = base.buildModalColumns($tableControl, orhc.join(''), namemodal);

                        $(w).appendTo($('<div/>', {
                            class: base.options.sortableDivClass
                        }).appendTo($tableControl)).on("click", "#updatechecks", function () {

                            var b = $(this);
                            b.data('loading-text', "<i class='fa fa-refresh fa-spin'></i>");
                            b.button('loading');

                            var ul = $(this).data("idul");
                            var id = $(this).data("idmodal");
                            var $list2 = $("#" + ul);

                            $("#" + id + "").modal("hide");
                            base.updateHeaderSort($list2);
                            b.button('reset');
                            return false;

                        });

                        rh = rh.concat(rhc);
                        rh.push('</tr>');

                        $('<thead/>', {
                            html: rh.join('')
                        }).appendTo($table);

                        $table.on('click', 'th a', function () {
                            var $_ma = $(this);
                            if ($_ma.data('name') == undefined || $_ma.data('name').length == 0)
                                return false;
                            base.options.orderName = $_ma.data('name');
                            base.options.orderSort = base.options.orderSort == ' DESC' ? ' ASC' : ' DESC';
                            base.options.ajaxData.orderName = base.options.orderName;
                            base.options.ajaxData.orderSort = base.options.orderSort;
                            base.update();
                            return false;
                        });

                        base.options.new = false;

                        $tableControl.appendTo($tc);

                        $table.appendTo($tc);

                        $tc.appendTo(base.$el);

                        if (!base.options.InstantRun && base.options.orderCCC == undefined) {
                            $table.hide();
                        }
                    }

                    if (!base.options.new) {
                        base.options.count++;
                        $table = base.$el.find('table');
                        if (base.options.count > 2) {
                            $table.show();
                        }
                        $table.find('tbody').remove();
                        base.$el.find("." + base.options.pagerClass.split(' ')[0]).remove();

                        //Body
                        var rb = new Array();
                        for (var k = 0, s = data.rows.length; k < s; k++) {
                            var rows = data.rows[k];
                            base.pushBody(rb, rows);
                        }

                        $('<tbody/>', {
                            html: rb.join('')
                        }).appendTo($table);


                        if (base.options.ajaxData.limit == 0)
                            base.options.ajaxData.limit = 1;

                        //Pager
                        var pagerItems = new Array();
                        var pageItems = Math.ceil(parseInt(data.count) / base.options.ajaxData.limit);

                        var p_idx = Math.ceil(base.options.pagerCount / 2);
                        var s = base.options.current - p_idx > 0 ? base.options.current - p_idx : 1;
                        var ls = base.options.ajaxData.limit * (pageItems - 1);

                        /*Pager Items*/
                        pagerItems.push('<ul class="{0}">'.format(base.options.pagerListClass));

                        //First
                        if (base.options.pagerHasFirst)
                            pagerItems.push('<li class="{4}"><a class="{1}" data-current="{3}" data-idx="{2}">{0}</a></li>'.format(base.options.pagerFirstLabel, base.options.pagerFirstClass, 0, 1, s == 1 ? 'disabled' : ''));

                        //Items
                        for (var i = 0; i < base.options.pagerCount && s <= pageItems; i++) {
                            pagerItems.push('<li class="{4}"><a class="{1} {5}" data-current="{3}" data-idx="{2}">{0}</a></li>'.format(s, s == base.options.current ? 'current' : '', (s - 1) * base.options.ajaxData.limit, s, s == base.options.current ? 'active' : '', s == base.options.current ? base.options.classPageLinks : ''));
                            s++;
                        }

                        //Last
                        if (base.options.pagerHasLast)
                            pagerItems.push('<li class="{4}" ><a class="{1}" data-current="{3}" data-idx="{2}">{0}</a></li>'.format(base.options.pagerLastLabel, base.options.pagerLastClass, ls, pageItems, pageItems == base.options.current ? 'disabled' : ''));

                        pagerItems.push('</ul>');

                        $('<div/>', {
                            html: pagerItems.join(''),
                            class: base.options.pagerClass
                        }).on('click', 'ul li a', function () {
                            var $_ma = $(this);
                            if (base.options.current != parseInt($_ma.data('current'))) {
                                base.options.current = $_ma.data('current');
                                base.options.ajaxData.offset = $_ma.data('idx');
                                base.update();
                            }
                            return false;
                        }).appendTo(base.$el);

                        $("#" + iUlModal + "").sortable({
                            stop: function (event, ui) {
                                var $list = ui.item.closest('ul');
                            }
                        });
                    }

                    base.options.onRefresh();
                    $moreLinks = null;
                    var linkcount = 0;
                    $(".table-striped tbody tr").each(function () {
                        $trtd = $(this);
                        if (linkcount < $trtd.find(".btn-circle").length) {
                            $moreLinks = $trtd;
                            linkcount = $trtd.find(".btn-circle").length;
                        }
                    });

                    $items = {};
                    if ($moreLinks != null) {
                        $moreLinks.find(".btn-circle").each(function () {
                            $buttons = $(this);
                            $iv = $buttons.find("i").attr("class").replace("fa ", "");
                            $items[$buttons.attr('title')] = {
                                name: $buttons.attr('title'),
                                icon: $iv,
                                callback: function (key, options) {
                                    var element = $(this);
                                    var button = element.find(".btn-circle[title='" + key + "']");
                                    if (button.prop("tagName") == "BUTTON") {
                                        button.click();
                                    }
                                    if (button.prop("tagName") == "A") {
                                        location.href = button.attr('href');
                                    }
                                }
                            };
                        });

                        //$.contextMenu({
                        //    selector: '.table-striped tbody tr',
                        //    callback: function (key, options) {
                        //        var element = $(this);
                        //        var m = "clicked: " + key;
                        //    },
                        //    items: $items
                        //});
                    }
                }
            });
        };

        base.getCookieID = function () {
            return 'cstable_cookie_header_' + this.$el.attr('id');
        };

        base.resertCookie = function () {
            $.cookie(base.getCookieID(), undefined);
        };

        base.buildSearch = function ($tableControl) {
            $('<input/>', {
                class: base.options.searchClass,
                name: base.options.searchName,
                placeholder: base.options.searchPlaceholder,
                value: base.options.searchValue
            }).keyup(function () {
                base.options.ajaxData[base.options.searchName] = $(this).val();
                base.options.searchValue = $(this).val();
                if (typeof base.options.searchTimer != undefined)
                    clearTimeout(base.options.searchTimer);
                base.options.searchTimer = setTimeout(function () {
                    base.options.ajaxData.offset = 0;
                    base.options.current = 1;
                    base.update();
                }, base.options.searchTimeout);
                return false;
            }).appendTo($('<label/>', { class: base.options.searchLabelClass }).appendTo($tableControl));
        };

        base.buildShow = function ($tableControl) {
            var select = "";
            for (var i in base.options.showOptions) {
                select += "<option {0} value='{1}'>{2}</option>".format(base.options.ajaxData.limit == i ? 'selected' : '', i, i);
            }
            $('<select/>', {
                html: select,
                name: base.options.showName,
                class: base.options.showClass
            }).appendTo($tableControl).change(function () {
                base.options.ajaxData.limit = $(this).val();
                base.options.ajaxData.offset = 0;
                base.options.current = 1;
                base.update();
            });
        }

        base.buildFilters = function ($tableControl) {
            for (var i in base.options.filterListSelector) {
                var f_opt = base.options.filterListSelector[i];
                var $el = $(i);
                if (base.options.sendInstantFilter) {
                    $el.unbind();
                    $el.on('change', function () {
                        var name = $(this).attr('name');
                        base.options.ajaxData[name] = $(this).val();
                        base.update();
                    });
                } else {
                    $el.unbind();
                    $el.on('change', function () {
                        var name = $(this).attr('name');
                        base.options.ajaxData[name] = $(this).val();
                    });
                }
                if (f_opt.isControl) {
                    $el.appendTo($tableControl);
                }
            }
        }

        base.buildButtonModalColumns = function ($tableControl, namemodal) {
            $('<button/>',
                {
                    text: 'Columns',
                    click: function () {
                        $('.modal-dialog').attr("style", "");
                        $("#" + namemodal + "").modal("show");
                        return false;
                    },
                    "data-toogle": "modal",
                    "data-target": namemodal,
                    class: "btn btn-" + primaryColor + " col-lg-2 p_text cho_text admCols",
                }).appendTo($tableControl);
        }
        base.buildModalColumns = function ($tableControl, cols, id) {

            var k = $('<div/>',
                  {
                      id: id,
                      html:
                          "<div class=\"modal-dialog\">" +
                          "<div class=\"modal-content\"> " +
                          "<div class=\"modal-header\">" +
                          "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>" +
                          "<h4 class=\"modal-title\">Columns</h4>" +
                          "</div>" +
                          "<div class=\"modal-body no-padding\">" +
                           "<form action=\"#\" class=\"smart-form\">" +
                      "<fieldset>" +
                      "<div class=\"dd\" >  <ol id=\"gg_" + id + "\" class=\"dd-list\">" +
                      cols +
                      "</ul> </div>" +
                          "</fieldset> <footer>" +
                          "<button id=\"updatechecks\" data-idmodal=\"" + id + "\" data-idul=\"gg_" + id + "\" class=\"btn btn-info refreshTable formClose fromColumns\" title=\"OK\" ><i class=\"glyphicon glyphicon-ok\"></i></button>" +
                      "</footer> </form>" +
                          "<div/>  </div> </div>"
                      , class: "buttonModal modal  tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"myModalLabel\" aria-hidden=\"true\""

                  });
            return k;
        }

        base.pushHeader = function (rhc, column) {
            if (column.n == 'actives') {
                return;
            }
            rhc.push('<th class="{3} text_th {4}"><a {0} data-name="{2}">{1}</a></th>'.format(column.p, column.t, column.n, column.n == base.options.orderName ? base.options.orderSort : '', column.tc));
            base.options.mycolumns.push(column.t);
        };


        base.pushHeaderSort = function (orhc, column, v, c) {
            var fffidex = '<input  class="baseCheck" data-dd="{0}" type="checkbox" data-name="{2}" value="{0}" {3}>  <i data-swchon-text="Yes" data-swchoff-text="No"></i>';
            if (column.n == 'actives') {
                return;
            }


            if (base.options.noCheckboxes == false) {
                var kk = ('<li class=" ' + "dd-item" + '" > ' +
                    '<div class="dd-handle dd3-handle" style="width:5px;">.</div>' +
                    '<div class="dd3-content">' +
                    '{1} <div class="pull-right"> <label class="toggle">  ' + fffidex  +
                    ' </label></div> </div> </li>').format(v, column.t, column.n, c ? 'checked' : '');

                orhc.push(kk);
            }
        };

        base.updateHeaderSort = function ($list) {
            var newColumnsOrder = [];

            $list.find('input').each(function (i, e) {

                var ka = $(this).val();
                if (ka.indexOf("false") > -1 || ka.indexOf("true") > -1) {

                    ka = $(this).data("dd");
                }
                newColumnsOrder.push({ v: ka, c: $(this).is(':checked') });
            });

            
            $.cookie(base.getCookieID(), newColumnsOrder);
            base.options.new = true;
            base.options.orderCCC = true;
            base.update();
        }

        base.pushBeforeHeader = function (rb, rows) {
            if (rows != null) {
                rb.push('<tr {0}>'.format(rows.p));
                for (var i in rows.d) {
                    var row = rows.d[i];
                    rb.push('<td {1}>{0}</td>'.format(row.d, row.p));
                }
                rb.push('</tr>');
            }
        };

        base.pushBody = function (rb, rows) {
            rb.push('<tr {0}>'.format(rows.p));

            if (base.options.headerSort != undefined) {
                for (var i in base.options.headerSort) {
                    var s = base.options.headerSort[i];
                    var row = rows.d[s.v];
                    if (row !== undefined) {
                        if (s.c) {
                            if (row.p.indexOf("lastRow") == -1) {

                                rb.push('<td {1}> {0} </td>'.format(ifLargeContent(row.d, base.options.mycolumns[i], base.options.columnMaxLength, base.options.columnCharactersToShow), row.p));
                            }
                            else {
                                rb.push('<td {1}>{0}</td>'.format(row.d, row.p));
                            }
                        }
                    }
                }
            } else {

                for (var i in rows.d) {
                    var row = rows.d[i];
                    if (row.column != 'actives') {
                        if (row.p.indexOf("lastRow") == -1) {
                            rb.push('<td {1}> {0} </td>'.format(ifLargeContent(row.d, base.options.mycolumns[i], base.options.columnMaxLength, base.options.columnCharactersToShow), row.p));
                        }
                        else {

                            rb.push('<td {1}>{0}</td>'.format(row.d, row.p));
                        }
                    }
                }
            }
            rb.push('</tr>');
        };



        base.update = function () {
            this.$el.CStable(this.options);
            nav_page_height();
        };

        base.init();
    };

    function ifLargeContent(content, header, maxLength, charactersToShow) {
        if (content.indexOf('<!--html-->') !== -1) {
            return content;
        }

        if (content.indexOf('#file&&&') !== -1) {
            var file = content.split('&&&')[1];
            var pp = file.split('<>');
            var idx = guid();
            var final = '<a style="line-height: 0;text-transform: inherit;display: unset;font-size: inherit;" class="btn btn-link" data-target="#' + idx + '" data-toggle="modal" href="javascript:void(0);">Preview </a>';
            final += '<div class="modal fade showmore" id="' + idx + '"> <div class="modal-dialog"> <div class="modal-content"> <div class="modal-header"> <button type="button" class="close" data-dismiss="modal" aria-hidden="true">X</button> <h4 class="modal-title" id="myModalLabel">' + pp[0] + '\'s document</h4> </div> <div class="modal-body"> <object data="files/' + pp[2] + '/' + pp[0] + '/' + pp[1] + '" style="width: 100%; min-height: 400px;"></object> </div> </div> </div> </div>';
            return final;
        }

        if (content.indexOf('btn-circle') !== -1 || content.indexOf('This list conta') !== -1) {
            return content;
        }
        if (content.indexOf('colorColumn') !== -1) {
            return "<div style='height:20px;background-color:" + content.split("=")[1] + " !important' class=\"noclick btn\"></div>";
        }

        var final = content;
        if (content.length > maxLength) {
            var idx = guid();
            var header = "<div class='modal-header'> <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">X</button><h4 class='modal-title' id='myModalLabel'>" + (header == undefined ? "More Detail" : header) + "</h4></div>";
            if (content.indexOf('</') !== -1) {
                final = "<a style='line-height: 0;text-transform: inherit;display: unset;font-size: inherit;' data-target=\"#" + idx + "\" data-toggle=\"modal\" href='javascript:void(0);' class='btn btn-link'>" + "..." + "</a><div class=\"modal fade showmore\" id='" + idx + "'><div class=\"modal-dialog\"><div class=\"modal-content\">" + header + "<div class='modal-body'><div class='htmlmodal'>" + content + "</div></div></div></div></div>";
                return final;
            }
            final = "<a title='" + content + "' style='line-height: 0;text-transform: inherit;display: unset;font-size: inherit;margin-left:-19px;' data-target=\"#" + idx + "\" data-toggle=\"modal\" href='javascript:void(0);' class='btn btn-link'>" + content.substring(0, charactersToShow) + "</a><div class=\"modal fade showmore\" id='" + idx + "'><div class=\"modal-dialog\"><div class=\"modal-content\">" + header + "<div class='modal-body'><p class='textlong'>" + content.replace("&nbsp;", "") + "</p></div></div></div></div>";
            return final;
        }

        return "<a class='editlinks' href='javascript:void(0)'>" + content + "</a>";
    }

    $.CStable.defaultOptions = {
        new: true,
        newby: true,
        mycolumns: new Array(),
        ajaxDataType: 'json',
        ajaxType: 'get',
        ajaxUrl: '',
        ajaxData: {
            limit: 10,
            offset: 0
        },
        current: 1,
        pagerCount: 10,
        pagerClass: 'dt-bottom-row borde_pagination',
        pagerListClass: 'pagination fr',
        pagerHasFirst: true,
        pagerHasLast: true,
        pagerFirstLabel: 'Fist',
        pagerFirstClass: 'first',
        pagerLastLabel: 'Last',
        pagerLastClass: 'last',
        tableClass: 'table table-striped table-hover',
        tableContainerClass: 'smart-form dataTables_wrapper form-inline pl0 pr0 overflowAuto',
        tableControlClass: 'col-lg-12 elementos_up pr pl',
        hasSearch: true,
        searchValue: '',
        defaultColumnOff: false,
        searchBind: true,
        searchTimeout: 300,
        searchTimer: null,
        searchClass: '',
        showColumnsButton: true,
        searchName: 'cstable_search',
        searchPlaceholder: 'Search',
        hasMultipleSearch: false,
        searchLabelClass: 'input col-lg-2 p_text',
        sortableDivClass: 'col-lg-8  p_text',
        refreshButton: '.refreshTable',
        hasShow: true,
        showOptions: {
            10: 10,
            20: 20,
            30: 30
        },
        showClass: 'cstable_show col-lg-1 p_text cho_text',
        showName: 'cstable_show',
        filterListSelector: {},
        syncAjax: true,
        noCheckboxes: false,
        classPageLinks: '',
        alldata: {},
        infoData: "",
        onRefresh: function () {
            for (var item in allminiTables) {
                eval('if (typeof makeHide' + allminiTables[item] + ' !== "undefined") {makeHide' + allminiTables[item] + '(); makeHide' + allminiTables[item] + '=undefined;}');
            }
        },
        fixedIndex: [],
        sendInstantFilter: false,
        InstantRun: true,
        columnMaxLength: 1000,
        columnCharactersToShow: 900,
    };

    $.fn.CStable = function (options) {
        return this.each(function () {
            (new $.CStable(this, options));
        });
    };

    $.cookie.json = true;
})(jQuery);