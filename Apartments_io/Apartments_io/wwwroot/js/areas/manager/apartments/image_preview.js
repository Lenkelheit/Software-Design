const photo_input_id = 'apartment-photo-input';
const default_img_src = $("#preview-image").attr('src');

document.getElementById(photo_input_id).onchange = function ()
{
    validate_file_extension
        ({
            file_input_id: photo_input_id,
            allowed_extension: ["jpg", "jpeg", "png", "gif"]
        });

    preview_image
        ({
            file_input_id: photo_input_id,
            image_preview_id: 'preview-image',
            default_img_src: default_img_src
        });
};
