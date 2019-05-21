// if notification get clicked..
$(".notification-container").click(function ()
{
    // get 'closed' class to container
    var container = this;
    
    $.post("/Resident/Notifications/ConfirmNotification",
        {
            notificationId: $(container).data("id")
        })
        .done(function ()
        {
            $(container).addClass("closed");
        });
});