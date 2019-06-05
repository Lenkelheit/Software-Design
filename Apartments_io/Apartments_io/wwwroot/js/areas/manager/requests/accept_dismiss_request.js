// ACCEPT
$(".btn-accept").click(function ()
{
    $.post("/manager/Requests/AcceptRequest",
        {
            requestId: $(this).data("request-id")
        })
        .done(function ()
        {
            location.reload();
        })
        .fail(function ()
        {
            ohSnap('Fail', { color: 'red' });
        });
});

// DISMISS
$(".btn-dismiss").click(function ()
{
    $.post("/manager/Requests/DismissRequest",
        {
            requestId: $(this).data("request-id")
        })
        .done(function ()
        {
            location.reload();
        })
        .fail(function ()
        {
            ohSnap('Fail', { color: 'red' });
        });
});