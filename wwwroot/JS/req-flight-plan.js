module.exports = function ReqFlightPlan(db, alerts) {
  this.SendFlightPlan = (flight) => {
    $.ajax({
      type: 'POST',
      contentType: 'application/json; charset=utf-8',
      dataType: 'json',
      url: '/api/FlightPlan',
      data: flight,
      error: function (jqXHR, exception) {
        alerts.FlightUpdate(jqXHR, exception)
      },
    })
  }

  this.GetFlightPlan = (id) => {
    $.ajax({
      type: 'GET',
      dataType: 'text',
      url: '/api/FlightPlan/' + id,
      success: function (data) {
        db.FlightUpdate(id, data)
      },
      error: function (jqXHR, exception) {
        alerts.FlightUpdate(jqXHR, exception)
      },
    })
  }
}