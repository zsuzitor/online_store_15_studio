﻿var Index_OBJECT = {};
Index_OBJECT.slider1;
Index_OBJECT.slider2;
Index_OBJECT.slider3;
Index_OBJECT.up_slider = () => {
    var count_img_in_list_ = document.getElementById("count_id_slider_main_index").value;
    var width = document.documentElement.clientWidth;
    if (width < 960) {
        width = 960;
    }
    
    var width_sl1 = width * 0.8 - width * 0.8 * 0.1;
    Index_OBJECT.slider1 = new Slider_(count_img_in_list_, "Index_block_type_1_id", "Index_main_slider_one_slide_id", 1000, width_sl1, 500, 0, true, "Index_OBJECT.slider1");
    Index_OBJECT.slider1.up();

    //
    var width_sl2 = document.getElementById("Index_lvl3_slider_id").offsetWidth;
    Index_OBJECT.slider2 = new Slider_(count_img_in_list_, "Index_lvl3_slider_id", "Index_slider_one_slide_id_lvl2", 1000, width_sl2, 800, 0, false, "Index_OBJECT.slider2");
    Index_OBJECT.slider2.up();

    //
    var count_img_in_list_3 = document.getElementById("count_obg_slider_3").value;
    if (count_img_in_list_3 > 0) {
        
   var width_sl3 = document.getElementById("Index_lvl4_slider_id").offsetWidth;
   Index_OBJECT.slider3 = new Slider_(document.getElementById("count_obg_slider_3").value, "Index_lvl4_slider_id", "Index_slider_one_slide_id_lvl3", 1000, width_sl3, 800, 0, false, "Index_OBJECT.slider3");
   Index_OBJECT.slider3.up();
  
    }
   
   
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