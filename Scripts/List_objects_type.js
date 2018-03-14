var List_objects_type_OBJECT = {};
List_objects_type_OBJECT.req_objects_flag = true;




List_objects_type_OBJECT.OnComplete_load_objects_LOT = (request, status) => {
    //alert("_"+request["responseText"]+"_");
    var block1 = document.getElementById("count_object_on_page");
    var block2 = document.getElementById("count_object_from_one_load");
    block1.value = +block1.value + +block2.value;

    

    var el = document.getElementById('Load_object_for_list_objects_type_count_request');
    el.parentNode.removeChild(el);
    if (el.value == 0) {
        List_objects_type_OBJECT.req_objects_flag = false;
    }
    else {
        List_objects_type_OBJECT.req_objects_flag = true;
        var page = (+block1.value - +block1.value % +block2.value) / +block2.value - 1;


        var ref = "List_objects_type?page=" + page;
        history.pushState(null, null, ref);
    }
   
    
       
    

}

List_objects_type_OBJECT.OnBegin_load_objects_LOT = () => {

    List_objects_type_OBJECT.req_objects_flag = false;
}
List_objects_type_OBJECT.Load_objects_list_LOT = () => {

    var comments = $("#List_objects_type_load_block");


    if (isVisible(comments)) {

        var comments_submit = document.getElementById("List_objects_type_submit_objects_load_id");
        if (List_objects_type_OBJECT.req_objects_flag) {
            comments_submit.click();
        }

    }
}

List_objects_type_OBJECT.Load_search = () => {
    var hidden = document.getElementById("List_objects_type_search_hidden");
    var form_params_search = document.getElementById("Menu_search_form_text_rearch");
    form_params_search.innerHTML = hidden.innerHTML;
    var form_submit = document.getElementById("Menu_search_form_main_form_submit_id");
    form_submit.click();

}
$(function () {
    $(window).scroll(function () {
        List_objects_type_OBJECT.Load_objects_list_LOT();


    });
});



document.addEventListener("DOMContentLoaded", List_objects_type_OBJECT.Load_search);