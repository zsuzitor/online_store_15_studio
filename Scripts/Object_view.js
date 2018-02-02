var Object_view_OBJECT = {};

Object_view_OBJECT.timer_slider_1;
Object_view_OBJECT.current_slider_1_id = 0;
Object_view_OBJECT.time_slider_1;
//-------------
Object_view_OBJECT.change_image_click = (a) => {
    Object_view_OBJECT.change_image(a);
    clearTimeout(Object_view_OBJECT.time_slider_1);
    clearInterval(Object_view_OBJECT.timer_slider_1);
    Object_view_OBJECT.time_slider_1 = setTimeout(
        Object_view_OBJECT.Up_slider_1_time, 6000);
}
Object_view_OBJECT.change_image = (a) => {
    var main_image = document.getElementById("Object_view_main_image_id");
    //alert(a.id);
    main_image.src = a.src;
    var arr_id = a.id.split('Object_view_img_for_main_id');
    Object_view_OBJECT.current_slider_1_id = arr_id[1];

}

Object_view_OBJECT.Edit_One_comment = (id_comm, id_obj) => {
    var block = document.getElementById("Object_view_one_comm_num_id" + id_comm);
    var str = "<form action='/Home/Edit_comment' method='post'><input id='id_object' name='id_object' type='hidden' value='";
    str += id_obj + "' /><textarea class='Object_view_text_area_comment' cols='20' id='text' name='text' rows='2'>";

    var block_text_b = document.getElementById("Object_view_text_one_comment_id" + id_comm);
    var text = block_text_b.innerHTML;
    str += text + "</textarea><input id='mark' name='mark' type='hidden' value='-1' /><input id='from' name='from' type='hidden' value='Object_view' />";
    str += "<div></div><input type='submit' class='submit' value='Отправить' /></form>";

    block.innerHTML = str;

}
//--------------------------SLIDERS------------------

Object_view_OBJECT.Load_slider = () => {

    Object_view_OBJECT.Up_slider_1_time();

}
Object_view_OBJECT.Up_slider_1_time = () => {
    Object_view_OBJECT.time_slider_1 = setTimeout(function () {
        Object_view_OBJECT.timer_slider_1 = setInterval(function () {
            Object_view_OBJECT.Next_slider_1();
        }, 6000);
    }, 6000);
}
Object_view_OBJECT.Next_slider_1 = () => {
    Object_view_OBJECT.current_slider_1_id = +Object_view_OBJECT.current_slider_1_id + 1;
    var curr_block_img = document.getElementById("Object_view_img_for_main_id" + Object_view_OBJECT.current_slider_1_id);

    if (curr_block_img == null) {
        Object_view_OBJECT.current_slider_1_id = 0;
        curr_block_img = document.getElementById("Object_view_img_for_main_id" + Object_view_OBJECT.current_slider_1_id);
        if (curr_block_img == null) {
            clearTimeout(Object_view_OBJECT.time_slider_1);
            clearInterval(Object_view_OBJECT.timer_slider_1);
            return;
        }
    }
    Object_view_OBJECT.change_image(curr_block_img);
}

//------------------------------------------------------------AJAX-LOAD---------
Object_view_OBJECT.req_comments_flag_ob_v = true;
Object_view_OBJECT.OnComplete_load_comments_Object_view = (request, status) => {
    //alert("_"+request["responseText"]+"_");
    var block1 = document.getElementById("count_comment_on_page");
    var block2 = document.getElementById("count_comment_from_one_load");
    block1.value = +block1.value + +block2.value;


    var el = document.getElementById('Load_comment_for_object_view_count_request');
    el.parentNode.removeChild(el);
    if (el.value == 0) {
        Object_view_OBJECT.req_comments_flag_ob_v = false;
    }
    else {
        Object_view_OBJECT.req_comments_flag_ob_v = true;
    }


}

Object_view_OBJECT.OnBegin_load_comments_Object_view = () => {

    Object_view_OBJECT.req_comments_flag_ob_v = false;
}
Object_view_OBJECT.Load_objects_list_LOT = () => {

    var comments = $("#Object_view_type_load_block");


    if (isVisible(comments)) {

        var comments_submit = document.getElementById("Object_view_submit_objects_load_id")

        if (Object_view_OBJECT.req_comments_flag_ob_v) {
            comments_submit.click();
        }

    }
}
$(function () {
    $(window).scroll(function () {
        Object_view_OBJECT.Load_objects_list_LOT();


    });
});



//-------------------------------------------------------------------------------------

document.addEventListener("DOMContentLoaded", Object_view_OBJECT.Load_slider);