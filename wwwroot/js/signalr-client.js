const connection = new signalR.HubConnectionBuilder()
    .withUrl("/pagehub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start().then(() => {
    console.log("SignalR connected.");
}).catch(err => {
    console.error("SignalR connection error: ", err);
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", { id: 1, content: message }).catch(err => {
        console.error("Error invoking SendMessage: ", err);
    });
    event.preventDefault();
});
