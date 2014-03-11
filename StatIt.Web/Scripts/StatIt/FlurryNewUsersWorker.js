self.onmessage = function GetFlurryNewUsers(event) {

    var xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            var newUsers = JSON.parse(xhr.responseText);
            self.postMessage(newUsers);
        }
    }

    xhr.open('GET', '/Home/GetNewUsers?dateStart=' + event.data.dateStart + '&dateEnd=' + event.data.dateEnd, true);
    xhr.send(null);

};