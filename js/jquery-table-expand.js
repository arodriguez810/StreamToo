(function ($) {
    $.fn.jExpand = function () {
        this.each(function () {
            var element = this;
            var $element = $(element);
            $element.find("tr.expandable").hide();
            $element.find("tr:not(.expandable)").click(function (e) {
                $_me = $(this);
                /*Hide sibling active elements*/
                $_parent = $_me.parent();
                $_active = $_parent.find('tr.active').not($_me);
                $_next = $_active.next('tr.expandable:visible');
                /*Hide them*/
                $_next.hide();
                $_active.removeClass('active');
                $_active.find('.arrow').toggleClass('arrow_up');
                $_me.toggleClass('active');
                $_me.next('tr.expandable').toggle();
                $_me.find('.arrow').toggleClass('arrow_up');
            });
        });
    };
})(jQuery);
