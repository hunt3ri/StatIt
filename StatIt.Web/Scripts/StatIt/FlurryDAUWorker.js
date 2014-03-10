self.onmessage = function GetFlurryDauData(event) {

    var xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            var iapData = JSON.parse(xhr.responseText);
            self.postMessage(iapData);
        }
    }

    xhr.open('GET', '/Home/GetDAU?dateStart=' + event.data.dateStart + '&dateEnd=' + event.data.dateEnd, true);
    xhr.send(null);

};