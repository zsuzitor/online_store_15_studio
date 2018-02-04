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

    //alert(Object_view_OBJECT.current_slider_1_id);//5
    //alert(Object_view_OBJECT.First_num_slider_2);//4
    //alert(Object_view_OBJECT.Count_block_for_slider_2);//5

    

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
    Object_view_OBJECT.Count_in_list_slider_2 = +document.getElementById("count_obg_slider_2_id").value;
    

    Object_view_OBJECT.Next_slider_2();

    if (Object_view_OBJECT.Count_block_for_slider_2 > Object_view_OBJECT.Count_in_list_slider_2) {

        
        Object_view_OBJECT.Hide_prev_next_block_slider_2();

    }
    


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

   // if (Object_view_OBJECT.current_slider_1_id > Object_view_OBJECT.First_num_slider_2) {// с 6 на 7 переходе доп прокрутка

       // Object_view_OBJECT.Next_slider_2();
    //}
    //alert(inviz())
   
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
    if (!inviz()) {
        Object_view_OBJECT.Next_slider_2();
    }

    Object_view_OBJECT.change_image(curr_block_img);
}

function inviz() {
    var res = false;
    var list = Object_view_OBJECT.First_num_slider_2;
    for (var i = 0; i < Object_view_OBJECT.Count_block_for_slider_2; ++i) {
       
        //alert(list);
        
        if (list == Object_view_OBJECT.current_slider_1_id) {
            res = true;
            break;
        }
        
        list = list - 1;
        if (list < 0) {
            list = Object_view_OBJECT.Count_in_list_slider_2 - 1;
        }
    }
    return res;
}

//-------------------------------------------- up\down slider


Object_view_OBJECT.First_num_slider_2 = 0;
Object_view_OBJECT.Count_in_list_slider_2 = 0;
Object_view_OBJECT.Count_block_for_slider_2 = 5;
//
Object_view_OBJECT.Prev_slider_2_click = () => {
    Object_view_OBJECT.Prev_slider_2();
    clearTimeout(Object_view_OBJECT.time_slider_1);
    clearInterval(Object_view_OBJECT.timer_slider_1);


    Object_view_OBJECT.time_slider_1 = setTimeout(
        Object_view_OBJECT.Up_slider_1_time, 5000);
};
Object_view_OBJECT.Next_slider_2_click = () => {
    Object_view_OBJECT.Next_slider_2();
    clearTimeout(Object_view_OBJECT.time_slider_1);
     clearInterval(Object_view_OBJECT.timer_slider_1);


     Object_view_OBJECT.time_slider_1 = setTimeout(
     Object_view_OBJECT.Up_slider_1_time, 5000);
};

Object_view_OBJECT.Next_slider_2 = () => {

    var slider_1 = document.getElementById("Object_view_small_img_change_block_id");
    slider_1.innerHTML = "";

    for (var i = 0; i < Object_view_OBJECT.Count_in_list_slider_2 && i < Object_view_OBJECT.Count_block_for_slider_2; ++i) {
        var slider_1_block_1 = document.getElementById("Object_view_small_img_change_hidden" + Object_view_OBJECT.First_num_slider_2);

        if (slider_1_block_1 != null) {
            slider_1.innerHTML += slider_1_block_1.innerHTML;
            ++Object_view_OBJECT.First_num_slider_2;

        }
        else {
            Object_view_OBJECT.First_num_slider_2 = 0;//First_num_slider_1 + 1 - 6
            slider_1_block_1 = document.getElementById("Object_view_small_img_change_hidden" + Object_view_OBJECT.First_num_slider_2);
            if (slider_1_block_1 != null) {
                slider_1.innerHTML += slider_1_block_1.innerHTML;
                ++Object_view_OBJECT.First_num_slider_2;
            }
            else {
                return;
            }
        }
    }
   

    if (Object_view_OBJECT.First_num_slider_2 >= 1)
        Object_view_OBJECT.First_num_slider_2 -= 1;
};


Object_view_OBJECT.Prev_slider_2 = () => {
    var num = Object_view_OBJECT.First_num_slider_2 + 2 * (1 - Object_view_OBJECT.Count_block_for_slider_2);

    if (num < 0)
        num = Object_view_OBJECT.Count_in_list_slider_2 + num;
    Object_view_OBJECT.First_num_slider_2 = num;

    Object_view_OBJECT.Next_slider_2();




}






























Object_view_OBJECT.Hide_prev_next_block_slider_2 = () => {
    var prev = document.getElementById("Object_view_external_button_prev_main_slider_id");
    var next = document.getElementById("Object_view_external_button_next_main_slider_id");
    prev.innerHTML = "";
    next.innerHTML = "";
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