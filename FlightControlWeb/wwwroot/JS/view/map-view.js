let L = require('leaflet')
let filledPlane = L.icon({
    iconUrl: 'https://image.flaticon.com/icons/svg/2996/2996069.svg',
    iconSize: [38, 95], // size of the icon
    popupAnchor: [-3, -76] // point from which the popup should open relative to the iconAnchor
});

let emptyPlane = L.icon({
    iconUrl: 'https://image.flaticon.com/icons/svg/2996/2996027.svg',
    iconSize: [50, 110], // size of the icon
    popupAnchor: [-3, -76] // point from which the popup should open relative to the iconAnchor
});

module.exports = function MapView() {
    let planes = {};
    let map;
    let clickedFlight = null;
    let rootOfClickedFlight = null;

    this.Initialize = () => {
        // Create the map
        map = L.map('map').setView([41.3921, 2.1705], 2);
        // Use OpenStreetMap tiles and attribution
        var osmTiles = 'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
        var attribution = '© OpenStreetMap contributors';
        // Create the basemap and add it to the map
        L.tileLayer(osmTiles, {
            maxZoom: 18,
            attribution: attribution
        }).addTo(map);
        //var polyline = L.polyline(polylinePoints).addTo(map);
        return map;
    }

    this.UpdateFlights = (list) => {
        Object.entries(planes).forEach(function ([key, value]) {
            map.removeLayer(value);
        });
        planes = {};
        for (const flight of list) {
            let id = flight.flight_id;
            if (id == clickedFlight) {
                planes[id] = L.marker([flight.latitude, flight.longitude], {
                    icon: emptyPlane,
                    rotationOrigin: 'center',
                    myCustomId: flight.flight_id
                }).addTo(map);
            } else {
                planes[id] = L.marker([flight.latitude, flight.longitude], {
                    icon: filledPlane,
                    rotationOrigin: 'center',
                    myCustomId: flight.flight_id
                }).addTo(map);
            }
            planes[id]._icon.id = (id + "_m");
        }
    }

    this.DeleteFlight = (id) => {
        let marker = planes[id];
        map.removeLayer(marker);
        map.removeLayer(rootOfClickedFlight);
    }

    this.UpdateClick = (id) => {
        this.RemoveClick();
        clickedFlight = id;
        let marker = planes[id];
        marker.setIcon(emptyPlane);
        map.setView(marker.getLatLng(), 7);
    }

    this.RemoveClick = () => {
        if (clickedFlight != null) {
            map.removeLayer(this.rootOfClickedFlight);
            planes[clickedFlight].setIcon(filledPlane);
            clickedFlight = null;
        }
    }

    this.UpdateRoot = (id, root) => {
        if (id == clickedFlight) {
            this.rootOfClickedFlight = L.polyline(root).addTo(map);
        }
    }
}