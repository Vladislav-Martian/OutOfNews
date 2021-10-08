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

$('#register').hover(
    function() {
        var $this = $(this); // caching $(this)
        $this.data('defaultText', $this.text());
        $this.text("Sign In");
    },
    function() {
        var $this = $(this); // caching $(this)
        $this.text($this.data('defaultText'));
    }
);

$('#login').hover(
    function() {
        var $this = $(this); // caching $(this)
        $this.data('defaultText', $this.text());
        $this.text("Sign Up");
    },
    function() {
        var $this = $(this); // caching $(this)
        $this.text($this.data('defaultText'));
    }
);