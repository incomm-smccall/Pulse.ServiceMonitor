setupConnection = (hubProxy) => {
    hubProxy.client.receiveJobStatusUpdate = (updateObject) => {

    };
    hubProxy.client.receiveMonitorStatusUpdate = (updateObject) => {

    };
};

$(function () {
    $.connection.hub.url = "http://localhost:8899/signalr";
});