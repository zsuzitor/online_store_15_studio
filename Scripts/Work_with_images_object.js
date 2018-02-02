var Work_with_images_object_OBJECT = {};
Work_with_images_object_OBJECT.num = 0;

function add_new_block_img() {

    var block = document.getElementById("Work_with_images_object_block_adds_img");
    var str = '<input type="file" name="uploadImage[' + Work_with_images_object_OBJECT.num + ']" accept="image/*,image/jpeg,image/png"/>';
    Work_with_images_object_OBJECT.num += 1;
    block.innerHTML = block.innerHTML + str;


}