class Slider_ {

    constructor(count_img_in_list_, id_main_block_, part_id_one_img_, speed_load_, width_slide_, height_slide_, timer_change_, horizontal_bool_, object_name_) {
        this.count_img_in_list = count_img_in_list_;
        this.id_main_block = id_main_block_;
        this.part_id_one_img = part_id_one_img_;
        this.speed_load = speed_load_;
        this.width_slide = width_slide_;
        this.height_slide = height_slide_;
        this.horizontal_bool = horizontal_bool_;
        this.object_name = object_name_;
        this.timer_change = timer_change_;

        this.interval_changes;// переменная для интервала смены слайдов
        this.current_num_img = 0;//текущий номер слайда
        this.activated_slider = false;//(если пользователь что то нажал но код еще не выполнился true)
        this.bias = 0;//смещение блока с 3мя слайдами
        this.start;//таймер для анимаци
        this.timer;//таймер для анимаци
        this.animation = false;//разрешение пользователем анимации
    }

    //-----

    load_sl() {
        var fff = () => {
            var res = "";
            let ttt = "'width:" + slider.width_slide + "px; " + "height:" + slider.height_slide + "px;'>";
            if (slider.horizontal_bool) {
                res= "<div class='_Slider_div_inline_block' style=" + ttt + img.innerHTML + "</div>"
            }
            else {
                res = "<div style=" + ttt + img.innerHTML + "</div>"
            }
            return res;
        }
        var slider = this;
        var div_ = document.getElementById("_Slider_3_view_block_id");
        if (slider.current_num_img < 0) {
            slider.current_num_img = +slider.count_img_in_list + +slider.current_num_img;
        }
        var img = document.getElementById(slider.part_id_one_img + (slider.current_num_img - 1));


        
        if (img != null) {
           
            //div_.innerHTML += "<div class='_Slider_div_inline_block' style=" + ttt  +img.innerHTML + "</div>"
            div_.innerHTML += fff();
        }
        else {
            //с конца
            img = document.getElementById(slider.part_id_one_img + (slider.count_img_in_list - 1));
            div_.innerHTML += fff();
        }
        img = document.getElementById(slider.part_id_one_img + slider.current_num_img);
        if (img != null) {
            div_.innerHTML += fff();
        }
        else {
            slider.current_num_img = 0;
            img = document.getElementById(slider.part_id_one_img + slider.current_num_img);
            div_.innerHTML += fff();
        }
        img = document.getElementById(slider.part_id_one_img + (slider.current_num_img + 1));
        if (img != null) {
            div_.innerHTML += fff();
        }
        else {
            img = document.getElementById(slider.part_id_one_img + 0);
            div_.innerHTML += fff();
        }
    }

