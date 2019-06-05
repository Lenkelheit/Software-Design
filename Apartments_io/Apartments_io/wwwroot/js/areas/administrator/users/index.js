
function validate_user(user)
{
    // first name
    if (!user.FirstName)            return { isValid: false, message: 'First name can not be empty' };
    if (user.FirstName.length > 10) return { isValid: false, message: 'First name can not be longer than 10 chars' };

    // last name 10
    if (!user.LastName)            return { isValid: false, message: 'Last name can not be empty' };
    if (user.LastName.length > 10) return { isValid: false, message: 'Last name can not be longer than 10 chars' };


    // email
    if (!user.Email)                    return { isValid: false, message: 'Email can not be empty' };
    if (!is_email_valid(user.Email))    return { isValid: false, message: 'Email is not valid' };
    if (user.Email.length > 25)         return { isValid: false, message: 'Email can not be longer than 25 chars' };


    // password 
    if (!user.Password)             return { isValid: false, message: 'Password can not be empty' };
    if (user.Password.length > 20)  return { isValid: false, message: 'Password can not be longer than 20 chars' };

    return { isValid: true, message: 'Model is valid' };
}

// CREATE
$("#create-user").click(function ()
{
    // get user
    var user = new Object();
    user.FirstName = $("#new_user_first_name").val().trim();
    user.LastName = $("#new_user_last_name").val().trim();
    user.Email = $("#new_user_email").val().trim();
    user.Password = $("#new_user_password").val().trim();
    user.Role = $("#new_user_role").children("option:selected").val();

    // validate
    var validdation_result = validate_user(user);
    if (!validdation_result.isValid)
    {
        ohSnap(validdation_result.message, { color: 'red' });
        return;
    }

    // send to server
    $.post('/Administrator/Users/Create/',
    {
        user: user
    })
    .done(function ()
    {
        location.reload();
    })
    .fail(function (respone)
    {
        ohSnap(respone.responseText, { color: 'red' });
    });
});

function update_user_on_server(user)
{
    $.post('/Administrator/Users/Update/',
    {
        user: user
    })
    .done(function ()
    {
        ohSnap('Successfully updated', { color: 'green' });
    })
    .fail(function (response)
    {
        if (response.responseText && response.status == 400) ohSnap(response.responseText, { color: 'red' });
        else                                                 ohSnap('Something went wrong', { color: 'red' });
    });
}

// UPDATE AND DELETE
$("#users-list table tr").each(function ()
{
    var row = this;

    // UPDATE
    $(row).find(".btn-update").click(function ()
    {
        // get user
        var user = new Object();
        user.Id = $(row).data('user-id');
        user.FirstName = $(row).find('[name="FirstName"]').val().trim();
        user.LastName = $(row).find('[name="LastName"]').val().trim();
        user.Email = $(row).find('[name="Email"]').val().trim();
        user.Password = $(row).find('[name="Password"]').val().trim();
        user.Role = $(row).find('[name="Role"]').children("option:selected").val();
        user.ManagerId = $(row).data('manager-id');
        
        // validate
        var validdation_result = validate_user(user);
        if (!validdation_result.isValid)
        {
            ohSnap(validdation_result.message, { color: 'red' });
            return;
        }

        // send to server
        if (user.Role === 'Deactivated')
        {
            bootbox.confirm(
                {
                    message: "Deactivated users lose all its data. Are you sure?",
                    buttons:
                    {
                        confirm:
                        {
                            label: 'Yes',
                            className: 'btn-success'
                        },
                        cancel:
                        {
                            label: 'No',
                            className: 'btn-danger'
                        }
                    },

                    callback: function (result)
                    {
                        if (result)
                        {
                            // send to server
                            update_user_on_server(user);
                        }
                    }

                });
        }
        else
        {
            // send to server
            update_user_on_server(user);
        }
        
       
    });

    // DELETE
    $(row).find(".btn-delete").click(function ()
    {
        bootbox.confirm(
        {
            message: "Do you realy want to delete this record?",
            buttons:
            {
                confirm:
                {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel:
                {
                    label: 'No',
                    className: 'btn-danger'
                }
            },

            callback: function (result)
            {
                if (result)
                {
                    // send to server
                    $.ajax(
                    {
                        url: '/Administrator/Users/Delete/',
                        type: "Post",
                        data:
                        {
                            id: $(row).data('user-id')
                        },
                        statusCode:
                        {
                            400: function (response)
                            {
                                ohSnap(response.responseText, { color: 'red' });
                            }
                        },
                        fail: function ()
                        {
                            ohSnap('Something went wrong', { color: 'red' });
                        },
                        success: function ()
                        {
                            location.reload();
                        }
                    });
                }
            }

        });
    });

});