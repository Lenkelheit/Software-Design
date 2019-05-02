
function is_email_valid(email)
{
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

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