// UPDATE MANAGER
$("#users-list table tr").each(function ()
{
    var row = this;

    // UPDATE
    $(row).find(".btn-update").click(function ()
    {
        // get data
        var manager_id =  $(row).find('[name="UserManager"]').children("option:selected").val();

        // validate data
        if (manager_id == -1)
        {
            ohSnap('Select manager', { color: 'red' });
            return;
        }

        // send to server
        $.post('/Administrator/Users/UpdateManager/',
            {
                userId: $(row).data('user-id'),
                managerId: manager_id
            })
            .done(function ()
            {
                ohSnap('Successfully updated', { color: 'green' });
            })
            .fail(function ()
            {
                ohSnap('Something went wrong', { color: 'red' });
            });
    });
});