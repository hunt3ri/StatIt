﻿
/*
 * Worker function allowing us to get IAP data in parallel
 */
self.onmessage = function RefreshIAPData(event) {

    var xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            var dauData = JSON.parse(xhr.responseText);
            self.postMessage(dauData);
        }
    }

    xhr.open('GET', '/Home/GetIAPRevenues?appId=FairySchool&dateStart=' + event.data.dateStart + '&dateEnd=' + event.data.dateEnd, true);
    xhr.send(null);

};



