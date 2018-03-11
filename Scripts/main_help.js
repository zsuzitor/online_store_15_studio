Main_help_OBJECT = {};



Main_help_OBJECT.test_11 = () => {

    var block = document.getElementById("layout_active_panel_id");
    var width = document.documentElement.clientWidth;
    var height = document.documentElement.clientHeight;
    var str = "";
    var left = width / 2 -150;
    var top = height / 2 -150;
    str += "<div onclick='close_present()' style='width:" + width + "px;height:" + height + "px;' id='Main_block_show_all_block'>";
    str += " <div style='margin-top:" + top + "px; margin-left:" + left + "px;' onclick='event.stopPropagation()' class='Main_help_block_show_all_block_v'>";
    str += "<div>";
    str+="<form action='/Home/Main_help_block' method='post'>";
    str+="<label>Имя</label>";
    str+="<input class='text-box single-line' id='Name' name='Name' type='text'";
    str+="<label>Номер телефона +7</label>";
    str+="<input class='text-box single-line' id='Phone' name='Phone' type='text'";
    str+="<label>Дата и время когда вам удобно</label>";
    str+="<input class='text-box single-line' id='Date_comfortable' name='Date_comfortable' type='text'";
    str+="<label>Оставьте сообщение</label>";
    str+="<input type='submit' class='submit' value='Отправить'";
    str+="</form>";
   




    str += "</div></div></div>";
    block.innerHTML = str;
}

/*
 @using (Html.BeginForm("Main_help_block","Home"))
{
    @Html.Label("Имя")
    @Html.Editor("Name")
    @Html.Label("Номер телефона +7")
    @Html.Editor("Phone")
    @Html.Label("Дата и время когда вам удобно")
    @Html.Editor("Date_comfortable")
    @Html.Label("Оставьте сообщение")
    @Html.Editor("Message")
      


    <input type="submit" class="submit" value="Отправить" />
    }
    */