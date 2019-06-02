// loads apartments list for seleced user

// combobox_selector = string
// user_id = int
// callback = function
function load_apartments(combobox_selector, user_id, callback)
{
    $.post('/Manager/Apartments/GetApartmentsList/',
        {
            userId: user_id,
        },
        function (data)
        {
            // clear combo box
            const combobox = $(combobox_selector);
            combobox.empty();

            // fill in combo box with new data
            for (var i = 0; i < data.length; ++i)
            {
                combobox.append('<option value="' + data[i].id + '">' + data[i].name + '</option>');
            }

            // call callback function
            if (typeof callback === 'function') callback();
        });
}

// loads image for selected apartment

// image_selector = string
// apartment_id = int
function load_apartment_image(image_selector, apartment_id)
{
    if (typeof apartment_id === 'undefined') return;

    // get image src from server...
    $.post('/Manager/Apartments/GetApartmentImage/',
        {
            apartmentId: apartment_id
        },
        function (data)
        {
            // .. and sets src to image
            $(image_selector).attr('src', data)
        });
}

// when document is ready
// loads apartments for current user
// and load image for first apartment
$(document).ready()
{
    load_apartments("#apartments", $("#residents option:selected").val(),
        function ()
        {
            load_apartment_image("#apartment-image", $("#apartments option:selected").val());
        });

}
// on resident change
// reloads apartments for current user
// and reload image for first apartment
$("#residents").change(function ()
{
    load_apartments("#apartments", $("#residents option:selected").val(), function ()
    {
        load_apartment_image("#apartment-image", $("#apartments option:selected").val());
    });
});
// on apartments change
// reload image for first apartment
$("#apartments").change(function ()
{
    const apartment_id = $(this).children("option:selected").val();

    load_apartment_image("#apartment-image", apartment_id);
});

// CREATE
$("#create-new-bill").click(function ()
{

    // get data
    var resident_id = $("#residents option:selected").val();
    var apartment_id = $("#apartments").children("option:selected").val();
    var bill_date = $("#bill-date").val();
    

    // validate data
    if (!resident_id)
    {
        ohSnap("Select resident", { color: 'red' });
        return;
    }
    if (!apartment_id)
    {
        ohSnap("Select apartment", { color: 'red' });
        return;
    }
    if (!bill_date)
    {
        ohSnap("Wrong date format", { color: 'red' })
        return;
    }
    if (new Date(bill_date) < new Date(Date.now()))
    {
        ohSnap("You can not create bill earlier than today", { color: 'red' })
        return;
    }
    

    // send data
    $.post("/Manager/Bills/CreateNewBill/",
        {
            residentId: resident_id,
            apartmentId: apartment_id,
            billDate: bill_date
        })
        .done(function () { location.reload(); })
        .fail(function () { ohSnap("Fail to create new bill", { color: 'red' }); });
});

// UPDATE

// update bill
function update_bill(bill_id, select_payment_status_value, bill_date)
{
    $.post("/Manager/Bills/UpdateBill/",
        {
            billId: bill_id,
            paymentStatus: select_payment_status_value,
            billDate: bill_date
        },
        function (data)
        {
            // redirect is server send such request
            // (on overdue)
            if (data.isRedirect) window.location.href = data.location;
        })
        .done(function () { ohSnap('Successfully updated', { color: 'green' }); })
        .fail(function () { ohSnap("Fail to update bill", { color: 'red' }); });
}

$("#bills-list tr").each(function ()
{
    const row = this;

    $(row).find(".btn-update").click(function ()
    {
        // get select value
        const bill_id = $(row).data("id");
        const select_payment_status_value = $(row).find('.bill-status option:selected').val();
        const bill_date = $(row).find(".bill-date").val();

        // validate date
        if (!bill_date)
        {
            ohSnap("Wrong date format", { color: 'red' })
            return;
        }
        if (new Date(bill_date) < new Date(Date.now()))
        {
            ohSnap("You can not set bill's end date earlier than today", { color: 'red' })
            return;
        }

        // confirm updating for overdue
        if (select_payment_status_value == "2")
        {
            bootbox.confirm(
                {
                    message: "In this case, the renter will lose an apartment. Are you sure?",
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
                        // send update request to the server
                        if (result) update_bill(bill_id, select_payment_status_value, bill_date);
                    }
                });
        }
        else // just send
        {
            // send update request to the server
            update_bill(bill_id, select_payment_status_value, bill_date);
        }
        
    });
});
