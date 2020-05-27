

/*let btn = $(".getBtn")
btn.click(()=>{
    $.ajax({
        type:'GET',
        url:'http://localhost:5200/api/Flights',
        success: (flights)=>{
            console.log("Flights====>")
            console.log(flights)
        },
        error: alert("error in get")
    })
})*/
var time = "relative_to=2020-12-26T23:51:00Z"
var getFlights = ()=>{
    $.ajax({
        type:"GET",
        url:'http://localhost:5200/api/Flights?'+time,
        contentType:"application/json",
        //crossDomain:true,
        //dataType: 'jsonp or json or text',
        success: (flights)=>console.log(flights),
        failure: alert("error in get")
    })   
}

var flightPlan = {
    "passengers": 1,
    "company_name": "After",
    "initial_location": {
    "longitude": 33.244,
    "latitude": 31.12,
    "date_time": "2020-12-27T00:00:00Z"
    },
    "segments": [
    {
    "longitude": 33.234,
    "latitude": 31.18,
    "timespan_seconds": 60
    },
    {
    "longitude": 33.224,
    "latitude": 31.22,
    "timespan_seconds":530	
    },
    {
    "longitude": 33.204,
    "latitude": 31.30,
    "timespan_seconds": 10	
    },
    {
    "longitude": 33.200,
    "latitude": 31.35,
    "timespan_seconds": 600	
    }
    ]
   }
var postFlightPlan = ()=>{
    $.ajax({
        type:'POST',
        url:'http://localhost:5200/api/FlightPlan',
        contentType: 'aplication/json',
        dataType: "json",
        data: flightPlan,
        success: function(respone){
            console.log(respone);
        } 
    })
}