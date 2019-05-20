// UPDATE MANAGER
$("#users-list table tr").each(function ()
{
    var row = this;

    // UPDATE
    $(row).find(".btn-update").click(function ()
    {
        // send to server
        $.post('/Administrator/Users/UpdateManager/',
            {
                userId: $(row).data('user-id'),
                managerId: $(row).find('[name="UserManager"]').children("option:selected").val()
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