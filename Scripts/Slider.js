//div_.innerHTML = div_.innerHTML + "<div height='300px' class='div_inline_block' width='" + this.width_one_image + "px;'>" + img.innerHTML + "</div>";

class slider_horizontal {

    constructor(count_img_in_list_, img_3_block_id_, part_id_one_img_, speed_load_, width_one_image_, timer_change_) {
        this.count_img_in_list = count_img_in_list_;
        this.img_3_block_id = img_3_block_id_;
        this.part_id_one_img = part_id_one_img_;
        this.speed_load = speed_load_;
        this.width_one_image = width_one_image_;
        
        this.current_num_img = 0;
        this.activated_slider = false;
        this.left_slider = 0;
        this.start;
        this.timer;
        //
        this.timer_change = timer_change_;
        this.interval_changes;
        //this.timer_changes;

    }

    //-----

    load_sl() {
        var div_ = document.getElementById(this.img_3_block_id);
        if (this.current_num_img < 0) {
            this.current_num_img = +this.count_img_in_list + +this.current_num_img;
        }
        var img = document.getElementById(this.part_id_one_img + (this.current_num_img - 1));
        //alert(part_id_one_img + (current_num_img - 1));
        if (img != null) {

          
        


            div_.innerHTML += img.innerHTML;
        }
        else {
            //с конца
            img = document.getElementById(this.part_id_one_img + (this.count_img_in_list - 1));
            div_.innerHTML += img.innerHTML;
        }
        img = document.getElementById(this.part_id_one_img + this.current_num_img);
        if (img != null) {
            div_.innerHTML += img.innerHTML;
        }
        else {
            this.current_num_img = 0;
            img = document.getElementById(this.part_id_one_img + this.current_num_img);
            div_.innerHTML += img.innerHTML;
        }
        img = document.getElementById(this.part_id_one_img + (this.current_num_img + 1));
        if (img != null) {
            div_.innerHTML += img.innerHTML;
        }
        else {
            img = document.getElementById(this.part_id_one_img + 0);
            div_.innerHTML += img.innerHTML;
        }
    }
    reload() {
        var div_ = document.getElementById(this.img_3_block_id);
        this.left_slider = -this.width_one_image;

        div_.style.left = this.left_slider + 'px';
        div_.innerHTML = "";
        this.load_sl();
        this.activated_slider = false;



        if (this.timer_change != 0) {
            this.interval_changes = setInterval(function () {
                slider.next();
            }, this.timer_change);
        }



        //

        //
    }
    action_slider(a) {

        if (!this.activated_slider) {
            var element = document.getElementById(this.img_3_block_id);
            this.activated_slider = true;
            this.start = Date.now();

            this.timer = setInterval(function () {

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

                    slider.left_slider -= ((slider.width_one_image * 20) / slider.speed_load);

                }
                else {
                    slider.left_slider += ((slider.width_one_image * 20) / slider.speed_load);
                }

                element.style.left = slider.left_slider + 'px';

            }, 20);

        }

    }
    next() {
        clearTimeout(this.interval_changes);
        this.action_slider(true);

    }
    prev() {
        clearTimeout(this.interval_changes);
        this.action_slider(false);

    }
}



//                ОСТАВИТЬ
/*


var slider;

function up_slider() {
    var count_img_in_list__ = document.getElementById("count_id_slider").value;
    slider = new slider_horizontal(count_img_in_list__, "slider_tt", "test_id_img", 1000, 300);

    slider.reload();
}

document.addEventListener("DOMContentLoaded", up_slider);

div.test_div{
    position:relative;
    height:300px;
    width:300px;
}
div.test_tt1{
     position:relative;
    height:300px;
    width:300px;
     overflow: hidden;
}
#slider_tt{
     position:relative;
    height:300px;
    width:910px;
    top: 0;
    
   
   
}

*/