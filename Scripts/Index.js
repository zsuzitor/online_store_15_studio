var Index_OBJECT = {};
var slider;
Index_OBJECT.up_slider = () => {
    var count_img_in_list_ = document.getElementById("count_id_slider_main_index").value;
    var width = document.documentElement.clientWidth;
    if (width < 960) {
        width = 960;
    }
    
    width = width * 0.8 - width * 0.8 * 0.1;
    slider = new Slider_(count_img_in_list_, "Index_block_type_1_id", "Index_main_slider_one_slide_id", 1000, width, 500, 0, true, "slider");
    slider.up();
   
   
}
//

//
Index_OBJECT.First_num_slider_1 = 0;
Index_OBJECT.Count_block_for_slider_1 = 0;
Index_OBJECT.Count_in_list_slider_1 = 0;
//
Index_OBJECT.timer_slider_1;
Index_OBJECT.time_slider_1;
//
Index_OBJECT.Prev_slider_1_click = ()=> {
    Index_OBJECT.Prev_slider_1();
    clearTimeout(Index_OBJECT.time_slider_1);
    clearInterval(Index_OBJECT.timer_slider_1);


    Index_OBJECT.time_slider_1 = setTimeout(
        Index_OBJECT.Up_slider_1_time, 15000);
};
Index_OBJECT.Next_slider_1_click = () => {
    Index_OBJECT.Next_slider_1();
    clearTimeout(Index_OBJECT.time_slider_1);
    clearInterval(Index_OBJECT.timer_slider_1);


    Index_OBJECT.time_slider_1 = setTimeout(
        Index_OBJECT.Up_slider_1_time, 15000);
};


Index_OBJECT.Next_slider_1 = () => {

    var slider_1 = document.getElementById("Index_one_block_slider_1_id_visible");
    if (slider_1 == null)
        return;
    slider_1.innerHTML = "";

    for (var i = 0; i < Index_OBJECT.Count_block_for_slider_1 && i < Index_OBJECT.Count_in_list_slider_1; ++i) {
        var slider_1_block_1 = document.getElementById("Index_one_block_slider_1_id" + Index_OBJECT.First_num_slider_1);

        if (slider_1_block_1 != null) {
            slider_1.innerHTML += slider_1_block_1.innerHTML;
            ++Index_OBJECT.First_num_slider_1;

        }
        else {
            Index_OBJECT.First_num_slider_1 = 0;//First_num_slider_1 + 1 - 6
            slider_1_block_1 = document.getElementById("Index_one_block_slider_1_id" + Index_OBJECT.First_num_slider_1);
            if (slider_1_block_1 != null) {
                slider_1.innerHTML += slider_1_block_1.innerHTML;
                ++Index_OBJECT.First_num_slider_1;
            }
            else {
                return;
            }
        }
    }
    //var tmp_s4et = First_num_slider_1 + 1 - 6;//след элемент

    if (Index_OBJECT.First_num_slider_1 >= 1)
        Index_OBJECT.First_num_slider_1 -= 1;
};
Index_OBJECT.Prev_slider_1=()=> {
    var num = Index_OBJECT.First_num_slider_1 + 2 * (1 - Index_OBJECT.Count_block_for_slider_1);

    if (num < 0)
        num = Index_OBJECT.Count_in_list_slider_1 + num;
    Index_OBJECT.First_num_slider_1 = num;

    Index_OBJECT.Next_slider_1();




}
Index_OBJECT.Up_slider_1_time=()=> {
    Index_OBJECT.time_slider_1 = setTimeout(function () {
        Index_OBJECT.timer_slider_1 = setInterval(function () {
            Index_OBJECT.Next_slider_1();
        }, 6000);
    }, 6000);
}
Index_OBJECT.Load_slider = () => {
    Index_OBJECT.up_slider();
    //


    var width_ = document.documentElement.clientWidth;
    Index_OBJECT.Count_block_for_slider_1 = Math.trunc((width_ - 200) / 165) - 1;

    Index_OBJECT.Count_in_list_slider_1 = +document.getElementById("count_obg_slider_1_id").value;
    Index_OBJECT.Next_slider_1();


    if (Index_OBJECT.Count_block_for_slider_1 < Index_OBJECT.Count_in_list_slider_1) {


        Index_OBJECT.Up_slider_1_time();

    }
    else {
        Index_OBJECT.Hide_prev_next_block_slider_1();
    }

}
Index_OBJECT.Hide_prev_next_block_slider_1=()=> {
    var prev = document.getElementById("Index_external_button_prev_slider_1_id");
    var next = document.getElementById("Index_external_button_next_slider_1_id");
    if(prev!=null)
    prev.innerHTML = "";
    if (next != null)
    next.innerHTML = "";
}
Index_OBJECT.load_page = () => {
    
    Index_OBJECT.Load_slider();
    // менюшка справа выезжает
    var timeout_menu;
    
    var right_menu = document.getElementById("layout_panel_move_block_id");
    timeout_menu = setTimeout(function () {
        right_menu.style.right = '0';
        timeout_menu = setTimeout(function () { right_menu.style.cssText = '' }, 4000);
    },6000);
    
    //
    
}
document.addEventListener("DOMContentLoaded", Index_OBJECT.load_page);