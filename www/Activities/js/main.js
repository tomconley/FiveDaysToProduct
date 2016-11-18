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
}


function submitForm() {
    name = document.getElementById("name").value;
    email = document.getElementById("email").value;
    password = document.getElementById("password").value;


    console.log(eventType);
    console.log(maximumGroupSize);
    console.log(startTime);
    console.log(endTime);
    console.log(name + " " + email + " " + password);

    document.getElementById("myForm").action ="http://slalommeetup.azurewebsites.net:80/slalom-connects-api/get-event-requests";

}



//    public HttpResponseMessage Post(string email, EventType eventType, DateTime startTime, DateTime endTime, int? maximumGroupSize)

