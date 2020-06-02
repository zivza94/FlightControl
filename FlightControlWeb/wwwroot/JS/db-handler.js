let ObserverHandler = require('./observers-handler')
let {
  GetDistanceFromLatLonInKm
} = require('./services/utils')
let {
  DateToStr
} = require('./services/utils')

module.exports = function DBHandler() {
  this.Subject = new ObserverHandler()
  let currFlight = {
    now_hour: null,
    id: null,
    passengers: null,
    company_name: null,
    from: null,
    to: null,
    start_time: null,
    end_time: null,
    start_date: null,
    end_date: null,
    total_distance: null,
    percent_distance: '10',
    time_until_now: null,
    total_time: null,
    flight_root: null,
  }
  let flightsList
  this.FlightUpdate = (id, data) => {
    let obj = JSON.parse(data)
    currFlight.id = id
    currFlight.passengers = obj.passengers
    currFlight.company_name = obj.company_name
    currFlight.from =
      '(' +
      obj.initial_location.latitude +
      ', ' +
      obj.initial_location.longitude +
      ')'
    this.IterateSegments(obj)
    currFlight.start_date = obj.initial_location.date_time.substr(0, 11)
    currFlight.start_time = obj.initial_location.date_time.substr(11, 13)
    this.Subject.Notify(1, currFlight)
  }

  this.FlightsListUpdate = (data) => {
    flightsList = JSON.parse(data)
    if (flightsList.length === undefined) {
      this.Subject.Notify(2, null)
    } else {
      this.Subject.Notify(2, flightsList)
    }
  }

  this.IterateSegments = (obj) => {
    let root = []
    let totalDist = 0
    let latBefore = obj.initial_location.latitude,
      lonBefore = obj.initial_location.longitude
    let point = [latBefore, lonBefore]
    root.push(point)

    let totalSec = 0
    for (const segment of obj.segments) {
      totalDist += GetDistanceFromLatLonInKm(
        latBefore,
        lonBefore,
        segment.latitude,
        segment.longitude
      )
      latBefore = segment.latitude
      lonBefore = segment.longitude
      totalSec += segment.timespan_seconds
      point = [latBefore, lonBefore]
      root.push(point)
    }
    currFlight.total_distance = totalDist.toFixed(2)
    let startDate = new Date(obj.initial_location.date_time)
    let untilNowSec = this.UpdateNowDate(startDate)
    this.UpdateEndDate(startDate, totalSec)
    currFlight.total_time = this.ChangeFormat(totalSec)
    currFlight.to = '(' + latBefore + ', ' + lonBefore + ')'
    currFlight.percent_distance = ((untilNowSec / totalSec) * 100).toFixed(2)
    currFlight.flight_root = root
  }

  this.UpdateEndDate = (date, seconds) => {
    date.setSeconds(date.getSeconds() + seconds)
    date = DateToStr(date)
    currFlight.end_date = date.substr(0, 11)
    currFlight.end_time = date.substr(12, 13)
  }

  this.UpdateNowDate = (startDate) => {
    let nowDate = new Date()
    let utcDate = DateToStr(nowDate)
    currFlight.now_hour = utcDate.substr(11, 11)
    let sec = this.CalcSecBetweenDates(nowDate, startDate)
    currFlight.time_until_now = this.ChangeFormat(sec)
    return sec
  }
  this.GetFlightById = (id) => {
    let array = flightsList.filter(function (flightsList) {
      return flightsList.flight_id == id
    })
    return array[0]
  }

  this.CalcSecBetweenDates = (start, end) => {
    return Math.abs((end.getTime() - start.getTime()) / 1000)
  }

  this.ChangeFormat = (seconds) => {
    var hours = Math.floor(seconds / 60 / 60)
    var minutes = Math.floor(seconds / 60) - hours * 60
    var seconds = seconds % 60
    return (
      hours.toString().padStart(2, '0') +
      ':' +
      minutes.toString().padStart(2, '0') +
      ':' +
      seconds.toFixed(0).toString().padStart(2, '0')
    )
  }
}