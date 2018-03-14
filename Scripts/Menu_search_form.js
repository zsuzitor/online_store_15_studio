var Menu_search_form_OBJECT = {};
Menu_search_form_OBJECT.html_for_menu_search_form = "";

Menu_search_form_OBJECT.request_from_server_load_form = (request, status) => {
    var block = document.getElementById('Menu_search_form_main_form_id');
    block.style.display = 'block';
    block.innerHTML = "<div class='Menu_search_form_click_div' onclick='Menu_search_form_OBJECT.show_form_search()'></div>";

    Menu_search_form_OBJECT.html_for_menu_search_form = request.responseJSON;//JSON.parse(request);

}
Menu_search_form_OBJECT.show_form_search = () => {
    var block = document.getElementById("layout_active_panel_id");
    var width = document.documentElement.clientWidth;
    var height = document.documentElement.clientHeight;
    var str = "";
    var left = width / 2 - 150;
    var top = height / 2 - 150;
    str += "<div onclick='close_present()' style='width:" + width + "px;height:" + height + "px;' id='Main_block_show_all_block'>";
    str += " <div style='margin-top:" + top + "px; margin-left:" + left + "px;' onclick='event.stopPropagation()' class='Main_help_block_show_all_block_v'>";
    str += "<div>";
    str += Menu_search_form_OBJECT.html_for_menu_search_form;
    str += "Сдесь форма для расширенного поиска";
    str += "(Main_help_OBJECT.load_main_block)";

    str += "</div></div></div>";
    block.innerHTML = str;
}
/* function gggggth_start() {
     var block = document.getElementById('Menu_search_form_main_form_id');
     block.style.display = 'block';

     OnBegin = "gggggth_start",

 }*/