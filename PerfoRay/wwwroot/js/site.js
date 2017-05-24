var protocol = location.protocol === "https:" ? "wss:" : "ws:";
        var wsUri = protocol + "//" + window.location.host + "/ws";
        var socket = new WebSocket(wsUri);
        socket.onopen = e => {
            console.log("socket opened", e);
        };

        socket.onclose = function (e) {
            console.log("socket closed", e);
        };

        socket.onmessage = function (e) {
            console.log(e);
            socket.send("pong");
        };

        socket.onerror = function (e) {
            console.error(e.data);
        };