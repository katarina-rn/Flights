var routeUrl = location.protocol + "//" + location.host;

let calendar;

const client = new signalR.HubConnectionBuilder().withUrl("/flightadded").build();

client.on("NewFlightAdded", newFlight => {
    calendar.refetchEvents();
});

$(document).ready(function () {
    InitializeCalendar();
});

function InitializeCalendar() {

    let calendarEl = document.getElementById('calendar');

    calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'listWeek',
        locale: 'sr',
        headerToolbar: {
            left: 'prev,next',
            center: 'title',
            right: 'dayGridMonth,listWeek,timeGridDay'
        },
        selectable: true,
        editable: true,
        eventClick: function (info) {
            getFlightById(info.event.id);
        },
        events: function (fetchInfo, successCallback, failureCallback) {
            $.ajax({
                url: routeUrl + '/api/flights/getFlights',
                type: 'GET',
                dataType: 'JSON',
                success: function (response) {
                    var events = [];
                    $.each(response, function (i, data) {
                        events.push({
                            title: data.destinationFrom.name + "-" + data.destinationTo.name,
                            start: $.date(data.flightDate),
                            borderColor: "#162466",
                            textColor: "white",
                            id: data.id
                        });
                    })
                    successCallback(events);
                },
            });
        },
    });
    client.start();
    calendar.render();
}

function onShowModal(flight) {
    console.log(flight);
    $("#flightDate").val($.dateModal(flight.flightDate));
    $("#flightFrom").val(flight.destinationFrom.name);
    $("#flightTo").val(flight.destinationTo.name);
    $("#flightId").val(flight.id);
    $("#seeDetails").modal("show");
}

function getFlightById(id) {
    $.ajax({
        url: routeUrl + '/api/flights/getFlightById/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            onShowModal(response);
        },
    });
}

function cancelFlight() {
    let id = parseInt($("#flightId").val());
    $.ajax({
        url: routeUrl + '/api/flights/cancelFlightById/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response > 0) {
                calendar.refetchEvents();
                $("#seeDetails").modal("hide");
            }
        },
    });
}

$.date = function (dateObject) {
    let d = new Date(dateObject);
    let day = d.getDate();
    let month = d.getMonth() + 1;
    let year = d.getFullYear();
    let hours = d.getHours();
    let minutes = d.getMinutes();
    if (day < 10) {
        day = "0" + day;
    }
    if (month < 10) {
        month = "0" + month;
    }
    if (hours < 10) {
        hours = "0" + hours;
    }
    if (minutes < 10) {
        minutes = "0" + minutes;
    }
    let date = year + "-" + month + "-" + day + "T" + hours + ":" + minutes + ":00";

    return date;
};

$.dateModal = function (dateObject) {
    let d = new Date(dateObject);
    let day = d.getDate();
    let month = d.getMonth() + 1;
    let year = d.getFullYear();
    let hours = d.getHours();
    let minutes = d.getMinutes();
    if (day < 10) {
        day = "0" + day;
    }
    if (month < 10) {
        month = "0" + month;
    }
    if (hours < 10) {
        hours = "0" + hours;
    }
    if (minutes < 10) {
        minutes = "0" + minutes;
    }
    let date = day + "." + month + "." + year + " " + hours + ":" + minutes;

    return date;
};