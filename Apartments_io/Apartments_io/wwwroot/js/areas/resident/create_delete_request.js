
function create_request(user_id, apartment_id)
{
    return $.post('/Resident/Requests/CreateRequest/',
        {
            userId: user_id,
            apartmentId: apartment_id
        }
    );

}

function delete_request(user_id, apartment_id)
{
    return $.post('/Resident/Requests/DeleteRequest/',
        {
            userId: user_id,
            apartmentId: apartment_id
        }
    );
}


// SEND REQUEST
$(".btn-request").click(function ()
{
    var btn = this;
    
    var user_id = $(this).data("resident-id");
    var apartment_id = $(this).data("apartment-id");

    // create OR delete request
    if ($(this).text() == "Send request")
    {
        create_request(user_id, apartment_id)
        .done(function ()
        {
            $(btn).removeClass("btn-info").addClass("btn-warning").text("Cancel");
        });
    }
    else
    {
        delete_request(user_id, apartment_id)
        .done(function ()
        {
            $(btn).removeClass("btn-warning").addClass("btn-info").text("Send request");
        });
    }
});