// FILTER SUBMIT
$("#filter-form").submit(function (event)
{
    var minPrice = Number($("#filter-min-price").val());
    var maxPrice = Number($("#filter-max-price").val());

    // validate
    if (minPrice > maxPrice)
    {
        event.preventDefault();
        ohSnap("Min price can not be higher than max price", { color: 'red' });
    }
});
