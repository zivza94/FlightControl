module.exports = function FlightsListView() {
    let deleteIcon2 = "<div class='delete float-right' title='Delete' data-toggle='tooltip'>" +
        "<i class='material-icons'>" + "&#xE872;" + "</i></div>";

    let clickedFlight = null;
    this.UpdateClickedFlight = (id) => {
        clickedFlight = id;
    }
    this.UpdateFlights = (list) => {
        for (const flight of list) {
            let id = $('<b>', {
                text: flight.flight_id
            });
            let company = $('<span>', {
                text: flight.company_name
            });
            let item = $('<li>', {
                class: "list-group-item",
                id: flight.flight_id
            })
            item.append(id);
            item.append("   ");
            item.append(company);
            if (flight.is_external == false) {
                item.append(deleteIcon2);
            }
            $("#flightsList").append(item);
        }

        $("#" + clickedFlight).toggleClass('active');

    }
    this.DeleteFlight = (id) => {
        $("#" + id).remove();
    }

    this.UpdateClick = (id) => {
        clickedFlight = id;
        $('.active').removeClass('active');
        $("#" + id).toggleClass('active');
    }

    this.RemoveClick = () => {
        $("#" + clickedFlight).toggleClass('active');
        clickedFlight = null;
    }
}