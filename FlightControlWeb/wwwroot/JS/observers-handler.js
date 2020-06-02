module.exports = function ObserverHandler() {
  var _observers = [];
  this.Attach = (observer) => _observers.push(observer);
  this.Notify = (type, obj) => {
    for (var o of _observers) {
      o.Update(type, obj);
    }
  };
}
