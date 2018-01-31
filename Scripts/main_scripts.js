
var time_for_page_up;// for up()

///--------------------------------------------------------------------------------------

function up() {
    var top = Math.max(document.body.scrollTop, document.documentElement.scrollTop);
    if (top > 0) {
        window.scrollBy(0, -100);
        time_for_page_up = setTimeout('up()', 20);
    } else clearTimeout(time_for_page_up);
    return false;
}

function isVisible(tag) {//работает для маленьких объектов(меньше экрана)
    var t = $(tag);
    var w = $(window);
    var top_window = w.scrollTop();
    var bot_window=top_window+document.documentElement.clientHeight;
    var top_tag = t.offset().top;
    var bot_tag = top_tag + t.height();
    //alert(bot_tag);
    //alert(top_window);
    return ((bot_tag >= top_window && bot_tag<=bot_window) || (top_tag>=top_window&&top_tag<=bot_window));
}

/*
function isVisible(tag) {
    var t = $(tag);
    var wt = window.pageYOffset;
    var tt = t.offset().top;
    wt += document.documentElement.clientHeight;
    alert(tt);
    alert(wt);
        return (tt <= wt);
    
}
*/








$(function () {
    $(window).scroll(function () {
        Change_main_header();
    });
});


//------------------------------------------------------------
function Change_main_header() {
    var b = $("#Main_header_check_small_or_big_header");
    if (!b.prop("shown") && !isVisible(b)) {
        b.prop("shown", true);
        var o = document.getElementById("Main_header_small_id")
        var o1 = document.getElementById("Main_header_back_to_top_id")
        o1.style.display = 'block';

        o.style.display = 'block';

    }
    else {
        if (b.prop("shown") && isVisible(b)) {
            b.prop("shown", false);
            var o = document.getElementById("Main_header_small_id")
            var o1 = document.getElementById("Main_header_back_to_top_id")
            o1.style.display = 'none';
            o.style.display = 'none';
        }
    }

}





