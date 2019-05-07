
function is_email_valid(email)
{
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

function preview_image({ file_input_id, image_preview_id } = {})
{
    var file = document.getElementById(file_input_id).files[0];
    if (file === undefined) return;

    var fileReader = new FileReader();
    fileReader.readAsDataURL(file);

    fileReader.onload = function (fileReaderEvent)
    {
        document.getElementById(image_preview_id).src = fileReaderEvent.target.result;
    };
};