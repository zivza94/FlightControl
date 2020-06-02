module.exports = function ReqFlights(db, alerts) {
  this.GetFlights = (dateTime) => {
    $.ajax({
      type: 'GET',
      dataType: 'text',
      url: '/api/Flights?relative_to=' +
        dateTime +
        '&sync_all',
      success: function (data) {
        db.FlightsListUpdate(data);
      },
      error: function (jqXHR, exception) {
        alerts.FlightsListUpdate(jqXHR, exception);
      },
    });
  };

  this.DeleteFlight = (id) => {
    $.ajax({
      type: 'DELETE',
      dataType: 'text',
      url: '/api/Flights/' + id,
      data: id,
      error: function (jqXHR, exception) {
        alerts.FlightsListUpdate(jqXHR, exception);
      },
    });
  };
};