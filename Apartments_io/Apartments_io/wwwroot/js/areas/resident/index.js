
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
