let ObserverHandler = require('./observers-handler')

module.exports = function AlertsHandler() {
    this.Subject = new ObserverHandler()

    this.FlightUpdate = (jqXHR, exception) => {
        let msg
        if (jqXHR.status === 0) {
            msg = 'Not Connected.\n Verify Network.'
        } else if (jqXHR.status == 404) {
            msg = 'Not Found, ' + jqXHR.responseText
        } else if (jqXHR.status == 400) {
            msg = 'Bad Request, ' + jqXHR.responseText
        } else if (exception === 'parsererror') {
            msg = 'Requested JSON parse failed.'
        } else if (exception === 'timeout') {
            msg = 'Time out error.'
        } else if (exception === 'abort') {
            msg = 'Ajax request aborted.'
        } else {
            'Uncaught Error.\n' + jqXHR.responseText
        }
        this.Subject.Notify(3, msg)
    }
    this.FlightsListUpdate = (jqXHR, exception) => {
        let msg
        if (jqXHR.status === 0) {
            msg = 'Not Connected.\n Verify Network.'
        } else if (jqXHR.status == 404) {
            msg = 'Not Found, ' + jqXHR.responseText
        } else if (jqXHR.status == 400) {
            msg = 'Bad Request, ' + jqXHR.responseText
        } else if (exception === 'parsererror') {
            msg = 'Requested JSON parse failed.'
        } else if (exception === 'timeout') {
            msg = 'Time out error.'
        } else if (exception === 'abort') {
            msg = 'Ajax request aborted.'
        } else {
            'Uncaught Error.\n' + jqXHR.responseText
        }
        this.Subject.Notify(4, msg)
    }
}