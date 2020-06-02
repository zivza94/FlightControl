let toastr = require('toastr');

module.exports = function AlertView() {
    toastr.options.closeButton = true;
    toastr.options.timeOut = 6000;
    toastr.options.extendedTimeOut = 120;
    this.AddAlert = (alert) => {
        toastr.error(alert);
    }
}