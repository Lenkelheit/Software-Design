$('#login-form').submit(function (event)
{
    event.preventDefault();

    // gather data
    var form = this;
    const email = $(form).find("#email-input").val().trim();
    const password = $(form).find("#password-input").val().trim()

    // validate
    if (!is_email_valid(email))
    {
        ohSnap('Email is not valid', { color: 'red' });
        return;
    }

    // send request
    $.ajax(
    {
        url: $(form).data('url'),
        type: 'POST',
        data:
        {
            Email: email,
            Password: password
        },
        error: function (respone)
        {
            ohSnap(respone.responseText, { color: 'red' });
        },
        success: function (respone)
        {
            location.reload();
        }
    });
});