// email = string
function is_email_valid(email)
{
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

// file_input_id    = string
// image_preview_id = string
// default_img_src  = string
function preview_image({ file_input_id, image_preview_id, default_img_src } = {})
{
    var file = document.getElementById(file_input_id).files[0];
    if (file === undefined)
    {
        document.getElementById(image_preview_id).src = default_img_src;
        return;
    }

    var fileReader = new FileReader();
    fileReader.readAsDataURL(file);

    fileReader.onload = function (fileReaderEvent)
    {
        document.getElementById(image_preview_id).src = fileReaderEvent.target.result;
    };
};

// file_input_id     = string
// allowed_extension = array of string
function validate_file_extension({ file_input_id, allowed_extension } = {})
{
    var file_input = document.getElementById(file_input_id);
    var file_name = file_input.value;
    var dot_index = file_name.lastIndexOf(".") + 1;
    var file_extension = file_name.substr(dot_index, file_name.length).toLowerCase();

    // if match any of array return
    for (var i = 0; i < allowed_extension.length; ++i)
    {
        if (file_extension == allowed_extension[i].toLowerCase())
        {
            return;
        }
    }

    // no matches, reset input
    file_input.value = "";
}
