// FILTER SUBMIT
$("#filter-form").submit(function (event)
{
    var minPrice = $("#filter-min-price").val();
    var maxPrice = $("#filter-max-price").val();

    // validate
    if (minPrice > maxPrice)
    {
        event.preventDefault();
        ohSnap("Min price can not be higher than max price", { color: 'red' });
    }
});
