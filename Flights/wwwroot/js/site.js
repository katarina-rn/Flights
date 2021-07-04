// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//import { type } from "jquery";

// Write your JavaScript code.

var routeUrl = location.protocol + "//" + location.host;

const client2 = new signalR.HubConnectionBuilder().withUrl("/reservationadded").build();

client2.on("AddReservation", newRes => {
    getReservationsSR();
    visitorReservations();
});

client2.on("EditReservation", newRes => {
    getReservationsSR();
    visitorReservations();
});

client2.on("DeleteReservation", newRes => {
    getReservationsSR();
    visitorReservations();
});

let res = [];

$(document).ready(function () {
    getReservations();
    visitorReservations();
});

function getReservations() {
    $.ajax({
        url: routeUrl + '/api/reservations/getReservations',
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            res = response;
            console.log(res);
            addReservations();
        },
    });
    client2.start();
}

function getReservationsSR() {
    $.ajax({
        url: routeUrl + '/api/reservations/getReservations',
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            res = response;
            addReservations();
        },
    });
}

function addReservations() {
    var tbl = $("#reservations tbody");
    tbl.empty();
    $("#reservations tbody").show();
    $.each(res, function (idx, elem) {
        if (elem.status === "Na cekanju") {
            tbl.append(`<tr>
                        <td>` + elem.destinationFrom + `-` + elem.destinationTo + `</td>
                        <td>` + elem.flightR.flightDate + `</td>
                        <td>` + elem.flightR.numberOfConnections + `</td>
                        <td>` + elem.flightR.availableSeats + `</td>
                        <td>` + elem.passanger.name + `</td>
                        <td>` + elem.status + `</td>
                        <td><button class="btn btn-success" onclick="editReservation(`+ elem.id + `)">Odobri</button></td>
                        <td><button class="btn btn-danger" onclick="deleteReservation(`+ elem.id + `)">Obriši</button></td>
                    </tr>`);
        }
        else {
            tbl.append(`<tr>
                        <td>` + elem.destinationFrom + `-` + elem.destinationTo + `</td>
                        <td>` + elem.flightR.flightDate + `</td>
                        <td>` + elem.flightR.numberOfConnections + `</td>
                        <td>` + elem.flightR.availableSeats + `</td>
                        <td>` + elem.passanger.name + `</td>
                        <td>` + elem.status + `</td>
                    </tr>`);
        }
    });
}

function editReservation(idRes) {
    let requestData = idRes;
    $.ajax({
        url: routeUrl + '/api/reservations/editReservations/' + idRes,
        type: 'PATCH',
        data: JSON.stringify(requestData),
        contentType: "application/json",
        success: function (response) {
            
        },
    });
}

function deleteReservation(idRes) {
    let requestData = idRes;
    $.ajax({
        url: routeUrl + '/api/reservations/deleteReservations/' + idRes,
        type: 'DELETE',
        data: JSON.stringify(requestData),
        contentType: "application/json",
        success: function (response) {
        },
    });
}

function addReservation(fl) {
    console.log(fl)
    $.ajax({
        url: routeUrl + '/api/reservations/addReservation/' + fl,
        type: 'POST',
        data: JSON.stringify(fl),
        contentType: "application/json",
        success: function (response) {
            if (response.status == "error")
                alert(response.message);
            else
                alert("Uspešno ste uneli rezervaciju");
        },
        error: function (data) {
            
        },
    });
}

function visitorReservations() {
    let id = $("#userId").val();
    $.ajax({
        url: routeUrl + '/api/reservations/visitorReservations/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            var table = $("#reservationsVisitor");
            table.empty();
            $("#reservationsVisitor").show();
            $.each(response, function (idx, elem) {
                table.append(`<tr>
                    <td>` + elem.destinationFrom + `-` + elem.destinationTo + `</td>
                    <td>` + $.dateModal(elem.flightR.flightDate) + `</td>
                    <td>` + elem.flightR.numberOfConnections + `</td>
                    <td>` + elem.status + `</td>
                </tr>`);
            });
        },
    });
}

function searchFlights() {
    let searchDesFrom = $("#desFrom").children("option:selected").val();
    let searchDesTo = $("#desTo").children("option:selected").val();
    let searchDate = $("#dateFlight").val();
    let searchConnections = $("#noConnections").is(':checked');
    console.log($("#noConnections").is(':checked'));
    $.ajax({
        url: routeUrl + '/api/flights/searchFlights/' + searchDesFrom + '/' + searchDesTo + '/' + searchDate + '/' + searchConnections,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            $("#searchResults").empty();
            if ($("#searchHeader").is(":empty")) {
                $("#searchHeader").append(`
                <div class="row">
                    <div class="col-6"><h3>Pronađeni letovi<h3></div>
                    <div class="col-6">
                        <label style="font-size: 25px;">Bez konekcija:<label>
                        <input type="checkbox" id="noConnections" onchange="searchFlights();" style="margin-left:10px; width:15px; height:15px;">
                    </div>
                </div>`)
            }
            if (response.length === 0) {
                $("#searchResults").append(`
                    <div class="row">
                        <div class="col-8">
                            <h4>Nisu pronadjeni letovi za unete kriterijume</h4>
                        </div>
                    </div>`)
            } else {
                $.each(response, function (i, f) {
                    $("#searchResults").append(`
                    <div class="row pb-3">
                        <div class="col-3">`+ f.destinationFrom.name + `-` + f.destinationTo.name + `</div>
                        <div class="col-3">`+ $.dateModal(f.flightDate) + `</div>
                        <div class="col-3">`+ f.numberOfConnections + `</div>
                        <button class="btn btn-success col-3" onclick="addReservation(`+ f.id + `)">Rezerviši</button>
                    </div>`)
                });
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
    let date = year + "-" + month + "-" + day +"T"+ hours +":" +minutes+":00";

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