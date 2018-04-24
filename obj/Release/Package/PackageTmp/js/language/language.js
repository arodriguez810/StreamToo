var message = new Array();


function tj(text) {
    return message[text];
}

$(window).bind("load", function () {
    $(".lan").each(function () {
        me = $(this);
        if (me.attr("title") != undefined)
            me.attr("title", message[me.attr("title")]);
        type = "(" + me.prop('tagName') + ")";
        if ("(button)(input)".indexOf(type) > -1) {
            me.id = me.val();
            $(this).val(message[me.id]);
        }
        else {
            me.id = me.html();
            $(this).html(message[me.id]);
        }
    });
});