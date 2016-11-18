var email;
var eventType;
var startTime;
var endTime;
var maximumGroupSize;


function setSize(event) {
    document.getElementById("activity-options").style.display = "none";
    document.getElementById("size").style.display = "block";
    eventType = event;

    console.log(eventType);
}

function setTime() {
    document.getElementById("size").style.display = "none";
    document.getElementById("time").style.display = "block";
    maximumGroupSize = document.getElementById("maximumGroupSize").value;

    console.log(maximumGroupSize);
}

function createUser() {
    document.getElementById("time").style.display = "none";
    document.getElementById("validUser").style.display = "block";
    startTime = document.getElementById("startTime").value;
    endTime = document.getElementById("endTime").value;

    console.log(startTime);
    console.log(endTime);
}


function submitForm() {
    name = document.getElementById("name").value;
    email = document.getElementById("email").value;
    password = document.getElementById("password").value;

    console.log(name + " " + email + " " + password);

    //document.getElementById("myForm").submit();

}





//    <form id="myForm" method="post" action="slalom-connects-api/post-event-request">

//    public HttpResponseMessage Post(string email, EventType eventType, DateTime startTime, DateTime endTime, int? maximumGroupSize)

