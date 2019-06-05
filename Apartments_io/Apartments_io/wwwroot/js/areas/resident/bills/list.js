// if any bill get clicked...
$(".present-bill, .past-bill").each(function ()
{
    var bill_container = this;

    $(bill_container).find("button").click(function ()
    {
        // send request and remove bill from list
        $.post("/Resident/Bills/PayBill/",
            {
                billId: $(this).data("id")
            })
            .done(function ()
            {
                $(bill_container).fadeOut(300, function () { $(this).remove(); });
            })
            .fail(function ()
            {
                ohSnap("Failed to pay the bill", { color: 'red' });
            });
    });
});