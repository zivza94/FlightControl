let ReqFlightPlan = require('./req-flight-plan');
let ReqFlights = require('./req-flights');
let DBHandler = require('./db-handler');
let AlertsHandler = require('./alerts-handler');
let FilesHandler = require('./files_handler');
let FlightInfoView = require('./view/flight-info-view');
let FlightsListView = require('./view/flights-list-view');
let AlertView = require('./view/alert-view');
let MapView = require('./view/map-view');

let {
  DateToStr
} = require('./services/utils');
let op = {
  dbFlightUpdate: 1,
  dbFlightsListUpdate: 2,
  errFlightUpdate: 3,
  errFlightListUpdate: 4,
};

const x = 'Hello';

let self = this;
let db = new DBHandler();
let alerts = new AlertsHandler();
let filesHandler = new FilesHandler();
let flightListView = new FlightsListView();
let flightPlanApi = new ReqFlightPlan(db, alerts);
let flightsApi = new ReqFlights(db, alerts);
let infoView = new FlightInfoView();
let alertView = new AlertView();
let mapView = new MapView();
self.markerClicked = false;

$(document).ready(function () {
  $('#flightInfo').hide();
  let map = mapView.Initialize();
  map.on('click', function () {
    if (!self.markerClicked) {
      mapView.RemoveClick();
      flightListView.RemoveClick();
      $('#flightInfo').hide();
    }
    self.markerClicked = false;
  });
  // db and alerts will notify the index whenever they will get updated
  db.Subject.Attach(self);
  alerts.Subject.Attach(self);

  self.GetAllFlights();
  // get all the flights from the server every 2 seconds
  setInterval(self.GetAllFlights, 2000);

  $('input[type="file"]').change(function (e) {
    let file = e.target.files[0];
    if (file != undefined) {
      filesHandler.AddFile(file);
    }
  });

  $('#submitBtn').click(function () {
    let file = filesHandler.GetFile();
    flightPlanApi.SendFlightPlan(file);
    $(this).attr('disabled', true);
    $('input[type="file"]').val('');
  });
});

this.Update = (type, obj) => {
  if (type === op.dbFlightUpdate) {
    infoView.UpdateFlight(obj);
    mapView.UpdateRoot(obj.id, obj.flight_root);
  }

  if (type === op.dbFlightsListUpdate) {
    $('#flightsList').empty();
    if (obj != null) {
      flightListView.UpdateFlights(obj);
      mapView.UpdateFlights(obj);
    }
    $('.list-group-item').on('click', function () {
      let id = $(this).attr('id');
      ClickFlightUpdate(id);
    });

    let markers = $('.leaflet-marker-icon');
    markers.on('click', function (e) {
      self.markerClicked = true;
      let el = $(e.srcElement || e.target);
      id = el.attr('id').replace('_m', '');
      ClickFlightUpdate(id);
    });

    $('.delete').on('click', function () {
      let id = $(this).parent().attr('id');
      flightListView.DeleteFlight(id);
      flightsApi.DeleteFlight(id);
      if (infoView.GetId() === id) {
        $('#flightInfo').hide();
      }
      mapView.DeleteFlight(id);
    });
  }

  if (type === op.errFlightUpdate || type === op.errFlightListUpdate) {
    alertView.AddAlert(obj);
  }
};

this.GetAllFlights = () => {
  flightsApi.GetFlights(DateToStr(new Date()));
};

function ClickFlightUpdate(id) {
  mapView.UpdateClick(id);
  flightListView.UpdateClick(id);
  flightPlanApi.GetFlightPlan(id);
}