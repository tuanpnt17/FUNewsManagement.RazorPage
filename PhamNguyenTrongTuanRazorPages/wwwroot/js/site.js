"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalrserver").build();

connection.on("ReceiveData", function (user, message) {
    console.log("ReceiveData");
});

connection.start().then(function () {
    console.log("Connection started");
}).catch(function (err) {
    return console.error(err.toString());
});
