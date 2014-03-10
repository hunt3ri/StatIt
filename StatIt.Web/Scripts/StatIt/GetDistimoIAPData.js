
self.onmessage = function RefreshIAPData(event) {

    var xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {
            var iapData = JSON.parse(xhr.responseText);
            self.postMessage(iapData);
        }
    }

    xhr.open('GET', '/Home/GetIAPRevenues?appId=FairySchool&dateStart=' + event.data.dateStart + '&dateEnd=' + event.data.dateEnd, true);
    xhr.send(null);


    //$.ajax({
    //    url: '/Home/GetIAPRevenues?appId=FairySchool&dateStart=' + event.data.dateStart + '&dateEnd=' + event.data.dateEnd,
    //    //url: '/Home/GetRevenues?from=all&revenue=total&view=line&breakdown=application,appstore',
    //    beforeSend: function () {
    //        $('#iapLoader').show();
    //        $('#iapWeekChart').hide();
    //    },
    //    complete: function () {
    //        $('#iapLoader').hide();
    //    }
    //})
    //.done(function (data) {
    //    $('#iapWeekChart').show();
    //    // callback(data);
    //    self.postmessage(data);
    //})
    //.error(function () {
    //    $('#target').append('Failed');
    //});
};



