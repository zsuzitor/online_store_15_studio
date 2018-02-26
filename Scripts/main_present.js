function test_11() {
   
    var block = document.getElementById("layout_active_panel_id");
    var width = document.documentElement.clientWidth;
    var height = document.documentElement.clientHeight;
    var str = "";
    var left=width/2-300;
    var top=height/2-400;
    str += "<div onclick='close_present()' style='width:" + width + "px;height:" + height + "px;' id='Main_present_block_show_all_block'>";
    str += " <div style='margin-top:" + top + "px; margin-left:" + left + "px;' onclick='event.stopPropagation()' class='Main_present_block_show_all_block_v'><div class='Main_present_block_show_image'></div>";
    str += "<div class='Main_present_block_show_form'>";
    
    var hidden = document.getElementById("Main_present_block_hideen_check").value;
    
    if (hidden == "false") {
        str += "<p>Подпишитесь на рассылку и получите скидку</p>";
        str += " <form method='post' action='/Home/Main_present_block_save'><label>Имя:</label><input type='text' name='Name' /><br>";
        str += "<label>Email*:</label><input type='text' name='Email' /><br><input type='button' value='Подписаться' /></form>";
    }
    if (hidden == "") {
        str += "<p>1) Зарегистрируйтесь</p>";
        str +="<p>2) Подпишитесь на рассылку и получите скидку</p>";
        str += "<h1><a href='/Account/Register'>Зарегистрироваться</a></h1>";
        
    }
   


   

    str += "</div></div></div>";
    block.innerHTML = str;
}


function close_present() {
    var block = document.getElementById("layout_active_panel_id");
    block.innerHTML = "";


}