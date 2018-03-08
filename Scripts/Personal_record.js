var Personal_record_OBJECT = {};

Personal_record_OBJECT.show_basket_flag = false;
Personal_record_OBJECT.show_follow_flag = false;
Personal_record_OBJECT.show_comment_flag = false;
Personal_record_OBJECT.show_purchase_flag = false;
//
Personal_record_OBJECT.req_comment_flag = true;
Personal_record_OBJECT.req_purchase_flag = true;
//
Personal_record_OBJECT.Edit_One_comment = (id_comm, id_obj) => {
    var block = document.getElementById("Personal_record_one_comm_num_id" + id_comm);
    var str = "<form action='/Home/Edit_comment' method='post'><input id='id_object' name='id_object' type='hidden' value='";
    str += id_obj + "' /><textarea class='Personal_record_text_area_comment' cols='20' id='text' name='text' rows='2'>";

    var block_text_b = document.getElementById("Personal_record_text_one_comment_id" + id_comm);
    var text = block_text_b.innerHTML;
    str += text + "</textarea><input id='mark' name='mark' type='hidden' value='-1' /><input id='from' name='from' type='hidden' value='Personal_record' />";
    str += "<div></div><input type='submit' class='submit' style='margin-top:15px;' value='Отправить' /></form>";

    block.innerHTML = str;

}

Personal_record_OBJECT.OnComplete_load_comments_pers_rec = (request, status) => {
    //alert("_"+request["responseText"]+"_");
    var block1 = document.getElementById("count_comment_on_page");
    var block2 = document.getElementById("count_comment_from_one_load");
    block1.value = +block1.value + +block2.value;
    var el = document.getElementById('Load_comment_for_personal_record_count_request');
    el.parentNode.removeChild(el);
    if (el.value == 0) {
        Personal_record_OBJECT.req_comment_flag = false;
    }
    else {
        Personal_record_OBJECT.req_comment_flag = true;
    }
}

Personal_record_OBJECT.OnBegin_load_comments_pers_rec = () => {

    Personal_record_OBJECT.req_comment_flag = false;
}



//
Personal_record_OBJECT.OnBegin_load_purchases_pers_rec = () => {

    Personal_record_OBJECT.req_purchase_flag = false;
}

Personal_record_OBJECT.OnComplete_load_purchases_pers_rec = (request, status) => {
    var block1 = document.getElementById("count_purchase_on_page");
    var block2 = document.getElementById("count_purchase_from_one_load");
    block1.value = +block1.value + +block2.value;
    var el = document.getElementById('Load_purchase_for_personal_record_count_request');
    el.parentNode.removeChild(el);
    if (el.value == 0) {
        Personal_record_OBJECT.req_purchase_flag = false;
    }
    else {
        Personal_record_OBJECT.req_purchase_flag = true;
    }

}
//-------------------------------------------------------------------------------------
Personal_record_OBJECT.Show_basket = () => {
    var block3 = document.getElementById("Personal_record_follow_block_hidden_id");
    var block = document.getElementById("Personal_record_basket_block_hidden_id");
    var triangle = document.getElementById("Personal_record_for_triangle_b");
    
    if (Personal_record_OBJECT.show_basket_flag) {
        block.style.display = 'none';
        triangle.innerHTML = "<div class='Personal_record_triangle_right'></div>";
    }
    else {
        block.style.display = 'block';
        triangle.innerHTML = "<div class='Personal_record_triangle_down'></div>";
    }
    Personal_record_OBJECT.show_basket_flag = !Personal_record_OBJECT.show_basket_flag;
}
Personal_record_OBJECT.Show_follow = () => {
    var block = document.getElementById("Personal_record_follow_block_hidden_id");
    var triangle = document.getElementById("Personal_record_for_triangle_f");
    if (Personal_record_OBJECT.show_follow_flag) {
        block.style.display = 'none';
        triangle.innerHTML = "<div class='Personal_record_triangle_right'></div>";
    }
    else {
        block.style.display = 'block';
        triangle.innerHTML = "<div class='Personal_record_triangle_down'></div>";
    }
    Personal_record_OBJECT.show_follow_flag = !Personal_record_OBJECT.show_follow_flag;
}
Personal_record_OBJECT.Show_comments = () => {
    var block = document.getElementById("Personal_record_comments_block_id_hidden");
    var triangle = document.getElementById("Personal_record_for_triangle_c");
    if (Personal_record_OBJECT.show_comment_flag) {
        block.style.display = 'none';
        triangle.innerHTML = "<div class='Personal_record_triangle_right'></div>";
    }
    else {
        block.style.display = 'block';
        triangle.innerHTML = "<div class='Personal_record_triangle_down'></div>";
    }
    Personal_record_OBJECT.show_comment_flag = !Personal_record_OBJECT.show_comment_flag;
}
Personal_record_OBJECT.Show_Purchases = () => {
    var block = document.getElementById("Personal_record_purchases_block_id_load");
    var triangle = document.getElementById("Personal_record_for_triangle_p");
    if (Personal_record_OBJECT.show_purchase_flag) {
        block.style.display = 'none';
        triangle.innerHTML = "<div class='Personal_record_triangle_right'></div>";
    }
    else {
        block.style.display = 'block';
        triangle.innerHTML = "<div class='Personal_record_triangle_down'></div>";
    }
    Personal_record_OBJECT.show_purchase_flag = !Personal_record_OBJECT.show_purchase_flag;
}
//--------------------------------------------------------------------------------------

Personal_record_OBJECT.Load_personal_record_comments_block = () => {

    var comments = $("#Personal_record_comments_load_block");


    if (Personal_record_OBJECT.show_comment_flag ? isVisible(comments) : false) {

        var comments_submit = document.getElementById("Personal_record_submit_comments_load_id")
        //alert("[vvv")
        //o.submit();
        if (Personal_record_OBJECT.req_comment_flag) {
            comments_submit.click();
        }

    }
}
Personal_record_OBJECT.Load_personal_record_purchase_block = () => {
    var purchase = $("#Personal_record_purchase_load_block");


    if (Personal_record_OBJECT.show_purchase_flag ? isVisible(purchase) : false) {

        var purchase_submit = document.getElementById("Personal_record_submit_purchase_load_id")

        if (Personal_record_OBJECT.req_purchase_flag) {

            purchase_submit.click();
        }

    }
}
$(function () {
    $(window).scroll(function () {
        Personal_record_OBJECT.Load_personal_record_comments_block();
        Personal_record_OBJECT.Load_personal_record_purchase_block();

    });
});