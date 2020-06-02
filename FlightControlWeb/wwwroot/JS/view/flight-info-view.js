module.exports = function FlightInfoView() {
    let id = null;

    this.GetId = () => {
        return id;
    }
    this.Show = () => {
        $("#flightInfo").show();
    }
    this.UpdateFlight = (data) => {

        $("#company-name").text(data.company_name);
        id = data.id;
        $("#flight-id").text(id);
        $("#passengers").text(data.passengers);
        $("#total-distance").text(data.total_distance);
        $("#from").text(data.from);
        $("#to").text(data.to);
        $("#start-time").text(data.start_time);
        $("#end-time").text(data.end_time);
        $("#start-date").text(data.start_date);
        $("#end-date").text(data.end_date);
        $("#percent-distance").attr("style", "width:" + data.percent_distance + "%");
        $("#time-until-now").text(data.time_until_now);
        $("#total-time").text(data.total_time);
        $("#time-now").text(data.now_hour);
        this.Show();
    }

}