    //старт слайдера
    up() {
        var slider = this;
        var main = document.getElementById(slider.id_main_block);
        var str = "";
        str = " <div id='_Slider_main_block_slider_id' class='_Slider_main_block_slider _Slider_div_inline_block' style='height:";
        str += slider.height_slide + "px; width:" + slider.width_slide + "px;'";
        str += "><div id='_Slider_3_view_block_id' style='";
        if (slider.horizontal_bool) {
            str += "height:100%; width:300%;";
        }
        else {
            str += "height:300%; width:100%;";
        }
        str += "'></div></div>";
        main.innerHTML = str;

        if (slider.timer_change != 0) {
            slider.animation = true;
        }
        slider.reload();
    }
    //обновить слайды+ добавить кнопки и тд
    reload() {
        var slider = this;
        var div_ = document.getElementById("_Slider_3_view_block_id");
        if (slider.horizontal_bool) {
            slider.bias = -slider.width_slide;
        }
        else {
            slider.bias = -slider.height_slide;
        }
        if (slider.horizontal_bool) {
            div_.style.left = slider.bias + 'px';
        }
        else {
            div_.style.top = slider.bias + 'px';
        }
        div_.innerHTML = "";
        slider.load_sl();
        slider.activated_slider = false;

        //добавление кнопок перехода
        var str = "<div id='_Slider_block_change_slide_id' class='_Slider_block_change_slide'>"
        if (slider.timer_change != 0) {
            str += "<div id='_Slider_cont_timer_id' onclick='" + slider.object_name + ".pause_slider()"
            str += "' class='_Slider_cont_timer";
            if (slider.horizontal_bool) {
                str += " _Slider_div_inline_block";
            }
            str += "'>";
            if (slider.animation) {
                str += "<div class='_Slider_cont_timer_true'></div>";
            }
            else {
                str += "<div class='_Slider_cont_timer_false'></div>";
            }
            str += "</div>";
        }
        for (var i = 0; i < slider.count_img_in_list; ++i) {
            let tmp = "<div  class='_Slider_one_button_change"
            if (slider.horizontal_bool) {
                tmp += " _Slider_div_inline_block";
            }
            tmp += "' onclick='" + slider.object_name + ".num_slide";
            tmp += "(" + i + ")'";
            tmp += ">";
            if (i == slider.current_num_img) {
                tmp += "<div class='_Slider_one_button_change_curr'></div>";
            }
            tmp += "</div>";
            str += tmp;
        }
        str += "</div>";
        div_.innerHTML += str;
        var but = document.getElementById("_Slider_block_change_slide_id");
        if (slider.horizontal_bool) {
            but.style.top = '0px';
            but.style.left = slider.width_slide + 20 + 'px';
        }
        else {
            but.style.top = slider.height_slide + 20 + 'px';
            but.style.left = 20 + 'px';
        }

        //добавление кнопок перехода: влево вправо
        str = "<div id='_Slider_next_prev_block_id'";
        if (!slider.horizontal_bool) {
            str += " class='_Slider_next_prev_block_tr'";
        }
        str += ">";
        str += "<div class='_Slider_next_prev_block'>";
        str += "<div class='_Slider_prev_butt _Slider_div_inline_block' onclick='" + slider.object_name + ".prev()'></div>";
        str += "<div class='_Slider_next_butt _Slider_div_inline_block' onclick='" + slider.object_name + ".next()'></div>";
        str += "</div></div>";
        div_.innerHTML += str;
        but = document.getElementById("_Slider_next_prev_block_id");
        if (slider.horizontal_bool) {
            but.style.left = slider.width_slide * 2 - 200 + "px";
            but.style.top = slider.height_slide - 100 + "px";
        }
        else {
            but.style.top = slider.height_slide * 2 - 150 + "px";
            but.style.left = slider.width_slide - 150 + "px";
        }
        if (slider.timer_change != 0 && slider.animation) {
            slider.interval_changes = setInterval(function () {
                slider.next();
            }, slider.timer_change);
        }
    }

    num_slide(num) {
        var slider = this;
        slider.current_num_img = num;
        clearInterval(slider.interval_changes);
        slider.reload();
    }

    pause_slider() {
        var slider = this;
        slider.animation = !slider.animation;
        var block = document.getElementById('_Slider_cont_timer_id');
        var str = "";
        if (slider.animation) {
            str += "<div class='_Slider_cont_timer_true'></div>";
            slider.interval_changes = setInterval(function () {
                slider.next();
            }, slider.timer_change);
        }
        else {
            str += "<div class='_Slider_cont_timer_false'></div>";
            clearInterval(slider.interval_changes);
        }
        block.innerHTML = str;
    }

    action_slider(a) {
        var slider = this;
        if (!slider.activated_slider) {
            var element = document.getElementById("_Slider_3_view_block_id");
            slider.activated_slider = true;
            slider.start = Date.now();
            slider.timer = setInterval(function () {
                var timePassed = Date.now() - slider.start;
                if (timePassed >= slider.speed_load) {
                    clearInterval(slider.timer);
                    if (a) {
                        ++slider.current_num_img;
                    }
                    else {
                        --slider.current_num_img;
                    }
                    slider.reload();
                    return;
                }
                if (a) {
                    if (slider.horizontal_bool) {
                        slider.bias -= ((slider.width_slide * 20) / slider.speed_load);
                    }
                    else {
                        slider.bias -= ((slider.height_slide * 20) / slider.speed_load);
                    }
                }
                else {

                    if (slider.horizontal_bool) {
                        slider.bias += ((slider.width_slide * 20) / slider.speed_load);
                    }
                    else {
                        slider.bias += ((slider.height_slide * 20) / slider.speed_load);
                    }
                }
                if (slider.horizontal_bool) {
                    element.style.left = slider.bias + 'px';
                }
                else {
                    element.style.top = slider.bias + 'px';
                }
            }, 20);
        }
    }

    clear_button() {
        var but = document.getElementById("_Slider_block_change_slide_id");
        but.innerHTML = "";
        but = document.getElementById("_Slider_next_prev_block_id");
        but.innerHTML = "";
    }

    next() {
        var slider = this;
        slider.clear_button();
        clearTimeout(slider.interval_changes);
        slider.action_slider(true);
        clearInterval(slider.interval_changes);

    }

    prev() {
        var slider = this;
        slider.clear_button();
        clearTimeout(slider.interval_changes);
        slider.action_slider(false);
        clearInterval(slider.interval_changes);


    }
}