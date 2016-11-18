var email;
var eventType;
var startTime;
var endTime;
var maximumGroupSize;

function setSize(event) {
    document.getElementById("activity-options").style.display = "none";
    document.getElementById("size").style.display = "block";
    eventType = event;
}

function setTime() {
    document.getElementById("size").style.display = "none";
    document.getElementById("time").style.display = "block";
    maximumGroupSize = document.getElementById("maximumGroupSize").value;
}

function createUser() {
    document.getElementById("time").style.display = "none";
    document.getElementById("validUser").style.display = "block";
    startTime = document.getElementById("startTime").value;
    endTime = document.getElementById("endTime").value;

    //Set hidden input values for form
    document.getElementById("start").value = startTime;
    document.getElementById("end").value = endTime;
    document.getElementById("max").value = maximumGroupSize;
    document.getElementById("event").value = eventType;
}

//    document.getElementById("myForm").action ="http://slalommeetupapi.azurewebsites.net:80/slalom-connects-api/get-event-requests";






