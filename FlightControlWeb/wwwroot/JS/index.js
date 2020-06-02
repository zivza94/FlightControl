let getFlights = () => {
    console.log("in click");
    $.ajax({
            type: "GET",
            dataType: "text",
            url: '/api/Flights?relative_to=2020-12-26T23:55:00Z',
            success: function (data) {
                console.log("GetFlights===> ");
                console.log(data);
                //db.FlightsListUpdate(data);
            },
            failure: function (errMsg) {
                console.log(errMsg);
            }
        })
}
