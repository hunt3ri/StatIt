
/*
 * Worker function allowing us to get IAP data in parallel
 */
self.onmessage = function RefreshDownloadData(event) {

    var xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            var downloadData = JSON.parse(xhr.responseText);
            self.postMessage(downloadData);
        }
    }
    xhr.open('GET', '/Home/GetRevenues?appId=FairySchool&dateStart=' + event.data.dateStart + '&dateEnd=' + event.data.dateEnd, true);
    xhr.send(null);

};