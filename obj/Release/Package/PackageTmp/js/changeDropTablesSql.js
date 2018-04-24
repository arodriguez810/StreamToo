
$(document).on("change", "#tableName", function () {

    $me = ($(this))
    if ($me.val() != "" && $me.val() != undefined) {

        var url = $(this).data("urldrop");

        //$("#Query").attr("data-validation", "required");
        //validateForm("Report-form");

        var idhidden = $("#id").val();
        $.ajax({
            //url: "@Url.Action("TableColumns")",
            url: "" + url + "",
            type: 'post',
            data: { tableName: $me.val(), idTab: idhidden },
            success: function (data) {
                //$(".select2-search-choice").remove();
                $('#Query').select2('data', null);
                $("#Query").empty();

                var selecteds = [];
                $.each(data, function (index, item) { // Iterates through a collection
                    if (item.selected == "true") {
                        $("#Query").append("<option value = \"" + item.name + "\" >" + item.name + "</option>");
                        selecteds.push(item.name);
                    } else {
                        $("#Query").append("<option value = \"" + item.name + "\" >" + item.name + "</option>");
                    }
                });

                $("#Query").val(selecteds).trigger("change");

                //$("#Query").html(data);
            }
        });
    } else {
        $("#Query").removeData("validation");
        //$(".select2-search-choice").remove();
        $("#Query").empty();
    }

});

$(document).on("change","#categoryID",function () {
    $yo = $(this);
    $("#consult").attr("disabled", "disabled");
    $("#ReportCategory").attr("disabled", "disabled");

    $("#printTbl").html("");
    $("#printTblWithParameters").html("");
    $("#toexport").hide();

    var bu = $("#consult");
    bu.data('loading-text', "<i class='fa fa-refresh fa-spin'></i>");
    bu.button('loading');

    if ($yo.val() != "" && $yo.val() != undefined) {
        var url = $yo.data("urldata");
        $.ajax({
            url: "" + url + "",
            type: 'post',
            data: { category: $yo.val() },
            async:false,
            success: function (data) {               
                $("#ReportCategory").empty();
                $("#ReportCategory").append("<option value = \"\" selected=\"selected\">Seleccione Reporte</option>");

                $.each(data, function (index, item) { // Iterates through a collection                                            
                    $("#ReportCategory").append("<option value = \"" + item.id + "\" >" + item.name + "</option>");
                });
                $("#ReportCategory").removeAttr("disabled");
                $("#consult").removeAttr("disabled");
            }
        });
    } else {
        $("#ReportCategory").attr("disabled", "disabled");
    }
    bu.button('reset');
});