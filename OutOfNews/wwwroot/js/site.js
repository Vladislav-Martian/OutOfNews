$(function() {
    $('.scroll-down').click (function() {
        $("html, body").animate({ scrollTop: $(document).height() }, 1000);
        return false;
    });
});

$(document).ready(function(){
    $('#scroller').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 1000);
        return false;
    });
});